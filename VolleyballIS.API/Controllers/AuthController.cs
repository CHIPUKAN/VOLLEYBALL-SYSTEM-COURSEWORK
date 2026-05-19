using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolleyballIS.Application.Common;
using VolleyballIS.Application.DTOs.Auth;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер аутентификации — вход в систему и получение профиля
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Поля
        private readonly IAuthService authService; // сервис аутентификации
        private readonly IConfiguration configuration; // конфигурация приложения
        #endregion

        #region Конструкторы
        public AuthController(IAuthService authService, IConfiguration configuration) // конструктор с внедрением зависимости
        {
            this.authService = authService;
            this.configuration = configuration;
        }
        #endregion

        #region Методы
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto) // самостоятельная регистрация
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] allowedRoles = ["Тренер", "Игрок", "Судья", "Организатор", "Зритель"];
            if (!allowedRoles.Contains(dto.Role))
            {
                dto.Role = "Зритель";
            }

            try
            {
                UserDto created = await authService.RegisterAsync(dto);
                string token = GenerateJwtToken(created);
                return Ok(new AuthResponseDto { Token = token, User = created });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto) // вход в систему
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDto? user = await authService.ValidateCredentialsAsync(dto.Email, dto.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Неверный email или пароль" });
            }

            string token = GenerateJwtToken(user);
            AuthResponseDto response = new AuthResponseDto { Token = token, User = user };
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser() // получить профиль текущего пользователя
        {
            string? idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idStr, out int userId))
            {
                return Unauthorized();
            }

            UserDto? user = await authService.GetByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }
        #endregion

        #region Вспомогательные методы
        private string GenerateJwtToken(UserDto user) // сгенерировать JWT-токен для пользователя
        {
            string secret = configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT secret не задан в конфигурации");
            int expiresInHours = int.TryParse(configuration["Jwt:ExpiresInHours"], out int h) ? h : 24;

            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            SymmetricSecurityKey key = new SymmetricSecurityKey(keyBytes);
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("fullName", user.FullName ?? string.Empty)
            ];

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiresInHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
        #endregion
    }
}

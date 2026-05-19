using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.Common;
using VolleyballIS.Application.DTOs.Auth;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер управления пользователями (только для Суперадминистратора)
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.SuperAdmin)]
    public class UsersController : ControllerBase
    {
        #region Поля
        private readonly IAuthService authService; // сервис аутентификации
        #endregion

        #region Конструкторы
        public UsersController(IAuthService authService) // конструктор с внедрением зависимости
        {
            this.authService = authService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll() // получить всех пользователей
        {
            IEnumerable<UserDto> result = await authService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id) // получить пользователя по идентификатору
        {
            UserDto? result = await authService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Пользователь с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] RegisterDto dto) // создать нового пользователя
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UserDto result = await authService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] RegisterDto dto) // обновить пользователя
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UserDto result = await authService.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) // удалить пользователя
        {
            try
            {
                await authService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion
    }
}

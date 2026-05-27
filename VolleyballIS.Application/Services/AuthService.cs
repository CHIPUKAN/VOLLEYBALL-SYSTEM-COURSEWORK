using System.Security.Cryptography;
using System.Text;
using VolleyballIS.Application.DTOs.Auth;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис аутентификации — проверка паролей, регистрация пользователей
    public class AuthService : IAuthService
    {
        #region Поля
        private readonly IUserRepository userRepository; // репозиторий пользователей
        #endregion

        #region Конструкторы
        public AuthService(IUserRepository userRepository) // конструктор с внедрением зависимости
        {
            this.userRepository = userRepository;
        }
        #endregion

        #region Методы
        public async Task<UserDto?> ValidateCredentialsAsync(string email, string password) // проверить email + пароль
        {
            AppUser? user = await userRepository.GetByEmailAsync(email);
            if (user == null) return null;
            if (!VerifyPassword(password, user.PasswordHash)) return null;
            return MapToDto(user);
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto) // зарегистрировать нового пользователя
        {
            bool exists = await userRepository.ExistsAsync(dto.Email);
            if (exists)
            {
                throw new InvalidOperationException($"Пользователь с email «{dto.Email}» уже существует");
            }

            AppUser user = new AppUser
            {
                Email = dto.Email.ToLowerInvariant(),
                PasswordHash = HashPassword(dto.Password),
                Role = dto.Role,
                FullName = dto.FullName,
                LinkedCoachId = dto.LinkedCoachId,
                LinkedPlayerId = dto.LinkedPlayerId,
                LinkedRefereeId = dto.LinkedRefereeId,
                LinkedOrganizerId = dto.LinkedOrganizerId,
                CreatedAt = DateTime.UtcNow
            };
            AppUser created = await userRepository.CreateAsync(user);
            return MapToDto(created);
        }

        public async Task<UserDto?> GetByIdAsync(int id) // получить пользователя по идентификатору
        {
            AppUser? user = await userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync() // получить всех пользователей
        {
            IEnumerable<AppUser> users = await userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto) // обновить данные пользователя
        {
            AppUser? existing = await userRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Пользователь с идентификатором {id} не найден");
            }

            existing.Email = dto.Email.ToLowerInvariant();
            existing.Role = dto.Role;
            existing.FullName = dto.FullName;
            existing.LinkedCoachId = dto.LinkedCoachId;
            existing.LinkedPlayerId = dto.LinkedPlayerId;
            existing.LinkedRefereeId = dto.LinkedRefereeId;
            existing.LinkedOrganizerId = dto.LinkedOrganizerId;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existing.PasswordHash = HashPassword(dto.Password);
            }

            AppUser updated = await userRepository.UpdateAsync(existing);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id) // удалить пользователя
        {
            bool exists = await userRepository.GetByIdAsync(id) != null;
            if (!exists)
            {
                throw new KeyNotFoundException($"Пользователь с идентификатором {id} не найден");
            }
            await userRepository.DeleteAsync(id);
        }
        #endregion

        #region Вспомогательные методы
        private static string HashPassword(string password) // PBKDF2-хэш пароля с солью
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string storedHash) // проверить пароль против хэша
        {
            string[] parts = storedHash.Split(':');
            if (parts.Length != 2) return false;
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);
            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        private static UserDto MapToDto(AppUser user) // маппинг AppUser -> UserDto
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                FullName = user.FullName,
                LinkedCoachId = user.LinkedCoachId,
                LinkedPlayerId = user.LinkedPlayerId,
                LinkedRefereeId = user.LinkedRefereeId,
                LinkedOrganizerId = user.LinkedOrganizerId,
                CreatedAt = user.CreatedAt
            };
        }
        #endregion
    }
}

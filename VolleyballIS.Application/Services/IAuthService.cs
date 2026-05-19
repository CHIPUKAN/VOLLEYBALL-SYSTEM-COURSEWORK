using VolleyballIS.Application.DTOs.Auth;

namespace VolleyballIS.Application.Services
{
    // Сервис аутентификации и управления пользователями
    public interface IAuthService
    {
        Task<UserDto?> ValidateCredentialsAsync(string email, string password); // проверить учётные данные

        Task<UserDto> RegisterAsync(RegisterDto dto); // зарегистрировать нового пользователя

        Task<UserDto?> GetByIdAsync(int id); // получить пользователя по идентификатору

        Task<IEnumerable<UserDto>> GetAllAsync(); // получить всех пользователей

        Task<UserDto> UpdateAsync(int id, RegisterDto dto); // обновить пользователя

        Task DeleteAsync(int id); // удалить пользователя
    }
}

using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Репозиторий пользователей системы
    public interface IUserRepository
    {
        Task<AppUser?> GetByEmailAsync(string email); // найти пользователя по email

        Task<AppUser?> GetByIdAsync(int id); // найти по идентификатору

        Task<IEnumerable<AppUser>> GetAllAsync(); // получить всех пользователей

        Task<AppUser> CreateAsync(AppUser user); // создать нового пользователя

        Task<AppUser> UpdateAsync(AppUser user); // обновить данные пользователя

        Task DeleteAsync(int id); // удалить пользователя

        Task<bool> ExistsAsync(string email); // проверить существование по email
    }
}

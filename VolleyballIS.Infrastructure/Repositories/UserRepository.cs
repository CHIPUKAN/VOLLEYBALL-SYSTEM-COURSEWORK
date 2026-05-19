using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий пользователей системы
    public class UserRepository : IUserRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public UserRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<AppUser?> GetByEmailAsync(string email) // найти пользователя по email
        {
            AppUser? result = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
            return result;
        }

        public async Task<AppUser?> GetByIdAsync(int id) // найти по идентификатору
        {
            AppUser? result = await dbContext.Users.FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync() // получить всех пользователей
        {
            IEnumerable<AppUser> result = await dbContext.Users
                .OrderBy(u => u.Role)
                .ThenBy(u => u.Email)
                .ToListAsync();
            return result;
        }

        public async Task<AppUser> CreateAsync(AppUser user) // создать нового пользователя
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<AppUser> UpdateAsync(AppUser user) // обновить данные пользователя
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id) // удалить пользователя
        {
            AppUser? user = await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string email) // проверить существование по email
        {
            bool result = await dbContext.Users
                .AnyAsync(u => u.Email == email.ToLowerInvariant());
            return result;
        }
        #endregion
    }
}

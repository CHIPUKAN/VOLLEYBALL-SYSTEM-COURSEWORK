using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий тренеров — реализует доступ к таблице t3_coaches
    public class CoachRepository : ICoachRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public CoachRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T3Coach>> GetAllAsync() // получить всех тренеров
        {
            IEnumerable<T3Coach> result = await dbContext.Coaches
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
            return result;
        }

        public async Task<T3Coach?> GetByIdAsync(int id) // получить тренера по идентификатору
        {
            T3Coach? result = await dbContext.Coaches.FindAsync(id);
            return result;
        }

        public async Task<T3Coach> CreateAsync(T3Coach coach) // создать тренера
        {
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();
            T3Coach result = (await GetByIdAsync(coach.Id))!;
            return result;
        }

        public async Task<T3Coach> UpdateAsync(T3Coach coach) // обновить тренера
        {
            dbContext.Coaches.Update(coach);
            await dbContext.SaveChangesAsync();
            T3Coach result = (await GetByIdAsync(coach.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить тренера
        {
            T3Coach? coach = await dbContext.Coaches.FindAsync(id);
            if (coach != null)
            {
                dbContext.Coaches.Remove(coach);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Coaches.AnyAsync(c => c.Id == id);
            return result;
        }
        #endregion
    }
}

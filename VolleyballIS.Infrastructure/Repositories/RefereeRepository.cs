using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий судей — реализует доступ к таблице t12_referees
    public class RefereeRepository : IRefereeRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public RefereeRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T12Referee>> GetAllAsync() // получить всех судей
        {
            IEnumerable<T12Referee> result = await dbContext.Referees
                .OrderBy(r => r.LastName)
                .ThenBy(r => r.FirstName)
                .ToListAsync();
            return result;
        }

        public async Task<T12Referee?> GetByIdAsync(int id) // получить судью по идентификатору
        {
            T12Referee? result = await dbContext.Referees.FindAsync(id);
            return result;
        }

        public async Task<T12Referee> CreateAsync(T12Referee referee) // создать судью
        {
            dbContext.Referees.Add(referee);
            await dbContext.SaveChangesAsync();
            T12Referee result = (await GetByIdAsync(referee.Id))!;
            return result;
        }

        public async Task<T12Referee> UpdateAsync(T12Referee referee) // обновить судью
        {
            dbContext.Referees.Update(referee);
            await dbContext.SaveChangesAsync();
            T12Referee result = (await GetByIdAsync(referee.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить судью
        {
            T12Referee? referee = await dbContext.Referees.FindAsync(id);
            if (referee != null)
            {
                dbContext.Referees.Remove(referee);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Referees.AnyAsync(r => r.Id == id);
            return result;
        }
        #endregion
    }
}

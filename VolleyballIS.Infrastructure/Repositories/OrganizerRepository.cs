using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий организаторов — реализует доступ к таблице t13_organizers
    public class OrganizerRepository : IOrganizerRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public OrganizerRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T13Organizer>> GetAllAsync() // получить всех организаторов
        {
            IEnumerable<T13Organizer> result = await dbContext.Organizers
                .OrderBy(o => o.LastName)
                .ThenBy(o => o.FirstName)
                .ToListAsync();
            return result;
        }

        public async Task<T13Organizer?> GetByIdAsync(int id) // получить организатора по идентификатору
        {
            T13Organizer? result = await dbContext.Organizers.FindAsync(id);
            return result;
        }

        public async Task<T13Organizer> CreateAsync(T13Organizer organizer) // создать организатора
        {
            dbContext.Organizers.Add(organizer);
            await dbContext.SaveChangesAsync();
            T13Organizer result = (await GetByIdAsync(organizer.Id))!;
            return result;
        }

        public async Task<T13Organizer> UpdateAsync(T13Organizer organizer) // обновить организатора
        {
            dbContext.Organizers.Update(organizer);
            await dbContext.SaveChangesAsync();
            T13Organizer result = (await GetByIdAsync(organizer.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить организатора
        {
            T13Organizer? organizer = await dbContext.Organizers.FindAsync(id);
            if (organizer != null)
            {
                dbContext.Organizers.Remove(organizer);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Organizers.AnyAsync(o => o.Id == id);
            return result;
        }
        #endregion
    }
}

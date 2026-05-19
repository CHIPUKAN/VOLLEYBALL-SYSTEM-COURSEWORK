using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий площадок — реализует доступ к таблице t2_venues
    public class VenueRepository : IVenueRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public VenueRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T2Venue>> GetAllAsync() // получить все площадки
        {
            IEnumerable<T2Venue> result = await dbContext.Venues
                .OrderBy(v => v.City)
                .ThenBy(v => v.Name)
                .ToListAsync();
            return result;
        }

        public async Task<T2Venue?> GetByIdAsync(int id) // получить площадку по идентификатору
        {
            T2Venue? result = await dbContext.Venues.FindAsync(id);
            return result;
        }

        public async Task<T2Venue> CreateAsync(T2Venue venue) // создать площадку
        {
            dbContext.Venues.Add(venue);
            await dbContext.SaveChangesAsync();
            T2Venue result = (await GetByIdAsync(venue.Id))!;
            return result;
        }

        public async Task<T2Venue> UpdateAsync(T2Venue venue) // обновить площадку
        {
            dbContext.Venues.Update(venue);
            await dbContext.SaveChangesAsync();
            T2Venue result = (await GetByIdAsync(venue.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить площадку
        {
            T2Venue? venue = await dbContext.Venues.FindAsync(id);
            if (venue != null)
            {
                dbContext.Venues.Remove(venue);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Venues.AnyAsync(v => v.Id == id);
            return result;
        }
        #endregion
    }
}

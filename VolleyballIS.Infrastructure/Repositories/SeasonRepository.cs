using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий сезонов — реализует доступ к таблице t9_seasons
    public class SeasonRepository : ISeasonRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public SeasonRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T9Season>> GetAllAsync() // получить все сезоны
        {
            IEnumerable<T9Season> result = await dbContext.Seasons
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
            return result;
        }

        public async Task<T9Season?> GetByIdAsync(int id) // получить сезон по идентификатору
        {
            T9Season? result = await dbContext.Seasons.FindAsync(id);
            return result;
        }

        public async Task<T9Season> CreateAsync(T9Season season) // создать сезон
        {
            dbContext.Seasons.Add(season);
            await dbContext.SaveChangesAsync();
            T9Season result = (await GetByIdAsync(season.Id))!;
            return result;
        }

        public async Task<T9Season> UpdateAsync(T9Season season) // обновить сезон
        {
            dbContext.Seasons.Update(season);
            await dbContext.SaveChangesAsync();
            T9Season result = (await GetByIdAsync(season.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить сезон
        {
            T9Season? season = await dbContext.Seasons.FindAsync(id);
            if (season != null)
            {
                dbContext.Seasons.Remove(season);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Seasons.AnyAsync(s => s.Id == id);
            return result;
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null) // проверить уникальность наименования
        {
            bool result = await dbContext.Seasons
                .AnyAsync(s => s.Name == name && (excludeId == null || s.Id != excludeId));
            return result;
        }
        #endregion
    }
}

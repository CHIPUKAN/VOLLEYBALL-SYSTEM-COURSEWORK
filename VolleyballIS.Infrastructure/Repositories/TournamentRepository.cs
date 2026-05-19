using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий турниров — реализует доступ к таблице t10_tournaments
    public class TournamentRepository : ITournamentRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public TournamentRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T10Tournament>> GetAllAsync() // получить все турниры со связанными данными
        {
            IEnumerable<T10Tournament> result = await dbContext.Tournaments
                .Include(t => t.Season)
                .Include(t => t.Organizer)
                .Include(t => t.Format)
                .Include(t => t.ScoringSystem)
                .OrderByDescending(t => t.StartDate)
                .ToListAsync();
            return result;
        }

        public async Task<T10Tournament?> GetByIdAsync(int id) // получить турнир по идентификатору
        {
            T10Tournament? result = await dbContext.Tournaments
                .Include(t => t.Season)
                .Include(t => t.Organizer)
                .Include(t => t.Format)
                .Include(t => t.ScoringSystem)
                .FirstOrDefaultAsync(t => t.Id == id);
            return result;
        }

        public async Task<T10Tournament> CreateAsync(T10Tournament tournament) // создать турнир
        {
            dbContext.Tournaments.Add(tournament);
            await dbContext.SaveChangesAsync();
            T10Tournament result = (await GetByIdAsync(tournament.Id))!;
            return result;
        }

        public async Task<T10Tournament> UpdateAsync(T10Tournament tournament) // обновить турнир
        {
            dbContext.Tournaments.Update(tournament);
            await dbContext.SaveChangesAsync();
            T10Tournament result = (await GetByIdAsync(tournament.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить турнир
        {
            T10Tournament? tournament = await dbContext.Tournaments.FindAsync(id);
            if (tournament != null)
            {
                dbContext.Tournaments.Remove(tournament);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Tournaments.AnyAsync(t => t.Id == id);
            return result;
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null) // проверить уникальность наименования
        {
            bool result = await dbContext.Tournaments
                .AnyAsync(t => t.Name == name && (excludeId == null || t.Id != excludeId));
            return result;
        }
        #endregion
    }
}

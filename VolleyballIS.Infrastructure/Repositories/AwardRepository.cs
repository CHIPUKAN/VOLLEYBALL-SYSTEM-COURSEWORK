using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий наград — реализует доступ к таблице t20_awards
    public class AwardRepository : IAwardRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public AwardRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T20Award>> GetByTournamentIdAsync(int tournamentId) // награды турнира
        {
            IEnumerable<T20Award> result = await dbContext.Awards
                .Include(a => a.AwardType)
                .Include(a => a.Player)
                .Include(a => a.Team)
                .Include(a => a.Tournament)
                .Where(a => a.TournamentId == tournamentId)
                .OrderBy(a => a.AwardTypeCode).ThenBy(a => a.Name)
                .ToListAsync();
            return result;
        }

        public async Task<T20Award?> GetByIdAsync(int id) // награда по id
        {
            T20Award? result = await dbContext.Awards
                .Include(a => a.AwardType)
                .Include(a => a.Player)
                .Include(a => a.Team)
                .Include(a => a.Tournament)
                .FirstOrDefaultAsync(a => a.Id == id);
            return result;
        }

        public async Task<T20Award> CreateAsync(T20Award award) // создать награду
        {
            dbContext.Awards.Add(award);
            await dbContext.SaveChangesAsync();
            T20Award result = (await GetByIdAsync(award.Id))!;
            return result;
        }

        public async Task<T20Award> UpdateAsync(T20Award award) // обновить награду
        {
            dbContext.Awards.Update(award);
            await dbContext.SaveChangesAsync();
            T20Award result = (await GetByIdAsync(award.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить награду
        {
            T20Award? award = await dbContext.Awards.FindAsync(id);
            if (award != null)
            {
                dbContext.Awards.Remove(award);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Awards.AnyAsync(a => a.Id == id);
            return result;
        }
        #endregion
    }
}

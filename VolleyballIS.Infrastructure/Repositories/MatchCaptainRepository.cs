using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий капитанов — реализует доступ к таблице t21_match_captains
    public class MatchCaptainRepository : IMatchCaptainRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public MatchCaptainRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T21MatchCaptain>> GetByMatchIdAsync(int matchId) // капитаны матча
        {
            IEnumerable<T21MatchCaptain> result = await dbContext.MatchCaptains
                .Include(c => c.Team)
                .Include(c => c.Player)
                .Where(c => c.MatchId == matchId)
                .ToListAsync();
            return result;
        }

        public async Task<T21MatchCaptain?> GetByKeyAsync(int matchId, int teamId) // капитан команды в матче
        {
            T21MatchCaptain? result = await dbContext.MatchCaptains
                .Include(c => c.Team)
                .Include(c => c.Player)
                .FirstOrDefaultAsync(c => c.MatchId == matchId && c.TeamId == teamId);
            return result;
        }

        public async Task<T21MatchCaptain> UpsertAsync(T21MatchCaptain captain) // назначить или обновить
        {
            T21MatchCaptain? existing = await dbContext.MatchCaptains
                .FirstOrDefaultAsync(c => c.MatchId == captain.MatchId && c.TeamId == captain.TeamId);

            if (existing == null)
            {
                dbContext.MatchCaptains.Add(captain);
            }
            else
            {
                existing.PlayerId = captain.PlayerId;
                dbContext.MatchCaptains.Update(existing);
                captain = existing;
            }

            await dbContext.SaveChangesAsync();
            return (await GetByKeyAsync(captain.MatchId, captain.TeamId))!;
        }

        public async Task DeleteAsync(int matchId, int teamId) // снять капитана
        {
            T21MatchCaptain? captain = await dbContext.MatchCaptains
                .FirstOrDefaultAsync(c => c.MatchId == matchId && c.TeamId == teamId);
            if (captain != null)
            {
                dbContext.MatchCaptains.Remove(captain);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int matchId, int teamId) // проверить существование
        {
            bool result = await dbContext.MatchCaptains
                .AnyAsync(c => c.MatchId == matchId && c.TeamId == teamId);
            return result;
        }
        #endregion
    }
}

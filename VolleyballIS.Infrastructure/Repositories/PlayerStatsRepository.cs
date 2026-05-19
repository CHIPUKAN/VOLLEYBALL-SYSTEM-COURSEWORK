using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий статистики игроков — реализует доступ к таблице r2_player_stats
    public class PlayerStatsRepository : IPlayerStatsRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public PlayerStatsRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<R2PlayerStats>> GetByMatchIdAsync(int matchId) // статистика матча
        {
            IEnumerable<R2PlayerStats> result = await dbContext.PlayerStats
                .Include(s => s.Player)
                .Include(s => s.Team)
                .Where(s => s.MatchId == matchId)
                .OrderBy(s => s.Team!.Name).ThenBy(s => s.Player!.LastName)
                .ToListAsync();
            return result;
        }

        public async Task<R2PlayerStats?> GetByKeyAsync(int matchId, int playerId) // статистика по ключу
        {
            R2PlayerStats? result = await dbContext.PlayerStats
                .Include(s => s.Player)
                .Include(s => s.Team)
                .FirstOrDefaultAsync(s => s.MatchId == matchId && s.PlayerId == playerId);
            return result;
        }

        public async Task<R2PlayerStats> UpsertAsync(R2PlayerStats stats) // создать или обновить
        {
            R2PlayerStats? existing = await dbContext.PlayerStats
                .FirstOrDefaultAsync(s => s.MatchId == stats.MatchId && s.PlayerId == stats.PlayerId);

            if (existing == null)
            {
                dbContext.PlayerStats.Add(stats);
            }
            else
            {
                existing.TeamId = stats.TeamId;
                existing.ServesTotal = stats.ServesTotal;
                existing.Aces = stats.Aces;
                existing.ServeErrors = stats.ServeErrors;
                existing.ReceptionsTotal = stats.ReceptionsTotal;
                existing.PositiveReceptions = stats.PositiveReceptions;
                existing.ReceptionErrors = stats.ReceptionErrors;
                existing.AttacksTotal = stats.AttacksTotal;
                existing.AttackPoints = stats.AttackPoints;
                existing.AttackErrors = stats.AttackErrors;
                existing.Blocks = stats.Blocks;
                existing.TotalPoints = stats.TotalPoints;
                dbContext.PlayerStats.Update(existing);
                stats = existing;
            }

            await dbContext.SaveChangesAsync();
            return (await GetByKeyAsync(stats.MatchId, stats.PlayerId))!;
        }

        public async Task DeleteAsync(int matchId, int playerId) // удалить запись
        {
            R2PlayerStats? stats = await dbContext.PlayerStats
                .FirstOrDefaultAsync(s => s.MatchId == matchId && s.PlayerId == playerId);
            if (stats != null)
            {
                dbContext.PlayerStats.Remove(stats);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int matchId, int playerId) // проверить существование
        {
            bool result = await dbContext.PlayerStats
                .AnyAsync(s => s.MatchId == matchId && s.PlayerId == playerId);
            return result;
        }
        #endregion
    }
}

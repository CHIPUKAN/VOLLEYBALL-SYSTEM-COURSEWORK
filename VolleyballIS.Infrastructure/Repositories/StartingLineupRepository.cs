using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий стартовых расстановок — реализует доступ к таблице r1_starting_lineups
    public class StartingLineupRepository : IStartingLineupRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public StartingLineupRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<R1StartingLineup>> GetByMatchTeamSetAsync(int matchId, int teamId, short setNumber) // расстановка команды
        {
            IEnumerable<R1StartingLineup> result = await dbContext.StartingLineups
                .Include(l => l.Player)
                .Include(l => l.Team)
                .Where(l => l.MatchId == matchId && l.TeamId == teamId && l.SetNumber == setNumber)
                .OrderBy(l => l.PositionNo)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<R1StartingLineup>> GetByMatchIdAsync(int matchId) // все расстановки матча
        {
            IEnumerable<R1StartingLineup> result = await dbContext.StartingLineups
                .Include(l => l.Player)
                .Include(l => l.Team)
                .Where(l => l.MatchId == matchId)
                .OrderBy(l => l.SetNumber).ThenBy(l => l.TeamId).ThenBy(l => l.PositionNo)
                .ToListAsync();
            return result;
        }

        public async Task<R1StartingLineup?> GetByKeyAsync(int matchId, int teamId, short setNumber, short positionNo) // позиция по ключу
        {
            R1StartingLineup? result = await dbContext.StartingLineups
                .Include(l => l.Player)
                .Include(l => l.Team)
                .FirstOrDefaultAsync(l =>
                    l.MatchId == matchId &&
                    l.TeamId == teamId &&
                    l.SetNumber == setNumber &&
                    l.PositionNo == positionNo);
            return result;
        }

        public async Task<R1StartingLineup> UpsertAsync(R1StartingLineup lineup) // создать или обновить позицию
        {
            // если игрок уже занимает другую позицию в этой же команде и партии — убрать его оттуда
            R1StartingLineup? existingByPlayer = await dbContext.StartingLineups
                .FirstOrDefaultAsync(l =>
                    l.MatchId == lineup.MatchId &&
                    l.TeamId == lineup.TeamId &&
                    l.SetNumber == lineup.SetNumber &&
                    l.PlayerId == lineup.PlayerId &&
                    l.PositionNo != lineup.PositionNo);
            if (existingByPlayer != null)
            {
                dbContext.StartingLineups.Remove(existingByPlayer);
            }

            R1StartingLineup? existing = await dbContext.StartingLineups
                .FirstOrDefaultAsync(l =>
                    l.MatchId == lineup.MatchId &&
                    l.TeamId == lineup.TeamId &&
                    l.SetNumber == lineup.SetNumber &&
                    l.PositionNo == lineup.PositionNo);

            if (existing == null)
            {
                dbContext.StartingLineups.Add(lineup);
            }
            else
            {
                existing.PlayerId = lineup.PlayerId;
                dbContext.StartingLineups.Update(existing);
                lineup = existing;
            }

            await dbContext.SaveChangesAsync();
            return lineup;
        }

        public async Task DeleteByMatchTeamSetAsync(int matchId, int teamId, short setNumber) // удалить расстановку команды в партии
        {
            List<R1StartingLineup> lineups = await dbContext.StartingLineups
                .Where(l => l.MatchId == matchId && l.TeamId == teamId && l.SetNumber == setNumber)
                .ToListAsync();
            if (lineups.Any())
            {
                dbContext.StartingLineups.RemoveRange(lineups);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int matchId, int teamId, short setNumber, short positionNo) // проверить существование
        {
            bool result = await dbContext.StartingLineups
                .AnyAsync(l =>
                    l.MatchId == matchId &&
                    l.TeamId == teamId &&
                    l.SetNumber == setNumber &&
                    l.PositionNo == positionNo);
            return result;
        }
        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий матчей — реализует доступ к таблице t14_matches
    public class MatchRepository : IMatchRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public MatchRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T14Match>> GetAllAsync() // получить все матчи со связанными данными
        {
            IEnumerable<T14Match> result = await dbContext.Matches
                .Include(m => m.Tournament)
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Venue)
                .Include(m => m.Stage)
                .Include(m => m.Status)
                .Include(m => m.Group)
                .Include(m => m.CoinTossChoice)
                .Include(m => m.CoinTossWinnerTeam)
                .Include(m => m.FirstServeTeam)
                .OrderByDescending(m => m.MatchDate)
                .ThenBy(m => m.StartTime)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T14Match>> GetByTournamentIdAsync(int tournamentId) // получить матчи по турниру
        {
            IEnumerable<T14Match> result = await dbContext.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Venue)
                .Include(m => m.Stage)
                .Include(m => m.Status)
                .Include(m => m.Group)
                .Include(m => m.CoinTossChoice)
                .Include(m => m.CoinTossWinnerTeam)
                .Include(m => m.FirstServeTeam)
                .Where(m => m.TournamentId == tournamentId)
                .OrderBy(m => m.MatchDate)
                .ThenBy(m => m.StartTime)
                .ToListAsync();
            return result;
        }

        public async Task<T14Match?> GetByIdAsync(int id) // получить матч по идентификатору
        {
            T14Match? result = await dbContext.Matches
                .Include(m => m.Tournament)
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Venue)
                .Include(m => m.Stage)
                .Include(m => m.Status)
                .Include(m => m.Group)
                .Include(m => m.CoinTossChoice)
                .Include(m => m.CoinTossWinnerTeam)
                .Include(m => m.FirstServeTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            return result;
        }

        public async Task<T14Match> CreateAsync(T14Match match) // создать матч
        {
            dbContext.Matches.Add(match);
            await dbContext.SaveChangesAsync();
            T14Match result = (await GetByIdAsync(match.Id))!;
            return result;
        }

        public async Task<T14Match> UpdateAsync(T14Match match) // обновить матч
        {
            dbContext.Matches.Update(match);
            await dbContext.SaveChangesAsync();
            T14Match result = (await GetByIdAsync(match.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить матч
        {
            T14Match? match = await dbContext.Matches.FindAsync(id);
            if (match != null)
            {
                dbContext.Matches.Remove(match);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Matches.AnyAsync(m => m.Id == id);
            return result;
        }
        #endregion
    }
}

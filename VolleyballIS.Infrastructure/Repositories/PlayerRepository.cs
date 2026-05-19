using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий игроков — реализует доступ к таблице t6_players
    public class PlayerRepository : IPlayerRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public PlayerRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T6Player>> GetAllAsync() // получить всех игроков со связанными данными
        {
            IEnumerable<T6Player> result = await dbContext.Players
                .Include(p => p.Team)
                .Include(p => p.Amplua)
                .Include(p => p.Status)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T6Player>> GetByTeamIdAsync(int teamId) // получить игроков по команде
        {
            IEnumerable<T6Player> result = await dbContext.Players
                .Include(p => p.Amplua)
                .Include(p => p.Status)
                .Where(p => p.TeamId == teamId)
                .OrderBy(p => p.LastName)
                .ToListAsync();
            return result;
        }

        public async Task<T6Player?> GetByIdAsync(int id) // получить игрока по идентификатору
        {
            T6Player? result = await dbContext.Players
                .Include(p => p.Team)
                .Include(p => p.Amplua)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task<T6Player> CreateAsync(T6Player player) // создать игрока
        {
            dbContext.Players.Add(player);
            await dbContext.SaveChangesAsync();
            T6Player result = (await GetByIdAsync(player.Id))!;
            return result;
        }

        public async Task<T6Player> UpdateAsync(T6Player player) // обновить игрока
        {
            dbContext.Players.Update(player);
            await dbContext.SaveChangesAsync();
            T6Player result = (await GetByIdAsync(player.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить игрока
        {
            T6Player? player = await dbContext.Players.FindAsync(id);
            if (player != null)
            {
                dbContext.Players.Remove(player);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Players.AnyAsync(p => p.Id == id);
            return result;
        }
        #endregion
    }
}

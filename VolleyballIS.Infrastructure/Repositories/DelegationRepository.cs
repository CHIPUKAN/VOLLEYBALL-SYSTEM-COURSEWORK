using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий делегаций — реализует доступ к таблице t5_delegations
    public class DelegationRepository : IDelegationRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public DelegationRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T5Delegation>> GetByMatchIdAsync(int matchId) // делегация матча
        {
            IEnumerable<T5Delegation> result = await dbContext.Delegations
                .Include(d => d.Team)
                .Where(d => d.MatchId == matchId)
                .OrderBy(d => d.TeamId).ThenBy(d => d.RoleType)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T5Delegation>> GetByMatchTeamAsync(int matchId, int teamId) // делегация команды
        {
            IEnumerable<T5Delegation> result = await dbContext.Delegations
                .Include(d => d.Team)
                .Where(d => d.MatchId == matchId && d.TeamId == teamId)
                .OrderBy(d => d.RoleType)
                .ToListAsync();
            return result;
        }

        public async Task<T5Delegation?> GetByIdAsync(int id) // участник по id
        {
            T5Delegation? result = await dbContext.Delegations
                .Include(d => d.Team)
                .FirstOrDefaultAsync(d => d.Id == id);
            return result;
        }

        public async Task<T5Delegation> CreateAsync(T5Delegation delegation) // добавить участника
        {
            dbContext.Delegations.Add(delegation);
            await dbContext.SaveChangesAsync();
            T5Delegation result = (await GetByIdAsync(delegation.Id))!;
            return result;
        }

        public async Task<T5Delegation> UpdateAsync(T5Delegation delegation) // обновить участника
        {
            dbContext.Delegations.Update(delegation);
            await dbContext.SaveChangesAsync();
            T5Delegation result = (await GetByIdAsync(delegation.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить участника
        {
            T5Delegation? delegation = await dbContext.Delegations.FindAsync(id);
            if (delegation != null)
            {
                dbContext.Delegations.Remove(delegation);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Delegations.AnyAsync(d => d.Id == id);
            return result;
        }
        #endregion
    }
}

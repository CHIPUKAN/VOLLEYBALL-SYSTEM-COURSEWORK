using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий санкций — реализует доступ к таблице t18_sanctions
    public class SanctionRepository : ISanctionRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public SanctionRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T18Sanction>> GetByMatchIdAsync(int matchId) // санкции матча
        {
            IEnumerable<T18Sanction> result = await dbContext.Sanctions
                .Include(s => s.Team)
                .Include(s => s.Player)
                .Include(s => s.RecipientType)
                .Include(s => s.SanctionType)
                .Include(s => s.SanctionKind)
                .Include(s => s.DelayViolation)
                .Where(s => s.MatchId == matchId)
                .OrderBy(s => s.SetNumber).ThenBy(s => s.Id)
                .ToListAsync();
            return result;
        }

        public async Task<T18Sanction?> GetByIdAsync(int id) // санкция по id
        {
            T18Sanction? result = await dbContext.Sanctions
                .Include(s => s.Team)
                .Include(s => s.Player)
                .Include(s => s.RecipientType)
                .Include(s => s.SanctionType)
                .Include(s => s.SanctionKind)
                .Include(s => s.DelayViolation)
                .FirstOrDefaultAsync(s => s.Id == id);
            return result;
        }

        public async Task<T18Sanction> CreateAsync(T18Sanction sanction) // создать санкцию
        {
            dbContext.Sanctions.Add(sanction);
            await dbContext.SaveChangesAsync();
            T18Sanction result = (await GetByIdAsync(sanction.Id))!;
            return result;
        }

        public async Task<T18Sanction> UpdateAsync(T18Sanction sanction) // обновить санкцию
        {
            dbContext.Sanctions.Update(sanction);
            await dbContext.SaveChangesAsync();
            T18Sanction result = (await GetByIdAsync(sanction.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить санкцию
        {
            T18Sanction? sanction = await dbContext.Sanctions.FindAsync(id);
            if (sanction != null)
            {
                dbContext.Sanctions.Remove(sanction);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Sanctions.AnyAsync(s => s.Id == id);
            return result;
        }
        #endregion
    }
}

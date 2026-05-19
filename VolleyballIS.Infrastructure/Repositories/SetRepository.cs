using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий партий матча — реализует доступ к таблице r3_sets
    public class SetRepository : ISetRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public SetRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<R3Set>> GetByMatchIdAsync(int matchId) // партии матча
        {
            IEnumerable<R3Set> result = await dbContext.Sets
                .Where(s => s.MatchId == matchId)
                .OrderBy(s => s.SetNumber)
                .ToListAsync();
            return result;
        }

        public async Task<R3Set?> GetByKeyAsync(int matchId, short setNumber) // партия по ключу
        {
            R3Set? result = await dbContext.Sets
                .FirstOrDefaultAsync(s => s.MatchId == matchId && s.SetNumber == setNumber);
            return result;
        }

        public async Task<R3Set> UpsertAsync(R3Set set) // создать или обновить партию
        {
            R3Set? existing = await dbContext.Sets
                .FirstOrDefaultAsync(s => s.MatchId == set.MatchId && s.SetNumber == set.SetNumber);

            if (existing == null)
            {
                dbContext.Sets.Add(set);
            }
            else
            {
                existing.HomeScore = set.HomeScore;
                existing.GuestScore = set.GuestScore;
                existing.DurationMin = set.DurationMin;
                dbContext.Sets.Update(existing);
                set = existing;
            }

            await dbContext.SaveChangesAsync();
            return set;
        }

        public async Task DeleteAsync(int matchId, short setNumber) // удалить партию
        {
            R3Set? set = await dbContext.Sets
                .FirstOrDefaultAsync(s => s.MatchId == matchId && s.SetNumber == setNumber);
            if (set != null)
            {
                dbContext.Sets.Remove(set);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int matchId, short setNumber) // проверить существование
        {
            bool result = await dbContext.Sets
                .AnyAsync(s => s.MatchId == matchId && s.SetNumber == setNumber);
            return result;
        }
        #endregion
    }
}

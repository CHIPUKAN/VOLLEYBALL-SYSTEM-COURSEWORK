using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий назначений судей — реализует доступ к таблице t15_referee_assignments
    public class RefereeAssignmentRepository : IRefereeAssignmentRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public RefereeAssignmentRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T15RefereeAssignment>> GetByMatchIdAsync(int matchId) // бригада матча
        {
            IEnumerable<T15RefereeAssignment> result = await dbContext.RefereeAssignments
                .Include(a => a.Referee)
                .Include(a => a.Role)
                .Where(a => a.MatchId == matchId)
                .OrderBy(a => a.RoleCode)
                .ToListAsync();
            return result;
        }

        public async Task<T15RefereeAssignment?> GetByIdAsync(int id) // назначение по id
        {
            T15RefereeAssignment? result = await dbContext.RefereeAssignments
                .Include(a => a.Referee)
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Id == id);
            return result;
        }

        public async Task<T15RefereeAssignment> CreateAsync(T15RefereeAssignment assignment) // создать назначение
        {
            dbContext.RefereeAssignments.Add(assignment);
            await dbContext.SaveChangesAsync();
            T15RefereeAssignment result = (await GetByIdAsync(assignment.Id))!;
            return result;
        }

        public async Task<T15RefereeAssignment> UpdateAsync(T15RefereeAssignment assignment) // обновить назначение
        {
            dbContext.RefereeAssignments.Update(assignment);
            await dbContext.SaveChangesAsync();
            T15RefereeAssignment result = (await GetByIdAsync(assignment.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить назначение
        {
            T15RefereeAssignment? assignment = await dbContext.RefereeAssignments.FindAsync(id);
            if (assignment != null)
            {
                dbContext.RefereeAssignments.Remove(assignment);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.RefereeAssignments.AnyAsync(a => a.Id == id);
            return result;
        }

        public async Task<bool> AssignmentExistsAsync(int matchId, int refereeId, int? excludeId = null) // дубликат назначения
        {
            IQueryable<T15RefereeAssignment> query = dbContext.RefereeAssignments
                .Where(a => a.MatchId == matchId && a.RefereeId == refereeId);
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }
            bool result = await query.AnyAsync();
            return result;
        }
        #endregion
    }
}

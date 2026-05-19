using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий групп турнира — реализует доступ к таблице t11_groups
    public class GroupRepository : IGroupRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public GroupRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T11Group>> GetByTournamentIdAsync(int tournamentId) // группы турнира
        {
            IEnumerable<T11Group> result = await dbContext.Groups
                .Include(g => g.Tournament)
                .Where(g => g.TournamentId == tournamentId)
                .OrderBy(g => g.Name)
                .ToListAsync();
            return result;
        }

        public async Task<T11Group?> GetByIdAsync(int id) // группа по id
        {
            T11Group? result = await dbContext.Groups
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(g => g.Id == id);
            return result;
        }

        public async Task<T11Group> CreateAsync(T11Group group) // создать группу
        {
            dbContext.Groups.Add(group);
            await dbContext.SaveChangesAsync();
            T11Group result = (await GetByIdAsync(group.Id))!;
            return result;
        }

        public async Task<T11Group> UpdateAsync(T11Group group) // обновить группу
        {
            dbContext.Groups.Update(group);
            await dbContext.SaveChangesAsync();
            T11Group result = (await GetByIdAsync(group.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить группу
        {
            T11Group? group = await dbContext.Groups.FindAsync(id);
            if (group != null)
            {
                dbContext.Groups.Remove(group);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Groups.AnyAsync(g => g.Id == id);
            return result;
        }

        public async Task<bool> NameExistsInTournamentAsync(int tournamentId, string name, int? excludeId = null) // уникальность имени в турнире
        {
            IQueryable<T11Group> query = dbContext.Groups
                .Where(g => g.TournamentId == tournamentId && g.Name == name);
            if (excludeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeId.Value);
            }
            bool result = await query.AnyAsync();
            return result;
        }
        #endregion
    }
}

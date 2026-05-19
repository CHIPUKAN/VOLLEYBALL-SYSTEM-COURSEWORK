using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий команд — реализует доступ к таблице t4_teams
    public class TeamRepository : ITeamRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public TeamRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T4Team>> GetAllAsync() // получить все команды с навигационными свойствами
        {
            IEnumerable<T4Team> result = await dbContext.Teams
                .Include(t => t.Region)
                .Include(t => t.HeadCoach)
                .Include(t => t.HomeVenue)
                .OrderBy(t => t.Name)
                .ToListAsync();
            return result;
        }

        public async Task<T4Team?> GetByIdAsync(int id) // получить команду по идентификатору со связанными данными
        {
            T4Team? result = await dbContext.Teams
                .Include(t => t.Region)
                .Include(t => t.HeadCoach)
                .Include(t => t.HomeVenue)
                .FirstOrDefaultAsync(t => t.Id == id);
            return result;
        }

        public async Task<T4Team> CreateAsync(T4Team team) // создать новую команду и сохранить в БД
        {
            dbContext.Teams.Add(team);
            await dbContext.SaveChangesAsync();
            T4Team result = (await GetByIdAsync(team.Id))!;
            return result;
        }

        public async Task<T4Team> UpdateAsync(T4Team team) // обновить существующую команду в БД
        {
            dbContext.Teams.Update(team);
            await dbContext.SaveChangesAsync();
            T4Team result = (await GetByIdAsync(team.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить команду из БД по идентификатору
        {
            T4Team? team = await dbContext.Teams.FindAsync(id);
            if (team != null)
            {
                dbContext.Teams.Remove(team);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить наличие команды в БД
        {
            bool result = await dbContext.Teams.AnyAsync(t => t.Id == id);
            return result;
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null) // проверить уникальность наименования (при исключении текущей записи)
        {
            bool result = await dbContext.Teams
                .AnyAsync(t => t.Name == name && (excludeId == null || t.Id != excludeId));
            return result;
        }
        #endregion
    }
}

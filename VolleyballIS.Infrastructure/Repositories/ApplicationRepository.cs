using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий заявок — реализует доступ к таблице t7_applications и t8_application_composition
    public class ApplicationRepository : IApplicationRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public ApplicationRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T7Application>> GetAllAsync() // все заявки
        {
            IEnumerable<T7Application> result = await dbContext.Applications
                .Include(a => a.Team)
                .Include(a => a.Tournament)
                .Include(a => a.Status)
                .Include(a => a.Composition).ThenInclude(c => c.Player)
                .OrderByDescending(a => a.SubmissionDate)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T7Application>> GetByTournamentIdAsync(int tournamentId) // заявки турнира
        {
            IEnumerable<T7Application> result = await dbContext.Applications
                .Include(a => a.Team)
                .Include(a => a.Status)
                .Include(a => a.Composition).ThenInclude(c => c.Player)
                .Where(a => a.TournamentId == tournamentId)
                .OrderBy(a => a.Team!.Name)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T7Application>> GetByTeamIdAsync(int teamId) // заявки команды
        {
            IEnumerable<T7Application> result = await dbContext.Applications
                .Include(a => a.Tournament)
                .Include(a => a.Status)
                .Include(a => a.Composition).ThenInclude(c => c.Player)
                .Where(a => a.TeamId == teamId)
                .OrderByDescending(a => a.SubmissionDate)
                .ToListAsync();
            return result;
        }

        public async Task<T7Application?> GetByIdAsync(int id) // заявка по id
        {
            T7Application? result = await dbContext.Applications
                .Include(a => a.Team)
                .Include(a => a.Tournament)
                .Include(a => a.Status)
                .Include(a => a.Composition).ThenInclude(c => c.Player)
                .FirstOrDefaultAsync(a => a.Id == id);
            return result;
        }

        public async Task<T7Application> CreateAsync(T7Application application) // создать заявку
        {
            dbContext.Applications.Add(application);
            await dbContext.SaveChangesAsync();
            T7Application result = (await GetByIdAsync(application.Id))!;
            return result;
        }

        public async Task<T7Application> UpdateAsync(T7Application application) // обновить заявку
        {
            dbContext.Applications.Update(application);
            await dbContext.SaveChangesAsync();
            T7Application result = (await GetByIdAsync(application.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить заявку
        {
            T7Application? app = await dbContext.Applications.FindAsync(id);
            if (app != null)
            {
                dbContext.Applications.Remove(app);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Applications.AnyAsync(a => a.Id == id);
            return result;
        }

        public async Task<bool> TeamApplicationExistsAsync(int teamId, int tournamentId, int? excludeId = null) // проверить дубликат
        {
            IQueryable<T7Application> query = dbContext.Applications
                .Where(a => a.TeamId == teamId && a.TournamentId == tournamentId);
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }
            bool result = await query.AnyAsync();
            return result;
        }

        public async Task<T8ApplicationComposition?> GetCompositionPlayerAsync(int applicationId, int playerId) // игрок в составе
        {
            T8ApplicationComposition? result = await dbContext.ApplicationCompositions
                .FirstOrDefaultAsync(c => c.ApplicationId == applicationId && c.PlayerId == playerId);
            return result;
        }

        public async Task<T8ApplicationComposition> AddCompositionPlayerAsync(T8ApplicationComposition comp) // добавить игрока
        {
            dbContext.ApplicationCompositions.Add(comp);
            await dbContext.SaveChangesAsync();
            return comp;
        }

        public async Task RemoveCompositionPlayerAsync(int applicationId, int playerId) // убрать игрока
        {
            T8ApplicationComposition? comp = await dbContext.ApplicationCompositions
                .FirstOrDefaultAsync(c => c.ApplicationId == applicationId && c.PlayerId == playerId);
            if (comp != null)
            {
                dbContext.ApplicationCompositions.Remove(comp);
                await dbContext.SaveChangesAsync();
            }
        }
        #endregion
    }
}

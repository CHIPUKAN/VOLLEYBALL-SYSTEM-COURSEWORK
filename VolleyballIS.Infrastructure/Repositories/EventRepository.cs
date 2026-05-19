using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий игровых событий — реализует доступ к таблице t17_events
    public class EventRepository : IEventRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public EventRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T17Event>> GetByMatchIdAsync(int matchId) // события матча
        {
            IEnumerable<T17Event> result = await dbContext.Events
                .Include(e => e.Team)
                .Include(e => e.EventType)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubOutPlayer)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubInPlayer)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubType)
                .Include(e => e.Timeout).ThenInclude(t => t!.TimeoutType)
                .Where(e => e.MatchId == matchId)
                .OrderBy(e => e.SetNumber).ThenBy(e => e.GlobalSeqInSet)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T17Event>> GetByMatchSetAsync(int matchId, short setNumber) // события партии
        {
            IEnumerable<T17Event> result = await dbContext.Events
                .Include(e => e.Team)
                .Include(e => e.EventType)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubOutPlayer)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubInPlayer)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubType)
                .Include(e => e.Timeout).ThenInclude(t => t!.TimeoutType)
                .Where(e => e.MatchId == matchId && e.SetNumber == setNumber)
                .OrderBy(e => e.GlobalSeqInSet)
                .ToListAsync();
            return result;
        }

        public async Task<T17Event?> GetByIdAsync(int id) // событие по id
        {
            T17Event? result = await dbContext.Events
                .Include(e => e.Team)
                .Include(e => e.EventType)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubOutPlayer)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubInPlayer)
                .Include(e => e.Substitution).ThenInclude(s => s!.SubType)
                .Include(e => e.Timeout).ThenInclude(t => t!.TimeoutType)
                .FirstOrDefaultAsync(e => e.Id == id);
            return result;
        }

        public async Task<T17Event> CreateAsync(T17Event ev) // создать событие
        {
            dbContext.Events.Add(ev);
            await dbContext.SaveChangesAsync();
            T17Event result = (await GetByIdAsync(ev.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить событие
        {
            T17Event? ev = await dbContext.Events.FindAsync(id);
            if (ev != null)
            {
                dbContext.Events.Remove(ev);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Events.AnyAsync(e => e.Id == id);
            return result;
        }
        #endregion
    }
}

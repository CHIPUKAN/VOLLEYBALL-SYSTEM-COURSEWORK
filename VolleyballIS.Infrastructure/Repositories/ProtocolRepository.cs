using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий протоколов — реализует доступ к таблице t16_protocols
    public class ProtocolRepository : IProtocolRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public ProtocolRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T16Protocol>> GetAllAsync() // все протоколы
        {
            IEnumerable<T16Protocol> result = await dbContext.Protocols
                .Include(p => p.Match)
                .Include(p => p.Organizer)
                .Include(p => p.Status)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            return result;
        }

        public async Task<T16Protocol?> GetByMatchIdAsync(int matchId) // протокол матча
        {
            T16Protocol? result = await dbContext.Protocols
                .Include(p => p.Organizer)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.MatchId == matchId);
            return result;
        }

        public async Task<T16Protocol?> GetByIdAsync(int id) // протокол по id
        {
            T16Protocol? result = await dbContext.Protocols
                .Include(p => p.Match)
                .Include(p => p.Organizer)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task<T16Protocol> CreateAsync(T16Protocol protocol) // создать протокол
        {
            dbContext.Protocols.Add(protocol);
            await dbContext.SaveChangesAsync();
            T16Protocol result = (await GetByIdAsync(protocol.Id))!;
            return result;
        }

        public async Task<T16Protocol> UpdateAsync(T16Protocol protocol) // обновить протокол
        {
            dbContext.Protocols.Update(protocol);
            await dbContext.SaveChangesAsync();
            T16Protocol result = (await GetByIdAsync(protocol.Id))!;
            return result;
        }

        public async Task DeleteAsync(int id) // удалить протокол
        {
            T16Protocol? protocol = await dbContext.Protocols.FindAsync(id);
            if (protocol != null)
            {
                dbContext.Protocols.Remove(protocol);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) // проверить существование
        {
            bool result = await dbContext.Protocols.AnyAsync(p => p.Id == id);
            return result;
        }

        public async Task<bool> MatchProtocolExistsAsync(int matchId, int? excludeId = null) // дубликат
        {
            IQueryable<T16Protocol> query = dbContext.Protocols.Where(p => p.MatchId == matchId);
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            bool result = await query.AnyAsync();
            return result;
        }
        #endregion
    }
}

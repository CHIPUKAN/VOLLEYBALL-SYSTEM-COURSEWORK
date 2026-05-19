using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.Infrastructure.Repositories
{
    // Репозиторий журнала изменений протоколов — реализует доступ к таблице t16_protocol_history
    public class ProtocolHistoryRepository : IProtocolHistoryRepository
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public ProtocolHistoryRepository(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<T16ProtocolHistory>> GetByProtocolIdAsync(int protocolId) // записи журнала по протоколу
        {
            IEnumerable<T16ProtocolHistory> result = await dbContext.ProtocolHistory
                .Where(h => h.ProtocolId == protocolId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
            return result;
        }

        public async Task<T16ProtocolHistory?> GetByIdAsync(int id) // запись журнала по id
        {
            T16ProtocolHistory? result = await dbContext.ProtocolHistory
                .FirstOrDefaultAsync(h => h.Id == id);
            return result;
        }

        public async Task<T16ProtocolHistory> CreateAsync(T16ProtocolHistory entry) // создать запись журнала
        {
            dbContext.ProtocolHistory.Add(entry);
            await dbContext.SaveChangesAsync();
            T16ProtocolHistory result = (await GetByIdAsync(entry.Id))!;
            return result;
        }
        #endregion
    }
}

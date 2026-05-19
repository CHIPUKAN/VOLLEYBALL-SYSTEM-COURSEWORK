using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория журнала изменений протоколов
    public interface IProtocolHistoryRepository
    {
        Task<IEnumerable<T16ProtocolHistory>> GetByProtocolIdAsync(int protocolId); // записи по протоколу
        Task<T16ProtocolHistory?> GetByIdAsync(int id);                              // запись по id
        Task<T16ProtocolHistory> CreateAsync(T16ProtocolHistory entry);              // создать запись
    }
}

using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория протоколов матчей
    public interface IProtocolRepository
    {
        Task<IEnumerable<T16Protocol>> GetAllAsync();                   // все протоколы
        Task<T16Protocol?> GetByMatchIdAsync(int matchId);             // протокол по матчу
        Task<T16Protocol?> GetByIdAsync(int id);                       // протокол по id
        Task<T16Protocol> CreateAsync(T16Protocol protocol);           // создать протокол
        Task<T16Protocol> UpdateAsync(T16Protocol protocol);           // обновить протокол
        Task DeleteAsync(int id);                                       // удалить протокол
        Task<bool> ExistsAsync(int id);                                // проверить существование
        Task<bool> MatchProtocolExistsAsync(int matchId, int? excludeId = null); // проверить дубликат
    }
}

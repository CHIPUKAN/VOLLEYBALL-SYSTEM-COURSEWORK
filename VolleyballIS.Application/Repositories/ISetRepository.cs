using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория партий матча
    public interface ISetRepository
    {
        Task<IEnumerable<R3Set>> GetByMatchIdAsync(int matchId);            // партии матча
        Task<R3Set?> GetByKeyAsync(int matchId, short setNumber);           // партия по составному ключу
        Task<R3Set> UpsertAsync(R3Set set);                                 // создать или обновить партию
        Task DeleteAsync(int matchId, short setNumber);                     // удалить партию
        Task<bool> ExistsAsync(int matchId, short setNumber);               // проверить существование
    }
}

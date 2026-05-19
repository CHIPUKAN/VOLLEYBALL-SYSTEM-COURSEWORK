using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория делегаций команд на матч
    public interface IDelegationRepository
    {
        Task<IEnumerable<T5Delegation>> GetByMatchIdAsync(int matchId);                     // делегация матча
        Task<IEnumerable<T5Delegation>> GetByMatchTeamAsync(int matchId, int teamId);       // делегация команды
        Task<T5Delegation?> GetByIdAsync(int id);                                           // участник по id
        Task<T5Delegation> CreateAsync(T5Delegation delegation);                            // добавить участника
        Task<T5Delegation> UpdateAsync(T5Delegation delegation);                            // обновить
        Task DeleteAsync(int id);                                                            // удалить
        Task<bool> ExistsAsync(int id);                                                     // проверить существование
    }
}

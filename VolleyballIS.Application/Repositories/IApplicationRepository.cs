using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория заявок команд на турниры
    public interface IApplicationRepository
    {
        Task<IEnumerable<T7Application>> GetAllAsync();                                         // все заявки
        Task<IEnumerable<T7Application>> GetByTournamentIdAsync(int tournamentId);              // заявки на турнир
        Task<IEnumerable<T7Application>> GetByTeamIdAsync(int teamId);                         // заявки команды
        Task<T7Application?> GetByIdAsync(int id);                                             // заявка по id
        Task<T7Application> CreateAsync(T7Application application);                            // создать заявку
        Task<T7Application> UpdateAsync(T7Application application);                            // обновить заявку
        Task DeleteAsync(int id);                                                               // удалить заявку
        Task<bool> ExistsAsync(int id);                                                        // проверить существование
        Task<bool> TeamApplicationExistsAsync(int teamId, int tournamentId, int? excludeId = null); // проверить дубликат заявки
        Task<T8ApplicationComposition?> GetCompositionPlayerAsync(int applicationId, int playerId); // игрок в составе
        Task<T8ApplicationComposition> AddCompositionPlayerAsync(T8ApplicationComposition comp); // добавить игрока
        Task RemoveCompositionPlayerAsync(int applicationId, int playerId);                    // убрать игрока
    }
}

using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория групп турнира
    public interface IGroupRepository
    {
        Task<IEnumerable<T11Group>> GetByTournamentIdAsync(int tournamentId); // получить группы турнира
        Task<T11Group?> GetByIdAsync(int id);                                 // получить группу по id
        Task<T11Group> CreateAsync(T11Group group);                           // создать группу
        Task<T11Group> UpdateAsync(T11Group group);                           // обновить группу
        Task DeleteAsync(int id);                                              // удалить группу
        Task<bool> ExistsAsync(int id);                                       // проверить существование
        Task<bool> NameExistsInTournamentAsync(int tournamentId, string name, int? excludeId = null); // проверить уникальность имени
    }
}

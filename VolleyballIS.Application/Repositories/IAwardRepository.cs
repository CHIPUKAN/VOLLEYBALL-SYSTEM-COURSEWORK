using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория наград турнира
    public interface IAwardRepository
    {
        Task<IEnumerable<T20Award>> GetByTournamentIdAsync(int tournamentId); // награды турнира
        Task<T20Award?> GetByIdAsync(int id);                                 // награда по id
        Task<T20Award> CreateAsync(T20Award award);                           // создать награду
        Task<T20Award> UpdateAsync(T20Award award);                           // обновить
        Task DeleteAsync(int id);                                              // удалить
        Task<bool> ExistsAsync(int id);                                       // проверить существование
    }
}

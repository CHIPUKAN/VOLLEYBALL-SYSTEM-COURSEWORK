using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория статистики игроков
    public interface IPlayerStatsRepository
    {
        Task<IEnumerable<R2PlayerStats>> GetByMatchIdAsync(int matchId);         // статистика матча
        Task<R2PlayerStats?> GetByKeyAsync(int matchId, int playerId);           // статистика по ключу
        Task<R2PlayerStats> UpsertAsync(R2PlayerStats stats);                    // создать или обновить
        Task DeleteAsync(int matchId, int playerId);                              // удалить запись
        Task<bool> ExistsAsync(int matchId, int playerId);                       // проверить существование
    }
}

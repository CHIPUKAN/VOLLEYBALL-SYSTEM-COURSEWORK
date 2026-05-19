using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория капитанов команд в матчах
    public interface IMatchCaptainRepository
    {
        Task<IEnumerable<T21MatchCaptain>> GetByMatchIdAsync(int matchId);           // капитаны матча
        Task<T21MatchCaptain?> GetByKeyAsync(int matchId, int teamId);               // капитан по ключу
        Task<T21MatchCaptain> UpsertAsync(T21MatchCaptain captain);                  // назначить/обновить
        Task DeleteAsync(int matchId, int teamId);                                    // снять капитана
        Task<bool> ExistsAsync(int matchId, int teamId);                             // проверить существование
    }
}

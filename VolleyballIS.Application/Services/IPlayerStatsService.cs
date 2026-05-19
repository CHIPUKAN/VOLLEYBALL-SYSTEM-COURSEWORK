using VolleyballIS.Application.DTOs.PlayerStats;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления статистикой игроков
    public interface IPlayerStatsService
    {
        Task<IEnumerable<PlayerStatsDto>> GetStatsByMatchAsync(int matchId);
        Task<PlayerStatsDto?> GetStatsAsync(int matchId, int playerId);
        Task<PlayerStatsDto> UpsertStatsAsync(int matchId, UpsertPlayerStatsDto dto);
        Task DeleteStatsAsync(int matchId, int playerId);
    }
}

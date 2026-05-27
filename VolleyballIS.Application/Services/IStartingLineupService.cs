using VolleyballIS.Application.DTOs.StartingLineups;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления стартовыми расстановками
    public interface IStartingLineupService
    {
        Task<IEnumerable<StartingLineupDto>> GetLineupsByMatchAsync(int matchId);
        Task<IEnumerable<StartingLineupDto>> GetLineupsByMatchTeamSetAsync(int matchId, int teamId, short setNumber);
        Task<StartingLineupDto> UpsertPositionAsync(int matchId, UpsertStartingLineupDto dto);
        Task DeleteLineupAsync(int matchId, int teamId, short setNumber);
        Task DeletePositionAsync(int matchId, int teamId, short setNumber, short positionNo);
    }
}

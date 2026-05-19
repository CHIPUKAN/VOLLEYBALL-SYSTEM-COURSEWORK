using VolleyballIS.Application.DTOs.MatchCaptains;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления капитанами команд в матчах
    public interface IMatchCaptainService
    {
        Task<IEnumerable<MatchCaptainDto>> GetCaptainsByMatchAsync(int matchId);
        Task<MatchCaptainDto?> GetCaptainAsync(int matchId, int teamId);
        Task<MatchCaptainDto> UpsertCaptainAsync(int matchId, UpsertMatchCaptainDto dto);
        Task DeleteCaptainAsync(int matchId, int teamId);
    }
}

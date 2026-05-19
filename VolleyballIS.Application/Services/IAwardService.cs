using VolleyballIS.Application.DTOs.Awards;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления наградами турниров
    public interface IAwardService
    {
        Task<IEnumerable<AwardDto>> GetAwardsByTournamentAsync(int tournamentId);
        Task<AwardDto?> GetAwardByIdAsync(int id);
        Task<AwardDto> CreateAwardAsync(CreateAwardDto dto);
        Task<AwardDto> UpdateAwardAsync(int id, UpdateAwardDto dto);
        Task DeleteAwardAsync(int id);
    }
}

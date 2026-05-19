using VolleyballIS.Application.DTOs.Applications;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления заявками команд
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync();
        Task<IEnumerable<ApplicationDto>> GetApplicationsByTournamentAsync(int tournamentId);
        Task<IEnumerable<ApplicationDto>> GetApplicationsByTeamAsync(int teamId);
        Task<ApplicationDto?> GetApplicationByIdAsync(int id);
        Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto);
        Task<ApplicationDto> UpdateApplicationAsync(int id, UpdateApplicationDto dto);
        Task DeleteApplicationAsync(int id);
        Task<ApplicationDto> AddPlayerToApplicationAsync(int applicationId, AddCompositionPlayerDto dto);
        Task RemovePlayerFromApplicationAsync(int applicationId, int playerId);
    }
}

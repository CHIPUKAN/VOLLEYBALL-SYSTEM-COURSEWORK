using VolleyballIS.Application.DTOs.Groups;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления группами турнира
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetGroupsByTournamentAsync(int tournamentId);
        Task<GroupDto?> GetGroupByIdAsync(int id);
        Task<GroupDto> CreateGroupAsync(CreateGroupDto dto);
        Task<GroupDto> UpdateGroupAsync(int id, UpdateGroupDto dto);
        Task DeleteGroupAsync(int id);
    }
}

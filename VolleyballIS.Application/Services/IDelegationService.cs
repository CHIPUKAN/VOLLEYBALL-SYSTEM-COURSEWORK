using VolleyballIS.Application.DTOs.Delegations;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления делегациями команд
    public interface IDelegationService
    {
        Task<IEnumerable<DelegationDto>> GetDelegationByMatchAsync(int matchId);
        Task<IEnumerable<DelegationDto>> GetDelegationByMatchTeamAsync(int matchId, int teamId);
        Task<DelegationDto?> GetMemberByIdAsync(int id);
        Task<DelegationDto> CreateMemberAsync(CreateDelegationDto dto);
        Task<DelegationDto> UpdateMemberAsync(int id, UpdateDelegationDto dto);
        Task DeleteMemberAsync(int id);
    }
}

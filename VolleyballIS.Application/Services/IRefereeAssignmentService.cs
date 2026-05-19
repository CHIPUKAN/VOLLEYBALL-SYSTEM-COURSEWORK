using VolleyballIS.Application.DTOs.RefereeAssignments;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления судейскими бригадами
    public interface IRefereeAssignmentService
    {
        Task<IEnumerable<RefereeAssignmentDto>> GetAssignmentsByMatchAsync(int matchId);
        Task<RefereeAssignmentDto?> GetAssignmentByIdAsync(int id);
        Task<RefereeAssignmentDto> CreateAssignmentAsync(CreateRefereeAssignmentDto dto);
        Task<RefereeAssignmentDto> UpdateAssignmentAsync(int id, UpdateRefereeAssignmentDto dto);
        Task DeleteAssignmentAsync(int id);
    }
}

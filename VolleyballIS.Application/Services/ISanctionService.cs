using VolleyballIS.Application.DTOs.Sanctions;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления санкциями
    public interface ISanctionService
    {
        Task<IEnumerable<SanctionDto>> GetSanctionsByMatchAsync(int matchId);
        Task<SanctionDto?> GetSanctionByIdAsync(int id);
        Task<SanctionDto> CreateSanctionAsync(CreateSanctionDto dto);
        Task<SanctionDto> UpdateSanctionAsync(int id, UpdateSanctionDto dto);
        Task DeleteSanctionAsync(int id);
    }
}

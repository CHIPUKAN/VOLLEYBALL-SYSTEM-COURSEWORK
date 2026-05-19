using VolleyballIS.Application.DTOs.Protocols;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления протоколами матчей
    public interface IProtocolService
    {
        Task<IEnumerable<ProtocolDto>> GetAllProtocolsAsync();
        Task<ProtocolDto?> GetProtocolByMatchAsync(int matchId);
        Task<ProtocolDto?> GetProtocolByIdAsync(int id);
        Task<ProtocolDto> CreateProtocolAsync(CreateProtocolDto dto);
        Task<ProtocolDto> UpdateProtocolAsync(int id, UpdateProtocolDto dto);
        Task DeleteProtocolAsync(int id);
    }
}

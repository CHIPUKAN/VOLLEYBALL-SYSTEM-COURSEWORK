using VolleyballIS.Application.DTOs.ProtocolHistory;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления журналом изменений протоколов
    public interface IProtocolHistoryService
    {
        Task<IEnumerable<ProtocolHistoryDto>> GetByProtocolIdAsync(int protocolId); // записи журнала по протоколу
        Task<ProtocolHistoryDto?> GetByIdAsync(int id);                              // запись по id
        Task<ProtocolHistoryDto> CreateEntryAsync(CreateProtocolHistoryDto dto);     // добавить запись вручную
    }
}

using VolleyballIS.Application.DTOs.ProtocolHistory;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления журналом изменений протоколов матчей
    public class ProtocolHistoryService : IProtocolHistoryService
    {
        #region Поля
        private readonly IProtocolHistoryRepository historyRepository; // репозиторий журнала
        #endregion

        #region Конструкторы
        public ProtocolHistoryService(IProtocolHistoryRepository historyRepository) // конструктор с внедрением зависимости
        {
            this.historyRepository = historyRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<ProtocolHistoryDto>> GetByProtocolIdAsync(int protocolId) // записи журнала протокола
        {
            IEnumerable<T16ProtocolHistory> entries = await historyRepository.GetByProtocolIdAsync(protocolId);
            IEnumerable<ProtocolHistoryDto> result = entries.Select(MapToDto);
            return result;
        }

        public async Task<ProtocolHistoryDto?> GetByIdAsync(int id) // запись журнала по id
        {
            T16ProtocolHistory? entry = await historyRepository.GetByIdAsync(id);
            ProtocolHistoryDto? result = entry == null ? null : MapToDto(entry);
            return result;
        }

        public async Task<ProtocolHistoryDto> CreateEntryAsync(CreateProtocolHistoryDto dto) // добавить запись вручную
        {
            T16ProtocolHistory entry = new T16ProtocolHistory
            {
                ProtocolId = dto.ProtocolId,
                ChangedAt = DateTime.UtcNow,
                StatusAtMoment = dto.StatusAtMoment,
                DataHash = dto.DataHash,
                Comment = dto.Comment
            };
            T16ProtocolHistory created = await historyRepository.CreateAsync(entry);
            ProtocolHistoryDto result = MapToDto(created);
            return result;
        }

        private static ProtocolHistoryDto MapToDto(T16ProtocolHistory h) // маппинг T16ProtocolHistory -> ProtocolHistoryDto
        {
            ProtocolHistoryDto result = new ProtocolHistoryDto
            {
                Id = h.Id,
                ProtocolId = h.ProtocolId,
                ChangedAt = h.ChangedAt,
                StatusAtMoment = h.StatusAtMoment,
                DataHash = h.DataHash,
                Comment = h.Comment
            };
            return result;
        }
        #endregion
    }
}

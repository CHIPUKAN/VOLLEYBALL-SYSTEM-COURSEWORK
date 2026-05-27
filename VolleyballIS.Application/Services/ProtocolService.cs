using VolleyballIS.Application.DTOs.Protocols;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления протоколами матчей
    public class ProtocolService : IProtocolService
    {
        #region Поля
        private readonly IProtocolRepository protocolRepository;    // репозиторий протоколов
        private readonly IProtocolHistoryRepository historyRepository; // репозиторий журнала изменений
        private readonly IMatchRepository matchRepository;          // репозиторий матчей (для проверки статуса)

        // коды статусов матча, при которых разрешено создание протокола
        private static readonly short[] MatchStatusesAllowProtocol = [2, 3, 6]; // В процессе, Завершён, Техническое поражение
        private const short ProtocolApprovedCode = 3; // код статуса протокола «Утверждён»
        #endregion

        #region Конструкторы
        public ProtocolService(
            IProtocolRepository protocolRepository,
            IProtocolHistoryRepository historyRepository,
            IMatchRepository matchRepository) // конструктор с внедрением зависимостей
        {
            this.protocolRepository = protocolRepository;
            this.historyRepository = historyRepository;
            this.matchRepository = matchRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<ProtocolDto>> GetAllProtocolsAsync() // все протоколы
        {
            IEnumerable<T16Protocol> protocols = await protocolRepository.GetAllAsync();
            IEnumerable<ProtocolDto> result = protocols.Select(MapToDto);
            return result;
        }

        public async Task<ProtocolDto?> GetProtocolByMatchAsync(int matchId) // протокол матча
        {
            T16Protocol? protocol = await protocolRepository.GetByMatchIdAsync(matchId);
            ProtocolDto? result = protocol == null ? null : MapToDto(protocol);
            return result;
        }

        public async Task<ProtocolDto?> GetProtocolByIdAsync(int id) // протокол по id
        {
            T16Protocol? protocol = await protocolRepository.GetByIdAsync(id);
            ProtocolDto? result = protocol == null ? null : MapToDto(protocol);
            return result;
        }

        public async Task<ProtocolDto> CreateProtocolAsync(CreateProtocolDto dto) // создать протокол
        {
            bool exists = await protocolRepository.MatchProtocolExistsAsync(dto.MatchId);
            if (exists)
            {
                throw new InvalidOperationException("Протокол для данного матча уже создан");
            }

            T14Match? match = await matchRepository.GetByIdAsync(dto.MatchId);
            if (match == null)
            {
                throw new KeyNotFoundException($"Матч с идентификатором {dto.MatchId} не найден");
            }
            if (!MatchStatusesAllowProtocol.Contains(match.StatusCode))
            {
                throw new InvalidOperationException(
                    "Протокол можно создать только для матча в статусе «В процессе», «Завершён» или «Техническое поражение»");
            }

            // утверждённый протокол должен иметь дату утверждения
            if (dto.StatusCode == ProtocolApprovedCode && !dto.ApprovalDate.HasValue)
            {
                throw new InvalidOperationException("Для статуса «Утверждён» требуется дата утверждения");
            }

            T16Protocol protocol = new T16Protocol
            {
                MatchId = dto.MatchId,
                OrganizerId = dto.OrganizerId,
                ApprovalDate = dto.ApprovalDate,
                StatusCode = dto.StatusCode
            };
            T16Protocol created = await protocolRepository.CreateAsync(protocol);
            ProtocolDto result = MapToDto(created);
            return result;
        }

        public async Task<ProtocolDto> UpdateProtocolAsync(int id, UpdateProtocolDto dto) // обновить протокол
        {
            T16Protocol? existing = await protocolRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Протокол с идентификатором {id} не найден");
            }

            // при переходе в статус «Утверждён» должна быть дата утверждения
            if (dto.StatusCode == ProtocolApprovedCode && !dto.ApprovalDate.HasValue && !existing.ApprovalDate.HasValue)
            {
                throw new InvalidOperationException("Для статуса «Утверждён» требуется дата утверждения");
            }

            short previousStatus = existing.StatusCode;
            existing.StatusCode = dto.StatusCode;
            // обновляем OrganizerId и ApprovalDate только если они явно переданы, не затирая существующие значения
            if (dto.OrganizerId.HasValue) existing.OrganizerId = dto.OrganizerId;
            if (dto.ApprovalDate.HasValue) existing.ApprovalDate = dto.ApprovalDate;

            T16Protocol updated = await protocolRepository.UpdateAsync(existing);

            if (updated.StatusCode != previousStatus)
            {
                T16ProtocolHistory historyEntry = new T16ProtocolHistory
                {
                    ProtocolId = updated.Id,
                    ChangedAt = DateTime.UtcNow,
                    StatusAtMoment = updated.StatusCode,
                    Comment = $"Статус изменён: {previousStatus} -> {updated.StatusCode}"
                };
                await historyRepository.CreateAsync(historyEntry);
            }

            ProtocolDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteProtocolAsync(int id) // удалить протокол
        {
            bool exists = await protocolRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Протокол с идентификатором {id} не найден");
            }
            await protocolRepository.DeleteAsync(id);
        }

        private static ProtocolDto MapToDto(T16Protocol p) // маппинг T16Protocol -> ProtocolDto
        {
            ProtocolDto result = new ProtocolDto
            {
                Id = p.Id,
                MatchId = p.MatchId,
                OrganizerId = p.OrganizerId,
                OrganizerFullName = p.Organizer != null
                    ? (p.Organizer.MiddleName != null
                        ? $"{p.Organizer.LastName} {p.Organizer.FirstName} {p.Organizer.MiddleName}"
                        : $"{p.Organizer.LastName} {p.Organizer.FirstName}")
                    : null,
                ApprovalDate = p.ApprovalDate,
                StatusCode = p.StatusCode,
                StatusName = p.Status?.Name
            };
            return result;
        }
        #endregion
    }
}

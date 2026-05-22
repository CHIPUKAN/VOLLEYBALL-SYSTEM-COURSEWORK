using VolleyballIS.Application.DTOs.Sanctions;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления санкциями (карточками) в матчах
    public class SanctionService : ISanctionService
    {
        #region Поля
        private readonly ISanctionRepository sanctionRepository; // репозиторий санкций
        #endregion

        #region Конструкторы
        public SanctionService(ISanctionRepository sanctionRepository) // конструктор с внедрением зависимости
        {
            this.sanctionRepository = sanctionRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<SanctionDto>> GetSanctionsByMatchAsync(int matchId) // санкции матча
        {
            IEnumerable<T18Sanction> sanctions = await sanctionRepository.GetByMatchIdAsync(matchId);
            IEnumerable<SanctionDto> result = sanctions.Select(MapToDto);
            return result;
        }

        public async Task<SanctionDto?> GetSanctionByIdAsync(int id) // санкция по id
        {
            T18Sanction? sanction = await sanctionRepository.GetByIdAsync(id);
            SanctionDto? result = sanction == null ? null : MapToDto(sanction);
            return result;
        }

        public async Task<SanctionDto> CreateSanctionAsync(CreateSanctionDto dto) // зарегистрировать санкцию
        {
            if (dto.RecipientTypeCode == 1 && dto.PlayerId == null)
            {
                throw new InvalidOperationException("Для получателя «Игрок» требуется указать игрока");
            }
            if (dto.RecipientTypeCode != 1 && dto.DelegationMemberId == null)
            {
                throw new InvalidOperationException("Для нечлена-команды требуется указать члена делегации");
            }
            if (dto.SanctionKindCode == 2 && dto.DelayViolationCode == null)
            {
                throw new InvalidOperationException("Для нарушения задержки игры требуется указать код нарушения");
            }

            T18Sanction sanction = new T18Sanction
            {
                MatchId = dto.MatchId,
                TeamId = dto.TeamId,
                PlayerId = dto.PlayerId,
                DelegationMemberId = dto.DelegationMemberId,
                RecipientTypeCode = dto.RecipientTypeCode,
                SanctionTypeCode = dto.SanctionTypeCode,
                SanctionKindCode = dto.SanctionKindCode,
                DelayViolationCode = dto.DelayViolationCode,
                SetNumber = dto.SetNumber,
                MemberSeqInMatch = dto.MemberSeqInMatch,
                HomeScoreAtMoment = dto.HomeScoreAtMoment,
                GuestScoreAtMoment = dto.GuestScoreAtMoment,
                MinuteMark = dto.MinuteMark,
                EventId = dto.EventId
            };
            T18Sanction created = await sanctionRepository.CreateAsync(sanction);
            SanctionDto result = MapToDto(created);
            return result;
        }

        public async Task<SanctionDto> UpdateSanctionAsync(int id, UpdateSanctionDto dto) // обновить санкцию
        {
            T18Sanction? existing = await sanctionRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Санкция с идентификатором {id} не найдена");
            }
            existing.TeamId = dto.TeamId;
            existing.PlayerId = dto.PlayerId;
            existing.DelegationMemberId = dto.DelegationMemberId;
            existing.RecipientTypeCode = dto.RecipientTypeCode;
            existing.SanctionTypeCode = dto.SanctionTypeCode;
            existing.SanctionKindCode = dto.SanctionKindCode;
            existing.DelayViolationCode = dto.DelayViolationCode;
            existing.SetNumber = dto.SetNumber;
            existing.MemberSeqInMatch = dto.MemberSeqInMatch;
            existing.HomeScoreAtMoment = dto.HomeScoreAtMoment;
            existing.GuestScoreAtMoment = dto.GuestScoreAtMoment;
            existing.MinuteMark = dto.MinuteMark;
            existing.EventId = dto.EventId;
            T18Sanction updated = await sanctionRepository.UpdateAsync(existing);
            SanctionDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteSanctionAsync(int id) // удалить санкцию
        {
            bool exists = await sanctionRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Санкция с идентификатором {id} не найдена");
            }
            await sanctionRepository.DeleteAsync(id);
        }

        private static SanctionDto MapToDto(T18Sanction s) // маппинг T18Sanction -> SanctionDto
        {
            string? playerFio = s.Player != null
                ? (s.Player.MiddleName != null
                    ? $"{s.Player.LastName} {s.Player.FirstName} {s.Player.MiddleName}"
                    : $"{s.Player.LastName} {s.Player.FirstName}")
                : null;

            SanctionDto result = new SanctionDto
            {
                Id = s.Id,
                MatchId = s.MatchId,
                TeamId = s.TeamId,
                TeamName = s.Team?.Name,
                PlayerId = s.PlayerId,
                PlayerFullName = playerFio,
                DelegationMemberId = s.DelegationMemberId,
                RecipientTypeCode = s.RecipientTypeCode,
                RecipientTypeName = s.RecipientType?.Name,
                SanctionTypeCode = s.SanctionTypeCode,
                SanctionTypeName = s.SanctionType?.Name,
                SanctionKindCode = s.SanctionKindCode,
                SanctionKindName = s.SanctionKind?.Name,
                DelayViolationCode = s.DelayViolationCode,
                DelayViolationName = s.DelayViolation?.Name,
                SetNumber = s.SetNumber,
                MemberSeqInMatch = s.MemberSeqInMatch,
                HomeScoreAtMoment = s.HomeScoreAtMoment,
                GuestScoreAtMoment = s.GuestScoreAtMoment,
                MinuteMark = s.MinuteMark,
                EventId = s.EventId
            };
            return result;
        }
        #endregion
    }
}

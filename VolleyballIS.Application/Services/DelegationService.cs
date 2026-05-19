using VolleyballIS.Application.DTOs.Delegations;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления делегациями команд на матчи
    public class DelegationService : IDelegationService
    {
        #region Поля
        private readonly IDelegationRepository delegationRepository; // репозиторий делегаций
        #endregion

        #region Конструкторы
        public DelegationService(IDelegationRepository delegationRepository) // конструктор с внедрением зависимости
        {
            this.delegationRepository = delegationRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<DelegationDto>> GetDelegationByMatchAsync(int matchId) // делегация матча
        {
            IEnumerable<T5Delegation> members = await delegationRepository.GetByMatchIdAsync(matchId);
            IEnumerable<DelegationDto> result = members.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<DelegationDto>> GetDelegationByMatchTeamAsync(int matchId, int teamId) // делегация команды
        {
            IEnumerable<T5Delegation> members = await delegationRepository.GetByMatchTeamAsync(matchId, teamId);
            IEnumerable<DelegationDto> result = members.Select(MapToDto);
            return result;
        }

        public async Task<DelegationDto?> GetMemberByIdAsync(int id) // участник по id
        {
            T5Delegation? member = await delegationRepository.GetByIdAsync(id);
            DelegationDto? result = member == null ? null : MapToDto(member);
            return result;
        }

        public async Task<DelegationDto> CreateMemberAsync(CreateDelegationDto dto) // добавить участника
        {
            T5Delegation member = new T5Delegation
            {
                MatchId = dto.MatchId,
                TeamId = dto.TeamId,
                RoleType = dto.RoleType,
                AssistantSeqNo = dto.AssistantSeqNo,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName
            };
            T5Delegation created = await delegationRepository.CreateAsync(member);
            DelegationDto result = MapToDto(created);
            return result;
        }

        public async Task<DelegationDto> UpdateMemberAsync(int id, UpdateDelegationDto dto) // обновить участника
        {
            T5Delegation? existing = await delegationRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Участник делегации с идентификатором {id} не найден");
            }

            existing.RoleType = dto.RoleType;
            existing.AssistantSeqNo = dto.AssistantSeqNo;
            existing.LastName = dto.LastName;
            existing.FirstName = dto.FirstName;
            existing.MiddleName = dto.MiddleName;
            T5Delegation updated = await delegationRepository.UpdateAsync(existing);
            DelegationDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteMemberAsync(int id) // удалить участника
        {
            bool exists = await delegationRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Участник делегации с идентификатором {id} не найден");
            }
            await delegationRepository.DeleteAsync(id);
        }

        private static DelegationDto MapToDto(T5Delegation d) // маппинг T5Delegation -> DelegationDto
        {
            DelegationDto result = new DelegationDto
            {
                Id = d.Id,
                MatchId = d.MatchId,
                TeamId = d.TeamId,
                TeamName = d.Team?.Name,
                RoleType = d.RoleType,
                AssistantSeqNo = d.AssistantSeqNo,
                LastName = d.LastName,
                FirstName = d.FirstName,
                MiddleName = d.MiddleName,
                FullName = d.FullName()
            };
            return result;
        }
        #endregion
    }
}

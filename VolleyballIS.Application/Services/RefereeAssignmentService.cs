using VolleyballIS.Application.DTOs.RefereeAssignments;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления судейскими бригадами матчей
    public class RefereeAssignmentService : IRefereeAssignmentService
    {
        #region Поля
        private readonly IRefereeAssignmentRepository assignmentRepository; // репозиторий назначений

        // роли, допускающие только одно назначение на матч (главный, второй судья, секретарь)
        private static readonly short[] SingletonRoles = [1, 2, 3];
        #endregion

        #region Конструкторы
        public RefereeAssignmentService(IRefereeAssignmentRepository assignmentRepository) // конструктор с внедрением зависимости
        {
            this.assignmentRepository = assignmentRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<RefereeAssignmentDto>> GetAssignmentsByMatchAsync(int matchId) // бригада матча
        {
            IEnumerable<T15RefereeAssignment> assignments = await assignmentRepository.GetByMatchIdAsync(matchId);
            IEnumerable<RefereeAssignmentDto> result = assignments.Select(MapToDto);
            return result;
        }

        public async Task<RefereeAssignmentDto?> GetAssignmentByIdAsync(int id) // назначение по id
        {
            T15RefereeAssignment? assignment = await assignmentRepository.GetByIdAsync(id);
            RefereeAssignmentDto? result = assignment == null ? null : MapToDto(assignment);
            return result;
        }

        public async Task<RefereeAssignmentDto> CreateAssignmentAsync(CreateRefereeAssignmentDto dto) // назначить судью
        {
            bool duplicate = await assignmentRepository.AssignmentExistsAsync(dto.MatchId, dto.RefereeId);
            if (duplicate)
            {
                throw new InvalidOperationException("Данный судья уже назначен на этот матч");
            }

            if (SingletonRoles.Contains(dto.RoleCode))
            {
                bool roleTaken = await assignmentRepository.RoleExistsForMatchAsync(dto.MatchId, dto.RoleCode);
                if (roleTaken)
                {
                    throw new InvalidOperationException(
                        $"Роль с кодом {dto.RoleCode} уже назначена на этот матч. Допускается только один судья в данной роли");
                }
            }

            T15RefereeAssignment assignment = new T15RefereeAssignment
            {
                MatchId = dto.MatchId,
                RefereeId = dto.RefereeId,
                RoleCode = dto.RoleCode,
                LineJudgeSeqNo = dto.LineJudgeSeqNo
            };
            T15RefereeAssignment created = await assignmentRepository.CreateAsync(assignment);
            RefereeAssignmentDto result = MapToDto(created);
            return result;
        }

        public async Task<RefereeAssignmentDto> UpdateAssignmentAsync(int id, UpdateRefereeAssignmentDto dto) // обновить назначение
        {
            T15RefereeAssignment? existing = await assignmentRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Назначение с идентификатором {id} не найдено");
            }

            if (SingletonRoles.Contains(dto.RoleCode) && dto.RoleCode != existing.RoleCode)
            {
                bool roleTaken = await assignmentRepository.RoleExistsForMatchAsync(existing.MatchId, dto.RoleCode, id);
                if (roleTaken)
                {
                    throw new InvalidOperationException(
                        $"Роль с кодом {dto.RoleCode} уже назначена на этот матч. Допускается только один судья в данной роли");
                }
            }

            existing.RoleCode = dto.RoleCode;
            existing.LineJudgeSeqNo = dto.LineJudgeSeqNo;
            T15RefereeAssignment updated = await assignmentRepository.UpdateAsync(existing);
            RefereeAssignmentDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteAssignmentAsync(int id) // снять назначение
        {
            bool exists = await assignmentRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Назначение с идентификатором {id} не найдено");
            }
            await assignmentRepository.DeleteAsync(id);
        }

        private static RefereeAssignmentDto MapToDto(T15RefereeAssignment a) // маппинг T15RefereeAssignment -> DTO
        {
            RefereeAssignmentDto result = new RefereeAssignmentDto
            {
                Id = a.Id,
                MatchId = a.MatchId,
                RefereeId = a.RefereeId,
                RefereeFullName = a.Referee != null
                    ? (a.Referee.MiddleName != null
                        ? $"{a.Referee.LastName} {a.Referee.FirstName} {a.Referee.MiddleName}"
                        : $"{a.Referee.LastName} {a.Referee.FirstName}")
                    : null,
                RoleCode = a.RoleCode,
                RoleName = a.Role?.Name,
                LineJudgeSeqNo = a.LineJudgeSeqNo
            };
            return result;
        }
        #endregion
    }
}

using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория назначений судей на матчи
    public interface IRefereeAssignmentRepository
    {
        Task<IEnumerable<T15RefereeAssignment>> GetByMatchIdAsync(int matchId); // бригада матча
        Task<T15RefereeAssignment?> GetByIdAsync(int id);                       // назначение по id
        Task<T15RefereeAssignment> CreateAsync(T15RefereeAssignment assignment); // создать назначение
        Task<T15RefereeAssignment> UpdateAsync(T15RefereeAssignment assignment); // обновить
        Task DeleteAsync(int id);                                                // удалить
        Task<bool> ExistsAsync(int id);                                         // проверить существование
        Task<bool> AssignmentExistsAsync(int matchId, int refereeId, int? excludeId = null); // дубликат назначения
        Task<bool> RoleExistsForMatchAsync(int matchId, short roleCode, int? excludeId = null); // уникальность роли в матче
    }
}

using VolleyballIS.Application.DTOs.Referees;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления судьями
    public interface IRefereeService
    {
        #region Методы
        Task<IEnumerable<RefereeDto>> GetAllRefereesAsync(); // получить всех судей

        Task<RefereeDto?> GetRefereeByIdAsync(int id); // получить судью по идентификатору

        Task<RefereeDto> CreateRefereeAsync(CreateRefereeDto dto); // создать судью

        Task<RefereeDto> UpdateRefereeAsync(int id, UpdateRefereeDto dto); // обновить судью

        Task DeleteRefereeAsync(int id); // удалить судью
        #endregion
    }
}

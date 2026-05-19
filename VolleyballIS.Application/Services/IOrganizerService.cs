using VolleyballIS.Application.DTOs.Organizers;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления организаторами
    public interface IOrganizerService
    {
        #region Методы
        Task<IEnumerable<OrganizerDto>> GetAllOrganizersAsync(); // получить всех организаторов

        Task<OrganizerDto?> GetOrganizerByIdAsync(int id); // получить организатора по идентификатору

        Task<OrganizerDto> CreateOrganizerAsync(CreateOrganizerDto dto); // создать организатора

        Task<OrganizerDto> UpdateOrganizerAsync(int id, UpdateOrganizerDto dto); // обновить организатора

        Task DeleteOrganizerAsync(int id); // удалить организатора
        #endregion
    }
}

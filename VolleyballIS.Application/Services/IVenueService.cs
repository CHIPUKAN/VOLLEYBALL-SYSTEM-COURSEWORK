using VolleyballIS.Application.DTOs.Venues;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления площадками
    public interface IVenueService
    {
        #region Методы
        Task<IEnumerable<VenueDto>> GetAllVenuesAsync(); // получить все площадки

        Task<VenueDto?> GetVenueByIdAsync(int id); // получить площадку по идентификатору

        Task<VenueDto> CreateVenueAsync(CreateVenueDto dto); // создать площадку

        Task<VenueDto> UpdateVenueAsync(int id, UpdateVenueDto dto); // обновить площадку

        Task DeleteVenueAsync(int id); // удалить площадку
        #endregion
    }
}

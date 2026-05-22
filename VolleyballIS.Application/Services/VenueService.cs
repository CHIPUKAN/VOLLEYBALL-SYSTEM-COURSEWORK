using VolleyballIS.Application.DTOs.Venues;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления площадками
    public class VenueService : IVenueService
    {
        #region Поля
        private readonly IVenueRepository venueRepository; // репозиторий площадок
        #endregion

        #region Конструкторы
        public VenueService(IVenueRepository venueRepository) // конструктор с внедрением зависимости
        {
            this.venueRepository = venueRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<VenueDto>> GetAllVenuesAsync() // получить все площадки
        {
            IEnumerable<T2Venue> venues = await venueRepository.GetAllAsync();
            IEnumerable<VenueDto> result = venues.Select(MapToDto);
            return result;
        }

        public async Task<VenueDto?> GetVenueByIdAsync(int id) // получить площадку по идентификатору
        {
            T2Venue? venue = await venueRepository.GetByIdAsync(id);
            VenueDto? result = venue == null ? null : MapToDto(venue);
            return result;
        }

        public async Task<VenueDto> CreateVenueAsync(CreateVenueDto dto) // создать площадку
        {
            T2Venue venue = new T2Venue
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Capacity = dto.Capacity
            };
            T2Venue created = await venueRepository.CreateAsync(venue);
            VenueDto result = MapToDto(created);
            return result;
        }

        public async Task<VenueDto> UpdateVenueAsync(int id, UpdateVenueDto dto) // обновить площадку
        {
            T2Venue? existing = await venueRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Площадка с идентификатором {id} не найдена");
            }

            existing.Name = dto.Name;
            existing.Address = dto.Address;
            existing.City = dto.City;
            existing.Capacity = dto.Capacity;
            T2Venue updated = await venueRepository.UpdateAsync(existing);
            VenueDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteVenueAsync(int id) // удалить площадку
        {
            bool exists = await venueRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Площадка с идентификатором {id} не найдена");
            }
            await venueRepository.DeleteAsync(id);
        }

        private static VenueDto MapToDto(T2Venue venue) // маппинг сущности T2Venue в VenueDto
        {
            VenueDto result = new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City,
                Capacity = venue.Capacity
            };
            return result;
        }
        #endregion
    }
}

using VolleyballIS.Application.DTOs.Organizers;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления организаторами
    public class OrganizerService : IOrganizerService
    {
        #region Поля
        private readonly IOrganizerRepository organizerRepository; // репозиторий организаторов
        #endregion

        #region Конструкторы
        public OrganizerService(IOrganizerRepository organizerRepository) // конструктор с внедрением зависимости
        {
            this.organizerRepository = organizerRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<OrganizerDto>> GetAllOrganizersAsync() // получить всех организаторов
        {
            IEnumerable<T13Organizer> organizers = await organizerRepository.GetAllAsync();
            IEnumerable<OrganizerDto> result = organizers.Select(MapToDto);
            return result;
        }

        public async Task<OrganizerDto?> GetOrganizerByIdAsync(int id) // получить организатора по идентификатору
        {
            T13Organizer? organizer = await organizerRepository.GetByIdAsync(id);
            OrganizerDto? result = organizer == null ? null : MapToDto(organizer);
            return result;
        }

        public async Task<OrganizerDto> CreateOrganizerAsync(CreateOrganizerDto dto) // создать организатора
        {
            T13Organizer organizer = new T13Organizer
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
                Phone = dto.Phone
            };
            T13Organizer created = await organizerRepository.CreateAsync(organizer);
            OrganizerDto result = MapToDto(created);
            return result;
        }

        public async Task<OrganizerDto> UpdateOrganizerAsync(int id, UpdateOrganizerDto dto) // обновить организатора
        {
            T13Organizer? existing = await organizerRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Организатор с идентификатором {id} не найден");
            }

            existing.LastName = dto.LastName;
            existing.FirstName = dto.FirstName;
            existing.MiddleName = dto.MiddleName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            T13Organizer updated = await organizerRepository.UpdateAsync(existing);
            OrganizerDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteOrganizerAsync(int id) // удалить организатора
        {
            bool exists = await organizerRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Организатор с идентификатором {id} не найден");
            }
            await organizerRepository.DeleteAsync(id);
        }

        private static OrganizerDto MapToDto(T13Organizer organizer) // маппинг сущности T13Organizer в OrganizerDto
        {
            OrganizerDto result = new OrganizerDto
            {
                Id = organizer.Id,
                LastName = organizer.LastName,
                FirstName = organizer.FirstName,
                MiddleName = organizer.MiddleName,
                FullName = organizer.FullName(),
                Email = organizer.Email,
                Phone = organizer.Phone
            };
            return result;
        }
        #endregion
    }
}

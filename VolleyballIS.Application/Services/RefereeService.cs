using VolleyballIS.Application.DTOs.Referees;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления судьями
    public class RefereeService : IRefereeService
    {
        #region Поля
        private readonly IRefereeRepository refereeRepository; // репозиторий судей
        #endregion

        #region Конструкторы
        public RefereeService(IRefereeRepository refereeRepository) // конструктор с внедрением зависимости
        {
            this.refereeRepository = refereeRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<RefereeDto>> GetAllRefereesAsync() // получить всех судей
        {
            IEnumerable<T12Referee> referees = await refereeRepository.GetAllAsync();
            IEnumerable<RefereeDto> result = referees.Select(MapToDto);
            return result;
        }

        public async Task<RefereeDto?> GetRefereeByIdAsync(int id) // получить судью по идентификатору
        {
            T12Referee? referee = await refereeRepository.GetByIdAsync(id);
            RefereeDto? result = referee == null ? null : MapToDto(referee);
            return result;
        }

        public async Task<RefereeDto> CreateRefereeAsync(CreateRefereeDto dto) // создать судью
        {
            T12Referee referee = new T12Referee
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Category = dto.Category,
                LicenseNumber = dto.LicenseNumber,
                Email = dto.Email,
                Phone = dto.Phone
            };
            T12Referee created = await refereeRepository.CreateAsync(referee);
            RefereeDto result = MapToDto(created);
            return result;
        }

        public async Task<RefereeDto> UpdateRefereeAsync(int id, UpdateRefereeDto dto) // обновить судью
        {
            T12Referee? existing = await refereeRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Судья с идентификатором {id} не найден");
            }

            existing.LastName = dto.LastName;
            existing.FirstName = dto.FirstName;
            existing.MiddleName = dto.MiddleName;
            existing.Category = dto.Category;
            existing.LicenseNumber = dto.LicenseNumber;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            T12Referee updated = await refereeRepository.UpdateAsync(existing);
            RefereeDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteRefereeAsync(int id) // удалить судью
        {
            bool exists = await refereeRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Судья с идентификатором {id} не найден");
            }
            await refereeRepository.DeleteAsync(id);
        }

        private static RefereeDto MapToDto(T12Referee referee) // маппинг сущности T12Referee в RefereeDto
        {
            RefereeDto result = new RefereeDto
            {
                Id = referee.Id,
                LastName = referee.LastName,
                FirstName = referee.FirstName,
                MiddleName = referee.MiddleName,
                FullName = referee.FullName(),
                Category = referee.Category,
                LicenseNumber = referee.LicenseNumber,
                Email = referee.Email,
                Phone = referee.Phone
            };
            return result;
        }
        #endregion
    }
}

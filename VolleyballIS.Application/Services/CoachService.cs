using VolleyballIS.Application.DTOs.Coaches;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления тренерами
    public class CoachService : ICoachService
    {
        #region Поля
        private readonly ICoachRepository coachRepository; // репозиторий тренеров
        #endregion

        #region Конструкторы
        public CoachService(ICoachRepository coachRepository) // конструктор с внедрением зависимости
        {
            this.coachRepository = coachRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<CoachDto>> GetAllCoachesAsync() // получить всех тренеров
        {
            IEnumerable<T3Coach> coaches = await coachRepository.GetAllAsync();
            IEnumerable<CoachDto> result = coaches.Select(MapToDto);
            return result;
        }

        public async Task<CoachDto?> GetCoachByIdAsync(int id) // получить тренера по идентификатору
        {
            T3Coach? coach = await coachRepository.GetByIdAsync(id);
            CoachDto? result = coach == null ? null : MapToDto(coach);
            return result;
        }

        public async Task<CoachDto> CreateCoachAsync(CreateCoachDto dto) // создать тренера
        {
            T3Coach coach = new T3Coach
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
                Phone = dto.Phone,
                Category = dto.Category
            };
            T3Coach created = await coachRepository.CreateAsync(coach);
            CoachDto result = MapToDto(created);
            return result;
        }

        public async Task<CoachDto> UpdateCoachAsync(int id, UpdateCoachDto dto) // обновить тренера
        {
            T3Coach? existing = await coachRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Тренер с идентификатором {id} не найден");
            }

            existing.LastName = dto.LastName;
            existing.FirstName = dto.FirstName;
            existing.MiddleName = dto.MiddleName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.Category = dto.Category;
            T3Coach updated = await coachRepository.UpdateAsync(existing);
            CoachDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteCoachAsync(int id) // удалить тренера
        {
            bool exists = await coachRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Тренер с идентификатором {id} не найден");
            }
            await coachRepository.DeleteAsync(id);
        }

        private static CoachDto MapToDto(T3Coach coach) // маппинг сущности T3Coach в CoachDto
        {
            CoachDto result = new CoachDto
            {
                Id = coach.Id,
                LastName = coach.LastName,
                FirstName = coach.FirstName,
                MiddleName = coach.MiddleName,
                FullName = coach.FullName(),
                Email = coach.Email,
                Phone = coach.Phone,
                Category = coach.Category
            };
            return result;
        }
        #endregion
    }
}

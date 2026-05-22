using VolleyballIS.Application.DTOs.Seasons;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления сезонами
    public class SeasonService : ISeasonService
    {
        #region Поля
        private readonly ISeasonRepository seasonRepository; // репозиторий сезонов
        #endregion

        #region Конструкторы
        public SeasonService(ISeasonRepository seasonRepository) // конструктор с внедрением зависимости
        {
            this.seasonRepository = seasonRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<SeasonDto>> GetAllSeasonsAsync() // получить все сезоны
        {
            IEnumerable<T9Season> seasons = await seasonRepository.GetAllAsync();
            IEnumerable<SeasonDto> result = seasons.Select(MapToDto);
            return result;
        }

        public async Task<SeasonDto?> GetSeasonByIdAsync(int id) // получить сезон по идентификатору
        {
            T9Season? season = await seasonRepository.GetByIdAsync(id);
            SeasonDto? result = season == null ? null : MapToDto(season);
            return result;
        }

        public async Task<SeasonDto> CreateSeasonAsync(CreateSeasonDto dto) // создать сезон
        {
            bool nameExists = await seasonRepository.NameExistsAsync(dto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"Сезон с наименованием «{dto.Name}» уже существует");
            }

            T9Season season = new T9Season
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status
            };
            T9Season created = await seasonRepository.CreateAsync(season);
            SeasonDto result = MapToDto(created);
            return result;
        }

        public async Task<SeasonDto> UpdateSeasonAsync(int id, UpdateSeasonDto dto) // обновить сезон
        {
            T9Season? existing = await seasonRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Сезон с идентификатором {id} не найден");
            }

            bool nameExists = await seasonRepository.NameExistsAsync(dto.Name, id);
            if (nameExists)
            {
                throw new InvalidOperationException($"Сезон с наименованием «{dto.Name}» уже существует");
            }

            existing.Name = dto.Name;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Status = dto.Status;
            T9Season updated = await seasonRepository.UpdateAsync(existing);
            SeasonDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteSeasonAsync(int id) // удалить сезон
        {
            bool exists = await seasonRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Сезон с идентификатором {id} не найден");
            }
            await seasonRepository.DeleteAsync(id);
        }

        private static SeasonDto MapToDto(T9Season season) // маппинг сущности T9Season в SeasonDto
        {
            SeasonDto result = new SeasonDto
            {
                Id = season.Id,
                Name = season.Name,
                StartDate = season.StartDate,
                EndDate = season.EndDate,
                Status = season.Status
            };
            return result;
        }
        #endregion
    }
}

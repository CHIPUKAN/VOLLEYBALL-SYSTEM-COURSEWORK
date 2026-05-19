using VolleyballIS.Application.DTOs.Awards;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления наградами турниров
    public class AwardService : IAwardService
    {
        #region Поля
        private readonly IAwardRepository awardRepository; // репозиторий наград
        #endregion

        #region Конструкторы
        public AwardService(IAwardRepository awardRepository) // конструктор с внедрением зависимости
        {
            this.awardRepository = awardRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<AwardDto>> GetAwardsByTournamentAsync(int tournamentId) // награды турнира
        {
            IEnumerable<T20Award> awards = await awardRepository.GetByTournamentIdAsync(tournamentId);
            IEnumerable<AwardDto> result = awards.Select(MapToDto);
            return result;
        }

        public async Task<AwardDto?> GetAwardByIdAsync(int id) // награда по id
        {
            T20Award? award = await awardRepository.GetByIdAsync(id);
            AwardDto? result = award == null ? null : MapToDto(award);
            return result;
        }

        public async Task<AwardDto> CreateAwardAsync(CreateAwardDto dto) // создать награду
        {
            T20Award award = new T20Award
            {
                TournamentId = dto.TournamentId,
                AwardTypeCode = dto.AwardTypeCode,
                Name = dto.Name,
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId
            };
            T20Award created = await awardRepository.CreateAsync(award);
            AwardDto result = MapToDto(created);
            return result;
        }

        public async Task<AwardDto> UpdateAwardAsync(int id, UpdateAwardDto dto) // обновить награду
        {
            T20Award? existing = await awardRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Награда с идентификатором {id} не найдена");
            }

            existing.AwardTypeCode = dto.AwardTypeCode;
            existing.Name = dto.Name;
            existing.PlayerId = dto.PlayerId;
            existing.TeamId = dto.TeamId;
            T20Award updated = await awardRepository.UpdateAsync(existing);
            AwardDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteAwardAsync(int id) // удалить награду
        {
            bool exists = await awardRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Награда с идентификатором {id} не найдена");
            }
            await awardRepository.DeleteAsync(id);
        }

        private static AwardDto MapToDto(T20Award a) // маппинг T20Award -> AwardDto
        {
            string? playerFio = a.Player != null
                ? (a.Player.MiddleName != null
                    ? $"{a.Player.LastName} {a.Player.FirstName} {a.Player.MiddleName}"
                    : $"{a.Player.LastName} {a.Player.FirstName}")
                : null;

            AwardDto result = new AwardDto
            {
                Id = a.Id,
                TournamentId = a.TournamentId,
                TournamentName = a.Tournament?.Name,
                AwardTypeCode = a.AwardTypeCode,
                AwardTypeName = a.AwardType?.Name,
                Name = a.Name,
                PlayerId = a.PlayerId,
                PlayerFullName = playerFio,
                TeamId = a.TeamId,
                TeamName = a.Team?.Name
            };
            return result;
        }
        #endregion
    }
}

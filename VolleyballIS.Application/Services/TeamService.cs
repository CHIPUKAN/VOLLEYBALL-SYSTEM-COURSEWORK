using VolleyballIS.Application.DTOs.Teams;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления командами — бизнес-логика для модуля «Команды»
    public class TeamService : ITeamService
    {
        #region Поля
        private readonly ITeamRepository teamRepository; // репозиторий команд
        #endregion

        #region Конструкторы
        public TeamService(ITeamRepository teamRepository) // конструктор с внедрением зависимости
        {
            this.teamRepository = teamRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync() // получить список всех команд в виде DTO
        {
            IEnumerable<T4Team> teams = await teamRepository.GetAllAsync();
            IEnumerable<TeamDto> result = teams.Select(MapToDto);
            return result;
        }

        public async Task<TeamDto?> GetTeamByIdAsync(int id) // получить команду по идентификатору в виде DTO
        {
            T4Team? team = await teamRepository.GetByIdAsync(id);
            TeamDto? result = team == null ? null : MapToDto(team);
            return result;
        }

        public async Task<TeamDto> CreateTeamAsync(CreateTeamDto dto) // создать новую команду из DTO
        {
            bool nameExists = await teamRepository.NameExistsAsync(dto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"Команда с наименованием «{dto.Name}» уже существует");
            }

            T4Team team = new T4Team
            {
                Name = dto.Name,
                LogoUrl = dto.LogoUrl,
                RegionOktmo = dto.RegionOktmo,
                HeadCoachId = dto.HeadCoachId,
                HomeVenueId = dto.HomeVenueId
            };

            T4Team created = await teamRepository.CreateAsync(team);
            TeamDto result = MapToDto(created);
            return result;
        }

        public async Task<TeamDto> UpdateTeamAsync(int id, UpdateTeamDto dto) // обновить команду из DTO
        {
            bool exists = await teamRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Команда с идентификатором {id} не найдена");
            }

            bool nameExists = await teamRepository.NameExistsAsync(dto.Name, id);
            if (nameExists)
            {
                throw new InvalidOperationException($"Команда с наименованием «{dto.Name}» уже существует");
            }

            T4Team team = new T4Team
            {
                Id = id,
                Name = dto.Name,
                LogoUrl = dto.LogoUrl,
                RegionOktmo = dto.RegionOktmo,
                HeadCoachId = dto.HeadCoachId,
                HomeVenueId = dto.HomeVenueId
            };

            T4Team updated = await teamRepository.UpdateAsync(team);
            TeamDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteTeamAsync(int id) // удалить команду по идентификатору
        {
            bool exists = await teamRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Команда с идентификатором {id} не найдена");
            }

            await teamRepository.DeleteAsync(id);
        }

        private static TeamDto MapToDto(T4Team team) // маппинг сущности T4Team в TeamDto
        {
            TeamDto result = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                LogoUrl = team.LogoUrl,
                RegionOktmo = team.RegionOktmo,
                RegionName = team.Region?.Name,
                HeadCoachId = team.HeadCoachId,
                HeadCoachFullName = team.HeadCoach?.FullName(),
                HomeVenueId = team.HomeVenueId,
                HomeVenueName = team.HomeVenue?.Name,
                HomeVenueCity = team.HomeVenue?.City
            };
            return result;
        }
        #endregion
    }
}

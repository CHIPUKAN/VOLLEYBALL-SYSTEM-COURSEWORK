using VolleyballIS.Application.DTOs.Applications;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления заявками команд на турниры
    public class ApplicationService : IApplicationService
    {
        #region Поля
        private readonly IApplicationRepository applicationRepository; // репозиторий заявок
        #endregion

        #region Конструкторы
        public ApplicationService(IApplicationRepository applicationRepository) // конструктор с внедрением зависимости
        {
            this.applicationRepository = applicationRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync() // получить все заявки
        {
            IEnumerable<T7Application> apps = await applicationRepository.GetAllAsync();
            IEnumerable<ApplicationDto> result = apps.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByTournamentAsync(int tournamentId) // заявки турнира
        {
            IEnumerable<T7Application> apps = await applicationRepository.GetByTournamentIdAsync(tournamentId);
            IEnumerable<ApplicationDto> result = apps.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByTeamAsync(int teamId) // заявки команды
        {
            IEnumerable<T7Application> apps = await applicationRepository.GetByTeamIdAsync(teamId);
            IEnumerable<ApplicationDto> result = apps.Select(MapToDto);
            return result;
        }

        public async Task<ApplicationDto?> GetApplicationByIdAsync(int id) // получить заявку по id
        {
            T7Application? app = await applicationRepository.GetByIdAsync(id);
            ApplicationDto? result = app == null ? null : MapToDto(app);
            return result;
        }

        public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto dto) // создать заявку
        {
            bool duplicate = await applicationRepository.TeamApplicationExistsAsync(dto.TeamId, dto.TournamentId);
            if (duplicate)
            {
                throw new InvalidOperationException("Команда уже подала заявку на данный турнир");
            }

            T7Application app = new T7Application
            {
                TeamId = dto.TeamId,
                TournamentId = dto.TournamentId,
                SubmissionDate = dto.SubmissionDate,
                StatusCode = dto.StatusCode
            };
            T7Application created = await applicationRepository.CreateAsync(app);
            ApplicationDto result = MapToDto(created);
            return result;
        }

        public async Task<ApplicationDto> UpdateApplicationAsync(int id, UpdateApplicationDto dto) // обновить статус заявки
        {
            T7Application? existing = await applicationRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Заявка с идентификатором {id} не найдена");
            }

            existing.StatusCode = dto.StatusCode;
            T7Application updated = await applicationRepository.UpdateAsync(existing);
            ApplicationDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteApplicationAsync(int id) // удалить заявку
        {
            bool exists = await applicationRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Заявка с идентификатором {id} не найдена");
            }
            await applicationRepository.DeleteAsync(id);
        }

        public async Task<ApplicationDto> AddPlayerToApplicationAsync(int applicationId, AddCompositionPlayerDto dto) // добавить игрока в заявку
        {
            T7Application? app = await applicationRepository.GetByIdAsync(applicationId);
            if (app == null)
            {
                throw new KeyNotFoundException($"Заявка с идентификатором {applicationId} не найдена");
            }

            T8ApplicationComposition? existing = await applicationRepository.GetCompositionPlayerAsync(applicationId, dto.PlayerId);
            if (existing != null)
            {
                throw new InvalidOperationException("Данный игрок уже включён в состав заявки");
            }

            T8ApplicationComposition comp = new T8ApplicationComposition
            {
                ApplicationId = applicationId,
                PlayerId = dto.PlayerId,
                JerseyNumberInApp = dto.JerseyNumberInApp,
                Role = dto.Role,
                IsLibero = dto.IsLibero
            };
            await applicationRepository.AddCompositionPlayerAsync(comp);
            ApplicationDto result = MapToDto((await applicationRepository.GetByIdAsync(applicationId))!);
            return result;
        }

        public async Task RemovePlayerFromApplicationAsync(int applicationId, int playerId) // убрать игрока из заявки
        {
            T8ApplicationComposition? existing = await applicationRepository.GetCompositionPlayerAsync(applicationId, playerId);
            if (existing == null)
            {
                throw new KeyNotFoundException("Игрок не найден в составе заявки");
            }
            await applicationRepository.RemoveCompositionPlayerAsync(applicationId, playerId);
        }

        private static ApplicationDto MapToDto(T7Application app) // маппинг T7Application -> ApplicationDto
        {
            ApplicationDto result = new ApplicationDto
            {
                Id = app.Id,
                TeamId = app.TeamId,
                TeamName = app.Team?.Name,
                TournamentId = app.TournamentId,
                TournamentName = app.Tournament?.Name,
                SubmissionDate = app.SubmissionDate,
                StatusCode = app.StatusCode,
                StatusName = app.Status?.Name,
                Composition = app.Composition.Select(c => new ApplicationCompositionDto
                {
                    ApplicationId = c.ApplicationId,
                    PlayerId = c.PlayerId,
                    PlayerFullName = c.Player != null
                        ? (c.Player.MiddleName != null
                            ? $"{c.Player.LastName} {c.Player.FirstName} {c.Player.MiddleName}"
                            : $"{c.Player.LastName} {c.Player.FirstName}")
                        : null,
                    JerseyNumberInApp = c.JerseyNumberInApp,
                    Role = c.Role,
                    IsLibero = c.IsLibero
                }).ToList()
            };
            return result;
        }
        #endregion
    }
}

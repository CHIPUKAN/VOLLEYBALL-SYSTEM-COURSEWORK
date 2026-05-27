using VolleyballIS.Application.DTOs.Tournaments;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления турнирами
    public class TournamentService : ITournamentService
    {
        #region Поля
        private readonly ITournamentRepository tournamentRepository; // репозиторий турниров
        #endregion

        #region Конструкторы
        public TournamentService(ITournamentRepository tournamentRepository) // конструктор с внедрением зависимости
        {
            this.tournamentRepository = tournamentRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync() // получить все турниры
        {
            IEnumerable<T10Tournament> tournaments = await tournamentRepository.GetAllAsync();
            IEnumerable<TournamentDto> result = tournaments.Select(MapToDto);
            return result;
        }

        public async Task<TournamentDto?> GetTournamentByIdAsync(int id) // получить турнир по идентификатору
        {
            T10Tournament? tournament = await tournamentRepository.GetByIdAsync(id);
            TournamentDto? result = tournament == null ? null : MapToDto(tournament);
            return result;
        }

        public async Task<TournamentDto> CreateTournamentAsync(CreateTournamentDto dto) // создать турнир
        {
            bool nameExists = await tournamentRepository.NameExistsAsync(dto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"Турнир с наименованием «{dto.Name}» уже существует");
            }

            ValidateTournamentDates(dto.StartDate, dto.EndDate, dto.ApplicationDeadline);
            ValidateMaxPlayersPerApp(dto.MaxPlayersPerApp);

            T10Tournament tournament = new T10Tournament
            {
                SeasonId = dto.SeasonId,
                OrganizerId = dto.OrganizerId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ApplicationDeadline = dto.ApplicationDeadline,
                City = dto.City,
                Description = dto.Description,
                MaxTeams = dto.MaxTeams,
                Gender = dto.Gender,
                AgeCategory = dto.AgeCategory,
                Level = dto.Level,
                MaxPlayersPerApp = dto.MaxPlayersPerApp,
                FormatCode = dto.FormatCode,
                ScoringSystemCode = dto.ScoringSystemCode,
                SetsToWin = dto.SetsToWin,
                TiebreakScoreTarget = dto.TiebreakScoreTarget
            };
            T10Tournament created = await tournamentRepository.CreateAsync(tournament);
            TournamentDto result = MapToDto(created);
            return result;
        }

        public async Task<TournamentDto> UpdateTournamentAsync(int id, UpdateTournamentDto dto) // обновить турнир
        {
            T10Tournament? existing = await tournamentRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Турнир с идентификатором {id} не найден");
            }

            bool nameExists = await tournamentRepository.NameExistsAsync(dto.Name, id);
            if (nameExists)
            {
                throw new InvalidOperationException($"Турнир с наименованием «{dto.Name}» уже существует");
            }

            ValidateTournamentDates(dto.StartDate, dto.EndDate, dto.ApplicationDeadline);
            ValidateMaxPlayersPerApp(dto.MaxPlayersPerApp);

            existing.SeasonId = dto.SeasonId;
            existing.OrganizerId = dto.OrganizerId;
            existing.Name = dto.Name;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.ApplicationDeadline = dto.ApplicationDeadline;
            existing.City = dto.City;
            existing.Description = dto.Description;
            existing.MaxTeams = dto.MaxTeams;
            existing.Gender = dto.Gender;
            existing.AgeCategory = dto.AgeCategory;
            existing.Level = dto.Level;
            existing.MaxPlayersPerApp = dto.MaxPlayersPerApp;
            existing.FormatCode = dto.FormatCode;
            existing.ScoringSystemCode = dto.ScoringSystemCode;
            existing.SetsToWin = dto.SetsToWin;
            existing.TiebreakScoreTarget = dto.TiebreakScoreTarget;
            T10Tournament updated = await tournamentRepository.UpdateAsync(existing);
            TournamentDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteTournamentAsync(int id) // удалить турнир
        {
            bool exists = await tournamentRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Турнир с идентификатором {id} не найден");
            }
            await tournamentRepository.DeleteAsync(id);
        }

        private static void ValidateTournamentDates(DateOnly startDate, DateOnly endDate, DateOnly? appDeadline) // проверить корректность дат турнира
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException(
                    $"Дата начала турнира ({startDate}) не может быть позже даты окончания ({endDate})");
            }
            if (appDeadline.HasValue && appDeadline.Value > startDate)
            {
                throw new InvalidOperationException(
                    $"Дедлайн подачи заявок ({appDeadline}) должен быть не позже начала турнира ({startDate})");
            }
        }

        private static void ValidateMaxPlayersPerApp(short value) // проверить допустимость значения max_players_per_app
        {
            short[] allowed = [12, 14];
            if (!allowed.Contains(value))
            {
                throw new InvalidOperationException("Максимальное количество игроков в заявке должно быть 12 или 14");
            }
        }

        private static TournamentDto MapToDto(T10Tournament tournament) // маппинг сущности T10Tournament в TournamentDto
        {
            TournamentDto result = new TournamentDto
            {
                Id = tournament.Id,
                SeasonId = tournament.SeasonId,
                SeasonName = tournament.Season?.Name,
                OrganizerId = tournament.OrganizerId,
                OrganizerFullName = tournament.Organizer?.FullName(),
                Name = tournament.Name,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                ApplicationDeadline = tournament.ApplicationDeadline,
                City = tournament.City,
                Description = tournament.Description,
                MaxTeams = tournament.MaxTeams,
                Gender = tournament.Gender,
                AgeCategory = tournament.AgeCategory,
                Level = tournament.Level,
                MaxPlayersPerApp = tournament.MaxPlayersPerApp,
                FormatCode = tournament.FormatCode,
                FormatName = tournament.Format?.Name,
                ScoringSystemCode = tournament.ScoringSystemCode,
                ScoringSystemName = tournament.ScoringSystem?.Name,
                SetsToWin = tournament.SetsToWin,
                TiebreakScoreTarget = tournament.TiebreakScoreTarget
            };
            return result;
        }
        #endregion
    }
}

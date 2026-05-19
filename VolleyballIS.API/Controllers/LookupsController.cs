using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolleyballIS.Application.DTOs;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для получения справочных данных (выпадающие списки)
    [ApiController]
    [Route("api/[controller]")]
    public class LookupsController : ControllerBase
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public LookupsController(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы — справочники (S1–S18)
        [HttpGet("amplua")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetAmplua() // С1. Амплуа
        {
            IEnumerable<LookupDto> result = await dbContext.Amplua
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("player-statuses")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetPlayerStatuses() // С2. Статусы игроков
        {
            IEnumerable<LookupDto> result = await dbContext.PlayerStatuses
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("application-statuses")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetApplicationStatuses() // С3. Статусы заявок
        {
            IEnumerable<LookupDto> result = await dbContext.ApplicationStatuses
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("tournament-formats")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetTournamentFormats() // С4. Форматы турниров
        {
            IEnumerable<LookupDto> result = await dbContext.TournamentFormats
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("tournament-stages")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetTournamentStages() // С5. Этапы турниров
        {
            IEnumerable<LookupDto> result = await dbContext.TournamentStages
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("match-statuses")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetMatchStatuses() // С6. Статусы матчей
        {
            IEnumerable<LookupDto> result = await dbContext.MatchStatuses
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("protocol-statuses")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetProtocolStatuses() // С7. Статусы протоколов
        {
            IEnumerable<LookupDto> result = await dbContext.ProtocolStatuses
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("referee-roles")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetRefereeRoles() // С8. Роли судей
        {
            IEnumerable<LookupDto> result = await dbContext.RefereeRoles
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("event-types")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetEventTypes() // С9. Типы событий
        {
            IEnumerable<LookupDto> result = await dbContext.EventTypes
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("substitution-types")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetSubstitutionTypes() // С10. Типы замен
        {
            IEnumerable<LookupDto> result = await dbContext.SubstitutionTypes
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("timeout-types")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetTimeoutTypes() // С11. Типы тайм-аутов
        {
            IEnumerable<LookupDto> result = await dbContext.TimeoutTypes
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("sanction-types")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetSanctionTypes() // С12. Типы санкций
        {
            IEnumerable<LookupDto> result = await dbContext.SanctionTypes
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("delay-violations")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetDelayViolations() // С13. Нарушения задержки
        {
            IEnumerable<LookupDto> result = await dbContext.DelayViolations
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("award-types")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetAwardTypes() // С14. Типы наград
        {
            IEnumerable<LookupDto> result = await dbContext.AwardTypes
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("sanction-kinds")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetSanctionKinds() // С15. Виды санкций
        {
            IEnumerable<LookupDto> result = await dbContext.SanctionKinds
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("recipient-types")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetRecipientTypes() // С16. Типы получателей санкций
        {
            IEnumerable<LookupDto> result = await dbContext.RecipientTypes
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("coin-toss-options")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetCoinTossOptions() // С17. Варианты жеребьёвки
        {
            IEnumerable<LookupDto> result = await dbContext.CoinTossOptions
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("scoring-systems")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetScoringSystems() // С18. Системы очков
        {
            IEnumerable<LookupDto> result = await dbContext.ScoringSystems
                .OrderBy(x => x.Code)
                .Select(x => new LookupDto { Code = x.Code, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }
        #endregion

        #region Методы — основные справочные сущности
        [HttpGet("regions")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetRegions() // Т1. Регионы (ОКТМО)
        {
            IEnumerable<LookupItemDto> result = await dbContext.Regions
                .OrderBy(x => x.Name)
                .Select(x => new LookupItemDto { Id = x.OktmoCode, Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("coaches")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetCoaches() // Т3. Тренеры (ФИО)
        {
            IEnumerable<LookupItemDto> result = await dbContext.Coaches
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Select(x => new LookupItemDto
                {
                    Id = x.Id.ToString(),
                    Name = x.MiddleName != null
                        ? x.LastName + " " + x.FirstName + " " + x.MiddleName
                        : x.LastName + " " + x.FirstName
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("venues")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetVenues() // Т2. Площадки (название и город)
        {
            IEnumerable<LookupItemDto> result = await dbContext.Venues
                .OrderBy(x => x.Name)
                .Select(x => new LookupItemDto
                {
                    Id = x.Id.ToString(),
                    Name = x.Name + ", " + x.City
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("teams")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetTeams() // Т4. Команды
        {
            IEnumerable<LookupItemDto> result = await dbContext.Teams
                .OrderBy(x => x.Name)
                .Select(x => new LookupItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("seasons")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetSeasons() // Т9. Сезоны
        {
            IEnumerable<LookupItemDto> result = await dbContext.Seasons
                .OrderByDescending(x => x.StartDate)
                .Select(x => new LookupItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("organizers")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetOrganizers() // Т13. Организаторы (ФИО)
        {
            IEnumerable<LookupItemDto> result = await dbContext.Organizers
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Select(x => new LookupItemDto
                {
                    Id = x.Id.ToString(),
                    Name = x.MiddleName != null
                        ? x.LastName + " " + x.FirstName + " " + x.MiddleName
                        : x.LastName + " " + x.FirstName
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("referees")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetReferees() // Т12. Судьи (ФИО)
        {
            IEnumerable<LookupItemDto> result = await dbContext.Referees
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Select(x => new LookupItemDto
                {
                    Id = x.Id.ToString(),
                    Name = x.MiddleName != null
                        ? x.LastName + " " + x.FirstName + " " + x.MiddleName
                        : x.LastName + " " + x.FirstName
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("tournaments")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetTournaments() // Т10. Турниры
        {
            IEnumerable<LookupItemDto> result = await dbContext.Tournaments
                .OrderByDescending(x => x.StartDate)
                .Select(x => new LookupItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("players")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetPlayers([FromQuery] int? teamId) // Т6. Игроки (с фильтром по команде)
        {
            IQueryable<Domain.Entities.T6Player> query = dbContext.Players;
            if (teamId.HasValue)
            {
                query = query.Where(x => x.TeamId == teamId.Value);
            }
            IEnumerable<LookupItemDto> result = await query
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Select(x => new LookupItemDto
                {
                    Id = x.Id.ToString(),
                    Name = x.MiddleName != null
                        ? x.LastName + " " + x.FirstName + " " + x.MiddleName
                        : x.LastName + " " + x.FirstName
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("groups")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetGroups([FromQuery] int? tournamentId) // Т11. Группы (с фильтром по турниру)
        {
            IQueryable<Domain.Entities.T11Group> query = dbContext.Groups;
            if (tournamentId.HasValue)
            {
                query = query.Where(x => x.TournamentId == tournamentId.Value);
            }
            IEnumerable<LookupItemDto> result = await query
                .OrderBy(x => x.Name)
                .Select(x => new LookupItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("matches")]
        public async Task<ActionResult<IEnumerable<LookupItemDto>>> GetMatches([FromQuery] int? tournamentId) // Т14. Матчи (с фильтром по турниру)
        {
            IQueryable<Domain.Entities.T14Match> query = dbContext.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam);
            if (tournamentId.HasValue)
            {
                query = query.Where(x => x.TournamentId == tournamentId.Value);
            }
            List<Domain.Entities.T14Match> matches = await query
                .OrderByDescending(x => x.MatchDate)
                .ToListAsync();
            IEnumerable<LookupItemDto> result = matches.Select(x => new LookupItemDto
            {
                Id = x.Id.ToString(),
                Name = $"{x.MatchDate:dd.MM.yyyy} {x.HomeTeam?.Name ?? "?"} — {x.GuestTeam?.Name ?? "?"}"
            });
            return Ok(result);
        }
        #endregion
    }
}

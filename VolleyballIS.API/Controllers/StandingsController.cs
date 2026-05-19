using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Standings;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер турнирной таблицы — расчёт и выдача позиций команд
    [ApiController]
    [Route("api/[controller]")]
    public class StandingsController : ControllerBase
    {
        #region Поля
        private readonly IStandingService standingService; // сервис расчёта таблицы
        #endregion

        #region Конструкторы
        public StandingsController(IStandingService standingService) // конструктор с внедрением зависимости
        {
            this.standingService = standingService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StandingDto>>> GetStandings( // получить турнирную таблицу
            [FromQuery] int tournamentId,
            [FromQuery] short? stageCode = null,
            [FromQuery] int? groupId = null)
        {
            if (tournamentId <= 0)
            {
                return BadRequest(new { message = "Необходимо указать идентификатор турнира" });
            }

            try
            {
                IEnumerable<StandingDto> result = await standingService.GetStandingsAsync(tournamentId, stageCode, groupId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion
    }
}

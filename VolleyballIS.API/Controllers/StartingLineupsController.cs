using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.StartingLineups;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы со стартовыми расстановками команд в матчах
    [ApiController]
    [Route("api/matches/{matchId:int}/lineups")]
    public class StartingLineupsController : ControllerBase
    {
        #region Поля
        private readonly IStartingLineupService lineupService; // сервис расстановок
        #endregion

        #region Конструкторы
        public StartingLineupsController(IStartingLineupService lineupService) // конструктор с внедрением зависимости
        {
            this.lineupService = lineupService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StartingLineupDto>>> GetAll(
            int matchId,
            [FromQuery] int? teamId,
            [FromQuery] short? setNumber) // расстановки матча (с фильтром по команде и партии)
        {
            IEnumerable<StartingLineupDto> result;
            if (teamId.HasValue && setNumber.HasValue)
            {
                result = await lineupService.GetLineupsByMatchTeamSetAsync(matchId, teamId.Value, setNumber.Value);
            }
            else
            {
                result = await lineupService.GetLineupsByMatchAsync(matchId);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча,ТренерКоманды")]
        public async Task<ActionResult<StartingLineupDto>> Upsert(int matchId, [FromBody] UpsertStartingLineupDto dto) // занести позицию расстановки
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            StartingLineupDto result = await lineupService.UpsertPositionAsync(matchId, dto);
            return Ok(result);
        }

        [HttpDelete("{teamId:int}/{setNumber:int}/{positionNo:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча,ТренерКоманды")]
        public async Task<IActionResult> DeletePosition(
            int matchId, int teamId, short setNumber, short positionNo) // удалить одну позицию расстановки
        {
            await lineupService.DeletePositionAsync(matchId, teamId, setNumber, positionNo);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча,ТренерКоманды")]
        public async Task<IActionResult> DeleteTeamSet(
            int matchId,
            [FromQuery] int teamId,
            [FromQuery] short setNumber) // удалить расстановку команды в партии
        {
            await lineupService.DeleteLineupAsync(matchId, teamId, setNumber);
            return NoContent();
        }
        #endregion
    }
}

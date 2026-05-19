using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.MatchCaptains;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с капитанами команд в матчах
    [ApiController]
    [Route("api/matches/{matchId:int}/captains")]
    public class MatchCaptainsController : ControllerBase
    {
        #region Поля
        private readonly IMatchCaptainService captainService; // сервис капитанов
        #endregion

        #region Конструкторы
        public MatchCaptainsController(IMatchCaptainService captainService) // конструктор с внедрением зависимости
        {
            this.captainService = captainService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchCaptainDto>>> GetAll(int matchId) // капитаны матча
        {
            IEnumerable<MatchCaptainDto> result = await captainService.GetCaptainsByMatchAsync(matchId);
            return Ok(result);
        }

        [HttpGet("{teamId:int}")]
        public async Task<ActionResult<MatchCaptainDto>> GetByTeam(int matchId, int teamId) // капитан команды в матче
        {
            MatchCaptainDto? result = await captainService.GetCaptainAsync(matchId, teamId);
            if (result == null)
            {
                return NotFound(new { message = $"Капитан команды {teamId} в матче {matchId} не назначен" });
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<MatchCaptainDto>> Upsert(int matchId, [FromBody] UpsertMatchCaptainDto dto) // назначить капитана
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MatchCaptainDto result = await captainService.UpsertCaptainAsync(matchId, dto);
            return Ok(result);
        }

        [HttpDelete("{teamId:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<IActionResult> Delete(int matchId, int teamId) // снять капитана
        {
            try
            {
                await captainService.DeleteCaptainAsync(matchId, teamId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion
    }
}

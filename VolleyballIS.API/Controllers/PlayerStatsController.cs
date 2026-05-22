using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.PlayerStats;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы со статистикой игроков в матчах
    [ApiController]
    [Route("api/matches/{matchId:int}/player-stats")]
    public class PlayerStatsController : ControllerBase
    {
        #region Поля
        private readonly IPlayerStatsService statsService; // сервис статистики
        #endregion

        #region Конструкторы
        public PlayerStatsController(IPlayerStatsService statsService) // конструктор с внедрением зависимости
        {
            this.statsService = statsService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerStatsDto>>> GetAll(int matchId) // статистика матча
        {
            IEnumerable<PlayerStatsDto> result = await statsService.GetStatsByMatchAsync(matchId);
            return Ok(result);
        }

        [HttpGet("{playerId:int}")]
        public async Task<ActionResult<PlayerStatsDto>> GetByPlayer(int matchId, int playerId) // статистика игрока
        {
            PlayerStatsDto? result = await statsService.GetStatsAsync(matchId, playerId);
            if (result == null)
            {
                return NotFound(new { message = $"Статистика игрока {playerId} в матче {matchId} не найдена" });
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<ActionResult<PlayerStatsDto>> Upsert(int matchId, [FromBody] UpsertPlayerStatsDto dto) // создать или обновить статистику
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PlayerStatsDto result = await statsService.UpsertStatsAsync(matchId, dto);
            return Ok(result);
        }

        [HttpDelete("{playerId:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<IActionResult> Delete(int matchId, int playerId) // удалить статистику
        {
            try
            {
                await statsService.DeleteStatsAsync(matchId, playerId);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Matches;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с матчами (модуль «Матчи»)
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        #region Поля
        private readonly IMatchService matchService; // сервис управления матчами
        #endregion

        #region Конструкторы
        public MatchesController(IMatchService matchService) // конструктор с внедрением зависимости
        {
            this.matchService = matchService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll([FromQuery] int? tournamentId) // получить матчи (с фильтром по турниру)
        {
            IEnumerable<MatchDto> result;
            if (tournamentId.HasValue)
            {
                result = await matchService.GetMatchesByTournamentAsync(tournamentId.Value);
            }
            else
            {
                result = await matchService.GetAllMatchesAsync();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MatchDto>> GetById(int id) // получить матч по id
        {
            MatchDto? result = await matchService.GetMatchByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Матч с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<MatchDto>> Create([FromBody] CreateMatchDto dto) // создать матч
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                MatchDto result = await matchService.CreateMatchAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<MatchDto>> Update(int id, [FromBody] UpdateMatchDto dto) // обновить матч
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                MatchDto result = await matchService.UpdateMatchAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<IActionResult> Delete(int id) // удалить матч
        {
            try
            {
                await matchService.DeleteMatchAsync(id);
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

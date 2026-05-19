using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Tournaments;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с турнирами (модуль «Турниры»)
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        #region Поля
        private readonly ITournamentService tournamentService; // сервис управления турнирами
        #endregion

        #region Конструкторы
        public TournamentsController(ITournamentService tournamentService) // конструктор с внедрением зависимости
        {
            this.tournamentService = tournamentService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAll() // получить все турниры
        {
            IEnumerable<TournamentDto> result = await tournamentService.GetAllTournamentsAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TournamentDto>> GetById(int id) // получить турнир по id
        {
            TournamentDto? result = await tournamentService.GetTournamentByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Турнир с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<TournamentDto>> Create([FromBody] CreateTournamentDto dto) // создать турнир
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                TournamentDto result = await tournamentService.CreateTournamentAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<TournamentDto>> Update(int id, [FromBody] UpdateTournamentDto dto) // обновить турнир
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                TournamentDto result = await tournamentService.UpdateTournamentAsync(id, dto);
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
        public async Task<IActionResult> Delete(int id) // удалить турнир
        {
            try
            {
                await tournamentService.DeleteTournamentAsync(id);
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

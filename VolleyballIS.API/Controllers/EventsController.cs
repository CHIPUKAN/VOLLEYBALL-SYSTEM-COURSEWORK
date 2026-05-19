using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Events;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с игровыми событиями матчей
    [ApiController]
    [Route("api/matches/{matchId:int}/events")]
    public class EventsController : ControllerBase
    {
        #region Поля
        private readonly IEventService eventService; // сервис событий
        #endregion

        #region Конструкторы
        public EventsController(IEventService eventService) // конструктор с внедрением зависимости
        {
            this.eventService = eventService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAll(int matchId, [FromQuery] short? setNumber) // события матча (с фильтром по партии)
        {
            IEnumerable<EventDto> result;
            if (setNumber.HasValue)
            {
                result = await eventService.GetEventsByMatchSetAsync(matchId, setNumber.Value);
            }
            else
            {
                result = await eventService.GetEventsByMatchAsync(matchId);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> GetById(int matchId, int id) // событие по id
        {
            EventDto? result = await eventService.GetEventByIdAsync(id);
            if (result == null || result.MatchId != matchId)
            {
                return NotFound(new { message = $"Событие с идентификатором {id} не найдено" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<ActionResult<EventDto>> Create(int matchId, [FromBody] CreateEventDto dto) // создать событие
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dto.MatchId = matchId;
            try
            {
                EventDto result = await eventService.CreateEventAsync(dto);
                return CreatedAtAction(nameof(GetById), new { matchId, id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<IActionResult> Delete(int matchId, int id) // удалить событие
        {
            try
            {
                await eventService.DeleteEventAsync(id);
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

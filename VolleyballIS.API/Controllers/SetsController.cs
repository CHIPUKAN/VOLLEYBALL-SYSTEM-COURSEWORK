using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Sets;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с партиями матчей (счёт по партиям)
    [ApiController]
    [Route("api/matches/{matchId:int}/sets")]
    public class SetsController : ControllerBase
    {
        #region Поля
        private readonly ISetService setService; // сервис партий
        #endregion

        #region Конструкторы
        public SetsController(ISetService setService) // конструктор с внедрением зависимости
        {
            this.setService = setService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetDto>>> GetAll(int matchId) // партии матча
        {
            IEnumerable<SetDto> result = await setService.GetSetsByMatchAsync(matchId);
            return Ok(result);
        }

        [HttpGet("{setNumber:int}")]
        public async Task<ActionResult<SetDto>> GetByNumber(int matchId, short setNumber) // конкретная партия
        {
            SetDto? result = await setService.GetSetAsync(matchId, setNumber);
            if (result == null)
            {
                return NotFound(new { message = $"Партия {setNumber} матча {matchId} не найдена" });
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<ActionResult<SetDto>> Upsert(int matchId, [FromBody] UpsertSetDto dto) // создать или обновить партию
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                SetDto result = await setService.UpsertSetAsync(matchId, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{setNumber:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<IActionResult> Delete(int matchId, short setNumber) // удалить партию
        {
            try
            {
                await setService.DeleteSetAsync(matchId, setNumber);
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

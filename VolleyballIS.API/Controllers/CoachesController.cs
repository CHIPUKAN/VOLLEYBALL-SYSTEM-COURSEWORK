using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Coaches;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с тренерами (модуль «Тренеры»)
    [ApiController]
    [Route("api/[controller]")]
    public class CoachesController : ControllerBase
    {
        #region Поля
        private readonly ICoachService coachService; // сервис управления тренерами
        #endregion

        #region Конструкторы
        public CoachesController(ICoachService coachService) // конструктор с внедрением зависимости
        {
            this.coachService = coachService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoachDto>>> GetAll() // получить всех тренеров
        {
            IEnumerable<CoachDto> result = await coachService.GetAllCoachesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CoachDto>> GetById(int id) // получить тренера по id
        {
            CoachDto? result = await coachService.GetCoachByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Тренер с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<CoachDto>> Create([FromBody] CreateCoachDto dto) // создать тренера
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CoachDto result = await coachService.CreateCoachAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<CoachDto>> Update(int id, [FromBody] UpdateCoachDto dto) // обновить тренера
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                CoachDto result = await coachService.UpdateCoachAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<IActionResult> Delete(int id) // удалить тренера
        {
            try
            {
                await coachService.DeleteCoachAsync(id);
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

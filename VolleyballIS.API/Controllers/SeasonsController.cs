using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Seasons;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с сезонами (модуль «Сезоны»)
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonsController : ControllerBase
    {
        #region Поля
        private readonly ISeasonService seasonService; // сервис управления сезонами
        #endregion

        #region Конструкторы
        public SeasonsController(ISeasonService seasonService) // конструктор с внедрением зависимости
        {
            this.seasonService = seasonService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeasonDto>>> GetAll() // получить все сезоны
        {
            IEnumerable<SeasonDto> result = await seasonService.GetAllSeasonsAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SeasonDto>> GetById(int id) // получить сезон по id
        {
            SeasonDto? result = await seasonService.GetSeasonByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Сезон с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<SeasonDto>> Create([FromBody] CreateSeasonDto dto) // создать сезон
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                SeasonDto result = await seasonService.CreateSeasonAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<SeasonDto>> Update(int id, [FromBody] UpdateSeasonDto dto) // обновить сезон
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                SeasonDto result = await seasonService.UpdateSeasonAsync(id, dto);
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
        public async Task<IActionResult> Delete(int id) // удалить сезон
        {
            try
            {
                await seasonService.DeleteSeasonAsync(id);
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

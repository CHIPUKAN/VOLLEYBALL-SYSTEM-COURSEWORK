using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Referees;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с судьями (модуль «Судьи»)
    [ApiController]
    [Route("api/[controller]")]
    public class RefereesController : ControllerBase
    {
        #region Поля
        private readonly IRefereeService refereeService; // сервис управления судьями
        #endregion

        #region Конструкторы
        public RefereesController(IRefereeService refereeService) // конструктор с внедрением зависимости
        {
            this.refereeService = refereeService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefereeDto>>> GetAll() // получить всех судей
        {
            IEnumerable<RefereeDto> result = await refereeService.GetAllRefereesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RefereeDto>> GetById(int id) // получить судью по id
        {
            RefereeDto? result = await refereeService.GetRefereeByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Судья с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<RefereeDto>> Create([FromBody] CreateRefereeDto dto) // создать судью
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            RefereeDto result = await refereeService.CreateRefereeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<RefereeDto>> Update(int id, [FromBody] UpdateRefereeDto dto) // обновить судью
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                RefereeDto result = await refereeService.UpdateRefereeAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<IActionResult> Delete(int id) // удалить судью
        {
            try
            {
                await refereeService.DeleteRefereeAsync(id);
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

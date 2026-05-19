using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.RefereeAssignments;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для управления судейскими бригадами матчей
    [ApiController]
    [Route("api/[controller]")]
    public class RefereeAssignmentsController : ControllerBase
    {
        #region Поля
        private readonly IRefereeAssignmentService assignmentService; // сервис назначений
        #endregion

        #region Конструкторы
        public RefereeAssignmentsController(IRefereeAssignmentService assignmentService) // конструктор с внедрением зависимости
        {
            this.assignmentService = assignmentService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefereeAssignmentDto>>> GetByMatch([FromQuery] int matchId) // бригада матча
        {
            IEnumerable<RefereeAssignmentDto> result = await assignmentService.GetAssignmentsByMatchAsync(matchId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RefereeAssignmentDto>> GetById(int id) // назначение по id
        {
            RefereeAssignmentDto? result = await assignmentService.GetAssignmentByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Назначение с идентификатором {id} не найдено" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<RefereeAssignmentDto>> Create([FromBody] CreateRefereeAssignmentDto dto) // назначить судью
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                RefereeAssignmentDto result = await assignmentService.CreateAssignmentAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<RefereeAssignmentDto>> Update(int id, [FromBody] UpdateRefereeAssignmentDto dto) // обновить назначение
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                RefereeAssignmentDto result = await assignmentService.UpdateAssignmentAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<IActionResult> Delete(int id) // снять назначение
        {
            try
            {
                await assignmentService.DeleteAssignmentAsync(id);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Organizers;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с организаторами (модуль «Организаторы»)
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizersController : ControllerBase
    {
        #region Поля
        private readonly IOrganizerService organizerService; // сервис управления организаторами
        #endregion

        #region Конструкторы
        public OrganizersController(IOrganizerService organizerService) // конструктор с внедрением зависимости
        {
            this.organizerService = organizerService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizerDto>>> GetAll() // получить всех организаторов
        {
            IEnumerable<OrganizerDto> result = await organizerService.GetAllOrganizersAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrganizerDto>> GetById(int id) // получить организатора по id
        {
            OrganizerDto? result = await organizerService.GetOrganizerByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Организатор с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<OrganizerDto>> Create([FromBody] CreateOrganizerDto dto) // создать организатора
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            OrganizerDto result = await organizerService.CreateOrganizerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<OrganizerDto>> Update(int id, [FromBody] UpdateOrganizerDto dto) // обновить организатора
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                OrganizerDto result = await organizerService.UpdateOrganizerAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<IActionResult> Delete(int id) // удалить организатора
        {
            try
            {
                await organizerService.DeleteOrganizerAsync(id);
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

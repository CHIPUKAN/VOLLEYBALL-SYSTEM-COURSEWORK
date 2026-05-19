using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Venues;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с площадками (модуль «Площадки»)
    [ApiController]
    [Route("api/[controller]")]
    public class VenuesController : ControllerBase
    {
        #region Поля
        private readonly IVenueService venueService; // сервис управления площадками
        #endregion

        #region Конструкторы
        public VenuesController(IVenueService venueService) // конструктор с внедрением зависимости
        {
            this.venueService = venueService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetAll() // получить все площадки
        {
            IEnumerable<VenueDto> result = await venueService.GetAllVenuesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VenueDto>> GetById(int id) // получить площадку по id
        {
            VenueDto? result = await venueService.GetVenueByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Площадка с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<VenueDto>> Create([FromBody] CreateVenueDto dto) // создать площадку
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            VenueDto result = await venueService.CreateVenueAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<VenueDto>> Update(int id, [FromBody] UpdateVenueDto dto) // обновить площадку
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                VenueDto result = await venueService.UpdateVenueAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<IActionResult> Delete(int id) // удалить площадку
        {
            try
            {
                await venueService.DeleteVenueAsync(id);
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

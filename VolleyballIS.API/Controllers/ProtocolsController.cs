using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Protocols;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с протоколами матчей
    [ApiController]
    [Route("api/[controller]")]
    public class ProtocolsController : ControllerBase
    {
        #region Поля
        private readonly IProtocolService protocolService; // сервис протоколов
        #endregion

        #region Конструкторы
        public ProtocolsController(IProtocolService protocolService) // конструктор с внедрением зависимости
        {
            this.protocolService = protocolService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProtocolDto>>> GetAll([FromQuery] int? matchId) // протоколы (с фильтром по матчу)
        {
            if (matchId.HasValue)
            {
                ProtocolDto? protocol = await protocolService.GetProtocolByMatchAsync(matchId.Value);
                if (protocol == null)
                {
                    return Ok(Array.Empty<ProtocolDto>());
                }
                return Ok(new[] { protocol });
            }
            IEnumerable<ProtocolDto> result = await protocolService.GetAllProtocolsAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProtocolDto>> GetById(int id) // протокол по id
        {
            ProtocolDto? result = await protocolService.GetProtocolByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Протокол с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча,Организатор")]
        public async Task<ActionResult<ProtocolDto>> Create([FromBody] CreateProtocolDto dto) // создать протокол
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ProtocolDto result = await protocolService.CreateProtocolAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча,Организатор")]
        public async Task<ActionResult<ProtocolDto>> Update(int id, [FromBody] UpdateProtocolDto dto) // обновить протокол
        {
            try
            {
                ProtocolDto result = await protocolService.UpdateProtocolAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча,Организатор")]
        public async Task<IActionResult> Delete(int id) // удалить протокол
        {
            try
            {
                await protocolService.DeleteProtocolAsync(id);
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.ProtocolHistory;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с журналом изменений протоколов матчей
    [ApiController]
    [Route("api/protocols/{protocolId:int}/history")]
    public class ProtocolHistoriesController : ControllerBase
    {
        #region Поля
        private readonly IProtocolHistoryService historyService; // сервис журнала
        #endregion

        #region Конструкторы
        public ProtocolHistoriesController(IProtocolHistoryService historyService) // конструктор с внедрением зависимости
        {
            this.historyService = historyService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProtocolHistoryDto>>> GetAll(int protocolId) // все записи журнала протокола
        {
            IEnumerable<ProtocolHistoryDto> result = await historyService.GetByProtocolIdAsync(protocolId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProtocolHistoryDto>> GetById(int protocolId, int id) // запись журнала по id
        {
            ProtocolHistoryDto? result = await historyService.GetByIdAsync(id);
            if (result == null || result.ProtocolId != protocolId)
            {
                return NotFound(new { message = $"Запись журнала с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<ActionResult<ProtocolHistoryDto>> Create(int protocolId, [FromBody] CreateProtocolHistoryDto dto) // добавить запись вручную
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dto.ProtocolId = protocolId;
            try
            {
                ProtocolHistoryDto result = await historyService.CreateEntryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { protocolId, id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}

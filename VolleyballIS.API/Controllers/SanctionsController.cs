using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Sanctions;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с санкциями (карточками) в матчах
    [ApiController]
    [Route("api/matches/{matchId:int}/sanctions")]
    public class SanctionsController : ControllerBase
    {
        #region Поля
        private readonly ISanctionService sanctionService; // сервис санкций
        #endregion

        #region Конструкторы
        public SanctionsController(ISanctionService sanctionService) // конструктор с внедрением зависимости
        {
            this.sanctionService = sanctionService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanctionDto>>> GetAll(int matchId) // санкции матча
        {
            IEnumerable<SanctionDto> result = await sanctionService.GetSanctionsByMatchAsync(matchId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SanctionDto>> GetById(int matchId, int id) // санкция по id
        {
            SanctionDto? result = await sanctionService.GetSanctionByIdAsync(id);
            if (result == null || result.MatchId != matchId)
            {
                return NotFound(new { message = $"Санкция с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<ActionResult<SanctionDto>> Create(int matchId, [FromBody] CreateSanctionDto dto) // зарегистрировать санкцию
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dto.MatchId = matchId;
            try
            {
                SanctionDto result = await sanctionService.CreateSanctionAsync(dto);
                return CreatedAtAction(nameof(GetById), new { matchId, id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<ActionResult<SanctionDto>> Update(int matchId, int id, [FromBody] UpdateSanctionDto dto) // обновить санкцию
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SanctionDto? existing = await sanctionService.GetSanctionByIdAsync(id);
            if (existing == null || existing.MatchId != matchId)
            {
                return NotFound(new { message = $"Санкция с идентификатором {id} не найдена" });
            }
            try
            {
                SanctionDto result = await sanctionService.UpdateSanctionAsync(id, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,СекретарьМатча")]
        public async Task<IActionResult> Delete(int matchId, int id) // удалить санкцию
        {
            try
            {
                await sanctionService.DeleteSanctionAsync(id);
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

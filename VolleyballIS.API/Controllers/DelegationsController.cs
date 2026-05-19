using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Delegations;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с делегациями команд на матчи
    [ApiController]
    [Route("api/matches/{matchId:int}/delegations")]
    public class DelegationsController : ControllerBase
    {
        #region Поля
        private readonly IDelegationService delegationService; // сервис делегаций
        #endregion

        #region Конструкторы
        public DelegationsController(IDelegationService delegationService) // конструктор с внедрением зависимости
        {
            this.delegationService = delegationService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DelegationDto>>> GetAll(int matchId, [FromQuery] int? teamId) // делегация матча
        {
            IEnumerable<DelegationDto> result;
            if (teamId.HasValue)
            {
                result = await delegationService.GetDelegationByMatchTeamAsync(matchId, teamId.Value);
            }
            else
            {
                result = await delegationService.GetDelegationByMatchAsync(matchId);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DelegationDto>> GetById(int matchId, int id) // участник по id
        {
            DelegationDto? result = await delegationService.GetMemberByIdAsync(id);
            if (result == null || result.MatchId != matchId)
            {
                return NotFound(new { message = $"Участник делегации с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Тренер")]
        public async Task<ActionResult<DelegationDto>> Create(int matchId, [FromBody] CreateDelegationDto dto) // добавить участника
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dto.MatchId = matchId;
            DelegationDto result = await delegationService.CreateMemberAsync(dto);
            return CreatedAtAction(nameof(GetById), new { matchId, id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Тренер")]
        public async Task<ActionResult<DelegationDto>> Update(int matchId, int id, [FromBody] UpdateDelegationDto dto) // обновить участника
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                DelegationDto result = await delegationService.UpdateMemberAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Тренер")]
        public async Task<IActionResult> Delete(int matchId, int id) // удалить участника
        {
            try
            {
                await delegationService.DeleteMemberAsync(id);
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

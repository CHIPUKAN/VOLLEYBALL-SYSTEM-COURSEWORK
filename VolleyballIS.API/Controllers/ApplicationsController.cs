using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Applications;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с заявками команд на турниры
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        #region Поля
        private readonly IApplicationService applicationService; // сервис заявок
        #endregion

        #region Конструкторы
        public ApplicationsController(IApplicationService applicationService) // конструктор с внедрением зависимости
        {
            this.applicationService = applicationService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAll(
            [FromQuery] int? tournamentId,
            [FromQuery] int? teamId) // заявки (с фильтром по турниру или команде)
        {
            IEnumerable<ApplicationDto> result;
            if (tournamentId.HasValue)
            {
                result = await applicationService.GetApplicationsByTournamentAsync(tournamentId.Value);
            }
            else if (teamId.HasValue)
            {
                result = await applicationService.GetApplicationsByTeamAsync(teamId.Value);
            }
            else
            {
                result = await applicationService.GetAllApplicationsAsync();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApplicationDto>> GetById(int id) // заявка по id
        {
            ApplicationDto? result = await applicationService.GetApplicationByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Заявка с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды")]
        public async Task<ActionResult<ApplicationDto>> Create([FromBody] CreateApplicationDto dto) // создать заявку
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ApplicationDto result = await applicationService.CreateApplicationAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды,Организатор,ПредставительКоманды")]
        public async Task<ActionResult<ApplicationDto>> Update(int id, [FromBody] UpdateApplicationDto dto) // обновить статус заявки
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ApplicationDto result = await applicationService.UpdateApplicationAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<IActionResult> Delete(int id) // удалить заявку
        {
            try
            {
                await applicationService.DeleteApplicationAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{id:int}/players")]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды")]
        public async Task<ActionResult<ApplicationDto>> AddPlayer(int id, [FromBody] AddCompositionPlayerDto dto) // добавить игрока в заявку
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ApplicationDto result = await applicationService.AddPlayerToApplicationAsync(id, dto);
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

        [HttpDelete("{id:int}/players/{playerId:int}")]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды,Организатор,ПредставительКоманды")]
        public async Task<IActionResult> RemovePlayer(int id, int playerId) // убрать игрока из заявки
        {
            try
            {
                await applicationService.RemovePlayerFromApplicationAsync(id, playerId);
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

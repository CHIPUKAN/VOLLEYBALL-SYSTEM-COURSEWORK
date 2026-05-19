using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Teams;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с командами (модуль «Команды»)
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        #region Поля
        private readonly ITeamService teamService; // сервис управления командами
        #endregion

        #region Конструкторы
        public TeamsController(ITeamService teamService) // конструктор с внедрением зависимости
        {
            this.teamService = teamService;
        }
        #endregion

        #region Методы
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll() // получить все команды
        {
            IEnumerable<TeamDto> result = await teamService.GetAllTeamsAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamDto>> GetById(int id) // получить команду по id
        {
            TeamDto? result = await teamService.GetTeamByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Команда с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<TeamDto>> Create([FromBody] CreateTeamDto dto) // создать новую команду
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TeamDto result = await teamService.CreateTeamAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<TeamDto>> Update(int id, [FromBody] UpdateTeamDto dto) // обновить команду
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TeamDto result = await teamService.UpdateTeamAsync(id, dto);
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
        public async Task<IActionResult> Delete(int id) // удалить команду
        {
            try
            {
                await teamService.DeleteTeamAsync(id);
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

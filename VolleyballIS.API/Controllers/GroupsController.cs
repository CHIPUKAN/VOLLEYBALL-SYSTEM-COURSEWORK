using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Groups;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с группами турниров
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        #region Поля
        private readonly IGroupService groupService; // сервис управления группами
        #endregion

        #region Конструкторы
        public GroupsController(IGroupService groupService) // конструктор с внедрением зависимости
        {
            this.groupService = groupService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetByTournament([FromQuery] int tournamentId) // группы турнира
        {
            IEnumerable<GroupDto> result = await groupService.GetGroupsByTournamentAsync(tournamentId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GroupDto>> GetById(int id) // группа по id
        {
            GroupDto? result = await groupService.GetGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Группа с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<GroupDto>> Create([FromBody] CreateGroupDto dto) // создать группу
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                GroupDto result = await groupService.CreateGroupAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<GroupDto>> Update(int id, [FromBody] UpdateGroupDto dto) // обновить группу
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                GroupDto result = await groupService.UpdateGroupAsync(id, dto);
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
        public async Task<IActionResult> Delete(int id) // удалить группу
        {
            try
            {
                await groupService.DeleteGroupAsync(id);
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

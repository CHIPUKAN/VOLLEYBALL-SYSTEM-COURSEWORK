using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Players;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с игроками (модуль «Игроки»)
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        #region Поля
        private readonly IPlayerService playerService; // сервис управления игроками
        #endregion

        #region Конструкторы
        public PlayersController(IPlayerService playerService) // конструктор с внедрением зависимости
        {
            this.playerService = playerService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll([FromQuery] int? teamId) // получить всех игроков (с фильтром по команде)
        {
            IEnumerable<PlayerDto> result;
            if (teamId.HasValue)
            {
                result = await playerService.GetPlayersByTeamAsync(teamId.Value);
            }
            else
            {
                result = await playerService.GetAllPlayersAsync();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PlayerDto>> GetById(int id) // получить игрока по id
        {
            PlayerDto? result = await playerService.GetPlayerByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Игрок с идентификатором {id} не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды")]
        public async Task<ActionResult<PlayerDto>> Create([FromBody] CreatePlayerDto dto) // создать игрока
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PlayerDto result = await playerService.CreatePlayerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды")]
        public async Task<ActionResult<PlayerDto>> Update(int id, [FromBody] UpdatePlayerDto dto) // обновить игрока
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PlayerDto result = await playerService.UpdatePlayerAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,ТренерКоманды")]
        public async Task<IActionResult> Delete(int id) // удалить игрока
        {
            try
            {
                await playerService.DeletePlayerAsync(id);
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

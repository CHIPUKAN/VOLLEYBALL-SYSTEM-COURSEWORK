using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolleyballIS.Application.DTOs.Awards;
using VolleyballIS.Application.Services;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с наградами турниров
    [ApiController]
    [Route("api/[controller]")]
    public class AwardsController : ControllerBase
    {
        #region Поля
        private readonly IAwardService awardService; // сервис наград
        #endregion

        #region Конструкторы
        public AwardsController(IAwardService awardService) // конструктор с внедрением зависимости
        {
            this.awardService = awardService;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AwardDto>>> GetByTournament([FromQuery] int tournamentId) // награды турнира
        {
            IEnumerable<AwardDto> result = await awardService.GetAwardsByTournamentAsync(tournamentId);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AwardDto>> GetById(int id) // награда по id
        {
            AwardDto? result = await awardService.GetAwardByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = $"Награда с идентификатором {id} не найдена" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<AwardDto>> Create([FromBody] CreateAwardDto dto) // создать награду
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AwardDto result = await awardService.CreateAwardAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<ActionResult<AwardDto>> Update(int id, [FromBody] UpdateAwardDto dto) // обновить награду
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                AwardDto result = await awardService.UpdateAwardAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Суперадминистратор,Организатор")]
        public async Task<IActionResult> Delete(int id) // удалить награду
        {
            try
            {
                await awardService.DeleteAwardAsync(id);
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

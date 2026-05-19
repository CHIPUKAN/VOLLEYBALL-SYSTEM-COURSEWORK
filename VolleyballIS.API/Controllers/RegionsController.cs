using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolleyballIS.Domain.Entities;
using VolleyballIS.Infrastructure.Data;

namespace VolleyballIS.API.Controllers
{
    // Контроллер для работы с регионами (Т1. Регионы ОКТМО)
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        #region Поля
        private readonly VolleyballDbContext dbContext; // контекст базы данных
        #endregion

        #region Конструкторы
        public RegionsController(VolleyballDbContext dbContext) // конструктор с внедрением зависимости
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Методы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T1Region>>> GetAll() // получить все регионы
        {
            IEnumerable<T1Region> result = await dbContext.Regions
                .OrderBy(r => r.Name)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("{oktmo}")]
        public async Task<ActionResult<T1Region>> GetByOktmo(string oktmo) // получить регион по коду ОКТМО
        {
            T1Region? result = await dbContext.Regions.FindAsync(oktmo);
            if (result == null)
            {
                return NotFound(new { message = $"Регион с кодом ОКТМО '{oktmo}' не найден" });
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<T1Region>> Create([FromBody] RegionRequest dto) // создать регион
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool exists = await dbContext.Regions.AnyAsync(r => r.OktmoCode == dto.OktmoCode);
            if (exists)
            {
                return Conflict(new { message = $"Регион с кодом ОКТМО '{dto.OktmoCode}' уже существует" });
            }
            T1Region region = new T1Region(dto.OktmoCode, dto.Name);
            dbContext.Regions.Add(region);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByOktmo), new { oktmo = region.OktmoCode }, region);
        }

        [HttpPut("{oktmo}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<ActionResult<T1Region>> Update(string oktmo, [FromBody] RegionRequest dto) // обновить регион
        {
            T1Region? region = await dbContext.Regions.FindAsync(oktmo);
            if (region == null)
            {
                return NotFound(new { message = $"Регион с кодом ОКТМО '{oktmo}' не найден" });
            }
            region.Name = dto.Name;
            await dbContext.SaveChangesAsync();
            return Ok(region);
        }

        [HttpDelete("{oktmo}")]
        [Authorize(Roles = "Суперадминистратор")]
        public async Task<IActionResult> Delete(string oktmo) // удалить регион
        {
            T1Region? region = await dbContext.Regions.FindAsync(oktmo);
            if (region == null)
            {
                return NotFound(new { message = $"Регион с кодом ОКТМО '{oktmo}' не найден" });
            }
            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }

    // DTO для создания/обновления региона
    public class RegionRequest
    {
        #region Свойства
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(11, MinimumLength = 11)]
        public string OktmoCode { get; set; } = string.Empty; // код ОКТМО (11 цифр)

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string Name { get; set; } = string.Empty; // наименование региона
        #endregion
    }
}

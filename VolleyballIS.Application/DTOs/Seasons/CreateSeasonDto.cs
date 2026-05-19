using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Seasons
{
    // DTO для создания нового сезона
    public class CreateSeasonDto
    {
        #region Свойства
        [Required(ErrorMessage = "Наименование сезона обязательно")]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty; // наименование (например, «2024/2025»)

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateOnly StartDate { get; set; } // дата начала

        [Required(ErrorMessage = "Дата окончания обязательна")]
        public DateOnly EndDate { get; set; } // дата окончания

        public string Status { get; set; } = "активен"; // статус: «активен» / «завершён»
        #endregion
    }
}

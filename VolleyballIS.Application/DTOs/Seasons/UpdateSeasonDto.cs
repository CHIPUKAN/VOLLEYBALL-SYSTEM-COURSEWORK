using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Seasons
{
    // DTO для обновления сезона
    public class UpdateSeasonDto
    {
        #region Свойства
        [Required(ErrorMessage = "Наименование сезона обязательно")]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty; // новое наименование

        [Required]
        public DateOnly StartDate { get; set; } // новая дата начала

        [Required]
        public DateOnly EndDate { get; set; } // новая дата окончания

        public string Status { get; set; } = "активен"; // новый статус
        #endregion
    }
}

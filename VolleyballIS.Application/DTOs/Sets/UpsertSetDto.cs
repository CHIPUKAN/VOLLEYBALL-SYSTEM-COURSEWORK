using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Sets
{
    // DTO для создания/обновления партии (upsert)
    public class UpsertSetDto
    {
        #region Свойства
        [Required(ErrorMessage = "Номер партии обязателен")]
        [Range(1, 5, ErrorMessage = "Номер партии должен быть от 1 до 5")]
        public short SetNumber { get; set; } // номер партии

        [Range(0, 50, ErrorMessage = "Счёт должен быть от 0 до 50")]
        public short? HomeScore { get; set; } // счёт хозяев

        [Range(0, 50, ErrorMessage = "Счёт должен быть от 0 до 50")]
        public short? GuestScore { get; set; } // счёт гостей

        [Range(0, 120, ErrorMessage = "Продолжительность партии не может превышать 120 минут")]
        public short? DurationMin { get; set; } // продолжительность в минутах
        #endregion
    }
}

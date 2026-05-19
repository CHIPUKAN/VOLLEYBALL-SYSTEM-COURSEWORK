using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Venues
{
    // DTO для создания новой площадки
    public class CreateVenueDto
    {
        #region Свойства
        [Required(ErrorMessage = "Наименование площадки обязательно")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // наименование площадки

        [MaxLength(255)]
        public string? Address { get; set; } // адрес (необязательно)

        [Required(ErrorMessage = "Город обязателен")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty; // город расположения

        [Range(1, int.MaxValue, ErrorMessage = "Вместимость должна быть положительной")]
        public int? Capacity { get; set; } // вместимость (необязательно)
        #endregion
    }
}

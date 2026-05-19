using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Venues
{
    // DTO для обновления площадки
    public class UpdateVenueDto
    {
        #region Свойства
        [Required(ErrorMessage = "Наименование площадки обязательно")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // новое наименование площадки

        [MaxLength(255)]
        public string? Address { get; set; } // новый адрес (необязательно)

        [Required(ErrorMessage = "Город обязателен")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty; // новый город расположения

        [Range(1, int.MaxValue)]
        public int? Capacity { get; set; } // новая вместимость (необязательно)
        #endregion
    }
}

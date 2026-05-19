using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Awards
{
    // DTO для обновления награды
    public class UpdateAwardDto
    {
        #region Свойства
        [Required(ErrorMessage = "Код типа награды обязателен")]
        public short AwardTypeCode { get; set; } // код типа награды

        [Required(ErrorMessage = "Наименование обязательно")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // наименование

        public int? PlayerId { get; set; } // идентификатор игрока

        public int? TeamId { get; set; } // идентификатор команды
        #endregion
    }
}

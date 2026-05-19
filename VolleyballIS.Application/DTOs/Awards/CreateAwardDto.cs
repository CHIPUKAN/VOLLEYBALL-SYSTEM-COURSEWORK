using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Awards
{
    // DTO для создания награды по итогам турнира
    public class CreateAwardDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор турнира обязателен")]
        public int TournamentId { get; set; } // идентификатор турнира

        [Required(ErrorMessage = "Код типа награды обязателен")]
        public short AwardTypeCode { get; set; } // код типа награды (из С14)

        [Required(ErrorMessage = "Наименование награды обязательно")]
        [MaxLength(100, ErrorMessage = "Наименование не должно превышать 100 символов")]
        public string Name { get; set; } = string.Empty; // наименование награды

        public int? PlayerId { get; set; } // идентификатор игрока (для индивидуальной)

        public int? TeamId { get; set; } // идентификатор команды (для командной)
        #endregion
    }
}

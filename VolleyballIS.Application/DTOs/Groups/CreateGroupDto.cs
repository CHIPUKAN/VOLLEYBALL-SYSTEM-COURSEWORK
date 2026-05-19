using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Groups
{
    // DTO для создания группы турнира
    public class CreateGroupDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор турнира обязателен")]
        public int TournamentId { get; set; } // идентификатор турнира

        [Required(ErrorMessage = "Наименование группы обязательно")]
        [MaxLength(50, ErrorMessage = "Наименование не должно превышать 50 символов")]
        public string Name { get; set; } = string.Empty; // наименование группы
        #endregion
    }
}

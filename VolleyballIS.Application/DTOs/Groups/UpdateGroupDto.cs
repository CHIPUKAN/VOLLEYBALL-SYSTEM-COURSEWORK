using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Groups
{
    // DTO для обновления группы турнира
    public class UpdateGroupDto
    {
        #region Свойства
        [Required(ErrorMessage = "Наименование группы обязательно")]
        [MaxLength(50, ErrorMessage = "Наименование не должно превышать 50 символов")]
        public string Name { get; set; } = string.Empty; // новое наименование группы
        #endregion
    }
}

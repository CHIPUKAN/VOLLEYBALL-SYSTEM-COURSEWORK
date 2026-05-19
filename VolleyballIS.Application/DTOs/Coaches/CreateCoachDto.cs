using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Coaches
{
    // DTO для создания нового тренера
    public class CreateCoachDto
    {
        #region Свойства
        [Required(ErrorMessage = "Фамилия тренера обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // фамилия

        [Required(ErrorMessage = "Имя тренера обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // отчество (необязательно)

        [MaxLength(120), EmailAddress]
        public string? Email { get; set; } // электронная почта (необязательно)

        [MaxLength(20)]
        public string? Phone { get; set; } // телефон (необязательно)

        [MaxLength(40)]
        public string? Category { get; set; } // квалификационная категория (необязательно)
        #endregion
    }
}

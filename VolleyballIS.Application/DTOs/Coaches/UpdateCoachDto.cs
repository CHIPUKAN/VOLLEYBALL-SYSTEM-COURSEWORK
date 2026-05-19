using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Coaches
{
    // DTO для обновления данных тренера
    public class UpdateCoachDto
    {
        #region Свойства
        [Required(ErrorMessage = "Фамилия тренера обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // новая фамилия

        [Required(ErrorMessage = "Имя тренера обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // новое имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // новое отчество (необязательно)

        [MaxLength(120), EmailAddress]
        public string? Email { get; set; } // новый email (необязательно)

        [MaxLength(20)]
        public string? Phone { get; set; } // новый телефон (необязательно)

        [MaxLength(40)]
        public string? Category { get; set; } // новая категория (необязательно)
        #endregion
    }
}

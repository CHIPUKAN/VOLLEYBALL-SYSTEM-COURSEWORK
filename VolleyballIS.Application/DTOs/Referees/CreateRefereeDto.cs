using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Referees
{
    // DTO для создания нового судьи
    public class CreateRefereeDto
    {
        #region Свойства
        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // фамилия

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // отчество (необязательно)

        [MaxLength(30)]
        public string? Category { get; set; } // категория (необязательно)

        [MaxLength(30)]
        public string? LicenseNumber { get; set; } // номер лицензии (необязательно)

        [MaxLength(120), EmailAddress]
        public string? Email { get; set; } // email (необязательно)

        [MaxLength(20)]
        public string? Phone { get; set; } // телефон (необязательно)
        #endregion
    }
}

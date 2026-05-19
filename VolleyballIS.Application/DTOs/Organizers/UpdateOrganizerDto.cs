using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Organizers
{
    // DTO для обновления данных организатора
    public class UpdateOrganizerDto
    {
        #region Свойства
        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // новая фамилия

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // новое имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // отчество (необязательно)

        [MaxLength(120), EmailAddress]
        public string? Email { get; set; } // email (необязательно)

        [MaxLength(20)]
        public string? Phone { get; set; } // телефон (необязательно)
        #endregion
    }
}

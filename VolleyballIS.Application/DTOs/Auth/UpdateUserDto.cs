using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Auth
{
    // DTO для обновления данных пользователя администратором (пароль необязателен)
    public class UpdateUserDto
    {
        #region Свойства
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = string.Empty; // адрес электронной почты

        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string? Password { get; set; } // новый пароль (null — не менять)

        [Required(ErrorMessage = "Роль обязательна")]
        public string Role { get; set; } = string.Empty; // роль пользователя

        public string? FullName { get; set; } // полное имя

        public int? LinkedCoachId { get; set; }    // привязка к тренеру

        public int? LinkedPlayerId { get; set; }   // привязка к игроку

        public int? LinkedRefereeId { get; set; }  // привязка к судье

        public int? LinkedOrganizerId { get; set; } // привязка к организатору
        #endregion
    }
}

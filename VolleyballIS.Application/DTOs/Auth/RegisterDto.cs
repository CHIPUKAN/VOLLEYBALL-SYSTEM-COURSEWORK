using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Auth
{
    // DTO для регистрации нового пользователя (только для Суперадминистратора)
    public class RegisterDto
    {
        #region Свойства
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty; // адрес электронной почты

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string Password { get; set; } = string.Empty; // пароль

        [Required(ErrorMessage = "Роль обязательна")]
        public string Role { get; set; } = string.Empty; // роль пользователя

        public string? FullName { get; set; } // полное имя

        public int? LinkedCoachId { get; set; } // ссылка на тренера

        public int? LinkedPlayerId { get; set; } // ссылка на игрока

        public int? LinkedRefereeId { get; set; } // ссылка на судью

        public int? LinkedOrganizerId { get; set; } // ссылка на организатора
        #endregion
    }
}

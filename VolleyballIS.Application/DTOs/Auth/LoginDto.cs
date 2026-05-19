using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Auth
{
    // DTO для входа в систему
    public class LoginDto
    {
        #region Свойства
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty; // адрес электронной почты

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty; // пароль
        #endregion
    }
}

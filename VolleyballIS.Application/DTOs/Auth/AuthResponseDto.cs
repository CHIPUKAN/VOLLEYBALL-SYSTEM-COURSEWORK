namespace VolleyballIS.Application.DTOs.Auth
{
    // DTO ответа при успешной аутентификации
    public class AuthResponseDto
    {
        #region Свойства
        public string Token { get; set; } = string.Empty; // JWT-токен

        public UserDto User { get; set; } = null!; // данные авторизованного пользователя
        #endregion
    }
}

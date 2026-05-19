namespace VolleyballIS.Domain.Entities
{
    // Т0. Пользователь системы (аутентификация и авторизация)
    public class AppUser
    {
        #region Свойства
        public int Id { get; set; } // первичный ключ

        public string Email { get; set; } = string.Empty; // адрес электронной почты (логин)

        public string PasswordHash { get; set; } = string.Empty; // хэш пароля (PBKDF2)

        public string Role { get; set; } = string.Empty; // роль пользователя

        public string? FullName { get; set; } // полное имя пользователя

        public int? LinkedCoachId { get; set; } // ссылка на запись тренера (для роли «Тренер»)

        public int? LinkedPlayerId { get; set; } // ссылка на запись игрока (для роли «Игрок»)

        public int? LinkedRefereeId { get; set; } // ссылка на запись судьи (для роли «Судья»)

        public int? LinkedOrganizerId { get; set; } // ссылка на запись организатора (для роли «Организатор»)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // дата регистрации

        // навигационные свойства
        public T3Coach? Coach { get; set; }         // тренер
        public T6Player? Player { get; set; }       // игрок
        public T12Referee? Referee { get; set; }    // судья
        public T13Organizer? Organizer { get; set; } // организатор
        #endregion

        #region Конструкторы
        public AppUser() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion
    }
}

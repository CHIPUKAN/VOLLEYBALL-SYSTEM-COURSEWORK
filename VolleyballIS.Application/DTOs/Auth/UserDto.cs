namespace VolleyballIS.Application.DTOs.Auth
{
    // DTO для передачи данных пользователя на клиент
    public class UserDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор пользователя

        public string Email { get; set; } = string.Empty; // email

        public string Role { get; set; } = string.Empty; // роль

        public string? FullName { get; set; } // полное имя

        public int? LinkedCoachId { get; set; } // ссылка на тренера

        public int? LinkedPlayerId { get; set; } // ссылка на игрока

        public int? LinkedRefereeId { get; set; } // ссылка на судью

        public int? LinkedOrganizerId { get; set; } // ссылка на организатора

        public DateTime CreatedAt { get; set; } // дата регистрации
        #endregion
    }
}

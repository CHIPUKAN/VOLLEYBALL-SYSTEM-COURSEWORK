namespace VolleyballIS.Application.DTOs.Coaches
{
    // DTO для передачи данных о тренере на клиент
    public class CoachDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор тренера

        public string LastName { get; set; } = string.Empty; // фамилия

        public string FirstName { get; set; } = string.Empty; // имя

        public string? MiddleName { get; set; } // отчество

        public string FullName { get; set; } = string.Empty; // полное ФИО

        public string? Email { get; set; } // электронная почта

        public string? Phone { get; set; } // телефон

        public string? Category { get; set; } // квалификационная категория
        #endregion
    }
}

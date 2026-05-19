namespace VolleyballIS.Application.DTOs.Referees
{
    // DTO для передачи данных о судье на клиент
    public class RefereeDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор судьи

        public string LastName { get; set; } = string.Empty; // фамилия

        public string FirstName { get; set; } = string.Empty; // имя

        public string? MiddleName { get; set; } // отчество

        public string FullName { get; set; } = string.Empty; // полное ФИО

        public string? Category { get; set; } // категория (FIVB / нац. / рег. / местная)

        public string? LicenseNumber { get; set; } // номер лицензии

        public string? Email { get; set; } // электронная почта

        public string? Phone { get; set; } // телефон
        #endregion
    }
}

namespace VolleyballIS.Application.DTOs.Organizers
{
    // DTO для передачи данных об организаторе на клиент
    public class OrganizerDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор организатора

        public string LastName { get; set; } = string.Empty; // фамилия

        public string FirstName { get; set; } = string.Empty; // имя

        public string? MiddleName { get; set; } // отчество

        public string FullName { get; set; } = string.Empty; // полное ФИО

        public string? Email { get; set; } // электронная почта

        public string? Phone { get; set; } // телефон
        #endregion
    }
}

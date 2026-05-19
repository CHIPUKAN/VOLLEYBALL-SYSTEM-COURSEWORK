namespace VolleyballIS.Application.DTOs.Seasons
{
    // DTO для передачи данных о сезоне на клиент
    public class SeasonDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор сезона

        public string Name { get; set; } = string.Empty; // наименование сезона

        public DateOnly StartDate { get; set; } // дата начала

        public DateOnly EndDate { get; set; } // дата окончания

        public string Status { get; set; } = string.Empty; // статус: «активен» / «завершён»
        #endregion
    }
}

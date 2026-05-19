namespace VolleyballIS.Application.DTOs.Venues
{
    // DTO для передачи данных о площадке на клиент
    public class VenueDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор площадки

        public string Name { get; set; } = string.Empty; // наименование площадки

        public string? Address { get; set; } // адрес площадки

        public string City { get; set; } = string.Empty; // город расположения

        public int? Capacity { get; set; } // вместимость
        #endregion
    }
}

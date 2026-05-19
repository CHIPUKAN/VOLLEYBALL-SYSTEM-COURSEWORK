namespace VolleyballIS.Application.DTOs.Players
{
    // DTO для передачи данных об игроке на клиент
    public class PlayerDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор игрока

        public int? TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public string LastName { get; set; } = string.Empty; // фамилия

        public string FirstName { get; set; } = string.Empty; // имя

        public string? MiddleName { get; set; } // отчество

        public string FullName { get; set; } = string.Empty; // полное ФИО

        public DateOnly BirthDate { get; set; } // дата рождения

        public short? HeightCm { get; set; } // рост (см)

        public short? WeightKg { get; set; } // вес (кг)

        public short? JerseyNumber { get; set; } // номер на футболке

        public short AmpluaCode { get; set; } // код амплуа

        public string? AmpluaName { get; set; } // наименование амплуа

        public string? SportsRank { get; set; } // спортивный разряд

        public string? Email { get; set; } // электронная почта

        public string? Phone { get; set; } // телефон

        public short StatusCode { get; set; } // код статуса

        public string? StatusName { get; set; } // наименование статуса

        public string? PhotoUrl { get; set; } // URL фотографии
        #endregion
    }
}

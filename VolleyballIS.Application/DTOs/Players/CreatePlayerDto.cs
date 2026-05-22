using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Players
{
    // DTO для создания нового игрока
    public class CreatePlayerDto
    {
        #region Свойства
        public int? TeamId { get; set; } // идентификатор команды (необязательно)

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // фамилия

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // отчество (необязательно)

        [Required(ErrorMessage = "Дата рождения обязательна")]
        public DateOnly BirthDate { get; set; } // дата рождения

        [Range(150, 220)]
        public short? HeightCm { get; set; } // рост (необязательно)

        [Range(50, 150)]
        public short? WeightKg { get; set; } // вес (необязательно)

        [Range(1, 99)]
        public short? JerseyNumber { get; set; } // номер на футболке (необязательно)

        [Required(ErrorMessage = "Амплуа обязательно")]
        [Range(1, short.MaxValue, ErrorMessage = "Код амплуа должен быть положительным")]
        public short AmpluaCode { get; set; } // код амплуа

        [MaxLength(30)]
        public string? SportsRank { get; set; } // разряд (необязательно)

        [MaxLength(120), EmailAddress]
        public string? Email { get; set; } // email (необязательно)

        [MaxLength(20)]
        public string? Phone { get; set; } // телефон (необязательно)

        public short StatusCode { get; set; } = 1; // код статуса (по умолчанию «активен»)

        public string? PhotoUrl { get; set; } // URL фото (необязательно)
        #endregion
    }
}

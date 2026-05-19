using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Players
{
    // DTO для обновления данных игрока
    public class UpdatePlayerDto
    {
        #region Свойства
        public int? TeamId { get; set; } // новый идентификатор команды (необязательно)

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // новая фамилия

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // новое имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // новое отчество (необязательно)

        [Required]
        public DateOnly BirthDate { get; set; } // дата рождения

        [Range(150, 220)]
        public short? HeightCm { get; set; } // рост (необязательно)

        [Range(50, 150)]
        public short? WeightKg { get; set; } // вес (необязательно)

        [Range(1, 99)]
        public short? JerseyNumber { get; set; } // номер на футболке (необязательно)

        [Required]
        public short AmpluaCode { get; set; } // код амплуа

        [MaxLength(30)]
        public string? SportsRank { get; set; } // разряд (необязательно)

        [MaxLength(120), EmailAddress]
        public string? Email { get; set; } // email (необязательно)

        [MaxLength(20)]
        public string? Phone { get; set; } // телефон (необязательно)

        public short StatusCode { get; set; } = 1; // код статуса

        public string? PhotoUrl { get; set; } // URL фото (необязательно)
        #endregion
    }
}

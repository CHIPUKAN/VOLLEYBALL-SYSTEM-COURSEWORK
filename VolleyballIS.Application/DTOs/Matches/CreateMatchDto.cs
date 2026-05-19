using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Matches
{
    // DTO для создания нового матча
    public class CreateMatchDto
    {
        #region Свойства
        [Required(ErrorMessage = "Турнир обязателен")]
        public int TournamentId { get; set; } // идентификатор турнира

        [Required(ErrorMessage = "Команда-хозяин обязательна")]
        public int HomeTeamId { get; set; } // идентификатор команды-хозяина

        [Required(ErrorMessage = "Команда-гость обязательна")]
        public int GuestTeamId { get; set; } // идентификатор команды-гостя

        [Required(ErrorMessage = "Дата матча обязательна")]
        public DateOnly MatchDate { get; set; } // дата матча

        [Required(ErrorMessage = "Время начала обязательно")]
        public TimeOnly StartTime { get; set; } // время начала

        public TimeOnly? EndTime { get; set; } // время окончания (необязательно)

        [Required(ErrorMessage = "Площадка обязательна")]
        public int VenueId { get; set; } // идентификатор площадки

        [Required(ErrorMessage = "Этап турнира обязателен")]
        public short StageCode { get; set; } // код этапа (из справочника С5)

        public int? GroupId { get; set; } // идентификатор группы (необязательно)

        public short StatusCode { get; set; } = 1; // код статуса (по умолчанию «Запланирован»)

        public string? TechDefeatReason { get; set; } // причина технического поражения

        public int? CoinTossWinnerTeamId { get; set; } // команда-победитель жеребьёвки

        public short? CoinTossChoiceCode { get; set; } // выбор победителя жеребьёвки (из С17)

        public int? FirstServeTeamId { get; set; } // команда, выполняющая первую подачу

        public bool HasVideoChallenge { get; set; } // наличие системы видеопросмотра

        [Range(2.20, 2.50, ErrorMessage = "Высота сетки должна быть от 2.20 до 2.50 м")]
        public decimal? NetHeight { get; set; } // высота сетки в метрах
        #endregion
    }
}

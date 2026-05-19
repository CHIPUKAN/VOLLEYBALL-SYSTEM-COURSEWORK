using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Matches
{
    // DTO для обновления матча
    public class UpdateMatchDto
    {
        #region Свойства
        [Required]
        public int TournamentId { get; set; } // новый идентификатор турнира

        [Required]
        public int HomeTeamId { get; set; } // новая команда-хозяин

        [Required]
        public int GuestTeamId { get; set; } // новая команда-гость

        [Required]
        public DateOnly MatchDate { get; set; } // новая дата матча

        [Required]
        public TimeOnly StartTime { get; set; } // новое время начала

        public TimeOnly? EndTime { get; set; } // новое время окончания (необязательно)

        [Required]
        public int VenueId { get; set; } // новая площадка

        [Required]
        public short StageCode { get; set; } // новый этап

        public int? GroupId { get; set; } // новая группа (необязательно)

        public short StatusCode { get; set; } = 1; // новый статус

        public string? TechDefeatReason { get; set; } // причина технического поражения (необязательно)

        public int? CoinTossWinnerTeamId { get; set; } // команда-победитель жеребьёвки

        public short? CoinTossChoiceCode { get; set; } // выбор победителя жеребьёвки (из С17)

        public int? FirstServeTeamId { get; set; } // команда, выполняющая первую подачу

        public bool HasVideoChallenge { get; set; } // наличие видеопросмотра

        [Range(2.20, 2.50)]
        public decimal? NetHeight { get; set; } // высота сетки (необязательно)
        #endregion
    }
}

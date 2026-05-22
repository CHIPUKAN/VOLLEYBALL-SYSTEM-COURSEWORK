namespace VolleyballIS.Application.DTOs.Matches
{
    // DTO для передачи данных о матче на клиент
    public class MatchDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор матча

        public int TournamentId { get; set; } // идентификатор турнира

        public string? TournamentName { get; set; } // наименование турнира

        public int HomeTeamId { get; set; } // идентификатор команды-хозяина

        public string? HomeTeamName { get; set; } // наименование команды-хозяина

        public int GuestTeamId { get; set; } // идентификатор команды-гостя

        public string? GuestTeamName { get; set; } // наименование команды-гостя

        public DateOnly MatchDate { get; set; } // дата матча

        public TimeOnly StartTime { get; set; } // время начала

        public TimeOnly? EndTime { get; set; } // время окончания

        public int VenueId { get; set; } // идентификатор площадки

        public string? VenueName { get; set; } // наименование площадки

        public string? VenueCity { get; set; } // город площадки

        public short StageCode { get; set; } // код этапа

        public string? StageName { get; set; } // наименование этапа

        public int? GroupId { get; set; } // идентификатор группы

        public string? GroupName { get; set; } // наименование группы

        public short StatusCode { get; set; } // код статуса

        public string? StatusName { get; set; } // наименование статуса

        public string? TechDefeatReason { get; set; } // причина технического поражения

        public int? CoinTossWinnerTeamId { get; set; } // команда-победитель жеребьёвки

        public string? CoinTossWinnerTeamName { get; set; } // название команды-победителя жеребьёвки

        public short? CoinTossChoiceCode { get; set; } // выбор победителя жеребьёвки (из С17)

        public string? CoinTossChoiceName { get; set; } // наименование выбора жеребьёвки

        public int? FirstServeTeamId { get; set; } // команда, выполняющая первую подачу

        public string? FirstServeTeamName { get; set; } // название команды первой подачи

        public bool HasVideoChallenge { get; set; } // наличие видеопросмотра

        public decimal? NetHeight { get; set; } // высота сетки
        #endregion
    }
}

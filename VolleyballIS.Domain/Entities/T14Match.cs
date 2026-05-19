namespace VolleyballIS.Domain.Entities
{
    // Т14. Матч
    public class T14Match
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор матча
        {
            get => id;
            set => id = value;
        }

        public int TournamentId { get; set; } // идентификатор турнира (внешний ключ)

        public int HomeTeamId { get; set; } // идентификатор команды-хозяина (внешний ключ)

        public int GuestTeamId { get; set; } // идентификатор команды-гостя (внешний ключ)

        public DateOnly MatchDate { get; set; } // дата матча

        public TimeOnly StartTime { get; set; } // время начала матча

        public TimeOnly? EndTime { get; set; } // время окончания матча (необязательно)

        public int VenueId { get; set; } // идентификатор площадки (внешний ключ)

        public short StageCode { get; set; } // код этапа турнира (внешний ключ -> s5_tournament_stages)

        public int? GroupId { get; set; } // идентификатор группы (NULL = плей-офф)

        public short StatusCode { get; set; } = 1; // код статуса матча (внешний ключ -> s6_match_statuses)

        public string? TechDefeatReason { get; set; } // причина технического поражения (необязательно)

        public int? CoinTossWinnerTeamId { get; set; } // команда-победитель жеребьёвки (необязательно)

        public short? CoinTossChoiceCode { get; set; } // выбор победителя жеребьёвки (необязательно)

        public int? FirstServeTeamId { get; set; } // команда, выполняющая первую подачу (необязательно)

        public decimal? NetHeight { get; set; } // высота сетки в метрах (необязательно)

        public bool HasVideoChallenge { get; set; } // наличие системы видеопросмотра

        // навигационные свойства
        public T10Tournament? Tournament { get; set; }          // турнир
        public T4Team? HomeTeam { get; set; }                   // команда-хозяин
        public T4Team? GuestTeam { get; set; }                  // команда-гость
        public T2Venue? Venue { get; set; }                     // площадка проведения
        public S5TournamentStage? Stage { get; set; }           // этап турнира
        public T11Group? Group { get; set; }                    // группа (если групповой этап)
        public S6MatchStatus? Status { get; set; }              // статус матча
        public S17CoinTossOption? CoinTossChoice { get; set; } // вариант выбора при жеребьёвке
        public ICollection<R3Set> Sets { get; set; } = new List<R3Set>(); // партии матча
        #endregion

        #region Конструкторы
        public T14Match() // конструктор по умолчанию (нужен EF Core)
        {
        }

        public T14Match(int tournamentId, int homeTeamId, int guestTeamId, DateOnly matchDate, TimeOnly startTime, int venueId, short stageCode) // конструктор с параметрами
        {
            TournamentId = tournamentId;
            HomeTeamId = homeTeamId;
            GuestTeamId = guestTeamId;
            MatchDate = matchDate;
            StartTime = startTime;
            VenueId = venueId;
            StageCode = stageCode;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Матч {MatchDate:dd.MM.yyyy} (турнир {TournamentId})";
        }
        #endregion
    }
}

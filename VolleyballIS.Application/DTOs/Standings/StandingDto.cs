namespace VolleyballIS.Application.DTOs.Standings
{
    // DTO для строки турнирной таблицы
    public class StandingDto
    {
        #region Свойства
        public int Rank { get; set; } // место в таблице

        public int TeamId { get; set; } // идентификатор команды

        public string TeamName { get; set; } = string.Empty; // наименование команды

        public int GamesPlayed { get; set; } // сыграно матчей

        public int Wins { get; set; } // победы

        public int Losses { get; set; } // поражения

        public int SetsWon { get; set; } // выигранные партии

        public int SetsLost { get; set; } // проигранные партии

        public int PointsWon { get; set; } // выигранные мячи

        public int PointsLost { get; set; } // проигранные мячи

        public int Points { get; set; } // турнирные очки
        #endregion
    }
}

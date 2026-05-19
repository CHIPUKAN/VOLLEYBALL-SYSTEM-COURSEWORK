namespace VolleyballIS.Application.DTOs.StartingLineups
{
    // DTO для отображения позиции в стартовой расстановке
    public class StartingLineupDto
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча

        public int TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public short SetNumber { get; set; } // номер партии

        public short PositionNo { get; set; } // позиция на площадке (1–6)

        public int PlayerId { get; set; } // идентификатор игрока

        public string? PlayerFullName { get; set; } // ФИО игрока
        #endregion
    }
}

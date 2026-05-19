namespace VolleyballIS.Application.DTOs.PlayerStats
{
    // DTO для отображения статистики игрока в матче
    public class PlayerStatsDto
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча

        public int PlayerId { get; set; } // идентификатор игрока

        public string? PlayerFullName { get; set; } // ФИО игрока

        public int TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public short ServesTotal { get; set; }        // всего подач

        public short Aces { get; set; }               // эйсы

        public short ServeErrors { get; set; }        // ошибки на подаче

        public short ReceptionsTotal { get; set; }    // всего приёмов

        public short PositiveReceptions { get; set; } // позитивные приёмы

        public short ReceptionErrors { get; set; }    // ошибки в приёме

        public short AttacksTotal { get; set; }       // всего атак

        public short AttackPoints { get; set; }       // очки в атаке

        public short AttackErrors { get; set; }       // ошибки в атаке

        public short Blocks { get; set; }             // блоки

        public short TotalPoints { get; set; }        // всего очков
        #endregion
    }
}

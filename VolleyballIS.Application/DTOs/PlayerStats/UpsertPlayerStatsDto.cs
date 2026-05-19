using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.PlayerStats
{
    // DTO для создания/обновления статистики игрока в матче
    public class UpsertPlayerStatsDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор игрока обязателен")]
        public int PlayerId { get; set; } // идентификатор игрока

        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        public int TeamId { get; set; } // идентификатор команды

        [Range(0, 200)] public short ServesTotal { get; set; }        // всего подач

        [Range(0, 100)] public short Aces { get; set; }               // эйсы

        [Range(0, 100)] public short ServeErrors { get; set; }        // ошибки на подаче

        [Range(0, 300)] public short ReceptionsTotal { get; set; }    // всего приёмов

        [Range(0, 300)] public short PositiveReceptions { get; set; } // позитивные приёмы

        [Range(0, 100)] public short ReceptionErrors { get; set; }    // ошибки в приёме

        [Range(0, 200)] public short AttacksTotal { get; set; }       // всего атак

        [Range(0, 100)] public short AttackPoints { get; set; }       // очки в атаке

        [Range(0, 100)] public short AttackErrors { get; set; }       // ошибки в атаке

        [Range(0, 50)]  public short Blocks { get; set; }             // блоки

        [Range(0, 150)] public short TotalPoints { get; set; }        // всего очков
        #endregion
    }
}

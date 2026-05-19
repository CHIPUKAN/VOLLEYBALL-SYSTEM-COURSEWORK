namespace VolleyballIS.Domain.Entities
{
    // Р2. Статистика игрока в матче (суммарно)
    public class R2PlayerStats
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча (часть составного PK)

        public int PlayerId { get; set; } // идентификатор игрока (часть составного PK)

        public int TeamId { get; set; } // идентификатор команды

        public short ServesTotal { get; set; }      // всего подач

        public short Aces { get; set; }             // эйсы

        public short ServeErrors { get; set; }      // ошибки на подаче

        public short ReceptionsTotal { get; set; }  // всего приёмов

        public short PositiveReceptions { get; set; } // позитивные приёмы

        public short ReceptionErrors { get; set; }  // ошибки в приёме

        public short AttacksTotal { get; set; }     // всего атак

        public short AttackPoints { get; set; }     // очки в атаке

        public short AttackErrors { get; set; }     // ошибки в атаке

        public short Blocks { get; set; }           // блоки

        public short TotalPoints { get; set; }      // всего очков

        // навигационные свойства
        public T14Match? Match { get; set; }  // матч
        public T6Player? Player { get; set; } // игрок
        public T4Team? Team { get; set; }     // команда
        #endregion

        #region Конструкторы
        public R2PlayerStats() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Статистика: матч {MatchId}, игрок {PlayerId}, очков {TotalPoints}";
        }
        #endregion
    }
}

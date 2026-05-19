namespace VolleyballIS.Domain.Entities
{
    // Р1. Стартовая расстановка команды в партии
    public class R1StartingLineup
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча (часть составного PK)

        public int TeamId { get; set; } // идентификатор команды (часть составного PK)

        public short SetNumber { get; set; } // номер партии (1–5, часть составного PK)

        public short PositionNo { get; set; } // номер позиции на площадке (1–6, часть составного PK)

        public int PlayerId { get; set; } // идентификатор игрока на данной позиции

        // навигационные свойства
        public T14Match? Match { get; set; }  // матч
        public T4Team? Team { get; set; }     // команда
        public T6Player? Player { get; set; } // игрок
        #endregion

        #region Конструкторы
        public R1StartingLineup() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Расстановка: матч {MatchId}, команда {TeamId}, партия {SetNumber}, позиция {PositionNo}";
        }
        #endregion
    }
}

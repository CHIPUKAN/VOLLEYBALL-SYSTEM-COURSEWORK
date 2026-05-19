namespace VolleyballIS.Domain.Entities
{
    // Т21. Капитан команды на конкретный матч (согласно пр.4.1.3 ФИВБ)
    public class T21MatchCaptain
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча (часть составного PK)

        public int TeamId { get; set; } // идентификатор команды (часть составного PK)

        public int PlayerId { get; set; } // идентификатор игрока-капитана

        // навигационные свойства
        public T14Match? Match { get; set; }   // матч
        public T4Team? Team { get; set; }      // команда
        public T6Player? Player { get; set; }  // игрок-капитан
        #endregion

        #region Конструкторы
        public T21MatchCaptain() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Капитан команды {TeamId} в матче {MatchId}: игрок {PlayerId}";
        }
        #endregion
    }
}

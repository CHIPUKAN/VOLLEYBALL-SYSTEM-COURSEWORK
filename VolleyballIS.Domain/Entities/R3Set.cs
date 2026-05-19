namespace VolleyballIS.Domain.Entities
{
    // Р3. Партия матча
    public class R3Set
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча (часть составного PK)

        public short SetNumber { get; set; } // номер партии (1–5, часть составного PK)

        public short? HomeScore { get; set; } // счёт хозяев в партии (необязательно — пока не сыграна)

        public short? GuestScore { get; set; } // счёт гостей в партии (необязательно)

        public short? DurationMin { get; set; } // продолжительность партии в минутах (необязательно)

        // навигационное свойство
        public T14Match? Match { get; set; } // матч
        #endregion

        #region Конструкторы
        public R3Set() // конструктор по умолчанию (нужен EF Core)
        {
        }

        public R3Set(int matchId, short setNumber) // конструктор с параметрами
        {
            MatchId = matchId;
            SetNumber = setNumber;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Партия {SetNumber} матча {MatchId}: {HomeScore}:{GuestScore}";
        }
        #endregion
    }
}

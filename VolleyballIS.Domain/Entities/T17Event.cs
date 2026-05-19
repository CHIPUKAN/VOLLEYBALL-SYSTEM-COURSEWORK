namespace VolleyballIS.Domain.Entities
{
    // Т17. Игровое событие
    public class T17Event
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор события
        {
            get => id;
            set => id = value;
        }

        public int MatchId { get; set; } // идентификатор матча (внешний ключ)

        public int? TeamId { get; set; } // идентификатор команды (NULL для матчевых событий)

        public short EventTypeCode { get; set; } // код типа события (внешний ключ -> s9_event_types)

        public short SetNumber { get; set; } // номер партии (1–5)

        public int GlobalSeqInSet { get; set; } // глобальный порядковый номер события в партии

        public short HomeScoreAtMoment { get; set; } // счёт хозяев в момент события

        public short GuestScoreAtMoment { get; set; } // счёт гостей в момент события

        public short? MinuteMark { get; set; } // игровая минута (необязательно)

        // навигационные свойства
        public T14Match? Match { get; set; }       // матч
        public T4Team? Team { get; set; }          // команда-инициатор (если командное событие)
        public S9EventType? EventType { get; set; } // тип события
        public T17aSubstitution? Substitution { get; set; } // детали замены (если событие типа 1/9)
        public T17bTimeout? Timeout { get; set; }            // детали тайм-аута (если событие типа 2/3)
        #endregion

        #region Конструкторы
        public T17Event() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Событие матча {MatchId}, партия {SetNumber}, поз. {GlobalSeqInSet}";
        }
        #endregion
    }
}

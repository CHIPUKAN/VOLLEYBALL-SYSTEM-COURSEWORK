namespace VolleyballIS.Domain.Entities
{
    // Т17а. Детали замены или переназначения либеро
    public class T17aSubstitution
    {
        #region Свойства
        public int EventId { get; set; } // идентификатор события (первичный ключ = внешний ключ -> t17_events)

        public int SubOutPlayerId { get; set; } // идентификатор заменяемого игрока

        public int SubInPlayerId { get; set; } // идентификатор выходящего игрока

        public short? SubTypeCode { get; set; } // код типа замены (необязательно, внешний ключ -> s10_substitution_types)

        public short? SubSeqInSet { get; set; } // порядковый номер обычной замены в партии (1–6)

        public bool IsLiberoSwap { get; set; } // признак замещения либеро (не засчитывается в лимит)

        // навигационные свойства
        public T17Event? Event { get; set; }        // родительское событие
        public T6Player? SubOutPlayer { get; set; } // заменяемый игрок
        public T6Player? SubInPlayer { get; set; }  // выходящий игрок
        public S10SubstitutionType? SubType { get; set; } // тип замены
        #endregion

        #region Конструкторы
        public T17aSubstitution() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Замена [событие {EventId}]: игрок {SubOutPlayerId} -> {SubInPlayerId}";
        }
        #endregion
    }
}

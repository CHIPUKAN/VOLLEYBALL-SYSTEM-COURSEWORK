namespace VolleyballIS.Domain.Entities
{
    // Т17б. Детали тайм-аута
    public class T17bTimeout
    {
        #region Свойства
        public int EventId { get; set; } // идентификатор события (первичный ключ = внешний ключ -> t17_events)

        public short TimeoutTypeCode { get; set; } // код типа тайм-аута (внешний ключ -> s11_timeout_types)

        public short? TimeoutSeqInSet { get; set; } // порядковый номер тайм-аута команды в партии (1–2)

        // навигационные свойства
        public T17Event? Event { get; set; }            // родительское событие
        public S11TimeoutType? TimeoutType { get; set; } // тип тайм-аута
        #endregion

        #region Конструкторы
        public T17bTimeout() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Тайм-аут [событие {EventId}]: тип {TimeoutTypeCode}";
        }
        #endregion
    }
}

namespace VolleyballIS.Domain.Entities
{
    // Т8. Состав заявки (игроки в заявке команды на турнир)
    public class T8ApplicationComposition
    {
        #region Свойства
        public int ApplicationId { get; set; } // идентификатор заявки (часть составного PK)

        public int PlayerId { get; set; } // идентификатор игрока (часть составного PK)

        public short JerseyNumberInApp { get; set; } // номер игрока в заявке (1–99)

        public string Role { get; set; } = "основной"; // роль: «основной» или «запасной»

        public bool IsLibero { get; set; } // признак либеро (не более 2 на заявку)

        // навигационные свойства
        public T7Application? Application { get; set; } // заявка
        public T6Player? Player { get; set; }            // игрок
        #endregion

        #region Конструкторы
        public T8ApplicationComposition() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"Заявка {ApplicationId}, игрок {PlayerId}, №{JerseyNumberInApp}";
        }
        #endregion
    }
}

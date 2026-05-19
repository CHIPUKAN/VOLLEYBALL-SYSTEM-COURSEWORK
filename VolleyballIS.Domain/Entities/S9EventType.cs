namespace VolleyballIS.Domain.Entities
{
    // С9. Типы игровых событий
    public class S9EventType
    {
        #region Поля
        private short code;  // код типа события (первичный ключ)
        private string name; // наименование типа события
        #endregion

        #region Свойства
        public short Code // код типа события — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование типа события
        {
            get => name;
            set => name = value;
        }

        public bool IsTeamEvent { get; set; } // признак: событие инициировано командой (team_id обязателен)

        public string? Description { get; set; } // описание типа события (необязательно)
        #endregion

        #region Конструкторы
        public S9EventType() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Code}] {Name}";
        }
        #endregion
    }
}

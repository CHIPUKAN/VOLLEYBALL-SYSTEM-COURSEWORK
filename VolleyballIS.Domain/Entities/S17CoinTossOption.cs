namespace VolleyballIS.Domain.Entities
{
    // С17. Варианты выбора победителя жеребьёвки
    public class S17CoinTossOption
    {
        #region Поля
        private short code; // код варианта (первичный ключ)
        private string name; // наименование варианта
        #endregion

        #region Свойства
        public short Code // код варианта жеребьёвки — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование варианта жеребьёвки
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S17CoinTossOption() // конструктор по умолчанию (нужен EF Core)
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

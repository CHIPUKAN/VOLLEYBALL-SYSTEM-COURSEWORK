namespace VolleyballIS.Domain.Entities
{
    // С14. Типы наград
    public class S14AwardType
    {
        #region Поля
        private short code; // код типа награды (первичный ключ)
        private string name; // наименование типа награды
        #endregion

        #region Свойства
        public short Code // код типа награды — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование типа награды
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S14AwardType() // конструктор по умолчанию (нужен EF Core)
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

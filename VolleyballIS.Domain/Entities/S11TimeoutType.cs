namespace VolleyballIS.Domain.Entities
{
    // С11. Типы тайм-аутов
    public class S11TimeoutType
    {
        #region Поля
        private short code; // код типа тайм-аута (первичный ключ)
        private string name; // наименование типа тайм-аута
        #endregion

        #region Свойства
        public short Code // код типа тайм-аута — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование типа тайм-аута
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S11TimeoutType() // конструктор по умолчанию (нужен EF Core)
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

namespace VolleyballIS.Domain.Entities
{
    // С7. Статусы протоколов
    public class S7ProtocolStatus
    {
        #region Поля
        private short code; // код статуса (первичный ключ)
        private string name; // наименование статуса
        #endregion

        #region Свойства
        public short Code // код статуса — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование статуса
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S7ProtocolStatus() // конструктор по умолчанию (нужен EF Core)
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

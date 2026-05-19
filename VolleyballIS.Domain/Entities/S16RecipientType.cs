namespace VolleyballIS.Domain.Entities
{
    // С16. Типы получателей санкций
    public class S16RecipientType
    {
        #region Поля
        private short code; // код типа получателя (первичный ключ)
        private string name; // наименование типа получателя
        #endregion

        #region Свойства
        public short Code // код типа получателя — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование типа получателя санкции
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S16RecipientType() // конструктор по умолчанию (нужен EF Core)
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

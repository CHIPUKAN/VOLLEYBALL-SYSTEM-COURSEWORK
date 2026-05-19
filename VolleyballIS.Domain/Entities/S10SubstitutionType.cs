namespace VolleyballIS.Domain.Entities
{
    // С10. Типы замен
    public class S10SubstitutionType
    {
        #region Поля
        private short code; // код типа замены (первичный ключ)
        private string name; // наименование типа замены
        #endregion

        #region Свойства
        public short Code // код типа замены — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование типа замены
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S10SubstitutionType() // конструктор по умолчанию (нужен EF Core)
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

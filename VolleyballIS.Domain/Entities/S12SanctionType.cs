namespace VolleyballIS.Domain.Entities
{
    // С12. Типы санкций (карточки)
    public class S12SanctionType
    {
        #region Поля
        private short code; // код типа санкции (первичный ключ)
        private string name; // наименование типа санкции
        #endregion

        #region Свойства
        public short Code // код типа санкции — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование типа санкции
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S12SanctionType() // конструктор по умолчанию (нужен EF Core)
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

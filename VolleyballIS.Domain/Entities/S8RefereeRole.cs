namespace VolleyballIS.Domain.Entities
{
    // С8. Роли судей
    public class S8RefereeRole
    {
        #region Поля
        private short code; // код роли (первичный ключ)
        private string name; // наименование роли
        #endregion

        #region Свойства
        public short Code // код роли — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование роли
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S8RefereeRole() // конструктор по умолчанию (нужен EF Core)
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

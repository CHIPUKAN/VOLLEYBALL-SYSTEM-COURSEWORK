namespace VolleyballIS.Domain.Entities
{
    // С4. Форматы турниров
    public class S4TournamentFormat
    {
        #region Поля
        private short code; // код формата (первичный ключ)
        private string name; // наименование формата
        #endregion

        #region Свойства
        public short Code // код формата — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование формата
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S4TournamentFormat() // конструктор по умолчанию (нужен EF Core)
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

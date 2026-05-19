namespace VolleyballIS.Domain.Entities
{
    // С5. Этапы турниров
    public class S5TournamentStage
    {
        #region Поля
        private short code; // код этапа (первичный ключ)
        private string name; // наименование этапа
        #endregion

        #region Свойства
        public short Code // код этапа — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование этапа
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S5TournamentStage() // конструктор по умолчанию (нужен EF Core)
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

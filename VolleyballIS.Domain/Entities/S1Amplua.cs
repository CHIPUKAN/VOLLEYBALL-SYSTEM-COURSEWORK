namespace VolleyballIS.Domain.Entities
{
    // С1. Амплуа игроков
    public class S1Amplua
    {
        #region Поля
        private short code; // код амплуа (первичный ключ)
        private string name; // наименование амплуа
        #endregion

        #region Свойства
        public short Code // код амплуа — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование амплуа
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S1Amplua() // конструктор по умолчанию (нужен EF Core)
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

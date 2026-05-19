namespace VolleyballIS.Domain.Entities
{
    // Т1. Регион (ОКТМО)
    public class T1Region
    {
        #region Поля
        private string oktmoCode; // код региона по ОКТМО (11 цифр)
        private string name;      // наименование региона
        #endregion

        #region Свойства
        public string OktmoCode // код ОКТМО — первичный ключ
        {
            get => oktmoCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 11)
                {
                    throw new ArgumentException("Код ОКТМО должен состоять из 11 цифр");
                }
                oktmoCode = value;
            }
        }

        public string Name // наименование региона
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование региона не может быть пустым");
                }
                name = value;
            }
        }

        // навигационное свойство: команды данного региона
        public ICollection<T4Team> Teams { get; set; } = new List<T4Team>();
        #endregion

        #region Конструкторы
        public T1Region() // конструктор по умолчанию (нужен EF Core)
        {
            oktmoCode = string.Empty;
            name = string.Empty;
        }

        public T1Region(string oktmoCode, string name) // конструктор с параметрами
        {
            OktmoCode = oktmoCode;
            Name = name;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{OktmoCode}] {Name}";
        }
        #endregion
    }
}

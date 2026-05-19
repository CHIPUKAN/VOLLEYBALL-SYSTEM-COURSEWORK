namespace VolleyballIS.Domain.Entities
{
    // С13. Типы нарушений задержки
    public class S13DelayViolation
    {
        #region Поля
        private short code; // код нарушения (первичный ключ)
        private string name; // наименование нарушения
        #endregion

        #region Свойства
        public short Code // код нарушения задержки — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование нарушения задержки
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S13DelayViolation() // конструктор по умолчанию (нужен EF Core)
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

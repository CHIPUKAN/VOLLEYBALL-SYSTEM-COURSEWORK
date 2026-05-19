namespace VolleyballIS.Domain.Entities
{
    // С15. Виды санкций (за поведение vs задержка)
    public class S15SanctionKind
    {
        #region Поля
        private short code; // код вида санкции (первичный ключ)
        private string name; // наименование вида санкции
        #endregion

        #region Свойства
        public short Code // код вида санкции — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование вида санкции
        {
            get => name;
            set => name = value;
        }
        #endregion

        #region Конструкторы
        public S15SanctionKind() // конструктор по умолчанию (нужен EF Core)
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

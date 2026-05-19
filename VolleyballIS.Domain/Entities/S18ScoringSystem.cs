namespace VolleyballIS.Domain.Entities
{
    // С18. Системы начисления очков
    public class S18ScoringSystem
    {
        #region Поля
        private short code; // код системы (первичный ключ)
        private string name; // наименование системы
        #endregion

        #region Свойства
        public short Code // код системы начисления очков — первичный ключ
        {
            get => code;
            set => code = value;
        }

        public string Name // наименование системы начисления очков
        {
            get => name;
            set => name = value;
        }

        public short PointsWin30 { get; set; } // очки за победу 3:0

        public short PointsWin31 { get; set; } // очки за победу 3:1

        public short PointsWin32 { get; set; } // очки за победу 3:2

        public short PointsLoss23 { get; set; } // очки за поражение 2:3

        public short PointsLoss13 { get; set; } // очки за поражение 1:3

        public short PointsLoss03 { get; set; } // очки за поражение 0:3
        #endregion

        #region Конструкторы
        public S18ScoringSystem() // конструктор по умолчанию (нужен EF Core)
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

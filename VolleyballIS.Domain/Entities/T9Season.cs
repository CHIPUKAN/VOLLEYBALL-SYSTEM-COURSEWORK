namespace VolleyballIS.Domain.Entities
{
    // Т9. Сезон
    public class T9Season
    {
        #region Поля
        private int id;      // первичный ключ
        private string name; // наименование сезона
        #endregion

        #region Свойства
        public int Id // идентификатор сезона
        {
            get => id;
            set => id = value;
        }

        public string Name // наименование сезона (например, «2024/2025»)
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование сезона не может быть пустым");
                }
                name = value;
            }
        }

        public DateOnly StartDate { get; set; } // дата начала сезона

        public DateOnly EndDate { get; set; } // дата окончания сезона

        public string Status { get; set; } = "активен"; // статус сезона: «активен» или «завершён»

        // навигационное свойство: турниры данного сезона
        public ICollection<T10Tournament> Tournaments { get; set; } = new List<T10Tournament>();
        #endregion

        #region Конструкторы
        public T9Season() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
        }

        public T9Season(string name, DateOnly startDate, DateOnly endDate) // конструктор с параметрами
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] {Name}";
        }
        #endregion
    }
}

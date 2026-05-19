namespace VolleyballIS.Domain.Entities
{
    // Т2. Площадка (спортивный зал)
    public class T2Venue
    {
        #region Поля
        private int id;          // первичный ключ
        private string name;     // наименование площадки
        private string city;     // город расположения
        #endregion

        #region Свойства
        public int Id // идентификатор площадки
        {
            get => id;
            set => id = value;
        }

        public string Name // наименование площадки
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование площадки не может быть пустым");
                }
                name = value;
            }
        }

        public string? Address { get; set; } // адрес площадки (необязательно)

        public string City // город расположения
        {
            get => city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Город не может быть пустым");
                }
                city = value;
            }
        }

        public int? Capacity { get; set; } // вместимость (мест), необязательно

        // навигационное свойство: команды, использующие данную площадку как домашнюю
        public ICollection<T4Team> HomeTeams { get; set; } = new List<T4Team>();
        #endregion

        #region Конструкторы
        public T2Venue() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
            city = string.Empty;
        }

        public T2Venue(string name, string city, string? address = null, int? capacity = null) // конструктор с параметрами
        {
            Name = name;
            City = city;
            Address = address;
            Capacity = capacity;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"{Name}, {City}";
        }
        #endregion
    }
}

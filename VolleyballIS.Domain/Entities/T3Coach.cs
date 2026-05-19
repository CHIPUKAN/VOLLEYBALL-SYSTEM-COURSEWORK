namespace VolleyballIS.Domain.Entities
{
    // Т3. Тренер
    public class T3Coach
    {
        #region Поля
        private int id;         // первичный ключ
        private string lastName;  // фамилия тренера
        private string firstName; // имя тренера
        #endregion

        #region Свойства
        public int Id // идентификатор тренера
        {
            get => id;
            set => id = value;
        }

        public string LastName // фамилия тренера
        {
            get => lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия тренера не может быть пустой");
                }
                lastName = value;
            }
        }

        public string FirstName // имя тренера
        {
            get => firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя тренера не может быть пустым");
                }
                firstName = value;
            }
        }

        public string? MiddleName { get; set; } // отчество тренера (необязательно)

        public string? Email { get; set; } // электронная почта (необязательно)

        public string? Phone { get; set; } // телефон (необязательно)

        public string? Category { get; set; } // квалификационная категория (необязательно)

        // навигационное свойство: команды под руководством данного тренера
        public ICollection<T4Team> Teams { get; set; } = new List<T4Team>();
        #endregion

        #region Конструкторы
        public T3Coach() // конструктор по умолчанию (нужен EF Core)
        {
            lastName = string.Empty;
            firstName = string.Empty;
        }

        public T3Coach(string lastName, string firstName, string? middleName = null) // конструктор с параметрами
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }
        #endregion

        #region Методы
        public string FullName() // полное ФИО тренера
        {
            string result = $"{LastName} {FirstName}";
            if (!string.IsNullOrWhiteSpace(MiddleName))
            {
                result += $" {MiddleName}";
            }
            return result;
        }

        public override string ToString() // строковое представление объекта
        {
            return FullName();
        }
        #endregion
    }
}

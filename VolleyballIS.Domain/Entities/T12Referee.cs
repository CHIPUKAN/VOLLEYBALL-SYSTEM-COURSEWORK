namespace VolleyballIS.Domain.Entities
{
    // Т12. Судья
    public class T12Referee
    {
        #region Поля
        private int id;           // первичный ключ
        private string lastName;  // фамилия судьи
        private string firstName; // имя судьи
        #endregion

        #region Свойства
        public int Id // идентификатор судьи
        {
            get => id;
            set => id = value;
        }

        public string LastName // фамилия судьи
        {
            get => lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия судьи не может быть пустой");
                }
                lastName = value;
            }
        }

        public string FirstName // имя судьи
        {
            get => firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя судьи не может быть пустым");
                }
                firstName = value;
            }
        }

        public string? MiddleName { get; set; } // отчество судьи (необязательно)

        public string? Category { get; set; } // категория: FIVB / национальная / региональная / местная

        public string? LicenseNumber { get; set; } // номер лицензии (необязательно)

        public string? Email { get; set; } // электронная почта (необязательно)

        public string? Phone { get; set; } // телефон (необязательно)
        #endregion

        #region Конструкторы
        public T12Referee() // конструктор по умолчанию (нужен EF Core)
        {
            lastName = string.Empty;
            firstName = string.Empty;
        }

        public T12Referee(string lastName, string firstName, string? middleName = null) // конструктор с параметрами
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }
        #endregion

        #region Методы
        public string FullName() // полное ФИО судьи
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
            return $"[{Id}] {FullName()}";
        }
        #endregion
    }
}

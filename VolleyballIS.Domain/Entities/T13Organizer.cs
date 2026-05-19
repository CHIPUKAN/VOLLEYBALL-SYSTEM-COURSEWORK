namespace VolleyballIS.Domain.Entities
{
    // Т13. Организатор турнира
    public class T13Organizer
    {
        #region Поля
        private int id;           // первичный ключ
        private string lastName;  // фамилия организатора
        private string firstName; // имя организатора
        #endregion

        #region Свойства
        public int Id // идентификатор организатора
        {
            get => id;
            set => id = value;
        }

        public string LastName // фамилия организатора
        {
            get => lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия организатора не может быть пустой");
                }
                lastName = value;
            }
        }

        public string FirstName // имя организатора
        {
            get => firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя организатора не может быть пустым");
                }
                firstName = value;
            }
        }

        public string? MiddleName { get; set; } // отчество организатора (необязательно)

        public string? Email { get; set; } // электронная почта (необязательно)

        public string? Phone { get; set; } // телефон (необязательно)

        // навигационное свойство: турниры, организованные данным лицом
        public ICollection<T10Tournament> Tournaments { get; set; } = new List<T10Tournament>();
        #endregion

        #region Конструкторы
        public T13Organizer() // конструктор по умолчанию (нужен EF Core)
        {
            lastName = string.Empty;
            firstName = string.Empty;
        }

        public T13Organizer(string lastName, string firstName, string? middleName = null) // конструктор с параметрами
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }
        #endregion

        #region Методы
        public string FullName() // полное ФИО организатора
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

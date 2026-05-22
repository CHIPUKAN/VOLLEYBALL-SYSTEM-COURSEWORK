namespace VolleyballIS.Domain.Entities
{
    // Т6. Игрок
    public class T6Player
    {
        #region Поля
        private int id;           // первичный ключ
        private string lastName;  // фамилия игрока
        private string firstName; // имя игрока
        #endregion

        #region Свойства
        public int Id // идентификатор игрока
        {
            get => id;
            set => id = value;
        }

        public int? TeamId { get; set; } // идентификатор команды (NULL = свободный агент)

        public string LastName // фамилия игрока
        {
            get => lastName;
            set => lastName = value ?? string.Empty;
        }

        public string FirstName // имя игрока
        {
            get => firstName;
            set => firstName = value ?? string.Empty;
        }

        public string? MiddleName { get; set; } // отчество игрока (необязательно)

        public DateOnly BirthDate { get; set; } // дата рождения

        public short? HeightCm { get; set; } // рост в сантиметрах (необязательно)

        public short? WeightKg { get; set; } // вес в килограммах (необязательно)

        public short? JerseyNumber { get; set; } // номер на футболке (необязательно)

        public short AmpluaCode { get; set; } // код амплуа (внешний ключ -> s1_amplua)

        public string? SportsRank { get; set; } // спортивный разряд/звание (необязательно)

        public string? Email { get; set; } // электронная почта (необязательно)

        public string? Phone { get; set; } // телефон (необязательно)

        public short StatusCode { get; set; } = 1; // код статуса игрока (внешний ключ -> s2_player_statuses)

        public string? PhotoUrl { get; set; } // URL фотографии (необязательно)

        // навигационные свойства
        public T4Team? Team { get; set; }            // команда игрока
        public S1Amplua? Amplua { get; set; }         // амплуа игрока
        public S2PlayerStatus? Status { get; set; }   // статус игрока
        #endregion

        #region Конструкторы
        public T6Player() // конструктор по умолчанию (нужен EF Core)
        {
            lastName = string.Empty;
            firstName = string.Empty;
        }

        public T6Player(string lastName, string firstName, DateOnly birthDate, short ampluaCode) // конструктор с параметрами
        {
            LastName = lastName;
            FirstName = firstName;
            BirthDate = birthDate;
            AmpluaCode = ampluaCode;
        }
        #endregion

        #region Методы
        public string FullName() // полное ФИО игрока
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

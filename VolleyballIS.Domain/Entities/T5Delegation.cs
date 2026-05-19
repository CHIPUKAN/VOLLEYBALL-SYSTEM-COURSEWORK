namespace VolleyballIS.Domain.Entities
{
    // Т5. Делегация команды на матч (согласно пр.4.1.1 ФИВБ)
    public class T5Delegation
    {
        #region Поля
        private int id;           // первичный ключ
        private string lastName;  // фамилия члена делегации
        private string firstName; // имя члена делегации
        private string roleType;  // роль в делегации
        #endregion

        #region Свойства
        public int Id // идентификатор члена делегации
        {
            get => id;
            set => id = value;
        }

        public int MatchId { get; set; } // идентификатор матча (внешний ключ)

        public int TeamId { get; set; } // идентификатор команды (внешний ключ)

        public string RoleType // роль: «помощник тренера», «массажист», «врач»
        {
            get => roleType;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Роль члена делегации не может быть пустой");
                }
                roleType = value;
            }
        }

        public short? AssistantSeqNo { get; set; } // порядковый номер помощника тренера (1 или 2, необязательно)

        public string LastName // фамилия члена делегации
        {
            get => lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия не может быть пустой");
                }
                lastName = value;
            }
        }

        public string FirstName // имя члена делегации
        {
            get => firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя не может быть пустым");
                }
                firstName = value;
            }
        }

        public string? MiddleName { get; set; } // отчество (необязательно)

        // навигационные свойства
        public T14Match? Match { get; set; } // матч
        public T4Team? Team { get; set; }    // команда
        #endregion

        #region Конструкторы
        public T5Delegation() // конструктор по умолчанию (нужен EF Core)
        {
            lastName = string.Empty;
            firstName = string.Empty;
            roleType = string.Empty;
        }
        #endregion

        #region Методы
        public string FullName() // полное ФИО члена делегации
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
            return $"[{Id}] {FullName()} ({RoleType})";
        }
        #endregion
    }
}

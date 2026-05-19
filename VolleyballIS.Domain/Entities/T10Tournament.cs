namespace VolleyballIS.Domain.Entities
{
    // Т10. Турнир
    public class T10Tournament
    {
        #region Поля
        private int id;      // первичный ключ
        private string name; // наименование турнира
        private string city; // город проведения
        #endregion

        #region Свойства
        public int Id // идентификатор турнира
        {
            get => id;
            set => id = value;
        }

        public int? SeasonId { get; set; } // идентификатор сезона (внешний ключ)

        public int? OrganizerId { get; set; } // идентификатор организатора (внешний ключ)

        public string Name // наименование турнира
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование турнира не может быть пустым");
                }
                name = value;
            }
        }

        public DateOnly StartDate { get; set; } // дата начала турнира

        public DateOnly EndDate { get; set; } // дата окончания турнира

        public DateOnly? ApplicationDeadline { get; set; } // крайний срок подачи заявок (необязательно)

        public string City // город проведения турнира
        {
            get => city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Город проведения не может быть пустым");
                }
                city = value;
            }
        }

        public string? Description { get; set; } // описание турнира (необязательно)

        public short? MaxTeams { get; set; } // максимальное количество команд (необязательно)

        public string Gender { get; set; } = "мужской"; // пол участников: мужской / женский / смешанный

        public string? AgeCategory { get; set; } // возрастная категория (необязательно)

        public string Level { get; set; } = "региональный"; // уровень: ФИВБ / национальный / региональный / местный

        public short MaxPlayersPerApp { get; set; } = 12; // максимум игроков в заявке: 12 или 14

        public short FormatCode { get; set; } // код формата турнира (внешний ключ -> s4_tournament_formats)

        public short ScoringSystemCode { get; set; } // код системы начисления очков (внешний ключ -> s18_scoring_systems)

        // навигационные свойства
        public T9Season? Season { get; set; }               // сезон турнира
        public T13Organizer? Organizer { get; set; }         // организатор турнира
        public S4TournamentFormat? Format { get; set; }      // формат турнира
        public S18ScoringSystem? ScoringSystem { get; set; } // система начисления очков
        public ICollection<T7Application> Applications { get; set; } = new List<T7Application>(); // заявки
        public ICollection<T11Group> Groups { get; set; } = new List<T11Group>();                  // группы
        #endregion

        #region Конструкторы
        public T10Tournament() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
            city = string.Empty;
        }

        public T10Tournament(string name, string city, DateOnly startDate, DateOnly endDate, short formatCode, short scoringSystemCode) // конструктор с параметрами
        {
            Name = name;
            City = city;
            StartDate = startDate;
            EndDate = endDate;
            FormatCode = formatCode;
            ScoringSystemCode = scoringSystemCode;
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

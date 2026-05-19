namespace VolleyballIS.Domain.Entities
{
    // Т4. Команда
    public class T4Team
    {
        #region Поля
        private int id;            // первичный ключ
        private string name;       // наименование команды
        private string regionOktmo; // код ОКТМО региона команды
        private int headCoachId;   // идентификатор главного тренера
        private int homeVenueId;   // идентификатор домашней площадки
        #endregion

        #region Свойства
        public int Id // идентификатор команды
        {
            get => id;
            set => id = value;
        }

        public string Name // наименование команды
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование команды не может быть пустым");
                }
                name = value;
            }
        }

        public string? LogoUrl { get; set; } // URL логотипа команды (необязательно)

        public string RegionOktmo // код ОКТМО региона (внешний ключ)
        {
            get => regionOktmo;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 11)
                {
                    throw new ArgumentException("Код ОКТМО должен состоять из 11 цифр");
                }
                regionOktmo = value;
            }
        }

        public int HeadCoachId // идентификатор главного тренера (внешний ключ)
        {
            get => headCoachId;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Идентификатор тренера должен быть положительным");
                }
                headCoachId = value;
            }
        }

        public int HomeVenueId // идентификатор домашней площадки (внешний ключ)
        {
            get => homeVenueId;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Идентификатор площадки должен быть положительным");
                }
                homeVenueId = value;
            }
        }

        // навигационные свойства
        public T1Region? Region { get; set; }    // регион команды
        public T3Coach? HeadCoach { get; set; }  // главный тренер команды
        public T2Venue? HomeVenue { get; set; }  // домашняя площадка
        #endregion

        #region Конструкторы
        public T4Team() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
            regionOktmo = string.Empty;
        }

        public T4Team(string name, string regionOktmo, int headCoachId, int homeVenueId) // конструктор с параметрами
        {
            Name = name;
            RegionOktmo = regionOktmo;
            HeadCoachId = headCoachId;
            HomeVenueId = homeVenueId;
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

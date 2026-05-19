namespace VolleyballIS.Domain.Entities
{
    // Т11. Турнирная группа
    public class T11Group
    {
        #region Поля
        private int id;      // первичный ключ
        private string name; // наименование группы
        #endregion

        #region Свойства
        public int Id // идентификатор группы
        {
            get => id;
            set => id = value;
        }

        public int TournamentId { get; set; } // идентификатор турнира (внешний ключ)

        public string Name // наименование группы (например, «А», «Б»)
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование группы не может быть пустым");
                }
                name = value;
            }
        }

        // навигационное свойство
        public T10Tournament? Tournament { get; set; } // турнир группы
        #endregion

        #region Конструкторы
        public T11Group() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
        }

        public T11Group(int tournamentId, string name) // конструктор с параметрами
        {
            TournamentId = tournamentId;
            Name = name;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Группа «{Name}» (турнир {TournamentId})";
        }
        #endregion
    }
}

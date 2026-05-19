namespace VolleyballIS.Domain.Entities
{
    // Т20. Награда по итогам турнира
    public class T20Award
    {
        #region Поля
        private int id;      // первичный ключ
        private string name; // наименование награды
        #endregion

        #region Свойства
        public int Id // идентификатор награды
        {
            get => id;
            set => id = value;
        }

        public int TournamentId { get; set; } // идентификатор турнира (внешний ключ)

        public short AwardTypeCode { get; set; } // код типа награды: 1=индивидуальная, 2=командная

        public string Name // наименование награды (например, «MVP турнира»)
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Наименование награды не может быть пустым");
                }
                name = value;
            }
        }

        public int? PlayerId { get; set; } // идентификатор игрока (для индивидуальной награды)

        public int? TeamId { get; set; } // идентификатор команды (для командной награды)

        // навигационные свойства
        public T10Tournament? Tournament { get; set; } // турнир
        public S14AwardType? AwardType { get; set; }   // тип награды
        public T6Player? Player { get; set; }          // игрок-лауреат
        public T4Team? Team { get; set; }              // команда-лауреат
        #endregion

        #region Конструкторы
        public T20Award() // конструктор по умолчанию (нужен EF Core)
        {
            name = string.Empty;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] {Name} (турнир {TournamentId})";
        }
        #endregion
    }
}

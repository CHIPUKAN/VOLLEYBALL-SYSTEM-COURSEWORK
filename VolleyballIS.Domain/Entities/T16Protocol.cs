namespace VolleyballIS.Domain.Entities
{
    // Т16. Протокол матча
    public class T16Protocol
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор протокола
        {
            get => id;
            set => id = value;
        }

        public int MatchId { get; set; } // идентификатор матча (внешний ключ, уникальный)

        public int? OrganizerId { get; set; } // идентификатор организатора, утвердившего протокол (необязательно)

        public DateOnly? ApprovalDate { get; set; } // дата утверждения протокола (необязательно)

        public short StatusCode { get; set; } = 1; // код статуса протокола (внешний ключ -> s7_protocol_statuses)

        // навигационные свойства
        public T14Match? Match { get; set; }         // матч
        public T13Organizer? Organizer { get; set; } // утвердивший организатор
        public S7ProtocolStatus? Status { get; set; } // статус протокола
        public ICollection<T16ProtocolHistory> History { get; set; } = new List<T16ProtocolHistory>(); // история изменений
        #endregion

        #region Конструкторы
        public T16Protocol() // конструктор по умолчанию (нужен EF Core)
        {
        }

        public T16Protocol(int matchId) // конструктор с параметрами
        {
            MatchId = matchId;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Протокол матча {MatchId}";
        }
        #endregion
    }
}

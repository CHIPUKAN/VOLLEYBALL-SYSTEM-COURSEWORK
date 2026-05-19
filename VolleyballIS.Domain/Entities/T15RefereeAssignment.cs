namespace VolleyballIS.Domain.Entities
{
    // Т15. Назначение судьи на матч (судейская бригада)
    public class T15RefereeAssignment
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор назначения
        {
            get => id;
            set => id = value;
        }

        public int MatchId { get; set; } // идентификатор матча (внешний ключ)

        public int RefereeId { get; set; } // идентификатор судьи (внешний ключ)

        public short RoleCode { get; set; } // код роли судьи (внешний ключ -> s8_referee_roles)

        public short? LineJudgeSeqNo { get; set; } // порядковый номер линейного судьи (1–4, необязательно)

        // навигационные свойства
        public T14Match? Match { get; set; }       // матч
        public T12Referee? Referee { get; set; }   // судья
        public S8RefereeRole? Role { get; set; }   // роль судьи
        #endregion

        #region Конструкторы
        public T15RefereeAssignment() // конструктор по умолчанию (нужен EF Core)
        {
        }

        public T15RefereeAssignment(int matchId, int refereeId, short roleCode) // конструктор с параметрами
        {
            MatchId = matchId;
            RefereeId = refereeId;
            RoleCode = roleCode;
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Судья {RefereeId} на матч {MatchId} (роль {RoleCode})";
        }
        #endregion
    }
}

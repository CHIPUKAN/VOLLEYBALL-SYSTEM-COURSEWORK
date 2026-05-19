namespace VolleyballIS.Domain.Entities
{
    // Т18. Санкция (карточка)
    public class T18Sanction
    {
        #region Поля
        private int id; // первичный ключ
        #endregion

        #region Свойства
        public int Id // идентификатор санкции
        {
            get => id;
            set => id = value;
        }

        public int MatchId { get; set; } // идентификатор матча (внешний ключ)

        public int TeamId { get; set; } // идентификатор команды (внешний ключ)

        public int? PlayerId { get; set; } // идентификатор игрока-получателя (NULL, если член делегации)

        public int? DelegationMemberId { get; set; } // идентификатор члена делегации-получателя (NULL, если игрок)

        public short RecipientTypeCode { get; set; } // код типа получателя (внешний ключ -> s16_recipient_types)

        public short SanctionTypeCode { get; set; } // код типа санкции (внешний ключ -> s12_sanction_types)

        public short SanctionKindCode { get; set; } // код вида санкции (внешний ключ -> s15_sanction_kinds)

        public short? DelayViolationCode { get; set; } // код нарушения задержки (только для sanction_kind=2)

        public short SetNumber { get; set; } // номер партии (1–5)

        public short MemberSeqInMatch { get; set; } // порядковый номер санкции данному члену в матче

        public short HomeScoreAtMoment { get; set; } // счёт хозяев в момент санкции

        public short GuestScoreAtMoment { get; set; } // счёт гостей в момент санкции

        public short? MinuteMark { get; set; } // игровая минута (необязательно)

        public int? EventId { get; set; } // событие, спровоцировавшее санкцию (необязательно)

        // навигационные свойства
        public T14Match? Match { get; set; }                    // матч
        public T4Team? Team { get; set; }                       // команда
        public T6Player? Player { get; set; }                   // игрок (если получатель — игрок)
        public S16RecipientType? RecipientType { get; set; }    // тип получателя
        public S12SanctionType? SanctionType { get; set; }      // тип санкции
        public S15SanctionKind? SanctionKind { get; set; }      // вид санкции
        public S13DelayViolation? DelayViolation { get; set; }  // нарушение задержки
        #endregion

        #region Конструкторы
        public T18Sanction() // конструктор по умолчанию (нужен EF Core)
        {
        }
        #endregion

        #region Методы
        public override string ToString() // строковое представление объекта
        {
            return $"[{Id}] Санкция в матче {MatchId}, партия {SetNumber}";
        }
        #endregion
    }
}

namespace VolleyballIS.Application.DTOs.Sanctions
{
    // DTO для отображения санкции (карточки) в матче
    public class SanctionDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор санкции

        public int MatchId { get; set; } // идентификатор матча

        public int TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public int? PlayerId { get; set; } // идентификатор игрока-получателя

        public string? PlayerFullName { get; set; } // ФИО игрока

        public int? DelegationMemberId { get; set; } // идентификатор члена делегации

        public short RecipientTypeCode { get; set; } // код типа получателя

        public string? RecipientTypeName { get; set; } // наименование типа получателя

        public short SanctionTypeCode { get; set; } // код типа санкции

        public string? SanctionTypeName { get; set; } // наименование типа санкции

        public short SanctionKindCode { get; set; } // код вида санкции

        public string? SanctionKindName { get; set; } // наименование вида санкции

        public short? DelayViolationCode { get; set; } // код нарушения задержки

        public string? DelayViolationName { get; set; } // наименование нарушения задержки

        public short SetNumber { get; set; } // номер партии

        public short MemberSeqInMatch { get; set; } // порядковый номер санкции

        public short HomeScoreAtMoment { get; set; } // счёт хозяев в момент санкции

        public short GuestScoreAtMoment { get; set; } // счёт гостей в момент санкции

        public short? MinuteMark { get; set; } // игровая минута

        public int? EventId { get; set; } // связанное событие
        #endregion
    }
}

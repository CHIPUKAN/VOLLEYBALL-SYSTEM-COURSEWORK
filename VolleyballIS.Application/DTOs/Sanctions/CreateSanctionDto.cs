using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Sanctions
{
    // DTO для регистрации санкции в матче
    public class CreateSanctionDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор матча обязателен")]
        public int MatchId { get; set; } // идентификатор матча

        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        public int TeamId { get; set; } // идентификатор команды

        public int? PlayerId { get; set; } // идентификатор игрока (если получатель — игрок)

        public int? DelegationMemberId { get; set; } // идентификатор члена делегации

        [Required(ErrorMessage = "Код типа получателя обязателен")]
        public short RecipientTypeCode { get; set; } // код типа получателя (из С16)

        [Required(ErrorMessage = "Код типа санкции обязателен")]
        public short SanctionTypeCode { get; set; } // код типа санкции (из С12)

        [Required(ErrorMessage = "Код вида санкции обязателен")]
        public short SanctionKindCode { get; set; } // код вида санкции (из С15)

        public short? DelayViolationCode { get; set; } // код нарушения задержки (из С13)

        [Required(ErrorMessage = "Номер партии обязателен")]
        [Range(1, 5, ErrorMessage = "Номер партии должен быть от 1 до 5")]
        public short SetNumber { get; set; } // номер партии

        public short MemberSeqInMatch { get; set; } // порядковый номер санкции данному члену

        public short HomeScoreAtMoment { get; set; } // счёт хозяев в момент санкции

        public short GuestScoreAtMoment { get; set; } // счёт гостей в момент санкции

        public short? MinuteMark { get; set; } // игровая минута

        public int? EventId { get; set; } // связанное событие
        #endregion
    }
}

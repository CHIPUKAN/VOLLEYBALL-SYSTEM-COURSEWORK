using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.RefereeAssignments
{
    // DTO для назначения судьи на матч
    public class CreateRefereeAssignmentDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор матча обязателен")]
        public int MatchId { get; set; } // идентификатор матча

        [Required(ErrorMessage = "Идентификатор судьи обязателен")]
        public int RefereeId { get; set; } // идентификатор судьи

        [Required(ErrorMessage = "Код роли судьи обязателен")]
        public short RoleCode { get; set; } // код роли (из справочника С8)

        public short? LineJudgeSeqNo { get; set; } // порядковый номер линейного судьи (1–4)
        #endregion
    }
}

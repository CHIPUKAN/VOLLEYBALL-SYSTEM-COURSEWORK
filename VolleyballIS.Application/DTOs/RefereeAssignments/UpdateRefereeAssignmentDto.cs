using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.RefereeAssignments
{
    // DTO для обновления назначения судьи
    public class UpdateRefereeAssignmentDto
    {
        #region Свойства
        [Required(ErrorMessage = "Код роли судьи обязателен")]
        public short RoleCode { get; set; } // новый код роли

        public short? LineJudgeSeqNo { get; set; } // порядковый номер линейного судьи
        #endregion
    }
}

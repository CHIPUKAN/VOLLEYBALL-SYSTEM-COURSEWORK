namespace VolleyballIS.Application.DTOs.RefereeAssignments
{
    // DTO для отображения назначения судьи на матч
    public class RefereeAssignmentDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор назначения

        public int MatchId { get; set; } // идентификатор матча

        public int RefereeId { get; set; } // идентификатор судьи

        public string? RefereeFullName { get; set; } // ФИО судьи

        public short RoleCode { get; set; } // код роли судьи

        public string? RoleName { get; set; } // наименование роли

        public short? LineJudgeSeqNo { get; set; } // порядковый номер линейного судьи
        #endregion
    }
}

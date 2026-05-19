namespace VolleyballIS.Application.DTOs.Protocols
{
    // DTO для отображения протокола матча
    public class ProtocolDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор протокола

        public int MatchId { get; set; } // идентификатор матча

        public int? OrganizerId { get; set; } // идентификатор организатора

        public string? OrganizerFullName { get; set; } // ФИО организатора

        public DateOnly? ApprovalDate { get; set; } // дата утверждения

        public short StatusCode { get; set; } // код статуса протокола

        public string? StatusName { get; set; } // наименование статуса
        #endregion
    }
}

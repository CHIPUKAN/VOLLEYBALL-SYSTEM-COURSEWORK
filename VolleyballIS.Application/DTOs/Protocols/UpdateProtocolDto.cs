namespace VolleyballIS.Application.DTOs.Protocols
{
    // DTO для обновления протокола матча
    public class UpdateProtocolDto
    {
        #region Свойства
        public int? OrganizerId { get; set; } // идентификатор организатора

        public DateOnly? ApprovalDate { get; set; } // дата утверждения

        public short StatusCode { get; set; } = 1; // новый код статуса
        #endregion
    }
}

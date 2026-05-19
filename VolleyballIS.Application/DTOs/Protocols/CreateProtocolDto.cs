using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Protocols
{
    // DTO для создания протокола матча
    public class CreateProtocolDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор матча обязателен")]
        public int MatchId { get; set; } // идентификатор матча

        public int? OrganizerId { get; set; } // идентификатор утверждающего организатора

        public DateOnly? ApprovalDate { get; set; } // дата утверждения

        public short StatusCode { get; set; } = 1; // код статуса (по умолчанию «Черновик»)
        #endregion
    }
}

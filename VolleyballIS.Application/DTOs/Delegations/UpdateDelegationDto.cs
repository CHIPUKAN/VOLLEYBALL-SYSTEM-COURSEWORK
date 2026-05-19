using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Delegations
{
    // DTO для обновления члена делегации
    public class UpdateDelegationDto
    {
        #region Свойства
        [Required(ErrorMessage = "Роль обязательна")]
        [MaxLength(30)]
        public string RoleType { get; set; } = string.Empty; // роль в делегации

        public short? AssistantSeqNo { get; set; } // порядковый номер помощника

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(60)]
        public string LastName { get; set; } = string.Empty; // фамилия

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(40)]
        public string FirstName { get; set; } = string.Empty; // имя

        [MaxLength(60)]
        public string? MiddleName { get; set; } // отчество
        #endregion
    }
}

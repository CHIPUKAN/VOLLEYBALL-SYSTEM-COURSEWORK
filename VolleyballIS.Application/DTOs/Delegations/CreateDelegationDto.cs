using System.ComponentModel.DataAnnotations;
using VolleyballIS.Application.Common;

namespace VolleyballIS.Application.DTOs.Delegations
{
    // DTO для добавления члена делегации команды на матч
    public class CreateDelegationDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор матча обязателен")]
        public int MatchId { get; set; } // идентификатор матча

        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        public int TeamId { get; set; } // идентификатор команды

        [Required(ErrorMessage = "Роль обязательна")]
        [MaxLength(30, ErrorMessage = "Роль не должна превышать 30 символов")]
        [DelegationRoleValidation]
        public string RoleType { get; set; } = string.Empty; // роль в делегации

        public short? AssistantSeqNo { get; set; } // порядковый номер помощника тренера

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

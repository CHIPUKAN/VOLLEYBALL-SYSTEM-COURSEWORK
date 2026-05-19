using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Teams
{
    // DTO для обновления существующей команды (входные данные от клиента)
    public class UpdateTeamDto
    {
        #region Свойства
        [Required(ErrorMessage = "Наименование команды обязательно")]
        [MaxLength(150, ErrorMessage = "Наименование не должно превышать 150 символов")]
        public string Name { get; set; } = string.Empty; // новое наименование команды

        [MaxLength(2048, ErrorMessage = "URL логотипа не должен превышать 2048 символов")]
        public string? LogoUrl { get; set; } // новый URL логотипа (необязательно)

        [Required(ErrorMessage = "Код региона ОКТМО обязателен")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Код ОКТМО должен содержать ровно 11 символов")]
        public string RegionOktmo { get; set; } = string.Empty; // новый код ОКТМО региона

        [Required(ErrorMessage = "Идентификатор главного тренера обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "Идентификатор тренера должен быть положительным")]
        public int HeadCoachId { get; set; } // новый идентификатор главного тренера

        [Required(ErrorMessage = "Идентификатор домашней площадки обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "Идентификатор площадки должен быть положительным")]
        public int HomeVenueId { get; set; } // новый идентификатор домашней площадки
        #endregion
    }
}

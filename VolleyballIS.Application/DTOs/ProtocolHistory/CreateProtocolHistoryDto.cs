using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.ProtocolHistory
{
    // DTO для создания записи в журнале изменений протокола
    public class CreateProtocolHistoryDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор протокола обязателен")]
        public int ProtocolId { get; set; } // идентификатор протокола

        public short? StatusAtMoment { get; set; } // код статуса на момент изменения

        [MaxLength(64, ErrorMessage = "Хеш не может превышать 64 символа")]
        public string? DataHash { get; set; } // хеш данных протокола (необязательно)

        public string? Comment { get; set; } // комментарий к изменению (необязательно)
        #endregion
    }
}

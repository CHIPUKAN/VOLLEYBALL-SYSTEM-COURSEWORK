using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.MatchCaptains
{
    // DTO для назначения/обновления капитана команды на матч
    public class UpsertMatchCaptainDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        public int TeamId { get; set; } // идентификатор команды

        [Required(ErrorMessage = "Идентификатор игрока-капитана обязателен")]
        public int PlayerId { get; set; } // идентификатор игрока-капитана
        #endregion
    }
}

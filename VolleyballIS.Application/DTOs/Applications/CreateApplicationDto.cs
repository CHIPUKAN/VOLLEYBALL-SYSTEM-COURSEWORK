using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для создания заявки команды на турнир
    public class CreateApplicationDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "Идентификатор команды должен быть положительным")]
        public int TeamId { get; set; } // идентификатор команды

        [Required(ErrorMessage = "Идентификатор турнира обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "Идентификатор турнира должен быть положительным")]
        public int TournamentId { get; set; } // идентификатор турнира

        public short StatusCode { get; set; } = 1; // код статуса (по умолчанию «На рассмотрении»)
        #endregion
    }
}

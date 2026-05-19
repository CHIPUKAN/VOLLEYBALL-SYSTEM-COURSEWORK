using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для создания заявки команды на турнир
    public class CreateApplicationDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        public int TeamId { get; set; } // идентификатор команды

        [Required(ErrorMessage = "Идентификатор турнира обязателен")]
        public int TournamentId { get; set; } // идентификатор турнира

        public DateOnly SubmissionDate { get; set; } = DateOnly.FromDateTime(DateTime.Today); // дата подачи

        public short StatusCode { get; set; } = 1; // код статуса (по умолчанию «На рассмотрении»)
        #endregion
    }
}

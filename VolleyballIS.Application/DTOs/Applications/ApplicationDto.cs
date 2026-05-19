using VolleyballIS.Application.DTOs.Applications;

namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для отображения заявки команды на турнир
    public class ApplicationDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор заявки

        public int TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public int TournamentId { get; set; } // идентификатор турнира

        public string? TournamentName { get; set; } // наименование турнира

        public DateOnly SubmittedAt { get; set; } // дата подачи заявки

        public short StatusCode { get; set; } // код статуса заявки

        public string? StatusName { get; set; } // наименование статуса

        public string? Comment { get; set; } // комментарий (причина отклонения и т.п.)

        public IEnumerable<ApplicationCompositionDto> Players { get; set; } = new List<ApplicationCompositionDto>(); // состав заявки
        #endregion
    }
}

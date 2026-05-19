namespace VolleyballIS.Application.DTOs.Groups
{
    // DTO для отображения данных группы турнира
    public class GroupDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор группы

        public int TournamentId { get; set; } // идентификатор турнира

        public string? TournamentName { get; set; } // наименование турнира

        public string Name { get; set; } = string.Empty; // наименование группы
        #endregion
    }
}

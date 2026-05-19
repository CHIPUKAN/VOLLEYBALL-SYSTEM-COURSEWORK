namespace VolleyballIS.Application.DTOs.Awards
{
    // DTO для отображения награды по итогам турнира
    public class AwardDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор награды

        public int TournamentId { get; set; } // идентификатор турнира

        public string? TournamentName { get; set; } // наименование турнира

        public short AwardTypeCode { get; set; } // код типа награды (1=индивидуальная, 2=командная)

        public string? AwardTypeName { get; set; } // наименование типа награды

        public string Name { get; set; } = string.Empty; // наименование награды

        public int? PlayerId { get; set; } // идентификатор игрока-лауреата

        public string? PlayerFullName { get; set; } // ФИО игрока

        public int? TeamId { get; set; } // идентификатор команды-лауреата

        public string? TeamName { get; set; } // наименование команды
        #endregion
    }
}

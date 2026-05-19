namespace VolleyballIS.Application.DTOs.MatchCaptains
{
    // DTO для отображения капитана команды на матч
    public class MatchCaptainDto
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча

        public int TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public int PlayerId { get; set; } // идентификатор игрока-капитана

        public string? PlayerFullName { get; set; } // ФИО капитана
        #endregion
    }
}

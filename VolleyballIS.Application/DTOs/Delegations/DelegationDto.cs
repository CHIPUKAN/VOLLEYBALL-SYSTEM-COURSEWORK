namespace VolleyballIS.Application.DTOs.Delegations
{
    // DTO для отображения члена делегации команды на матч
    public class DelegationDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор члена делегации

        public int MatchId { get; set; } // идентификатор матча

        public int TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public string RoleType { get; set; } = string.Empty; // роль в делегации

        public short? AssistantSeqNo { get; set; } // номер помощника тренера (1 или 2)

        public string LastName { get; set; } = string.Empty; // фамилия

        public string FirstName { get; set; } = string.Empty; // имя

        public string? MiddleName { get; set; } // отчество

        public string FullName { get; set; } = string.Empty; // полное ФИО
        #endregion
    }
}

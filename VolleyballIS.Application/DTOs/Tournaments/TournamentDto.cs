namespace VolleyballIS.Application.DTOs.Tournaments
{
    // DTO для передачи данных о турнире на клиент
    public class TournamentDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор турнира

        public int? SeasonId { get; set; } // идентификатор сезона

        public string? SeasonName { get; set; } // наименование сезона

        public int? OrganizerId { get; set; } // идентификатор организатора

        public string? OrganizerFullName { get; set; } // ФИО организатора

        public string Name { get; set; } = string.Empty; // наименование турнира

        public DateOnly StartDate { get; set; } // дата начала

        public DateOnly EndDate { get; set; } // дата окончания

        public DateOnly? ApplicationDeadline { get; set; } // крайний срок подачи заявок

        public string City { get; set; } = string.Empty; // город проведения

        public string? Description { get; set; } // описание

        public short? MaxTeams { get; set; } // максимальное количество команд

        public string Gender { get; set; } = string.Empty; // пол участников

        public string? AgeCategory { get; set; } // возрастная категория

        public string Level { get; set; } = string.Empty; // уровень турнира

        public short MaxPlayersPerApp { get; set; } // максимум игроков в заявке

        public short FormatCode { get; set; } // код формата

        public string? FormatName { get; set; } // наименование формата

        public short ScoringSystemCode { get; set; } // код системы очков

        public string? ScoringSystemName { get; set; } // наименование системы очков
        #endregion
    }
}

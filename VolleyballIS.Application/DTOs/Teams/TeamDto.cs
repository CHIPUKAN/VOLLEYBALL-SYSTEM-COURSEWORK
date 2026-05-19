namespace VolleyballIS.Application.DTOs.Teams
{
    // DTO для передачи данных о команде на клиент
    public class TeamDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор команды

        public string Name { get; set; } = string.Empty; // наименование команды

        public string? LogoUrl { get; set; } // URL логотипа (необязательно)

        public string RegionOktmo { get; set; } = string.Empty; // код ОКТМО региона

        public string? RegionName { get; set; } // наименование региона (из связанной таблицы)

        public int HeadCoachId { get; set; } // идентификатор главного тренера

        public string? HeadCoachFullName { get; set; } // ФИО главного тренера (из связанной таблицы)

        public int HomeVenueId { get; set; } // идентификатор домашней площадки

        public string? HomeVenueName { get; set; } // наименование домашней площадки (из связанной таблицы)

        public string? HomeVenueCity { get; set; } // город домашней площадки (из связанной таблицы)
        #endregion
    }
}

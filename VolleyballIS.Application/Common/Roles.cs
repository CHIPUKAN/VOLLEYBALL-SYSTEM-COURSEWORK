namespace VolleyballIS.Application.Common
{
    // Константы ролей пользователей системы
    public static class Roles
    {
        public const string SuperAdmin  = "Суперадминистратор";    // полный доступ
        public const string Organizer   = "Организатор";           // управление турнирами
        public const string Coach       = "ТренерКоманды";         // управление командой и заявками
        public const string Secretary   = "СекретарьМатча";        // ведение протокола матча
        public const string Referee     = "СудьяМатча";            // просмотр назначений и ведение матча
        public const string TeamRep     = "ПредставительКоманды";  // представитель команды
        public const string Spectator   = "Зритель";               // просмотр без изменений

        // строка из всех ролей с правом записи (для конфигурации атрибутов)
        public const string WritersAll = "Суперадминистратор,Организатор,ТренерКоманды,СекретарьМатча";

        // строка из ролей, которым разрешено изменять протокол матча
        public const string ProtocolWriters = "Суперадминистратор,СекретарьМатча,Организатор";

        // роли, которым разрешено управлять турнирами, группами, матчами
        public const string TournamentManagers = "Суперадминистратор,Организатор";

        // все роли (для ссылки и Policy)
        public static readonly string[] All =
        [
            SuperAdmin, Organizer, Coach, Secretary, Referee, TeamRep, Spectator
        ];
    }
}

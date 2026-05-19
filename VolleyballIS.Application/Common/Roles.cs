namespace VolleyballIS.Application.Common
{
    // Константы ролей пользователей системы
    public static class Roles
    {
        public const string SuperAdmin = "Суперадминистратор"; // полный доступ
        public const string Organizer = "Организатор";         // управление турнирами
        public const string Coach = "Тренер";                  // управление командой и заявками
        public const string Secretary = "СекретарьМатча";      // ведение протокола матча
        public const string Referee = "Судья";                 // просмотр назначений
        public const string Analyst = "Аналитик";              // только чтение статистики
        public const string Player = "Игрок";                  // просмотр своей статистики

        // строка из всех ролей с правом записи (для конфигурации атрибутов)
        public const string WritersAll = "Суперадминистратор,Организатор,Тренер,СекретарьМатча";

        // строка из ролей, которым разрешено изменять протокол матча
        public const string ProtocolWriters = "Суперадминистратор,СекретарьМатча";

        // роли, которым разрешено управлять турнирами, группами, матчами
        public const string TournamentManagers = "Суперадминистратор,Организатор";

        // все роли через запятую (для Policy)
        public static readonly string[] All =
        [
            SuperAdmin, Organizer, Coach, Secretary, Referee, Analyst, Player
        ];
    }
}

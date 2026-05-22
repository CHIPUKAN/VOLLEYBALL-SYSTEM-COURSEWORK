namespace VolleyballIS.Application.Common
{
    // Константы кодов статусов матча (соответствуют s6_match_statuses)
    public static class MatchStatusCodes
    {
        public const short Planned    = 1; // запланирован
        public const short InProgress = 2; // идёт
        public const short Completed  = 3; // завершён
        public const short Postponed  = 4; // перенесён
        public const short Cancelled  = 5; // отменён
        public const short TechDefeat = 6; // техническое поражение
    }
}

namespace VolleyballIS.Application.DTOs.Sets
{
    // DTO для отображения партии матча
    public class SetDto
    {
        #region Свойства
        public int MatchId { get; set; } // идентификатор матча

        public short SetNumber { get; set; } // номер партии (1–5)

        public short? HomeScore { get; set; } // счёт команды-хозяина

        public short? GuestScore { get; set; } // счёт команды-гостя

        public short? DurationMin { get; set; } // продолжительность партии в минутах
        #endregion
    }
}

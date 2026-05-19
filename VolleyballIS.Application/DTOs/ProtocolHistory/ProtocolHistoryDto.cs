namespace VolleyballIS.Application.DTOs.ProtocolHistory
{
    // DTO для просмотра записи журнала изменений протокола
    public class ProtocolHistoryDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор записи

        public int ProtocolId { get; set; } // идентификатор протокола

        public DateTime ChangedAt { get; set; } // дата и время изменения

        public short? StatusAtMoment { get; set; } // код статуса на момент изменения

        public string? DataHash { get; set; } // хеш данных (для аудита)

        public string? Comment { get; set; } // комментарий
        #endregion
    }
}

namespace VolleyballIS.Application.DTOs.Events
{
    // DTO для отображения игрового события матча
    public class EventDto
    {
        #region Свойства
        public int Id { get; set; } // идентификатор события

        public int MatchId { get; set; } // идентификатор матча

        public int? TeamId { get; set; } // идентификатор команды

        public string? TeamName { get; set; } // наименование команды

        public short EventTypeCode { get; set; } // код типа события

        public string? EventTypeName { get; set; } // наименование типа события

        public short SetNumber { get; set; } // номер партии

        public int GlobalSeqInSet { get; set; } // порядковый номер события в партии

        public short HomeScoreAtMoment { get; set; } // счёт хозяев в момент события

        public short GuestScoreAtMoment { get; set; } // счёт гостей в момент события

        public short? MinuteMark { get; set; } // игровая минута

        // детали замены (если применимо)
        public SubstitutionDetailDto? Substitution { get; set; }

        // детали тайм-аута (если применимо)
        public TimeoutDetailDto? Timeout { get; set; }
        #endregion
    }

    // Встроенный DTO для деталей замены
    public class SubstitutionDetailDto
    {
        public int SubOutPlayerId { get; set; }        // заменяемый игрок
        public string? SubOutPlayerName { get; set; }  // ФИО заменяемого
        public int SubInPlayerId { get; set; }         // выходящий игрок
        public string? SubInPlayerName { get; set; }   // ФИО выходящего
        public short? SubTypeCode { get; set; }        // код типа замены
        public string? SubTypeName { get; set; }       // тип замены
        public short? SubSeqInSet { get; set; }        // порядковый номер замены
        public bool IsLiberoSwap { get; set; }         // признак замещения либеро
    }

    // Встроенный DTO для деталей тайм-аута
    public class TimeoutDetailDto
    {
        public short TimeoutTypeCode { get; set; }     // код типа тайм-аута
        public string? TimeoutTypeName { get; set; }   // наименование типа
        public short? TimeoutSeqInSet { get; set; }    // порядковый номер тайм-аута
    }
}

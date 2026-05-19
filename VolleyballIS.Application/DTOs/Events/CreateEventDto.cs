using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Events
{
    // DTO для создания игрового события
    public class CreateEventDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор матча обязателен")]
        public int MatchId { get; set; } // идентификатор матча

        public int? TeamId { get; set; } // идентификатор команды (NULL для матчевых событий)

        [Required(ErrorMessage = "Код типа события обязателен")]
        public short EventTypeCode { get; set; } // код типа события (из С9)

        [Required(ErrorMessage = "Номер партии обязателен")]
        [Range(1, 5, ErrorMessage = "Номер партии должен быть от 1 до 5")]
        public short SetNumber { get; set; } // номер партии

        [Required(ErrorMessage = "Порядковый номер обязателен")]
        public int GlobalSeqInSet { get; set; } // порядковый номер события в партии

        public short HomeScoreAtMoment { get; set; } // счёт хозяев в момент события

        public short GuestScoreAtMoment { get; set; } // счёт гостей в момент события

        public short? MinuteMark { get; set; } // игровая минута

        // детали замены (заполняется если EventTypeCode = 1)
        public CreateSubstitutionDto? Substitution { get; set; }

        // детали тайм-аута (заполняется если EventTypeCode = 2 или 3)
        public CreateTimeoutDto? Timeout { get; set; }
        #endregion
    }

    // DTO для деталей замены при создании события
    public class CreateSubstitutionDto
    {
        [Required] public int SubOutPlayerId { get; set; }     // заменяемый игрок
        [Required] public int SubInPlayerId { get; set; }      // выходящий игрок
        public short? SubTypeCode { get; set; }                // код типа замены
        public short? SubSeqInSet { get; set; }                // порядковый номер замены
        public bool IsLiberoSwap { get; set; } = false;        // признак замещения либеро
    }

    // DTO для деталей тайм-аута при создании события
    public class CreateTimeoutDto
    {
        [Required] public short TimeoutTypeCode { get; set; }  // код типа тайм-аута
        public short? TimeoutSeqInSet { get; set; }            // порядковый номер тайм-аута
    }
}

namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для отображения состава заявки (игрок в заявке)
    public class ApplicationCompositionDto
    {
        #region Свойства
        public int ApplicationId { get; set; } // идентификатор заявки

        public int PlayerId { get; set; } // идентификатор игрока

        public string? PlayerName { get; set; } // ФИО игрока

        public short? ShirtNumber { get; set; } // номер в заявке

        public string Role { get; set; } = "основной"; // тип записи: основной / запасной

        public short? AmpluaCode { get; set; } // код амплуа из карточки игрока

        public string? AmpluaName { get; set; } // наименование амплуа из карточки игрока

        public bool IsLibero { get; set; } // признак либеро
        #endregion
    }
}

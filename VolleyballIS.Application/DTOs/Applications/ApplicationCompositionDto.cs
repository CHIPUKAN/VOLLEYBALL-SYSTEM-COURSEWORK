namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для отображения состава заявки (игрок в заявке)
    public class ApplicationCompositionDto
    {
        #region Свойства
        public int ApplicationId { get; set; } // идентификатор заявки

        public int PlayerId { get; set; } // идентификатор игрока

        public string? PlayerFullName { get; set; } // ФИО игрока

        public short JerseyNumberInApp { get; set; } // номер в заявке

        public string Role { get; set; } = string.Empty; // роль: «основной» / «запасной»

        public bool IsLibero { get; set; } // признак либеро
        #endregion
    }
}

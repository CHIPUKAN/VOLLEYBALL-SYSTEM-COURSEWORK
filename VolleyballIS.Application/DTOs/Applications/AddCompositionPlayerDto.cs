using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для добавления игрока в состав заявки
    public class AddCompositionPlayerDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор игрока обязателен")]
        public int PlayerId { get; set; } // идентификатор игрока

        [Required(ErrorMessage = "Номер футболки обязателен")]
        [Range(1, 99, ErrorMessage = "Номер должен быть от 1 до 99")]
        public short ShirtNumber { get; set; } // номер игрока в заявке

        public bool IsLibero { get; set; } = false; // признак либеро
        #endregion
    }
}

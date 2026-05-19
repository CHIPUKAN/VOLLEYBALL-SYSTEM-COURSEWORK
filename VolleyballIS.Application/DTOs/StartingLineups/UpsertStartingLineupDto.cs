using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.StartingLineups
{
    // DTO для создания/обновления позиции в стартовой расстановке
    public class UpsertStartingLineupDto
    {
        #region Свойства
        [Required(ErrorMessage = "Идентификатор команды обязателен")]
        public int TeamId { get; set; } // идентификатор команды

        [Required(ErrorMessage = "Номер партии обязателен")]
        [Range(1, 5, ErrorMessage = "Номер партии должен быть от 1 до 5")]
        public short SetNumber { get; set; } // номер партии

        [Required(ErrorMessage = "Номер позиции обязателен")]
        [Range(1, 6, ErrorMessage = "Позиция должна быть от 1 до 6")]
        public short PositionNo { get; set; } // позиция на площадке

        [Required(ErrorMessage = "Идентификатор игрока обязателен")]
        public int PlayerId { get; set; } // идентификатор игрока
        #endregion
    }
}

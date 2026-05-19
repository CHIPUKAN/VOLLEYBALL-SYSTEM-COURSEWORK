using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Applications
{
    // DTO для обновления заявки (изменение статуса)
    public class UpdateApplicationDto
    {
        #region Свойства
        [Required(ErrorMessage = "Код статуса обязателен")]
        public short StatusCode { get; set; } // новый код статуса заявки
        #endregion
    }
}

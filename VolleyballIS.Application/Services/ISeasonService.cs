using VolleyballIS.Application.DTOs.Seasons;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления сезонами
    public interface ISeasonService
    {
        #region Методы
        Task<IEnumerable<SeasonDto>> GetAllSeasonsAsync(); // получить все сезоны

        Task<SeasonDto?> GetSeasonByIdAsync(int id); // получить сезон по идентификатору

        Task<SeasonDto> CreateSeasonAsync(CreateSeasonDto dto); // создать сезон

        Task<SeasonDto> UpdateSeasonAsync(int id, UpdateSeasonDto dto); // обновить сезон

        Task DeleteSeasonAsync(int id); // удалить сезон
        #endregion
    }
}

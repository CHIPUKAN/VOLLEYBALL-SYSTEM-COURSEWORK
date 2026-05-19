using VolleyballIS.Application.DTOs.Coaches;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления тренерами
    public interface ICoachService
    {
        #region Методы
        Task<IEnumerable<CoachDto>> GetAllCoachesAsync(); // получить всех тренеров

        Task<CoachDto?> GetCoachByIdAsync(int id); // получить тренера по идентификатору

        Task<CoachDto> CreateCoachAsync(CreateCoachDto dto); // создать тренера

        Task<CoachDto> UpdateCoachAsync(int id, UpdateCoachDto dto); // обновить тренера

        Task DeleteCoachAsync(int id); // удалить тренера
        #endregion
    }
}

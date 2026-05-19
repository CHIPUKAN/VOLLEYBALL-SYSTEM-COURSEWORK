using VolleyballIS.Application.DTOs.Standings;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса турнирной таблицы
    public interface IStandingService
    {
        #region Методы
        Task<IEnumerable<StandingDto>> GetStandingsAsync(int tournamentId, short? stageCode, int? groupId); // рассчитать турнирную таблицу
        #endregion
    }
}

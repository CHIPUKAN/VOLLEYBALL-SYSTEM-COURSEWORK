using VolleyballIS.Application.DTOs.Matches;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления матчами
    public interface IMatchService
    {
        #region Методы
        Task<IEnumerable<MatchDto>> GetAllMatchesAsync(); // получить все матчи

        Task<IEnumerable<MatchDto>> GetMatchesByTournamentAsync(int tournamentId); // получить матчи по турниру

        Task<MatchDto?> GetMatchByIdAsync(int id); // получить матч по идентификатору

        Task<MatchDto> CreateMatchAsync(CreateMatchDto dto); // создать матч

        Task<MatchDto> UpdateMatchAsync(int id, UpdateMatchDto dto); // обновить матч

        Task DeleteMatchAsync(int id); // удалить матч
        #endregion
    }
}

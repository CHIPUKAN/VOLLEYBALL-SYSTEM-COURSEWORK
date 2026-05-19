using VolleyballIS.Application.DTOs.Tournaments;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления турнирами
    public interface ITournamentService
    {
        #region Методы
        Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync(); // получить все турниры

        Task<TournamentDto?> GetTournamentByIdAsync(int id); // получить турнир по идентификатору

        Task<TournamentDto> CreateTournamentAsync(CreateTournamentDto dto); // создать турнир

        Task<TournamentDto> UpdateTournamentAsync(int id, UpdateTournamentDto dto); // обновить турнир

        Task DeleteTournamentAsync(int id); // удалить турнир
        #endregion
    }
}

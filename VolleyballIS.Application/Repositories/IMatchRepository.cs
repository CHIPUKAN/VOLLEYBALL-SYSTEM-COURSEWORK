using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория матчей (т14)
    public interface IMatchRepository
    {
        #region Методы
        Task<IEnumerable<T14Match>> GetAllAsync(); // получить все матчи

        Task<IEnumerable<T14Match>> GetByTournamentIdAsync(int tournamentId); // получить матчи по турниру

        Task<T14Match?> GetByIdAsync(int id); // получить матч по идентификатору

        Task<T14Match> CreateAsync(T14Match match); // создать матч

        Task<T14Match> UpdateAsync(T14Match match); // обновить матч

        Task DeleteAsync(int id); // удалить матч

        Task<bool> ExistsAsync(int id); // проверить существование
        #endregion
    }
}

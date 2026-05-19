using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория игроков (т6)
    public interface IPlayerRepository
    {
        #region Методы
        Task<IEnumerable<T6Player>> GetAllAsync(); // получить всех игроков

        Task<IEnumerable<T6Player>> GetByTeamIdAsync(int teamId); // получить игроков по команде

        Task<T6Player?> GetByIdAsync(int id); // получить игрока по идентификатору

        Task<T6Player> CreateAsync(T6Player player); // создать игрока

        Task<T6Player> UpdateAsync(T6Player player); // обновить игрока

        Task DeleteAsync(int id); // удалить игрока

        Task<bool> ExistsAsync(int id); // проверить существование
        #endregion
    }
}

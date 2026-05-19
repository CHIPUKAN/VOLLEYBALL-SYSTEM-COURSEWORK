using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория судей (т12)
    public interface IRefereeRepository
    {
        #region Методы
        Task<IEnumerable<T12Referee>> GetAllAsync(); // получить всех судей

        Task<T12Referee?> GetByIdAsync(int id); // получить судью по идентификатору

        Task<T12Referee> CreateAsync(T12Referee referee); // создать судью

        Task<T12Referee> UpdateAsync(T12Referee referee); // обновить судью

        Task DeleteAsync(int id); // удалить судью

        Task<bool> ExistsAsync(int id); // проверить существование
        #endregion
    }
}

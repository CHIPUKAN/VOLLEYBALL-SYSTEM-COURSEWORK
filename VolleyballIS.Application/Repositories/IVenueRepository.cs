using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория площадок (т2)
    public interface IVenueRepository
    {
        #region Методы
        Task<IEnumerable<T2Venue>> GetAllAsync(); // получить все площадки

        Task<T2Venue?> GetByIdAsync(int id); // получить площадку по идентификатору

        Task<T2Venue> CreateAsync(T2Venue venue); // создать площадку

        Task<T2Venue> UpdateAsync(T2Venue venue); // обновить площадку

        Task DeleteAsync(int id); // удалить площадку

        Task<bool> ExistsAsync(int id); // проверить существование
        #endregion
    }
}

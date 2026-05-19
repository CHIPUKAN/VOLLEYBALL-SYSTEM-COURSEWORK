using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория организаторов (т13)
    public interface IOrganizerRepository
    {
        #region Методы
        Task<IEnumerable<T13Organizer>> GetAllAsync(); // получить всех организаторов

        Task<T13Organizer?> GetByIdAsync(int id); // получить организатора по идентификатору

        Task<T13Organizer> CreateAsync(T13Organizer organizer); // создать организатора

        Task<T13Organizer> UpdateAsync(T13Organizer organizer); // обновить организатора

        Task DeleteAsync(int id); // удалить организатора

        Task<bool> ExistsAsync(int id); // проверить существование
        #endregion
    }
}

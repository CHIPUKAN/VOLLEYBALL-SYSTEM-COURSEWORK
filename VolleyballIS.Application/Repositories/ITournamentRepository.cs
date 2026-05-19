using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория турниров (т10)
    public interface ITournamentRepository
    {
        #region Методы
        Task<IEnumerable<T10Tournament>> GetAllAsync(); // получить все турниры

        Task<T10Tournament?> GetByIdAsync(int id); // получить турнир по идентификатору

        Task<T10Tournament> CreateAsync(T10Tournament tournament); // создать турнир

        Task<T10Tournament> UpdateAsync(T10Tournament tournament); // обновить турнир

        Task DeleteAsync(int id); // удалить турнир

        Task<bool> ExistsAsync(int id); // проверить существование

        Task<bool> NameExistsAsync(string name, int? excludeId = null); // проверить уникальность наименования
        #endregion
    }
}

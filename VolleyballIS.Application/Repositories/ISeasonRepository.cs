using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория сезонов (т9)
    public interface ISeasonRepository
    {
        #region Методы
        Task<IEnumerable<T9Season>> GetAllAsync(); // получить все сезоны

        Task<T9Season?> GetByIdAsync(int id); // получить сезон по идентификатору

        Task<T9Season> CreateAsync(T9Season season); // создать сезон

        Task<T9Season> UpdateAsync(T9Season season); // обновить сезон

        Task DeleteAsync(int id); // удалить сезон

        Task<bool> ExistsAsync(int id); // проверить существование

        Task<bool> NameExistsAsync(string name, int? excludeId = null); // проверить уникальность наименования
        #endregion
    }
}

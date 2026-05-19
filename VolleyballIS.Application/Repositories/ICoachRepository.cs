using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория тренеров (т3)
    public interface ICoachRepository
    {
        #region Методы
        Task<IEnumerable<T3Coach>> GetAllAsync(); // получить всех тренеров

        Task<T3Coach?> GetByIdAsync(int id); // получить тренера по идентификатору

        Task<T3Coach> CreateAsync(T3Coach coach); // создать тренера

        Task<T3Coach> UpdateAsync(T3Coach coach); // обновить тренера

        Task DeleteAsync(int id); // удалить тренера

        Task<bool> ExistsAsync(int id); // проверить существование
        #endregion
    }
}

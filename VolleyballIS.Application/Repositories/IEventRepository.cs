using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория игровых событий
    public interface IEventRepository
    {
        Task<IEnumerable<T17Event>> GetByMatchIdAsync(int matchId);      // события матча
        Task<IEnumerable<T17Event>> GetByMatchSetAsync(int matchId, short setNumber); // события партии
        Task<T17Event?> GetByIdAsync(int id);                            // событие по id
        Task<T17Event> CreateAsync(T17Event ev);                         // создать событие
        Task DeleteAsync(int id);                                         // удалить событие
        Task<bool> ExistsAsync(int id);                                  // проверить существование
    }
}

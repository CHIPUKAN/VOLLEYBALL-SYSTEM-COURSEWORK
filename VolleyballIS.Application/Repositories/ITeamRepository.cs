using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория для работы с командами (т4) — определён в Application для инверсии зависимостей
    public interface ITeamRepository
    {
        #region Методы
        Task<IEnumerable<T4Team>> GetAllAsync(); // получить все команды с навигационными свойствами

        Task<T4Team?> GetByIdAsync(int id); // получить команду по идентификатору

        Task<T4Team> CreateAsync(T4Team team); // создать новую команду

        Task<T4Team> UpdateAsync(T4Team team); // обновить существующую команду

        Task DeleteAsync(int id); // удалить команду по идентификатору

        Task<bool> ExistsAsync(int id); // проверить существование команды по идентификатору

        Task<bool> NameExistsAsync(string name, int? excludeId = null); // проверить уникальность наименования
        #endregion
    }
}

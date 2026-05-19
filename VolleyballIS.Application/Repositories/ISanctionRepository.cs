using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория санкций
    public interface ISanctionRepository
    {
        Task<IEnumerable<T18Sanction>> GetByMatchIdAsync(int matchId); // санкции матча
        Task<T18Sanction?> GetByIdAsync(int id);                       // санкция по id
        Task<T18Sanction> CreateAsync(T18Sanction sanction);           // зарегистрировать санкцию
        Task<T18Sanction> UpdateAsync(T18Sanction sanction);           // обновить санкцию
        Task DeleteAsync(int id);                                        // удалить санкцию
        Task<bool> ExistsAsync(int id);                                 // проверить существование
    }
}

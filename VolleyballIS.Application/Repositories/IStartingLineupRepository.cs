using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Repositories
{
    // Интерфейс репозитория стартовых расстановок
    public interface IStartingLineupRepository
    {
        Task<IEnumerable<R1StartingLineup>> GetByMatchTeamSetAsync(int matchId, int teamId, short setNumber); // расстановка команды в партии
        Task<IEnumerable<R1StartingLineup>> GetByMatchIdAsync(int matchId);                                   // все расстановки матча
        Task<R1StartingLineup?> GetByKeyAsync(int matchId, int teamId, short setNumber, short positionNo);   // позиция по ключу
        Task<R1StartingLineup> UpsertAsync(R1StartingLineup lineup);                                          // создать или обновить
        Task DeleteByMatchTeamSetAsync(int matchId, int teamId, short setNumber);                             // удалить расстановку команды в партии
        Task DeleteByMatchTeamSetPositionAsync(int matchId, int teamId, short setNumber, short positionNo);  // удалить одну позицию расстановки
        Task<bool> ExistsAsync(int matchId, int teamId, short setNumber, short positionNo);                  // проверить существование
    }
}

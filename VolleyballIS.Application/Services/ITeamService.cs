using VolleyballIS.Application.DTOs.Teams;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса для управления командами
    public interface ITeamService
    {
        #region Методы
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync(); // получить список всех команд

        Task<TeamDto?> GetTeamByIdAsync(int id); // получить команду по идентификатору

        Task<TeamDto> CreateTeamAsync(CreateTeamDto dto); // создать новую команду

        Task<TeamDto> UpdateTeamAsync(int id, UpdateTeamDto dto); // обновить существующую команду

        Task DeleteTeamAsync(int id); // удалить команду по идентификатору
        #endregion
    }
}

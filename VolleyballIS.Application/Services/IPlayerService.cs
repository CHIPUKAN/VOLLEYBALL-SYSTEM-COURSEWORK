using VolleyballIS.Application.DTOs.Players;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления игроками
    public interface IPlayerService
    {
        #region Методы
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync(); // получить всех игроков

        Task<IEnumerable<PlayerDto>> GetPlayersByTeamAsync(int teamId); // получить игроков команды

        Task<PlayerDto?> GetPlayerByIdAsync(int id); // получить игрока по идентификатору

        Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto dto); // создать игрока

        Task<PlayerDto> UpdatePlayerAsync(int id, UpdatePlayerDto dto); // обновить игрока

        Task DeletePlayerAsync(int id); // удалить игрока
        #endregion
    }
}

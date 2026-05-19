using VolleyballIS.Application.DTOs.Players;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления игроками
    public class PlayerService : IPlayerService
    {
        #region Поля
        private readonly IPlayerRepository playerRepository; // репозиторий игроков
        #endregion

        #region Конструкторы
        public PlayerService(IPlayerRepository playerRepository) // конструктор с внедрением зависимости
        {
            this.playerRepository = playerRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync() // получить всех игроков
        {
            IEnumerable<T6Player> players = await playerRepository.GetAllAsync();
            IEnumerable<PlayerDto> result = players.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<PlayerDto>> GetPlayersByTeamAsync(int teamId) // получить игроков команды
        {
            IEnumerable<T6Player> players = await playerRepository.GetByTeamIdAsync(teamId);
            IEnumerable<PlayerDto> result = players.Select(MapToDto);
            return result;
        }

        public async Task<PlayerDto?> GetPlayerByIdAsync(int id) // получить игрока по идентификатору
        {
            T6Player? player = await playerRepository.GetByIdAsync(id);
            PlayerDto? result = player == null ? null : MapToDto(player);
            return result;
        }

        public async Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto dto) // создать игрока
        {
            T6Player player = new T6Player
            {
                TeamId = dto.TeamId,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                BirthDate = dto.BirthDate,
                HeightCm = dto.HeightCm,
                WeightKg = dto.WeightKg,
                JerseyNumber = dto.JerseyNumber,
                AmpluaCode = dto.AmpluaCode,
                SportsRank = dto.SportsRank,
                Email = dto.Email,
                Phone = dto.Phone,
                StatusCode = dto.StatusCode,
                PhotoUrl = dto.PhotoUrl
            };
            T6Player created = await playerRepository.CreateAsync(player);
            PlayerDto result = MapToDto(created);
            return result;
        }

        public async Task<PlayerDto> UpdatePlayerAsync(int id, UpdatePlayerDto dto) // обновить игрока
        {
            bool exists = await playerRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Игрок с идентификатором {id} не найден");
            }

            T6Player player = new T6Player
            {
                Id = id,
                TeamId = dto.TeamId,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                BirthDate = dto.BirthDate,
                HeightCm = dto.HeightCm,
                WeightKg = dto.WeightKg,
                JerseyNumber = dto.JerseyNumber,
                AmpluaCode = dto.AmpluaCode,
                SportsRank = dto.SportsRank,
                Email = dto.Email,
                Phone = dto.Phone,
                StatusCode = dto.StatusCode,
                PhotoUrl = dto.PhotoUrl
            };
            T6Player updated = await playerRepository.UpdateAsync(player);
            PlayerDto result = MapToDto(updated);
            return result;
        }

        public async Task DeletePlayerAsync(int id) // удалить игрока
        {
            bool exists = await playerRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Игрок с идентификатором {id} не найден");
            }
            await playerRepository.DeleteAsync(id);
        }

        private static PlayerDto MapToDto(T6Player player) // маппинг сущности T6Player в PlayerDto
        {
            PlayerDto result = new PlayerDto
            {
                Id = player.Id,
                TeamId = player.TeamId,
                TeamName = player.Team?.Name,
                LastName = player.LastName,
                FirstName = player.FirstName,
                MiddleName = player.MiddleName,
                FullName = player.FullName(),
                BirthDate = player.BirthDate,
                HeightCm = player.HeightCm,
                WeightKg = player.WeightKg,
                JerseyNumber = player.JerseyNumber,
                AmpluaCode = player.AmpluaCode,
                AmpluaName = player.Amplua?.Name,
                SportsRank = player.SportsRank,
                Email = player.Email,
                Phone = player.Phone,
                StatusCode = player.StatusCode,
                StatusName = player.Status?.Name,
                PhotoUrl = player.PhotoUrl
            };
            return result;
        }
        #endregion
    }
}

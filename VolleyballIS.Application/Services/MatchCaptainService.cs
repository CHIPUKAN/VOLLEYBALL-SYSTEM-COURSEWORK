using VolleyballIS.Application.DTOs.MatchCaptains;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления капитанами команд в матчах
    public class MatchCaptainService : IMatchCaptainService
    {
        #region Поля
        private readonly IMatchCaptainRepository captainRepository; // репозиторий капитанов
        private readonly IPlayerRepository playerRepository;        // репозиторий игроков
        #endregion

        #region Конструкторы
        public MatchCaptainService(IMatchCaptainRepository captainRepository, IPlayerRepository playerRepository) // конструктор с внедрением зависимости
        {
            this.captainRepository = captainRepository;
            this.playerRepository = playerRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<MatchCaptainDto>> GetCaptainsByMatchAsync(int matchId) // капитаны матча
        {
            IEnumerable<T21MatchCaptain> captains = await captainRepository.GetByMatchIdAsync(matchId);
            IEnumerable<MatchCaptainDto> result = captains.Select(MapToDto);
            return result;
        }

        public async Task<MatchCaptainDto?> GetCaptainAsync(int matchId, int teamId) // капитан команды в матче
        {
            T21MatchCaptain? captain = await captainRepository.GetByKeyAsync(matchId, teamId);
            MatchCaptainDto? result = captain == null ? null : MapToDto(captain);
            return result;
        }

        public async Task<MatchCaptainDto> UpsertCaptainAsync(int matchId, UpsertMatchCaptainDto dto) // назначить капитана
        {
            T6Player? player = await playerRepository.GetByIdAsync(dto.PlayerId);
            if (player == null)
                throw new KeyNotFoundException($"Игрок с идентификатором {dto.PlayerId} не найден");
            if (player.TeamId != dto.TeamId)
                throw new InvalidOperationException(
                    $"Игрок {player.LastName} {player.FirstName} не принадлежит команде id={dto.TeamId}");

            T21MatchCaptain captain = new T21MatchCaptain
            {
                MatchId = matchId,
                TeamId = dto.TeamId,
                PlayerId = dto.PlayerId
            };
            T21MatchCaptain upserted = await captainRepository.UpsertAsync(captain);
            MatchCaptainDto result = MapToDto(upserted);
            return result;
        }

        public async Task DeleteCaptainAsync(int matchId, int teamId) // снять капитана
        {
            bool exists = await captainRepository.ExistsAsync(matchId, teamId);
            if (!exists)
            {
                throw new KeyNotFoundException($"Запись о капитане команды {teamId} в матче {matchId} не найдена");
            }
            await captainRepository.DeleteAsync(matchId, teamId);
        }

        private static MatchCaptainDto MapToDto(T21MatchCaptain c) // маппинг T21MatchCaptain -> MatchCaptainDto
        {
            string? playerFio = c.Player != null
                ? (c.Player.MiddleName != null
                    ? $"{c.Player.LastName} {c.Player.FirstName} {c.Player.MiddleName}"
                    : $"{c.Player.LastName} {c.Player.FirstName}")
                : null;

            MatchCaptainDto result = new MatchCaptainDto
            {
                MatchId = c.MatchId,
                TeamId = c.TeamId,
                TeamName = c.Team?.Name,
                PlayerId = c.PlayerId,
                PlayerFullName = playerFio
            };
            return result;
        }
        #endregion
    }
}

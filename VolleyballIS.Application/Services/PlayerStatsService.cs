using VolleyballIS.Application.DTOs.PlayerStats;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления статистикой игроков в матчах
    public class PlayerStatsService : IPlayerStatsService
    {
        #region Поля
        private readonly IPlayerStatsRepository statsRepository; // репозиторий статистики
        private readonly IMatchRepository matchRepository;        // репозиторий матчей (для проверки статуса)

        private static readonly short[] TerminalStatusCodes = [3, 5, 6]; // Завершён, Отменён, Техническое поражение
        #endregion

        #region Конструкторы
        public PlayerStatsService(IPlayerStatsRepository statsRepository, IMatchRepository matchRepository) // конструктор с внедрением зависимости
        {
            this.statsRepository = statsRepository;
            this.matchRepository = matchRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<PlayerStatsDto>> GetStatsByMatchAsync(int matchId) // статистика матча
        {
            IEnumerable<R2PlayerStats> stats = await statsRepository.GetByMatchIdAsync(matchId);
            IEnumerable<PlayerStatsDto> result = stats.Select(MapToDto);
            return result;
        }

        public async Task<PlayerStatsDto?> GetStatsAsync(int matchId, int playerId) // статистика игрока в матче
        {
            R2PlayerStats? stats = await statsRepository.GetByKeyAsync(matchId, playerId);
            PlayerStatsDto? result = stats == null ? null : MapToDto(stats);
            return result;
        }

        public async Task<PlayerStatsDto> UpsertStatsAsync(int matchId, UpsertPlayerStatsDto dto) // создать или обновить
        {
            T14Match? match = await matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException($"Матч с идентификатором {matchId} не найден");
            if (TerminalStatusCodes.Contains(match.StatusCode))
                throw new InvalidOperationException("Нельзя изменять статистику в завершённом, отменённом или технически проигранном матче");

            R2PlayerStats stats = new R2PlayerStats
            {
                MatchId = matchId,
                PlayerId = dto.PlayerId,
                TeamId = dto.TeamId,
                ServesTotal = dto.ServesTotal,
                Aces = dto.Aces,
                ServeErrors = dto.ServeErrors,
                ReceptionsTotal = dto.ReceptionsTotal,
                PositiveReceptions = dto.PositiveReceptions,
                ReceptionErrors = dto.ReceptionErrors,
                AttacksTotal = dto.AttacksTotal,
                AttackPoints = dto.AttackPoints,
                AttackErrors = dto.AttackErrors,
                Blocks = dto.Blocks,
                TotalPoints = dto.TotalPoints
            };
            R2PlayerStats upserted = await statsRepository.UpsertAsync(stats);
            PlayerStatsDto result = MapToDto(upserted);
            return result;
        }

        public async Task DeleteStatsAsync(int matchId, int playerId) // удалить статистику
        {
            bool exists = await statsRepository.ExistsAsync(matchId, playerId);
            if (!exists)
                throw new KeyNotFoundException($"Статистика игрока {playerId} в матче {matchId} не найдена");

            T14Match? match = await matchRepository.GetByIdAsync(matchId);
            if (match != null && TerminalStatusCodes.Contains(match.StatusCode))
                throw new InvalidOperationException("Нельзя удалять статистику в завершённом, отменённом или технически проигранном матче");

            await statsRepository.DeleteAsync(matchId, playerId);
        }

        private static PlayerStatsDto MapToDto(R2PlayerStats s) // маппинг R2PlayerStats -> PlayerStatsDto
        {
            string? playerFio = s.Player != null
                ? (s.Player.MiddleName != null
                    ? $"{s.Player.LastName} {s.Player.FirstName} {s.Player.MiddleName}"
                    : $"{s.Player.LastName} {s.Player.FirstName}")
                : null;

            PlayerStatsDto result = new PlayerStatsDto
            {
                MatchId = s.MatchId,
                PlayerId = s.PlayerId,
                PlayerFullName = playerFio,
                TeamId = s.TeamId,
                TeamName = s.Team?.Name,
                ServesTotal = s.ServesTotal,
                Aces = s.Aces,
                ServeErrors = s.ServeErrors,
                ReceptionsTotal = s.ReceptionsTotal,
                PositiveReceptions = s.PositiveReceptions,
                ReceptionErrors = s.ReceptionErrors,
                AttacksTotal = s.AttacksTotal,
                AttackPoints = s.AttackPoints,
                AttackErrors = s.AttackErrors,
                Blocks = s.Blocks,
                TotalPoints = s.TotalPoints
            };
            return result;
        }
        #endregion
    }
}

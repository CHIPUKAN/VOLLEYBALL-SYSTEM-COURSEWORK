using VolleyballIS.Application.DTOs.StartingLineups;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления стартовыми расстановками команд
    public class StartingLineupService : IStartingLineupService
    {
        #region Поля
        private readonly IStartingLineupRepository lineupRepository; // репозиторий расстановок
        #endregion

        #region Конструкторы
        public StartingLineupService(IStartingLineupRepository lineupRepository) // конструктор с внедрением зависимости
        {
            this.lineupRepository = lineupRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<StartingLineupDto>> GetLineupsByMatchAsync(int matchId) // все расстановки матча
        {
            IEnumerable<R1StartingLineup> lineups = await lineupRepository.GetByMatchIdAsync(matchId);
            IEnumerable<StartingLineupDto> result = lineups.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<StartingLineupDto>> GetLineupsByMatchTeamSetAsync(int matchId, int teamId, short setNumber) // расстановка команды в партии
        {
            IEnumerable<R1StartingLineup> lineups = await lineupRepository.GetByMatchTeamSetAsync(matchId, teamId, setNumber);
            IEnumerable<StartingLineupDto> result = lineups.Select(MapToDto);
            return result;
        }

        public async Task<StartingLineupDto> UpsertPositionAsync(int matchId, UpsertStartingLineupDto dto) // занести позицию
        {
            R1StartingLineup lineup = new R1StartingLineup
            {
                MatchId = matchId,
                TeamId = dto.TeamId,
                SetNumber = dto.SetNumber,
                PositionNo = dto.PositionNo,
                PlayerId = dto.PlayerId
            };
            R1StartingLineup upserted = await lineupRepository.UpsertAsync(lineup);
            StartingLineupDto result = MapToDto(upserted);
            return result;
        }

        public async Task DeleteLineupAsync(int matchId, int teamId, short setNumber) // удалить расстановку команды в партии
        {
            await lineupRepository.DeleteByMatchTeamSetAsync(matchId, teamId, setNumber);
        }

        public async Task DeletePositionAsync(int matchId, int teamId, short setNumber, short positionNo) // удалить одну позицию расстановки
        {
            await lineupRepository.DeleteByMatchTeamSetPositionAsync(matchId, teamId, setNumber, positionNo);
        }

        private static StartingLineupDto MapToDto(R1StartingLineup l) // маппинг R1StartingLineup -> StartingLineupDto
        {
            string? playerFio = l.Player != null
                ? (l.Player.MiddleName != null
                    ? $"{l.Player.LastName} {l.Player.FirstName} {l.Player.MiddleName}"
                    : $"{l.Player.LastName} {l.Player.FirstName}")
                : null;

            StartingLineupDto result = new StartingLineupDto
            {
                MatchId = l.MatchId,
                TeamId = l.TeamId,
                TeamName = l.Team?.Name,
                SetNumber = l.SetNumber,
                PositionNo = l.PositionNo,
                PlayerId = l.PlayerId,
                PlayerFullName = playerFio
            };
            return result;
        }
        #endregion
    }
}

using VolleyballIS.Application.DTOs.Standings;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис расчёта турнирной таблицы
    public class StandingService : IStandingService
    {
        #region Поля
        private readonly IMatchRepository matchRepository;       // репозиторий матчей
        private readonly ISetRepository setRepository;           // репозиторий партий
        private readonly ITournamentRepository tournamentRepository; // репозиторий турниров
        #endregion

        #region Конструкторы
        public StandingService( // конструктор с внедрением зависимостей
            IMatchRepository matchRepository,
            ISetRepository setRepository,
            ITournamentRepository tournamentRepository)
        {
            this.matchRepository = matchRepository;
            this.setRepository = setRepository;
            this.tournamentRepository = tournamentRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<StandingDto>> GetStandingsAsync(int tournamentId, short? stageCode, int? groupId) // рассчитать турнирную таблицу
        {
            T10Tournament? tournament = await tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
            {
                throw new KeyNotFoundException($"Турнир с идентификатором {tournamentId} не найден");
            }

            S18ScoringSystem scoring = tournament.ScoringSystem ?? new S18ScoringSystem
            {
                PointsWin30 = 3, PointsWin31 = 3, PointsWin32 = 2,
                PointsLoss23 = 1, PointsLoss13 = 0, PointsLoss03 = 0
            };

            IEnumerable<T14Match> allMatches = await matchRepository.GetByTournamentIdAsync(tournamentId);

            // учитываем только завершённые матчи
            IEnumerable<T14Match> matches = allMatches.Where(m => m.StatusCode == 3);
            if (stageCode.HasValue) matches = matches.Where(m => m.StageCode == stageCode.Value);
            if (groupId.HasValue) matches = matches.Where(m => m.GroupId == groupId.Value);

            Dictionary<int, TeamStats> teamStats = new Dictionary<int, TeamStats>();

            foreach (T14Match match in matches)
            {
                IEnumerable<R3Set> sets = await setRepository.GetByMatchIdAsync(match.Id);
                List<R3Set> completedSets = sets.Where(s => s.HomeScore.HasValue && s.GuestScore.HasValue).ToList();
                if (completedSets.Count == 0) continue;

                int homeSetsWon = completedSets.Count(s => s.HomeScore > s.GuestScore);
                int guestSetsWon = completedSets.Count(s => s.GuestScore > s.HomeScore);
                if (homeSetsWon == guestSetsWon) continue;

                bool homeWins = homeSetsWon > guestSetsWon;
                int minSets = Math.Min(homeSetsWon, guestSetsWon);

                short winnerPts = minSets switch
                {
                    0 => scoring.PointsWin30,
                    1 => scoring.PointsWin31,
                    _ => scoring.PointsWin32,
                };
                short loserPts = minSets switch
                {
                    0 => scoring.PointsLoss03,
                    1 => scoring.PointsLoss13,
                    _ => scoring.PointsLoss23,
                };

                int homeMatchPoints = homeWins ? winnerPts : loserPts;
                int guestMatchPoints = homeWins ? loserPts : winnerPts;

                int homePointsWon = completedSets.Sum(s => s.HomeScore ?? 0);
                int guestPointsWon = completedSets.Sum(s => s.GuestScore ?? 0);

                EnsureTeam(teamStats, match.HomeTeamId, match.HomeTeam?.Name ?? $"Команда {match.HomeTeamId}");
                teamStats[match.HomeTeamId].AddMatch(homeWins, homeSetsWon, guestSetsWon, homePointsWon, guestPointsWon, homeMatchPoints);

                EnsureTeam(teamStats, match.GuestTeamId, match.GuestTeam?.Name ?? $"Команда {match.GuestTeamId}");
                teamStats[match.GuestTeamId].AddMatch(!homeWins, guestSetsWon, homeSetsWon, guestPointsWon, homePointsWon, guestMatchPoints);
            }

            List<TeamStats> sorted = teamStats.Values
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.SetsLost > 0 ? (double)t.SetsWon / t.SetsLost : t.SetsWon)
                .ThenByDescending(t => t.PointsLost > 0 ? (double)t.PointsWon / t.PointsLost : t.PointsWon)
                .ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].Rank = i + 1;
            }

            IEnumerable<StandingDto> result = sorted.Select(t => new StandingDto
            {
                Rank = t.Rank,
                TeamId = t.TeamId,
                TeamName = t.TeamName,
                GamesPlayed = t.GamesPlayed,
                Wins = t.Wins,
                Losses = t.Losses,
                SetsWon = t.SetsWon,
                SetsLost = t.SetsLost,
                PointsWon = t.PointsWon,
                PointsLost = t.PointsLost,
                Points = t.Points,
            });

            return result;
        }
        #endregion

        #region Вспомогательные методы
        private static void EnsureTeam(Dictionary<int, TeamStats> dict, int teamId, string teamName) // добавить команду в словарь если ещё не добавлена
        {
            if (!dict.ContainsKey(teamId))
            {
                dict[teamId] = new TeamStats { TeamId = teamId, TeamName = teamName };
            }
        }
        #endregion

        #region Вспомогательные классы
        private sealed class TeamStats // накопительная статистика команды для расчёта таблицы
        {
            public int TeamId { get; init; }
            public string TeamName { get; init; } = string.Empty;
            public int Rank { get; set; }
            public int GamesPlayed { get; private set; }
            public int Wins { get; private set; }
            public int Losses { get; private set; }
            public int SetsWon { get; private set; }
            public int SetsLost { get; private set; }
            public int PointsWon { get; private set; }
            public int PointsLost { get; private set; }
            public int Points { get; private set; }

            public void AddMatch(bool won, int setsWon, int setsLost, int pointsWon, int pointsLost, int matchPoints) // добавить результат матча
            {
                GamesPlayed++;
                if (won) Wins++; else Losses++;
                SetsWon += setsWon;
                SetsLost += setsLost;
                PointsWon += pointsWon;
                PointsLost += pointsLost;
                Points += matchPoints;
            }
        }
        #endregion
    }
}

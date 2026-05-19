using VolleyballIS.Application.DTOs.Matches;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления матчами
    public class MatchService : IMatchService
    {
        #region Поля
        private readonly IMatchRepository matchRepository; // репозиторий матчей
        #endregion

        #region Конструкторы
        public MatchService(IMatchRepository matchRepository) // конструктор с внедрением зависимости
        {
            this.matchRepository = matchRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync() // получить все матчи
        {
            IEnumerable<T14Match> matches = await matchRepository.GetAllAsync();
            IEnumerable<MatchDto> result = matches.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesByTournamentAsync(int tournamentId) // получить матчи по турниру
        {
            IEnumerable<T14Match> matches = await matchRepository.GetByTournamentIdAsync(tournamentId);
            IEnumerable<MatchDto> result = matches.Select(MapToDto);
            return result;
        }

        public async Task<MatchDto?> GetMatchByIdAsync(int id) // получить матч по идентификатору
        {
            T14Match? match = await matchRepository.GetByIdAsync(id);
            MatchDto? result = match == null ? null : MapToDto(match);
            return result;
        }

        public async Task<MatchDto> CreateMatchAsync(CreateMatchDto dto) // создать матч
        {
            if (dto.HomeTeamId == dto.GuestTeamId)
            {
                throw new InvalidOperationException("Команда-хозяин и команда-гость не могут совпадать");
            }

            T14Match match = new T14Match
            {
                TournamentId = dto.TournamentId,
                HomeTeamId = dto.HomeTeamId,
                GuestTeamId = dto.GuestTeamId,
                MatchDate = dto.MatchDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                VenueId = dto.VenueId,
                StageCode = dto.StageCode,
                GroupId = dto.GroupId,
                StatusCode = dto.StatusCode,
                TechDefeatReason = dto.TechDefeatReason,
                CoinTossWinnerTeamId = dto.CoinTossWinnerTeamId,
                CoinTossChoiceCode = dto.CoinTossChoiceCode,
                FirstServeTeamId = dto.FirstServeTeamId,
                HasVideoChallenge = dto.HasVideoChallenge,
                NetHeight = dto.NetHeight
            };
            T14Match created = await matchRepository.CreateAsync(match);
            MatchDto result = MapToDto(created);
            return result;
        }

        public async Task<MatchDto> UpdateMatchAsync(int id, UpdateMatchDto dto) // обновить матч
        {
            bool exists = await matchRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Матч с идентификатором {id} не найден");
            }

            if (dto.HomeTeamId == dto.GuestTeamId)
            {
                throw new InvalidOperationException("Команда-хозяин и команда-гость не могут совпадать");
            }

            T14Match match = new T14Match
            {
                Id = id,
                TournamentId = dto.TournamentId,
                HomeTeamId = dto.HomeTeamId,
                GuestTeamId = dto.GuestTeamId,
                MatchDate = dto.MatchDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                VenueId = dto.VenueId,
                StageCode = dto.StageCode,
                GroupId = dto.GroupId,
                StatusCode = dto.StatusCode,
                TechDefeatReason = dto.TechDefeatReason,
                CoinTossWinnerTeamId = dto.CoinTossWinnerTeamId,
                CoinTossChoiceCode = dto.CoinTossChoiceCode,
                FirstServeTeamId = dto.FirstServeTeamId,
                HasVideoChallenge = dto.HasVideoChallenge,
                NetHeight = dto.NetHeight
            };
            T14Match updated = await matchRepository.UpdateAsync(match);
            MatchDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteMatchAsync(int id) // удалить матч
        {
            bool exists = await matchRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Матч с идентификатором {id} не найден");
            }
            await matchRepository.DeleteAsync(id);
        }

        private static MatchDto MapToDto(T14Match match) // маппинг сущности T14Match в MatchDto
        {
            MatchDto result = new MatchDto
            {
                Id = match.Id,
                TournamentId = match.TournamentId,
                TournamentName = match.Tournament?.Name,
                HomeTeamId = match.HomeTeamId,
                HomeTeamName = match.HomeTeam?.Name,
                GuestTeamId = match.GuestTeamId,
                GuestTeamName = match.GuestTeam?.Name,
                MatchDate = match.MatchDate,
                StartTime = match.StartTime,
                EndTime = match.EndTime,
                VenueId = match.VenueId,
                VenueName = match.Venue?.Name,
                VenueCity = match.Venue?.City,
                StageCode = match.StageCode,
                StageName = match.Stage?.Name,
                GroupId = match.GroupId,
                GroupName = match.Group?.Name,
                StatusCode = match.StatusCode,
                StatusName = match.Status?.Name,
                TechDefeatReason = match.TechDefeatReason,
                CoinTossWinnerTeamId = match.CoinTossWinnerTeamId,
                CoinTossChoiceCode = match.CoinTossChoiceCode,
                CoinTossChoiceName = match.CoinTossChoice?.Name,
                FirstServeTeamId = match.FirstServeTeamId,
                HasVideoChallenge = match.HasVideoChallenge,
                NetHeight = match.NetHeight
            };
            return result;
        }
        #endregion
    }
}

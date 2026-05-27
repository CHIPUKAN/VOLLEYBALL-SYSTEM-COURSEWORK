using VolleyballIS.Application.DTOs.Sets;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления партиями матча
    public class SetService : ISetService
    {
        #region Поля
        private readonly ISetRepository setRepository;   // репозиторий партий
        private readonly IMatchRepository matchRepository; // репозиторий матчей (для проверки статуса)

        private static readonly short[] TerminalStatusCodes = [3, 5, 6]; // Завершён, Отменён, Техническое поражение
        #endregion

        #region Конструкторы
        public SetService(ISetRepository setRepository, IMatchRepository matchRepository) // конструктор с внедрением зависимости
        {
            this.setRepository = setRepository;
            this.matchRepository = matchRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<SetDto>> GetSetsByMatchAsync(int matchId) // партии матча
        {
            IEnumerable<R3Set> sets = await setRepository.GetByMatchIdAsync(matchId);
            IEnumerable<SetDto> result = sets.Select(MapToDto);
            return result;
        }

        public async Task<SetDto?> GetSetAsync(int matchId, short setNumber) // конкретная партия
        {
            R3Set? set = await setRepository.GetByKeyAsync(matchId, setNumber);
            SetDto? result = set == null ? null : MapToDto(set);
            return result;
        }

        public async Task<SetDto> UpsertSetAsync(int matchId, UpsertSetDto dto) // создать или обновить партию
        {
            T14Match? match = await matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException($"Матч с идентификатором {matchId} не найден");
            if (TerminalStatusCodes.Contains(match.StatusCode))
                throw new InvalidOperationException("Нельзя изменять партии в завершённом, отменённом или технически проигранном матче");

            short setsToWin = match.Tournament?.SetsToWin ?? 3;
            short tiebreakTarget = match.Tournament?.TiebreakScoreTarget ?? 15;
            short maxSetNumber = (short)(setsToWin * 2 - 1); // 5 для best-of-5, 3 для best-of-3, 1 для best-of-1

            if (dto.SetNumber > maxSetNumber)
                throw new InvalidOperationException(
                    $"В матче этого формата максимально допустимый номер партии: {maxSetNumber}");

            if (dto.HomeScore.HasValue && dto.GuestScore.HasValue)
            {
                short hs = dto.HomeScore.Value;
                short gs = dto.GuestScore.Value;
                if (hs == gs)
                    throw new InvalidOperationException("Счёт партии не может быть равным");

                // решающая партия — последняя возможная (maxSetNumber) при setsToWin > 1
                bool isTiebreak = dto.SetNumber == maxSetNumber && setsToWin > 1;
                short threshold = isTiebreak ? tiebreakTarget : (short)25;

                short max = Math.Max(hs, gs);
                short min = Math.Min(hs, gs);
                if (max < threshold)
                    throw new InvalidOperationException($"Победитель должен набрать минимум {threshold} очков");
                if (max - min < 2)
                    throw new InvalidOperationException("Разница в счёте должна быть минимум 2 очка");
                if (max > threshold && max - min != 2)
                    throw new InvalidOperationException(
                        $"При превышении {threshold} разница должна быть ровно 2 очка (получено {max}:{min})");
            }
            R3Set set = new R3Set
            {
                MatchId = matchId,
                SetNumber = dto.SetNumber,
                HomeScore = dto.HomeScore,
                GuestScore = dto.GuestScore,
                DurationMin = dto.DurationMin
            };
            R3Set upserted = await setRepository.UpsertAsync(set);
            SetDto result = MapToDto(upserted);
            return result;
        }

        public async Task DeleteSetAsync(int matchId, short setNumber) // удалить партию
        {
            bool exists = await setRepository.ExistsAsync(matchId, setNumber);
            if (!exists)
                throw new KeyNotFoundException($"Партия {setNumber} матча {matchId} не найдена");

            T14Match? match = await matchRepository.GetByIdAsync(matchId);
            if (match != null && TerminalStatusCodes.Contains(match.StatusCode))
                throw new InvalidOperationException("Нельзя удалять партии в завершённом, отменённом или технически проигранном матче");

            await setRepository.DeleteAsync(matchId, setNumber);
        }

        private static SetDto MapToDto(R3Set s) // маппинг R3Set -> SetDto
        {
            SetDto result = new SetDto
            {
                MatchId = s.MatchId,
                SetNumber = s.SetNumber,
                HomeScore = s.HomeScore,
                GuestScore = s.GuestScore,
                DurationMin = s.DurationMin
            };
            return result;
        }
        #endregion
    }
}

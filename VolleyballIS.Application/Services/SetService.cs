using VolleyballIS.Application.DTOs.Sets;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления партиями матча
    public class SetService : ISetService
    {
        #region Поля
        private readonly ISetRepository setRepository; // репозиторий партий
        #endregion

        #region Конструкторы
        public SetService(ISetRepository setRepository) // конструктор с внедрением зависимости
        {
            this.setRepository = setRepository;
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
            {
                throw new KeyNotFoundException($"Партия {setNumber} матча {matchId} не найдена");
            }
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

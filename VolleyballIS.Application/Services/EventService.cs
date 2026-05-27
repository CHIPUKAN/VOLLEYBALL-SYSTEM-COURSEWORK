using VolleyballIS.Application.DTOs.Events;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления игровыми событиями матча
    public class EventService : IEventService
    {
        #region Поля
        private readonly IEventRepository eventRepository; // репозиторий событий
        private readonly IMatchRepository matchRepository; // репозиторий матчей (для проверки статуса)

        // коды терминальных статусов матча (запись запрещена)
        private static readonly short[] TerminalStatusCodes = [3, 5, 6];
        #endregion

        #region Конструкторы
        public EventService(IEventRepository eventRepository, IMatchRepository matchRepository) // конструктор с внедрением зависимости
        {
            this.eventRepository = eventRepository;
            this.matchRepository = matchRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<EventDto>> GetEventsByMatchAsync(int matchId) // события матча
        {
            IEnumerable<T17Event> events = await eventRepository.GetByMatchIdAsync(matchId);
            IEnumerable<EventDto> result = events.Select(MapToDto);
            return result;
        }

        public async Task<IEnumerable<EventDto>> GetEventsByMatchSetAsync(int matchId, short setNumber) // события партии
        {
            IEnumerable<T17Event> events = await eventRepository.GetByMatchSetAsync(matchId, setNumber);
            IEnumerable<EventDto> result = events.Select(MapToDto);
            return result;
        }

        public async Task<EventDto?> GetEventByIdAsync(int id) // событие по id
        {
            T17Event? ev = await eventRepository.GetByIdAsync(id);
            EventDto? result = ev == null ? null : MapToDto(ev);
            return result;
        }

        public async Task<EventDto> CreateEventAsync(CreateEventDto dto) // создать событие
        {
            T14Match? match = await matchRepository.GetByIdAsync(dto.MatchId);
            if (match == null)
                throw new KeyNotFoundException($"Матч с идентификатором {dto.MatchId} не найден");
            if (TerminalStatusCodes.Contains(match.StatusCode))
                throw new InvalidOperationException("Нельзя добавлять события в завершённый, отменённый или технически проигранный матч");

            T17Event ev = new T17Event
            {
                MatchId = dto.MatchId,
                TeamId = dto.TeamId,
                EventTypeCode = dto.EventTypeCode,
                SetNumber = dto.SetNumber,
                GlobalSeqInSet = dto.GlobalSeqInSet,
                HomeScoreAtMoment = dto.HomeScoreAtMoment,
                GuestScoreAtMoment = dto.GuestScoreAtMoment,
                MinuteMark = dto.MinuteMark
            };

            if (dto.Substitution != null)
            {
                ev.Substitution = new T17aSubstitution
                {
                    SubOutPlayerId = dto.Substitution.SubOutPlayerId,
                    SubInPlayerId = dto.Substitution.SubInPlayerId,
                    SubTypeCode = dto.Substitution.SubTypeCode,
                    SubSeqInSet = dto.Substitution.SubSeqInSet,
                    IsLiberoSwap = dto.Substitution.IsLiberoSwap
                };
            }

            if (dto.Timeout != null)
            {
                ev.Timeout = new T17bTimeout
                {
                    TimeoutTypeCode = dto.Timeout.TimeoutTypeCode,
                    TimeoutSeqInSet = dto.Timeout.TimeoutSeqInSet
                };
            }

            T17Event created = await eventRepository.CreateAsync(ev);
            EventDto result = MapToDto(created);
            return result;
        }

        public async Task DeleteEventAsync(int id) // удалить событие
        {
            T17Event? ev = await eventRepository.GetByIdAsync(id);
            if (ev == null)
                throw new KeyNotFoundException($"Событие с идентификатором {id} не найдено");

            T14Match? match = await matchRepository.GetByIdAsync(ev.MatchId);
            if (match != null && TerminalStatusCodes.Contains(match.StatusCode))
                throw new InvalidOperationException("Нельзя удалять события в завершённом, отменённом или технически проигранном матче");

            await eventRepository.DeleteAsync(id);
        }

        private static string? BuildFio(T6Player? player) // вспомогательный метод для ФИО
        {
            if (player == null) return null;
            return player.MiddleName != null
                ? $"{player.LastName} {player.FirstName} {player.MiddleName}"
                : $"{player.LastName} {player.FirstName}";
        }

        private static EventDto MapToDto(T17Event ev) // маппинг T17Event -> EventDto
        {
            EventDto result = new EventDto
            {
                Id = ev.Id,
                MatchId = ev.MatchId,
                TeamId = ev.TeamId,
                TeamName = ev.Team?.Name,
                EventTypeCode = ev.EventTypeCode,
                EventTypeName = ev.EventType?.Name,
                SetNumber = ev.SetNumber,
                GlobalSeqInSet = ev.GlobalSeqInSet,
                HomeScoreAtMoment = ev.HomeScoreAtMoment,
                GuestScoreAtMoment = ev.GuestScoreAtMoment,
                MinuteMark = ev.MinuteMark
            };

            if (ev.Substitution != null)
            {
                result.Substitution = new SubstitutionDetailDto
                {
                    SubOutPlayerId = ev.Substitution.SubOutPlayerId,
                    SubOutPlayerName = BuildFio(ev.Substitution.SubOutPlayer),
                    SubInPlayerId = ev.Substitution.SubInPlayerId,
                    SubInPlayerName = BuildFio(ev.Substitution.SubInPlayer),
                    SubTypeCode = ev.Substitution.SubTypeCode,
                    SubTypeName = ev.Substitution.SubType?.Name,
                    SubSeqInSet = ev.Substitution.SubSeqInSet,
                    IsLiberoSwap = ev.Substitution.IsLiberoSwap
                };
            }

            if (ev.Timeout != null)
            {
                result.Timeout = new TimeoutDetailDto
                {
                    TimeoutTypeCode = ev.Timeout.TimeoutTypeCode,
                    TimeoutTypeName = ev.Timeout.TimeoutType?.Name,
                    TimeoutSeqInSet = ev.Timeout.TimeoutSeqInSet
                };
            }

            return result;
        }
        #endregion
    }
}

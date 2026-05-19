using VolleyballIS.Application.DTOs.Events;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления игровыми событиями
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetEventsByMatchAsync(int matchId);
        Task<IEnumerable<EventDto>> GetEventsByMatchSetAsync(int matchId, short setNumber);
        Task<EventDto?> GetEventByIdAsync(int id);
        Task<EventDto> CreateEventAsync(CreateEventDto dto);
        Task DeleteEventAsync(int id);
    }
}

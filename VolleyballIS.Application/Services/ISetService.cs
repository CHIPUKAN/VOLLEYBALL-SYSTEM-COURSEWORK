using VolleyballIS.Application.DTOs.Sets;

namespace VolleyballIS.Application.Services
{
    // Интерфейс сервиса управления партиями матча
    public interface ISetService
    {
        Task<IEnumerable<SetDto>> GetSetsByMatchAsync(int matchId);
        Task<SetDto?> GetSetAsync(int matchId, short setNumber);
        Task<SetDto> UpsertSetAsync(int matchId, UpsertSetDto dto);
        Task DeleteSetAsync(int matchId, short setNumber);
    }
}

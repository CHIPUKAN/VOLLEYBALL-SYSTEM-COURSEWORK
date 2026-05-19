using VolleyballIS.Application.DTOs.Groups;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Application.Services
{
    // Сервис управления группами турнира
    public class GroupService : IGroupService
    {
        #region Поля
        private readonly IGroupRepository groupRepository; // репозиторий групп
        #endregion

        #region Конструкторы
        public GroupService(IGroupRepository groupRepository) // конструктор с внедрением зависимости
        {
            this.groupRepository = groupRepository;
        }
        #endregion

        #region Методы
        public async Task<IEnumerable<GroupDto>> GetGroupsByTournamentAsync(int tournamentId) // получить группы турнира
        {
            IEnumerable<T11Group> groups = await groupRepository.GetByTournamentIdAsync(tournamentId);
            IEnumerable<GroupDto> result = groups.Select(MapToDto);
            return result;
        }

        public async Task<GroupDto?> GetGroupByIdAsync(int id) // получить группу по id
        {
            T11Group? group = await groupRepository.GetByIdAsync(id);
            GroupDto? result = group == null ? null : MapToDto(group);
            return result;
        }

        public async Task<GroupDto> CreateGroupAsync(CreateGroupDto dto) // создать группу
        {
            bool nameExists = await groupRepository.NameExistsInTournamentAsync(dto.TournamentId, dto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"Группа «{dto.Name}» уже существует в данном турнире");
            }

            T11Group group = new T11Group
            {
                TournamentId = dto.TournamentId,
                Name = dto.Name
            };
            T11Group created = await groupRepository.CreateAsync(group);
            GroupDto result = MapToDto(created);
            return result;
        }

        public async Task<GroupDto> UpdateGroupAsync(int id, UpdateGroupDto dto) // обновить группу
        {
            T11Group? existing = await groupRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Группа с идентификатором {id} не найдена");
            }

            bool nameExists = await groupRepository.NameExistsInTournamentAsync(existing.TournamentId, dto.Name, id);
            if (nameExists)
            {
                throw new InvalidOperationException($"Группа «{dto.Name}» уже существует в данном турнире");
            }

            existing.Name = dto.Name;
            T11Group updated = await groupRepository.UpdateAsync(existing);
            GroupDto result = MapToDto(updated);
            return result;
        }

        public async Task DeleteGroupAsync(int id) // удалить группу
        {
            bool exists = await groupRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Группа с идентификатором {id} не найдена");
            }
            await groupRepository.DeleteAsync(id);
        }

        private static GroupDto MapToDto(T11Group group) // маппинг T11Group -> GroupDto
        {
            GroupDto result = new GroupDto
            {
                Id = group.Id,
                TournamentId = group.TournamentId,
                TournamentName = group.Tournament?.Name,
                Name = group.Name
            };
            return result;
        }
        #endregion
    }
}

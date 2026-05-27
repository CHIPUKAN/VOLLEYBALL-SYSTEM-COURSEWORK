using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Tournaments
{
    // DTO для обновления турнира
    public class UpdateTournamentDto
    {
        #region Свойства
        public int? SeasonId { get; set; } // новый идентификатор сезона

        public int? OrganizerId { get; set; } // новый идентификатор организатора

        [Required(ErrorMessage = "Наименование турнира обязательно")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // новое наименование

        [Required]
        public DateOnly StartDate { get; set; } // новая дата начала

        [Required]
        public DateOnly EndDate { get; set; } // новая дата окончания

        public DateOnly? ApplicationDeadline { get; set; } // новый крайний срок

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty; // новый город

        public string? Description { get; set; } // новое описание

        [Range(1, 9999)]
        public short? MaxTeams { get; set; } // новый максимум команд

        [Required]
        public string Gender { get; set; } = "мужской"; // новый пол

        [MaxLength(20)]
        public string? AgeCategory { get; set; } // новая возрастная категория

        [Required]
        public string Level { get; set; } = "региональный"; // новый уровень

        public short MaxPlayersPerApp { get; set; } = 12; // новый максимум игроков

        [Required]
        public short FormatCode { get; set; } // новый формат

        [Required]
        public short ScoringSystemCode { get; set; } // новая система очков

        [Range(1, 3, ErrorMessage = "Количество побед должно быть от 1 до 3")]
        public short SetsToWin { get; set; } = 3; // до скольки побед по партиям играется матч

        [Range(5, 30, ErrorMessage = "Порог решающей партии должен быть от 5 до 30 очков")]
        public short TiebreakScoreTarget { get; set; } = 15; // порог счёта в решающей партии
        #endregion
    }
}

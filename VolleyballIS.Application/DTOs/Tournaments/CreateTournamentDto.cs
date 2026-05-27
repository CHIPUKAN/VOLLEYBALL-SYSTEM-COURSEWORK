using System.ComponentModel.DataAnnotations;

namespace VolleyballIS.Application.DTOs.Tournaments
{
    // DTO для создания нового турнира
    public class CreateTournamentDto
    {
        #region Свойства
        public int? SeasonId { get; set; } // идентификатор сезона (необязательно)

        public int? OrganizerId { get; set; } // идентификатор организатора (необязательно)

        [Required(ErrorMessage = "Наименование турнира обязательно")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // наименование турнира

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateOnly StartDate { get; set; } // дата начала

        [Required(ErrorMessage = "Дата окончания обязательна")]
        public DateOnly EndDate { get; set; } // дата окончания

        public DateOnly? ApplicationDeadline { get; set; } // крайний срок подачи заявок (необязательно)

        [Required(ErrorMessage = "Город обязателен")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty; // город проведения

        public string? Description { get; set; } // описание (необязательно)

        [Range(1, 9999)]
        public short? MaxTeams { get; set; } // максимальное количество команд (необязательно)

        [Required(ErrorMessage = "Пол участников обязателен")]
        public string Gender { get; set; } = "мужской"; // пол: мужской / женский / смешанный

        [MaxLength(20)]
        public string? AgeCategory { get; set; } // возрастная категория (необязательно)

        [Required(ErrorMessage = "Уровень турнира обязателен")]
        public string Level { get; set; } = "региональный"; // уровень: ФИВБ / нац. / рег. / местный

        public short MaxPlayersPerApp { get; set; } = 12; // максимум игроков: 12 или 14

        [Required(ErrorMessage = "Формат турнира обязателен")]
        [Range(1, short.MaxValue, ErrorMessage = "Код формата должен быть положительным")]
        public short FormatCode { get; set; } // код формата

        [Required(ErrorMessage = "Система начисления очков обязательна")]
        [Range(1, short.MaxValue, ErrorMessage = "Код системы очков должен быть положительным")]
        public short ScoringSystemCode { get; set; } // код системы очков

        [Range(1, 3, ErrorMessage = "Количество побед должно быть от 1 до 3")]
        public short SetsToWin { get; set; } = 3; // до скольки побед по партиям играется матч

        [Range(5, 30, ErrorMessage = "Порог решающей партии должен быть от 5 до 30 очков")]
        public short TiebreakScoreTarget { get; set; } = 15; // порог счёта в решающей партии
        #endregion
    }
}

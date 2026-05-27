using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Infrastructure.Data
{
    // Класс для первоначального заполнения справочников данными
    public static class DataSeeder
    {
        public static async Task SeedAsync(VolleyballDbContext dbContext) // заполнить все справочники если они пустые
        {
            await SeedDefaultAdminAsync(dbContext);
            await SeedAmplua(dbContext);
            await SeedPlayerStatuses(dbContext);
            await SeedApplicationStatuses(dbContext);
            await SeedTournamentFormats(dbContext);
            await SeedTournamentStages(dbContext);
            await SeedMatchStatuses(dbContext);
            await SeedProtocolStatuses(dbContext);
            await SeedRefereeRoles(dbContext);
            await SeedEventTypes(dbContext);
            await SeedSubstitutionTypes(dbContext);
            await SeedTimeoutTypes(dbContext);
            await SeedSanctionTypes(dbContext);
            await SeedDelayViolations(dbContext);
            await SeedAwardTypes(dbContext);
            await SeedSanctionKinds(dbContext);
            await SeedRecipientTypes(dbContext);
            await SeedCoinTossOptions(dbContext);
            await SeedScoringSystems(dbContext);
        }

        private static async Task SeedDefaultAdminAsync(VolleyballDbContext db) // создать дефолтного суперадминистратора
        {
            await SeedUserIfNotExistsAsync(db, "admin@volleyball.ru",        "Admin123!",      "Суперадминистратор",    "Суперадминистратор");
            await SeedUserIfNotExistsAsync(db, "coach@volleyball.ru",        "Coach123!",      "ТренерКоманды",         "Тренер Команды");
            await SeedUserIfNotExistsAsync(db, "secretary@volleyball.ru",    "Secretary123!",  "СекретарьМатча",        "Секретарь Матча");
            await SeedUserIfNotExistsAsync(db, "organizer@volleyball.ru",    "Organizer123!",  "Организатор",           "Организатор");
            await SeedUserIfNotExistsAsync(db, "referee_user@volleyball.ru", "Referee123!",    "СудьяМатча",            "Судья Матча");
            await SeedUserIfNotExistsAsync(db, "teamrep@volleyball.ru",      "TeamRep123!",    "ПредставительКоманды",  "Представитель Команды");
        }

        private static async Task SeedUserIfNotExistsAsync( // создать пользователя если не существует
            VolleyballDbContext db, string email, string password, string role, string fullName)
        {
            if (await db.Users.AnyAsync(u => u.Email == email)) return;

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(password),
                salt: salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);
            string passwordHash = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);

            db.Users.Add(new AppUser
            {
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                FullName = fullName,
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        private static async Task SeedAmplua(VolleyballDbContext db) // С1. Амплуа игроков
        {
            if (await db.Amplua.AnyAsync()) return;

            db.Amplua.AddRange(
                new S1Amplua { Code = 1, Name = "Доигровщик" },
                new S1Amplua { Code = 2, Name = "Диагональный" },
                new S1Amplua { Code = 3, Name = "Центральный блокирующий" },
                new S1Amplua { Code = 4, Name = "Связующий" },
                new S1Amplua { Code = 5, Name = "Либеро" },
                new S1Amplua { Code = 6, Name = "Универсальный" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedPlayerStatuses(VolleyballDbContext db) // С2. Статусы игроков
        {
            if (await db.PlayerStatuses.AnyAsync()) return;

            db.PlayerStatuses.AddRange(
                new S2PlayerStatus { Code = 1, Name = "Активен" },
                new S2PlayerStatus { Code = 2, Name = "Заявлен" },
                new S2PlayerStatus { Code = 3, Name = "Дисквалифицирован" },
                new S2PlayerStatus { Code = 4, Name = "Травмирован" },
                new S2PlayerStatus { Code = 5, Name = "Покинул команду" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedApplicationStatuses(VolleyballDbContext db) // С3. Статусы заявок
        {
            if (await db.ApplicationStatuses.AnyAsync()) return;

            db.ApplicationStatuses.AddRange(
                new S3ApplicationStatus { Code = 1, Name = "На рассмотрении" },
                new S3ApplicationStatus { Code = 2, Name = "Принята" },
                new S3ApplicationStatus { Code = 3, Name = "Отклонена" },
                new S3ApplicationStatus { Code = 4, Name = "Отозвана" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedTournamentFormats(VolleyballDbContext db) // С4. Форматы турниров
        {
            if (await db.TournamentFormats.AnyAsync()) return;

            db.TournamentFormats.AddRange(
                new S4TournamentFormat { Code = 1, Name = "Олимпийская система (плей-офф)" },
                new S4TournamentFormat { Code = 2, Name = "Круговая система" },
                new S4TournamentFormat { Code = 3, Name = "Смешанная (группы + плей-офф)" },
                new S4TournamentFormat { Code = 4, Name = "Двойное выбывание" },
                new S4TournamentFormat { Code = 5, Name = "Лига (несколько туров)" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedTournamentStages(VolleyballDbContext db) // С5. Этапы турниров
        {
            if (await db.TournamentStages.AnyAsync()) return;

            db.TournamentStages.AddRange(
                new S5TournamentStage { Code = 1, Name = "Квалификация" },
                new S5TournamentStage { Code = 2, Name = "Групповой этап" },
                new S5TournamentStage { Code = 3, Name = "1/8 финала" },
                new S5TournamentStage { Code = 4, Name = "Четвертьфинал" },
                new S5TournamentStage { Code = 5, Name = "Полуфинал" },
                new S5TournamentStage { Code = 6, Name = "Матч за 3-е место" },
                new S5TournamentStage { Code = 7, Name = "Финал" },
                new S5TournamentStage { Code = 8, Name = "Тур регулярного чемпионата" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedMatchStatuses(VolleyballDbContext db) // С6. Статусы матчей
        {
            if (await db.MatchStatuses.AnyAsync()) return;

            db.MatchStatuses.AddRange(
                new S6MatchStatus { Code = 1, Name = "Запланирован" },
                new S6MatchStatus { Code = 2, Name = "В процессе" },
                new S6MatchStatus { Code = 3, Name = "Завершён" },
                new S6MatchStatus { Code = 4, Name = "Перенесён" },
                new S6MatchStatus { Code = 5, Name = "Отменён" },
                new S6MatchStatus { Code = 6, Name = "Техническое поражение" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedProtocolStatuses(VolleyballDbContext db) // С7. Статусы протоколов
        {
            if (await db.ProtocolStatuses.AnyAsync()) return;

            db.ProtocolStatuses.AddRange(
                new S7ProtocolStatus { Code = 1, Name = "Черновик" },
                new S7ProtocolStatus { Code = 2, Name = "На проверке" },
                new S7ProtocolStatus { Code = 3, Name = "Утверждён" },
                new S7ProtocolStatus { Code = 4, Name = "Оспорен" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedRefereeRoles(VolleyballDbContext db) // С8. Роли судей
        {
            if (await db.RefereeRoles.AnyAsync()) return;

            db.RefereeRoles.AddRange(
                new S8RefereeRole { Code = 1, Name = "Первый судья" },
                new S8RefereeRole { Code = 2, Name = "Второй судья" },
                new S8RefereeRole { Code = 3, Name = "Секретарь" },
                new S8RefereeRole { Code = 4, Name = "Линейный судья" },
                new S8RefereeRole { Code = 5, Name = "Судья-счётчик" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedEventTypes(VolleyballDbContext db) // С9. Типы событий матча
        {
            // upsert: добавляем новые типы и обновляем существующие
            var allTypes = new List<S9EventType>
            {
                new S9EventType { Code = 1,  Name = "Замена игрока",            IsTeamEvent = true,  Description = "Плановая замена полевого игрока" },
                new S9EventType { Code = 2,  Name = "Командный тайм-аут",        IsTeamEvent = true,  Description = "Тайм-аут по просьбе команды" },
                new S9EventType { Code = 3,  Name = "Технический тайм-аут",      IsTeamEvent = false, Description = "Автоматический тайм-аут при счёте 8 и 16 в партиях 1-4" },
                new S9EventType { Code = 4,  Name = "Очко хозяев",               IsTeamEvent = true,  Description = "Команда-хозяин выиграла розыгрыш" },
                new S9EventType { Code = 5,  Name = "Очко гостей",               IsTeamEvent = true,  Description = "Команда-гость выиграла розыгрыш" },
                new S9EventType { Code = 6,  Name = "Начало партии",             IsTeamEvent = false, Description = "Партия начата" },
                new S9EventType { Code = 7,  Name = "Конец партии",              IsTeamEvent = false, Description = "Партия завершена" },
                new S9EventType { Code = 8,  Name = "Замена либеро",             IsTeamEvent = true,  Description = "Выход/возврат либеро" },
                new S9EventType { Code = 9,  Name = "Экстренная замена",         IsTeamEvent = true,  Description = "Замена по медицинским показаниям" },
                new S9EventType { Code = 10, Name = "Видеопросмотр (challenge)", IsTeamEvent = true,  Description = "Запрос видеопросмотра" },
                new S9EventType { Code = 11, Name = "Эйс",                       IsTeamEvent = true,  Description = "Очко с подачи" },
                new S9EventType { Code = 12, Name = "Ошибка подачи",             IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 13, Name = "Подача",                    IsTeamEvent = true,  Description = "Подача введена в игру (нейтральное)" },
                new S9EventType { Code = 14, Name = "Приём отличный",            IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 15, Name = "Приём хороший",             IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 16, Name = "Приём слабый",              IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 17, Name = "Ошибка приёма",             IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 18, Name = "Передача",                  IsTeamEvent = true,  Description = "Связующая передача" },
                new S9EventType { Code = 19, Name = "Очко атаки",                IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 20, Name = "Атака заблокирована",       IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 21, Name = "Ошибка атаки",              IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 22, Name = "Блок-очко",                 IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 23, Name = "Блок-касание",              IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 24, Name = "Ошибка блока",              IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 25, Name = "Техническое очко",          IsTeamEvent = true,  Description = "Очко без выбора игрока (кнопка +1)" },
                new S9EventType { Code = 26, Name = "Ошибка передачи",           IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 27, Name = "Касание сетки",             IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 28, Name = "Заступ",                    IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 29, Name = "Ошибка расстановки",        IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 30, Name = "Двойное касание",           IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 31, Name = "Четыре касания",            IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 32, Name = "Захват мяча",               IsTeamEvent = true,  Description = null },
                new S9EventType { Code = 33, Name = "Спорный мяч",               IsTeamEvent = true,  Description = null },
            };

            foreach (S9EventType t in allTypes)
            {
                S9EventType? existing = await db.EventTypes.FindAsync(t.Code);
                if (existing == null)
                    db.EventTypes.Add(t);
                else
                {
                    existing.Name = t.Name;
                    existing.Description = t.Description;
                    existing.IsTeamEvent = t.IsTeamEvent;
                }
            }
            await db.SaveChangesAsync();
        }

        private static async Task SeedSubstitutionTypes(VolleyballDbContext db) // С10. Типы замен
        {
            if (await db.SubstitutionTypes.AnyAsync()) return;

            db.SubstitutionTypes.AddRange(
                new S10SubstitutionType { Code = 1, Name = "Плановая замена" },
                new S10SubstitutionType { Code = 2, Name = "Экстренная замена (травма)" },
                new S10SubstitutionType { Code = 3, Name = "Замещение либеро" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedTimeoutTypes(VolleyballDbContext db) // С11. Типы тайм-аутов
        {
            if (await db.TimeoutTypes.AnyAsync()) return;

            db.TimeoutTypes.AddRange(
                new S11TimeoutType { Code = 1, Name = "Командный тайм-аут" },
                new S11TimeoutType { Code = 2, Name = "Технический тайм-аут" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedSanctionTypes(VolleyballDbContext db) // С12. Типы санкций
        {
            if (await db.SanctionTypes.AnyAsync()) return;

            db.SanctionTypes.AddRange(
                new S12SanctionType { Code = 1, Name = "Предупреждение (жёлтая карточка)" },
                new S12SanctionType { Code = 2, Name = "Штрафное очко (красная карточка)" },
                new S12SanctionType { Code = 3, Name = "Удаление с площадки" },
                new S12SanctionType { Code = 4, Name = "Дисквалификация (красная + жёлтая)" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedDelayViolations(VolleyballDbContext db) // С13. Нарушения задержки
        {
            if (await db.DelayViolations.AnyAsync()) return;

            db.DelayViolations.AddRange(
                new S13DelayViolation { Code = 1, Name = "Задержка игры (8 секунд)" },
                new S13DelayViolation { Code = 2, Name = "Превышение времени тайм-аута" },
                new S13DelayViolation { Code = 3, Name = "Нарушение порядка выхода" },
                new S13DelayViolation { Code = 4, Name = "Задержка замены" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedAwardTypes(VolleyballDbContext db) // С14. Типы наград
        {
            if (await db.AwardTypes.AnyAsync()) return;

            db.AwardTypes.AddRange(
                new S14AwardType { Code = 1, Name = "Индивидуальная" },
                new S14AwardType { Code = 2, Name = "Командная" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedSanctionKinds(VolleyballDbContext db) // С15. Виды санкций
        {
            if (await db.SanctionKinds.AnyAsync()) return;

            db.SanctionKinds.AddRange(
                new S15SanctionKind { Code = 1, Name = "Неспортивное поведение" },
                new S15SanctionKind { Code = 2, Name = "Нарушение задержки игры" },
                new S15SanctionKind { Code = 3, Name = "Агрессивное поведение" },
                new S15SanctionKind { Code = 4, Name = "Оскорбительные высказывания" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedRecipientTypes(VolleyballDbContext db) // С16. Типы получателей санкций
        {
            if (await db.RecipientTypes.AnyAsync()) return;

            db.RecipientTypes.AddRange(
                new S16RecipientType { Code = 1, Name = "Игрок" },
                new S16RecipientType { Code = 2, Name = "Главный тренер" },
                new S16RecipientType { Code = 3, Name = "Помощник тренера" },
                new S16RecipientType { Code = 4, Name = "Другой член делегации" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedCoinTossOptions(VolleyballDbContext db) // С17. Варианты выбора при жеребьёвке
        {
            if (await db.CoinTossOptions.AnyAsync()) return;

            db.CoinTossOptions.AddRange(
                new S17CoinTossOption { Code = 1, Name = "Выбрал подачу" },
                new S17CoinTossOption { Code = 2, Name = "Выбрал приём" },
                new S17CoinTossOption { Code = 3, Name = "Выбрал сторону площадки" }
            );
            await db.SaveChangesAsync();
        }

        private static async Task SeedScoringSystems(VolleyballDbContext db) // С18. Системы начисления очков
        {
            if (await db.ScoringSystems.AnyAsync()) return;

            db.ScoringSystems.AddRange(
                new S18ScoringSystem
                {
                    Code = 1, Name = "Стандартная ФИВБ (3/2/1/0)",
                    PointsWin30 = 3, PointsWin31 = 3, PointsWin32 = 2,
                    PointsLoss23 = 1, PointsLoss13 = 0, PointsLoss03 = 0
                },
                new S18ScoringSystem
                {
                    Code = 2, Name = "Упрощённая (2/1/0)",
                    PointsWin30 = 2, PointsWin31 = 2, PointsWin32 = 1,
                    PointsLoss23 = 1, PointsLoss13 = 0, PointsLoss03 = 0
                },
                new S18ScoringSystem
                {
                    Code = 3, Name = "Бинарная (1/0)",
                    PointsWin30 = 1, PointsWin31 = 1, PointsWin32 = 1,
                    PointsLoss23 = 0, PointsLoss13 = 0, PointsLoss03 = 0
                }
            );
            await db.SaveChangesAsync();
        }
    }
}

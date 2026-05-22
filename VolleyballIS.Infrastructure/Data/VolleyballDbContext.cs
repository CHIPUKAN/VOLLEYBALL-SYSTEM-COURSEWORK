using Microsoft.EntityFrameworkCore;
using VolleyballIS.Domain.Entities;

namespace VolleyballIS.Infrastructure.Data
{
    // контекст базы данных информационной системы волейбольных турниров
    public class VolleyballDbContext : DbContext
    {
        #region Конструкторы
        public VolleyballDbContext(DbContextOptions<VolleyballDbContext> options) // конструктор с параметрами DI
            : base(options)
        {
        }
        #endregion

        #region DbSet-свойства — справочники
        public DbSet<S1Amplua> Amplua { get; set; } = null!;                          // С1. Амплуа
        public DbSet<S2PlayerStatus> PlayerStatuses { get; set; } = null!;            // С2. Статусы игроков
        public DbSet<S3ApplicationStatus> ApplicationStatuses { get; set; } = null!;  // С3. Статусы заявок
        public DbSet<S4TournamentFormat> TournamentFormats { get; set; } = null!;     // С4. Форматы турниров
        public DbSet<S5TournamentStage> TournamentStages { get; set; } = null!;       // С5. Этапы турниров
        public DbSet<S6MatchStatus> MatchStatuses { get; set; } = null!;              // С6. Статусы матчей
        public DbSet<S7ProtocolStatus> ProtocolStatuses { get; set; } = null!;        // С7. Статусы протоколов
        public DbSet<S8RefereeRole> RefereeRoles { get; set; } = null!;               // С8. Роли судей
        public DbSet<S9EventType> EventTypes { get; set; } = null!;                   // С9. Типы событий
        public DbSet<S10SubstitutionType> SubstitutionTypes { get; set; } = null!;    // С10. Типы замен
        public DbSet<S11TimeoutType> TimeoutTypes { get; set; } = null!;              // С11. Типы тайм-аутов
        public DbSet<S12SanctionType> SanctionTypes { get; set; } = null!;            // С12. Типы санкций
        public DbSet<S13DelayViolation> DelayViolations { get; set; } = null!;        // С13. Нарушения задержки
        public DbSet<S14AwardType> AwardTypes { get; set; } = null!;                  // С14. Типы наград
        public DbSet<S15SanctionKind> SanctionKinds { get; set; } = null!;            // С15. Виды санкций
        public DbSet<S16RecipientType> RecipientTypes { get; set; } = null!;          // С16. Типы получателей
        public DbSet<S17CoinTossOption> CoinTossOptions { get; set; } = null!;        // С17. Варианты жеребьёвки
        public DbSet<S18ScoringSystem> ScoringSystems { get; set; } = null!;          // С18. Системы очков
        #endregion

        #region DbSet-свойства — основные сущности
        public DbSet<T1Region> Regions { get; set; } = null!;                         // Т1. Регионы
        public DbSet<T2Venue> Venues { get; set; } = null!;                           // Т2. Площадки
        public DbSet<T3Coach> Coaches { get; set; } = null!;                          // Т3. Тренеры
        public DbSet<T4Team> Teams { get; set; } = null!;                             // Т4. Команды
        public DbSet<T5Delegation> Delegations { get; set; } = null!;                 // Т5. Делегации
        public DbSet<T6Player> Players { get; set; } = null!;                         // Т6. Игроки
        public DbSet<T7Application> Applications { get; set; } = null!;               // Т7. Заявки
        public DbSet<T8ApplicationComposition> ApplicationCompositions { get; set; } = null!; // Т8. Состав заявки
        public DbSet<T9Season> Seasons { get; set; } = null!;                         // Т9. Сезоны
        public DbSet<T10Tournament> Tournaments { get; set; } = null!;                // Т10. Турниры
        public DbSet<T11Group> Groups { get; set; } = null!;                          // Т11. Группы
        public DbSet<T12Referee> Referees { get; set; } = null!;                      // Т12. Судьи
        public DbSet<T13Organizer> Organizers { get; set; } = null!;                  // Т13. Организаторы
        public DbSet<T14Match> Matches { get; set; } = null!;                         // Т14. Матчи
        public DbSet<T15RefereeAssignment> RefereeAssignments { get; set; } = null!;  // Т15. Бригады
        public DbSet<T16Protocol> Protocols { get; set; } = null!;                    // Т16. Протоколы
        public DbSet<T16ProtocolHistory> ProtocolHistory { get; set; } = null!;       // Т16h. Журнал
        public DbSet<T17Event> Events { get; set; } = null!;                          // Т17. События
        public DbSet<T17aSubstitution> Substitutions { get; set; } = null!;           // Т17а. Замены
        public DbSet<T17bTimeout> Timeouts { get; set; } = null!;                     // Т17б. Тайм-ауты
        public DbSet<T18Sanction> Sanctions { get; set; } = null!;                    // Т18. Санкции
        public DbSet<T20Award> Awards { get; set; } = null!;                          // Т20. Награды
        public DbSet<T21MatchCaptain> MatchCaptains { get; set; } = null!;            // Т21. Капитаны
        public DbSet<R1StartingLineup> StartingLineups { get; set; } = null!;         // Р1. Расстановки
        public DbSet<R2PlayerStats> PlayerStats { get; set; } = null!;                // Р2. Статистика
        public DbSet<R3Set> Sets { get; set; } = null!;                               // Р3. Партии
        public DbSet<AppUser> Users { get; set; } = null!;                            // Т0. Пользователи
        #endregion

        #region Конфигурация модели
        protected override void OnModelCreating(ModelBuilder modelBuilder) // настройка маппинга сущностей на таблицы БД
        {
            base.OnModelCreating(modelBuilder);

            ConfigureReferenceBooks(modelBuilder);
            ConfigureRegion(modelBuilder);
            ConfigureVenue(modelBuilder);
            ConfigureCoach(modelBuilder);
            ConfigureTeam(modelBuilder);
            ConfigurePlayer(modelBuilder);
            ConfigureSeason(modelBuilder);
            ConfigureOrganizer(modelBuilder);
            ConfigureTournament(modelBuilder);
            ConfigureApplication(modelBuilder);
            ConfigureApplicationComposition(modelBuilder);
            ConfigureGroup(modelBuilder);
            ConfigureReferee(modelBuilder);
            ConfigureMatch(modelBuilder);
            ConfigureRefereeAssignment(modelBuilder);
            ConfigureDelegation(modelBuilder);
            ConfigureProtocol(modelBuilder);
            ConfigureProtocolHistory(modelBuilder);
            ConfigureEvent(modelBuilder);
            ConfigureSubstitution(modelBuilder);
            ConfigureTimeout(modelBuilder);
            ConfigureSanction(modelBuilder);
            ConfigureAward(modelBuilder);
            ConfigureMatchCaptain(modelBuilder);
            ConfigureStartingLineup(modelBuilder);
            ConfigurePlayerStats(modelBuilder);
            ConfigureSet(modelBuilder);
            ConfigureAppUser(modelBuilder);
        }

        private static void ConfigureReferenceBooks(ModelBuilder modelBuilder) // конфигурация всех справочников С1–С18
        {
            modelBuilder.Entity<S1Amplua>(e =>
            {
                e.ToTable("s1_amplua");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S2PlayerStatus>(e =>
            {
                e.ToTable("s2_player_statuses");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S3ApplicationStatus>(e =>
            {
                e.ToTable("s3_application_statuses");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S4TournamentFormat>(e =>
            {
                e.ToTable("s4_tournament_formats");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S5TournamentStage>(e =>
            {
                e.ToTable("s5_tournament_stages");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired();
            });

            modelBuilder.Entity<S6MatchStatus>(e =>
            {
                e.ToTable("s6_match_statuses");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired();
            });

            modelBuilder.Entity<S7ProtocolStatus>(e =>
            {
                e.ToTable("s7_protocol_statuses");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S8RefereeRole>(e =>
            {
                e.ToTable("s8_referee_roles");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired();
            });

            modelBuilder.Entity<S9EventType>(e =>
            {
                e.ToTable("s9_event_types");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                e.Property(x => x.IsTeamEvent).HasColumnName("is_team_event");
                e.Property(x => x.Description).HasColumnName("description").HasColumnType("text");
            });

            modelBuilder.Entity<S10SubstitutionType>(e =>
            {
                e.ToTable("s10_substitution_types");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired();
            });

            modelBuilder.Entity<S11TimeoutType>(e =>
            {
                e.ToTable("s11_timeout_types");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired();
            });

            modelBuilder.Entity<S12SanctionType>(e =>
            {
                e.ToTable("s12_sanction_types");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(80).IsRequired();
            });

            modelBuilder.Entity<S13DelayViolation>(e =>
            {
                e.ToTable("s13_delay_violations");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<S14AwardType>(e =>
            {
                e.ToTable("s14_award_types");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S15SanctionKind>(e =>
            {
                e.ToTable("s15_sanction_kinds");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(60).IsRequired();
            });

            modelBuilder.Entity<S16RecipientType>(e =>
            {
                e.ToTable("s16_recipient_types");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S17CoinTossOption>(e =>
            {
                e.ToTable("s17_coin_toss_options");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<S18ScoringSystem>(e =>
            {
                e.ToTable("s18_scoring_systems");
                e.HasKey(x => x.Code);
                e.Property(x => x.Code).HasColumnName("code");
                e.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                e.Property(x => x.PointsWin30).HasColumnName("points_win_3_0");
                e.Property(x => x.PointsWin31).HasColumnName("points_win_3_1");
                e.Property(x => x.PointsWin32).HasColumnName("points_win_3_2");
                e.Property(x => x.PointsLoss23).HasColumnName("points_loss_2_3");
                e.Property(x => x.PointsLoss13).HasColumnName("points_loss_1_3");
                e.Property(x => x.PointsLoss03).HasColumnName("points_loss_0_3");
            });
        }

        private static void ConfigureRegion(ModelBuilder modelBuilder) // конфигурация сущности T1Region - t1_regions
        {
            modelBuilder.Entity<T1Region>(entity =>
            {
                entity.ToTable("t1_regions");
                entity.HasKey(e => e.OktmoCode);
                entity.Property(e => e.OktmoCode).HasColumnName("oktmo_code").HasColumnType("char(11)").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            });
        }

        private static void ConfigureVenue(ModelBuilder modelBuilder) // конфигурация сущности T2Venue - t2_venues
        {
            modelBuilder.Entity<T2Venue>(entity =>
            {
                entity.ToTable("t2_venues");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
                entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(255);
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Capacity).HasColumnName("capacity");
            });
        }

        private static void ConfigureCoach(ModelBuilder modelBuilder) // конфигурация сущности T3Coach - t3_coaches
        {
            modelBuilder.Entity<T3Coach>(entity =>
            {
                entity.ToTable("t3_coaches");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(60).IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(40).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(60);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(120);
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
                entity.Property(e => e.Category).HasColumnName("category").HasMaxLength(40);
            });
        }

        private static void ConfigureTeam(ModelBuilder modelBuilder) // конфигурация сущности T4Team - t4_teams
        {
            modelBuilder.Entity<T4Team>(entity =>
            {
                entity.ToTable("t4_teams");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.LogoUrl).HasColumnName("logo_url").HasColumnType("text");
                entity.Property(e => e.RegionOktmo).HasColumnName("region_oktmo").HasColumnType("char(11)").IsRequired();
                entity.Property(e => e.HeadCoachId).HasColumnName("head_coach_id").IsRequired();
                entity.Property(e => e.HomeVenueId).HasColumnName("home_venue_id").IsRequired();

                entity.HasOne(e => e.Region)
                    .WithMany(r => r.Teams)
                    .HasForeignKey(e => e.RegionOktmo)
                    .HasPrincipalKey(r => r.OktmoCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HeadCoach)
                    .WithMany(c => c.Teams)
                    .HasForeignKey(e => e.HeadCoachId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HomeVenue)
                    .WithMany(v => v.HomeTeams)
                    .HasForeignKey(e => e.HomeVenueId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigurePlayer(ModelBuilder modelBuilder) // конфигурация сущности T6Player - t6_players
        {
            modelBuilder.Entity<T6Player>(entity =>
            {
                entity.ToTable("t6_players");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.TeamId).HasColumnName("team_id");
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(60).IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(40).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(60);
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.HeightCm).HasColumnName("height_cm");
                entity.Property(e => e.WeightKg).HasColumnName("weight_kg");
                entity.Property(e => e.JerseyNumber).HasColumnName("jersey_number");
                entity.Property(e => e.AmpluaCode).HasColumnName("amplua_code").IsRequired();
                entity.Property(e => e.SportsRank).HasColumnName("sports_rank").HasMaxLength(30);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(120);
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
                entity.Property(e => e.StatusCode).HasColumnName("status_code");
                entity.Property(e => e.PhotoUrl).HasColumnName("photo_url").HasColumnType("text");

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Amplua)
                    .WithMany()
                    .HasForeignKey(e => e.AmpluaCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Status)
                    .WithMany()
                    .HasForeignKey(e => e.StatusCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureSeason(ModelBuilder modelBuilder) // конфигурация сущности T9Season - t9_seasons
        {
            modelBuilder.Entity<T9Season>(entity =>
            {
                entity.ToTable("t9_seasons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(15);
            });
        }

        private static void ConfigureOrganizer(ModelBuilder modelBuilder) // конфигурация сущности T13Organizer - t13_organizers
        {
            modelBuilder.Entity<T13Organizer>(entity =>
            {
                entity.ToTable("t13_organizers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(60).IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(40).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(60);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(120);
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            });
        }

        private static void ConfigureTournament(ModelBuilder modelBuilder) // конфигурация сущности T10Tournament - t10_tournaments
        {
            modelBuilder.Entity<T10Tournament>(entity =>
            {
                entity.ToTable("t10_tournaments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.SeasonId).HasColumnName("season_id");
                entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.ApplicationDeadline).HasColumnName("application_deadline");
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
                entity.Property(e => e.MaxTeams).HasColumnName("max_teams");
                entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(10).IsRequired();
                entity.Property(e => e.AgeCategory).HasColumnName("age_category").HasMaxLength(20);
                entity.Property(e => e.Level).HasColumnName("level").HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaxPlayersPerApp).HasColumnName("max_players_per_app");
                entity.Property(e => e.FormatCode).HasColumnName("format_code").IsRequired();
                entity.Property(e => e.ScoringSystemCode).HasColumnName("scoring_system_code").IsRequired();

                entity.HasOne(e => e.Season)
                    .WithMany(s => s.Tournaments)
                    .HasForeignKey(e => e.SeasonId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Organizer)
                    .WithMany(o => o.Tournaments)
                    .HasForeignKey(e => e.OrganizerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Format)
                    .WithMany()
                    .HasForeignKey(e => e.FormatCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ScoringSystem)
                    .WithMany()
                    .HasForeignKey(e => e.ScoringSystemCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureApplication(ModelBuilder modelBuilder) // конфигурация сущности T7Application - t7_applications
        {
            modelBuilder.Entity<T7Application>(entity =>
            {
                entity.ToTable("t7_applications");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.TeamId).HasColumnName("team_id").IsRequired();
                entity.Property(e => e.TournamentId).HasColumnName("tournament_id").IsRequired();
                entity.Property(e => e.SubmissionDate).HasColumnName("submission_date");
                entity.Property(e => e.StatusCode).HasColumnName("status_code");
                entity.Property(e => e.Comment).HasColumnName("comment").HasColumnType("text");
                entity.HasIndex(e => new { e.TeamId, e.TournamentId }).IsUnique();

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Tournament)
                    .WithMany(t => t.Applications)
                    .HasForeignKey(e => e.TournamentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Status)
                    .WithMany()
                    .HasForeignKey(e => e.StatusCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureApplicationComposition(ModelBuilder modelBuilder) // конфигурация сущности T8ApplicationComposition - t8_application_composition
        {
            modelBuilder.Entity<T8ApplicationComposition>(entity =>
            {
                entity.ToTable("t8_application_composition");
                entity.HasKey(e => new { e.ApplicationId, e.PlayerId });
                entity.Property(e => e.ApplicationId).HasColumnName("application_id");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.JerseyNumberInApp).HasColumnName("jersey_number_in_app");
                entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(10);
                entity.Property(e => e.IsLibero).HasColumnName("is_libero");
                entity.HasIndex(e => new { e.ApplicationId, e.JerseyNumberInApp }).IsUnique();

                entity.HasOne(e => e.Application)
                    .WithMany(a => a.Composition)
                    .HasForeignKey(e => e.ApplicationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureGroup(ModelBuilder modelBuilder) // конфигурация сущности T11Group - t11_groups
        {
            modelBuilder.Entity<T11Group>(entity =>
            {
                entity.ToTable("t11_groups");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.TournamentId).HasColumnName("tournament_id").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.HasIndex(e => new { e.TournamentId, e.Name }).IsUnique();

                entity.HasOne(e => e.Tournament)
                    .WithMany(t => t.Groups)
                    .HasForeignKey(e => e.TournamentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureReferee(ModelBuilder modelBuilder) // конфигурация сущности T12Referee - t12_referees
        {
            modelBuilder.Entity<T12Referee>(entity =>
            {
                entity.ToTable("t12_referees");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(60).IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(40).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(60);
                entity.Property(e => e.Category).HasColumnName("category").HasMaxLength(30);
                entity.Property(e => e.LicenseNumber).HasColumnName("license_number").HasMaxLength(30);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(120);
                entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            });
        }

        private static void ConfigureMatch(ModelBuilder modelBuilder) // конфигурация сущности T14Match - t14_matches
        {
            modelBuilder.Entity<T14Match>(entity =>
            {
                entity.ToTable("t14_matches");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.TournamentId).HasColumnName("tournament_id").IsRequired();
                entity.Property(e => e.HomeTeamId).HasColumnName("home_team_id").IsRequired();
                entity.Property(e => e.GuestTeamId).HasColumnName("guest_team_id").IsRequired();
                entity.Property(e => e.MatchDate).HasColumnName("match_date");
                entity.Property(e => e.StartTime).HasColumnName("start_time");
                entity.Property(e => e.EndTime).HasColumnName("end_time");
                entity.Property(e => e.VenueId).HasColumnName("venue_id").IsRequired();
                entity.Property(e => e.StageCode).HasColumnName("stage_code").IsRequired();
                entity.Property(e => e.GroupId).HasColumnName("group_id");
                entity.Property(e => e.StatusCode).HasColumnName("status_code");
                entity.Property(e => e.TechDefeatReason).HasColumnName("tech_defeat_reason").HasColumnType("text");
                entity.Property(e => e.CoinTossWinnerTeamId).HasColumnName("coin_toss_winner_team_id");
                entity.Property(e => e.CoinTossChoiceCode).HasColumnName("coin_toss_choice_code");
                entity.Property(e => e.FirstServeTeamId).HasColumnName("first_serve_team_id");
                entity.Property(e => e.NetHeight).HasColumnName("net_height").HasColumnType("numeric(3,2)");
                entity.Property(e => e.HasVideoChallenge).HasColumnName("has_video_challenge");

                entity.HasOne(e => e.Tournament)
                    .WithMany()
                    .HasForeignKey(e => e.TournamentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.HomeTeam)
                    .WithMany()
                    .HasForeignKey(e => e.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.GuestTeam)
                    .WithMany()
                    .HasForeignKey(e => e.GuestTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Venue)
                    .WithMany()
                    .HasForeignKey(e => e.VenueId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Stage)
                    .WithMany()
                    .HasForeignKey(e => e.StageCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Status)
                    .WithMany()
                    .HasForeignKey(e => e.StatusCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Group)
                    .WithMany()
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.CoinTossChoice)
                    .WithMany()
                    .HasForeignKey(e => e.CoinTossChoiceCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CoinTossWinnerTeam)
                    .WithMany()
                    .HasForeignKey(e => e.CoinTossWinnerTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FirstServeTeam)
                    .WithMany()
                    .HasForeignKey(e => e.FirstServeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureRefereeAssignment(ModelBuilder modelBuilder) // конфигурация сущности T15RefereeAssignment - t15_referee_assignments
        {
            modelBuilder.Entity<T15RefereeAssignment>(entity =>
            {
                entity.ToTable("t15_referee_assignments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.MatchId).HasColumnName("match_id").IsRequired();
                entity.Property(e => e.RefereeId).HasColumnName("referee_id").IsRequired();
                entity.Property(e => e.RoleCode).HasColumnName("role_code").IsRequired();
                entity.Property(e => e.LineJudgeSeqNo).HasColumnName("line_judge_seq_no");
                entity.HasIndex(e => new { e.MatchId, e.RefereeId }).IsUnique();

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Referee)
                    .WithMany()
                    .HasForeignKey(e => e.RefereeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(e => e.RoleCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureDelegation(ModelBuilder modelBuilder) // конфигурация сущности T5Delegation - t5_delegations
        {
            modelBuilder.Entity<T5Delegation>(entity =>
            {
                entity.ToTable("t5_delegations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.MatchId).HasColumnName("match_id").IsRequired();
                entity.Property(e => e.TeamId).HasColumnName("team_id").IsRequired();
                entity.Property(e => e.RoleType).HasColumnName("role_type").HasMaxLength(30).IsRequired();
                entity.Property(e => e.AssistantSeqNo).HasColumnName("assistant_seq_no");
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(60).IsRequired();
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(40).IsRequired();
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(60);

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureProtocol(ModelBuilder modelBuilder) // конфигурация сущности T16Protocol - t16_protocols
        {
            modelBuilder.Entity<T16Protocol>(entity =>
            {
                entity.ToTable("t16_protocols");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.MatchId).HasColumnName("match_id").IsRequired();
                entity.HasIndex(e => e.MatchId).IsUnique();
                entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
                entity.Property(e => e.ApprovalDate).HasColumnName("approval_date");
                entity.Property(e => e.StatusCode).HasColumnName("status_code");

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Organizer)
                    .WithMany()
                    .HasForeignKey(e => e.OrganizerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Status)
                    .WithMany()
                    .HasForeignKey(e => e.StatusCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureProtocolHistory(ModelBuilder modelBuilder) // конфигурация сущности T16ProtocolHistory - t16_protocol_history
        {
            modelBuilder.Entity<T16ProtocolHistory>(entity =>
            {
                entity.ToTable("t16_protocol_history");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.ProtocolId).HasColumnName("protocol_id").IsRequired();
                entity.Property(e => e.ChangedAt).HasColumnName("changed_at");
                entity.Property(e => e.StatusAtMoment).HasColumnName("status_at_moment");
                entity.Property(e => e.DataHash).HasColumnName("data_hash").HasMaxLength(64);
                entity.Property(e => e.Comment).HasColumnName("comment").HasColumnType("text");

                entity.HasOne(e => e.Protocol)
                    .WithMany(p => p.History)
                    .HasForeignKey(e => e.ProtocolId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureEvent(ModelBuilder modelBuilder) // конфигурация сущности T17Event - t17_events
        {
            modelBuilder.Entity<T17Event>(entity =>
            {
                entity.ToTable("t17_events");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.MatchId).HasColumnName("match_id").IsRequired();
                entity.Property(e => e.TeamId).HasColumnName("team_id");
                entity.Property(e => e.EventTypeCode).HasColumnName("event_type_code").IsRequired();
                entity.Property(e => e.SetNumber).HasColumnName("set_number");
                entity.Property(e => e.GlobalSeqInSet).HasColumnName("global_seq_in_set");
                entity.Property(e => e.HomeScoreAtMoment).HasColumnName("home_score_at_moment");
                entity.Property(e => e.GuestScoreAtMoment).HasColumnName("guest_score_at_moment");
                entity.Property(e => e.MinuteMark).HasColumnName("minute_mark");
                entity.HasIndex(e => new { e.MatchId, e.SetNumber, e.GlobalSeqInSet }).IsUnique();

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EventType)
                    .WithMany()
                    .HasForeignKey(e => e.EventTypeCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureSubstitution(ModelBuilder modelBuilder) // конфигурация сущности T17aSubstitution - t17a_substitutions
        {
            modelBuilder.Entity<T17aSubstitution>(entity =>
            {
                entity.ToTable("t17a_substitutions");
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId).HasColumnName("event_id").ValueGeneratedNever();
                entity.Property(e => e.SubOutPlayerId).HasColumnName("sub_out_player_id").IsRequired();
                entity.Property(e => e.SubInPlayerId).HasColumnName("sub_in_player_id").IsRequired();
                entity.Property(e => e.SubTypeCode).HasColumnName("sub_type_code");
                entity.Property(e => e.SubSeqInSet).HasColumnName("sub_seq_in_set");
                entity.Property(e => e.IsLiberoSwap).HasColumnName("is_libero_swap");

                entity.HasOne(e => e.Event)
                    .WithOne(ev => ev.Substitution)
                    .HasForeignKey<T17aSubstitution>(e => e.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubOutPlayer)
                    .WithMany()
                    .HasForeignKey(e => e.SubOutPlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubInPlayer)
                    .WithMany()
                    .HasForeignKey(e => e.SubInPlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubType)
                    .WithMany()
                    .HasForeignKey(e => e.SubTypeCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureTimeout(ModelBuilder modelBuilder) // конфигурация сущности T17bTimeout - t17b_timeouts
        {
            modelBuilder.Entity<T17bTimeout>(entity =>
            {
                entity.ToTable("t17b_timeouts");
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId).HasColumnName("event_id").ValueGeneratedNever();
                entity.Property(e => e.TimeoutTypeCode).HasColumnName("timeout_type_code").IsRequired();
                entity.Property(e => e.TimeoutSeqInSet).HasColumnName("timeout_seq_in_set");

                entity.HasOne(e => e.Event)
                    .WithOne(ev => ev.Timeout)
                    .HasForeignKey<T17bTimeout>(e => e.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TimeoutType)
                    .WithMany()
                    .HasForeignKey(e => e.TimeoutTypeCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureSanction(ModelBuilder modelBuilder) // конфигурация сущности T18Sanction - t18_sanctions
        {
            modelBuilder.Entity<T18Sanction>(entity =>
            {
                entity.ToTable("t18_sanctions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.MatchId).HasColumnName("match_id").IsRequired();
                entity.Property(e => e.TeamId).HasColumnName("team_id").IsRequired();
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.DelegationMemberId).HasColumnName("delegation_member_id");
                entity.Property(e => e.RecipientTypeCode).HasColumnName("recipient_type_code").IsRequired();
                entity.Property(e => e.SanctionTypeCode).HasColumnName("sanction_type_code").IsRequired();
                entity.Property(e => e.SanctionKindCode).HasColumnName("sanction_kind_code").IsRequired();
                entity.Property(e => e.DelayViolationCode).HasColumnName("delay_violation_code");
                entity.Property(e => e.SetNumber).HasColumnName("set_number");
                entity.Property(e => e.MemberSeqInMatch).HasColumnName("member_seq_in_match");
                entity.Property(e => e.HomeScoreAtMoment).HasColumnName("home_score_at_moment");
                entity.Property(e => e.GuestScoreAtMoment).HasColumnName("guest_score_at_moment");
                entity.Property(e => e.MinuteMark).HasColumnName("minute_mark");
                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.RecipientType)
                    .WithMany()
                    .HasForeignKey(e => e.RecipientTypeCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SanctionType)
                    .WithMany()
                    .HasForeignKey(e => e.SanctionTypeCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SanctionKind)
                    .WithMany()
                    .HasForeignKey(e => e.SanctionKindCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DelayViolation)
                    .WithMany()
                    .HasForeignKey(e => e.DelayViolationCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureAward(ModelBuilder modelBuilder) // конфигурация сущности T20Award - t20_awards
        {
            modelBuilder.Entity<T20Award>(entity =>
            {
                entity.ToTable("t20_awards");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.TournamentId).HasColumnName("tournament_id").IsRequired();
                entity.Property(e => e.AwardTypeCode).HasColumnName("award_type_code").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.HasOne(e => e.Tournament)
                    .WithMany()
                    .HasForeignKey(e => e.TournamentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.AwardType)
                    .WithMany()
                    .HasForeignKey(e => e.AwardTypeCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureMatchCaptain(ModelBuilder modelBuilder) // конфигурация сущности T21MatchCaptain - t21_match_captains
        {
            modelBuilder.Entity<T21MatchCaptain>(entity =>
            {
                entity.ToTable("t21_match_captains");
                entity.HasKey(e => new { e.MatchId, e.TeamId });
                entity.Property(e => e.MatchId).HasColumnName("match_id");
                entity.Property(e => e.TeamId).HasColumnName("team_id");
                entity.Property(e => e.PlayerId).HasColumnName("player_id").IsRequired();

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureStartingLineup(ModelBuilder modelBuilder) // конфигурация сущности R1StartingLineup - r1_starting_lineups
        {
            modelBuilder.Entity<R1StartingLineup>(entity =>
            {
                entity.ToTable("r1_starting_lineups");
                entity.HasKey(e => new { e.MatchId, e.TeamId, e.SetNumber, e.PositionNo });
                entity.Property(e => e.MatchId).HasColumnName("match_id");
                entity.Property(e => e.TeamId).HasColumnName("team_id");
                entity.Property(e => e.SetNumber).HasColumnName("set_number");
                entity.Property(e => e.PositionNo).HasColumnName("position_no");
                entity.Property(e => e.PlayerId).HasColumnName("player_id").IsRequired();
                entity.HasIndex(e => new { e.MatchId, e.TeamId, e.SetNumber, e.PlayerId }).IsUnique();

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigurePlayerStats(ModelBuilder modelBuilder) // конфигурация сущности R2PlayerStats - r2_player_stats
        {
            modelBuilder.Entity<R2PlayerStats>(entity =>
            {
                entity.ToTable("r2_player_stats");
                entity.HasKey(e => new { e.MatchId, e.PlayerId });
                entity.Property(e => e.MatchId).HasColumnName("match_id");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.TeamId).HasColumnName("team_id").IsRequired();
                entity.Property(e => e.ServesTotal).HasColumnName("serves_total");
                entity.Property(e => e.Aces).HasColumnName("aces");
                entity.Property(e => e.ServeErrors).HasColumnName("serve_errors");
                entity.Property(e => e.ReceptionsTotal).HasColumnName("receptions_total");
                entity.Property(e => e.PositiveReceptions).HasColumnName("positive_receptions");
                entity.Property(e => e.ReceptionErrors).HasColumnName("reception_errors");
                entity.Property(e => e.AttacksTotal).HasColumnName("attacks_total");
                entity.Property(e => e.AttackPoints).HasColumnName("attack_points");
                entity.Property(e => e.AttackErrors).HasColumnName("attack_errors");
                entity.Property(e => e.Blocks).HasColumnName("blocks");
                entity.Property(e => e.TotalPoints).HasColumnName("total_points");

                entity.HasOne(e => e.Match)
                    .WithMany()
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Player)
                    .WithMany()
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Team)
                    .WithMany()
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureSet(ModelBuilder modelBuilder) // конфигурация сущности R3Set - r3_sets
        {
            modelBuilder.Entity<R3Set>(entity =>
            {
                entity.ToTable("r3_sets");
                entity.HasKey(e => new { e.MatchId, e.SetNumber });
                entity.Property(e => e.MatchId).HasColumnName("match_id");
                entity.Property(e => e.SetNumber).HasColumnName("set_number");
                entity.Property(e => e.HomeScore).HasColumnName("home_score");
                entity.Property(e => e.GuestScore).HasColumnName("guest_score");
                entity.Property(e => e.DurationMin).HasColumnName("duration_min");

                entity.HasOne(e => e.Match)
                    .WithMany(m => m.Sets)
                    .HasForeignKey(e => e.MatchId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureAppUser(ModelBuilder modelBuilder) // конфигурация сущности AppUser - app_users
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("app_users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.Role).HasColumnName("role").IsRequired();
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.LinkedCoachId).HasColumnName("linked_coach_id");
                entity.Property(e => e.LinkedPlayerId).HasColumnName("linked_player_id");
                entity.Property(e => e.LinkedRefereeId).HasColumnName("linked_referee_id");
                entity.Property(e => e.LinkedOrganizerId).HasColumnName("linked_organizer_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(e => e.Coach).WithMany().HasForeignKey(e => e.LinkedCoachId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Player).WithMany().HasForeignKey(e => e.LinkedPlayerId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Referee).WithMany().HasForeignKey(e => e.LinkedRefereeId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Organizer).WithMany().HasForeignKey(e => e.LinkedOrganizerId).OnDelete(DeleteBehavior.SetNull);
            });
        }
        #endregion
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VolleyballIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "s1_amplua",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s1_amplua", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s10_substitution_types",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s10_substitution_types", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s11_timeout_types",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s11_timeout_types", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s12_sanction_types",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s12_sanction_types", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s13_delay_violations",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s13_delay_violations", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s14_award_types",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s14_award_types", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s15_sanction_kinds",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s15_sanction_kinds", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s16_recipient_types",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s16_recipient_types", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s17_coin_toss_options",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s17_coin_toss_options", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s18_scoring_systems",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    points_win_3_0 = table.Column<short>(type: "smallint", nullable: false),
                    points_win_3_1 = table.Column<short>(type: "smallint", nullable: false),
                    points_win_3_2 = table.Column<short>(type: "smallint", nullable: false),
                    points_loss_2_3 = table.Column<short>(type: "smallint", nullable: false),
                    points_loss_1_3 = table.Column<short>(type: "smallint", nullable: false),
                    points_loss_0_3 = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s18_scoring_systems", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s2_player_statuses",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s2_player_statuses", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s3_application_statuses",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s3_application_statuses", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s4_tournament_formats",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s4_tournament_formats", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s5_tournament_stages",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s5_tournament_stages", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s6_match_statuses",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s6_match_statuses", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s7_protocol_statuses",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s7_protocol_statuses", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s8_referee_roles",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s8_referee_roles", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "s9_event_types",
                columns: table => new
                {
                    code = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_team_event = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s9_event_types", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "t1_regions",
                columns: table => new
                {
                    oktmo_code = table.Column<string>(type: "char(11)", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t1_regions", x => x.oktmo_code);
                });

            migrationBuilder.CreateTable(
                name: "t12_referees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    first_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    license_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t12_referees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t13_organizers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    first_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t13_organizers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t2_venues",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t2_venues", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t3_coaches",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    first_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    category = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t3_coaches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t9_seasons",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t9_seasons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t4_teams",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    region_oktmo = table.Column<string>(type: "char(11)", nullable: false),
                    head_coach_id = table.Column<int>(type: "integer", nullable: false),
                    home_venue_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t4_teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_t4_teams_t1_regions_region_oktmo",
                        column: x => x.region_oktmo,
                        principalTable: "t1_regions",
                        principalColumn: "oktmo_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t4_teams_t2_venues_home_venue_id",
                        column: x => x.home_venue_id,
                        principalTable: "t2_venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t4_teams_t3_coaches_head_coach_id",
                        column: x => x.head_coach_id,
                        principalTable: "t3_coaches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t10_tournaments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    season_id = table.Column<int>(type: "integer", nullable: true),
                    organizer_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    application_deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    max_teams = table.Column<short>(type: "smallint", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    age_category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    max_players_per_app = table.Column<short>(type: "smallint", nullable: false),
                    format_code = table.Column<short>(type: "smallint", nullable: false),
                    scoring_system_code = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t10_tournaments", x => x.id);
                    table.ForeignKey(
                        name: "FK_t10_tournaments_s18_scoring_systems_scoring_system_code",
                        column: x => x.scoring_system_code,
                        principalTable: "s18_scoring_systems",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t10_tournaments_s4_tournament_formats_format_code",
                        column: x => x.format_code,
                        principalTable: "s4_tournament_formats",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t10_tournaments_t13_organizers_organizer_id",
                        column: x => x.organizer_id,
                        principalTable: "t13_organizers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_t10_tournaments_t9_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "t9_seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "t6_players",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(type: "integer", nullable: true),
                    last_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    first_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    height_cm = table.Column<short>(type: "smallint", nullable: true),
                    weight_kg = table.Column<short>(type: "smallint", nullable: true),
                    jersey_number = table.Column<short>(type: "smallint", nullable: true),
                    amplua_code = table.Column<short>(type: "smallint", nullable: false),
                    sports_rank = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    status_code = table.Column<short>(type: "smallint", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t6_players", x => x.id);
                    table.ForeignKey(
                        name: "FK_t6_players_s1_amplua_amplua_code",
                        column: x => x.amplua_code,
                        principalTable: "s1_amplua",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t6_players_s2_player_statuses_status_code",
                        column: x => x.status_code,
                        principalTable: "s2_player_statuses",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t6_players_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "t11_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t11_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_t11_groups_t10_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "t10_tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t7_applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    submission_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status_code = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t7_applications", x => x.id);
                    table.ForeignKey(
                        name: "FK_t7_applications_s3_application_statuses_status_code",
                        column: x => x.status_code,
                        principalTable: "s3_application_statuses",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t7_applications_t10_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "t10_tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t7_applications_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: true),
                    linked_coach_id = table.Column<int>(type: "integer", nullable: true),
                    linked_player_id = table.Column<int>(type: "integer", nullable: true),
                    linked_referee_id = table.Column<int>(type: "integer", nullable: true),
                    linked_organizer_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_users_t12_referees_linked_referee_id",
                        column: x => x.linked_referee_id,
                        principalTable: "t12_referees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_app_users_t13_organizers_linked_organizer_id",
                        column: x => x.linked_organizer_id,
                        principalTable: "t13_organizers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_app_users_t3_coaches_linked_coach_id",
                        column: x => x.linked_coach_id,
                        principalTable: "t3_coaches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_app_users_t6_players_linked_player_id",
                        column: x => x.linked_player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "t20_awards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    award_type_code = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: true),
                    team_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t20_awards", x => x.id);
                    table.ForeignKey(
                        name: "FK_t20_awards_s14_award_types_award_type_code",
                        column: x => x.award_type_code,
                        principalTable: "s14_award_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t20_awards_t10_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "t10_tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t20_awards_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t20_awards_t6_players_player_id",
                        column: x => x.player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t14_matches",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(type: "integer", nullable: false),
                    home_team_id = table.Column<int>(type: "integer", nullable: false),
                    guest_team_id = table.Column<int>(type: "integer", nullable: false),
                    match_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    stage_code = table.Column<short>(type: "smallint", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: true),
                    status_code = table.Column<short>(type: "smallint", nullable: false),
                    tech_defeat_reason = table.Column<string>(type: "text", nullable: true),
                    coin_toss_winner_team_id = table.Column<int>(type: "integer", nullable: true),
                    coin_toss_choice_code = table.Column<short>(type: "smallint", nullable: true),
                    first_serve_team_id = table.Column<int>(type: "integer", nullable: true),
                    net_height = table.Column<decimal>(type: "numeric(3,2)", nullable: true),
                    has_video_challenge = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t14_matches", x => x.id);
                    table.ForeignKey(
                        name: "FK_t14_matches_s17_coin_toss_options_coin_toss_choice_code",
                        column: x => x.coin_toss_choice_code,
                        principalTable: "s17_coin_toss_options",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t14_matches_s5_tournament_stages_stage_code",
                        column: x => x.stage_code,
                        principalTable: "s5_tournament_stages",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t14_matches_s6_match_statuses_status_code",
                        column: x => x.status_code,
                        principalTable: "s6_match_statuses",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t14_matches_t10_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "t10_tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t14_matches_t11_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "t11_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_t14_matches_t2_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "t2_venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t14_matches_t4_teams_guest_team_id",
                        column: x => x.guest_team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t14_matches_t4_teams_home_team_id",
                        column: x => x.home_team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t8_application_composition",
                columns: table => new
                {
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    jersey_number_in_app = table.Column<short>(type: "smallint", nullable: false),
                    role = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_libero = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t8_application_composition", x => new { x.application_id, x.player_id });
                    table.ForeignKey(
                        name: "FK_t8_application_composition_t6_players_player_id",
                        column: x => x.player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t8_application_composition_t7_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "t7_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "r1_starting_lineups",
                columns: table => new
                {
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    set_number = table.Column<short>(type: "smallint", nullable: false),
                    position_no = table.Column<short>(type: "smallint", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r1_starting_lineups", x => new { x.match_id, x.team_id, x.set_number, x.position_no });
                    table.ForeignKey(
                        name: "FK_r1_starting_lineups_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_r1_starting_lineups_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_r1_starting_lineups_t6_players_player_id",
                        column: x => x.player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "r2_player_stats",
                columns: table => new
                {
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    serves_total = table.Column<short>(type: "smallint", nullable: false),
                    aces = table.Column<short>(type: "smallint", nullable: false),
                    serve_errors = table.Column<short>(type: "smallint", nullable: false),
                    receptions_total = table.Column<short>(type: "smallint", nullable: false),
                    positive_receptions = table.Column<short>(type: "smallint", nullable: false),
                    reception_errors = table.Column<short>(type: "smallint", nullable: false),
                    attacks_total = table.Column<short>(type: "smallint", nullable: false),
                    attack_points = table.Column<short>(type: "smallint", nullable: false),
                    attack_errors = table.Column<short>(type: "smallint", nullable: false),
                    blocks = table.Column<short>(type: "smallint", nullable: false),
                    total_points = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r2_player_stats", x => new { x.match_id, x.player_id });
                    table.ForeignKey(
                        name: "FK_r2_player_stats_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_r2_player_stats_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_r2_player_stats_t6_players_player_id",
                        column: x => x.player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "r3_sets",
                columns: table => new
                {
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    set_number = table.Column<short>(type: "smallint", nullable: false),
                    home_score = table.Column<short>(type: "smallint", nullable: true),
                    guest_score = table.Column<short>(type: "smallint", nullable: true),
                    duration_min = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r3_sets", x => new { x.match_id, x.set_number });
                    table.ForeignKey(
                        name: "FK_r3_sets_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t15_referee_assignments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    referee_id = table.Column<int>(type: "integer", nullable: false),
                    role_code = table.Column<short>(type: "smallint", nullable: false),
                    line_judge_seq_no = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t15_referee_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_t15_referee_assignments_s8_referee_roles_role_code",
                        column: x => x.role_code,
                        principalTable: "s8_referee_roles",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t15_referee_assignments_t12_referees_referee_id",
                        column: x => x.referee_id,
                        principalTable: "t12_referees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t15_referee_assignments_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t16_protocols",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    organizer_id = table.Column<int>(type: "integer", nullable: true),
                    approval_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status_code = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t16_protocols", x => x.id);
                    table.ForeignKey(
                        name: "FK_t16_protocols_s7_protocol_statuses_status_code",
                        column: x => x.status_code,
                        principalTable: "s7_protocol_statuses",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t16_protocols_t13_organizers_organizer_id",
                        column: x => x.organizer_id,
                        principalTable: "t13_organizers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_t16_protocols_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t17_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: true),
                    event_type_code = table.Column<short>(type: "smallint", nullable: false),
                    set_number = table.Column<short>(type: "smallint", nullable: false),
                    global_seq_in_set = table.Column<int>(type: "integer", nullable: false),
                    home_score_at_moment = table.Column<short>(type: "smallint", nullable: false),
                    guest_score_at_moment = table.Column<short>(type: "smallint", nullable: false),
                    minute_mark = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t17_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_t17_events_s9_event_types_event_type_code",
                        column: x => x.event_type_code,
                        principalTable: "s9_event_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t17_events_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t17_events_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t18_sanctions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: true),
                    delegation_member_id = table.Column<int>(type: "integer", nullable: true),
                    recipient_type_code = table.Column<short>(type: "smallint", nullable: false),
                    sanction_type_code = table.Column<short>(type: "smallint", nullable: false),
                    sanction_kind_code = table.Column<short>(type: "smallint", nullable: false),
                    delay_violation_code = table.Column<short>(type: "smallint", nullable: true),
                    set_number = table.Column<short>(type: "smallint", nullable: false),
                    member_seq_in_match = table.Column<short>(type: "smallint", nullable: false),
                    home_score_at_moment = table.Column<short>(type: "smallint", nullable: false),
                    guest_score_at_moment = table.Column<short>(type: "smallint", nullable: false),
                    minute_mark = table.Column<short>(type: "smallint", nullable: true),
                    event_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t18_sanctions", x => x.id);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_s12_sanction_types_sanction_type_code",
                        column: x => x.sanction_type_code,
                        principalTable: "s12_sanction_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_s13_delay_violations_delay_violation_code",
                        column: x => x.delay_violation_code,
                        principalTable: "s13_delay_violations",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_s15_sanction_kinds_sanction_kind_code",
                        column: x => x.sanction_kind_code,
                        principalTable: "s15_sanction_kinds",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_s16_recipient_types_recipient_type_code",
                        column: x => x.recipient_type_code,
                        principalTable: "s16_recipient_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t18_sanctions_t6_players_player_id",
                        column: x => x.player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t21_match_captains",
                columns: table => new
                {
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t21_match_captains", x => new { x.match_id, x.team_id });
                    table.ForeignKey(
                        name: "FK_t21_match_captains_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t21_match_captains_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t21_match_captains_t6_players_player_id",
                        column: x => x.player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t5_delegations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<int>(type: "integer", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    role_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    assistant_seq_no = table.Column<short>(type: "smallint", nullable: true),
                    last_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    first_name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t5_delegations", x => x.id);
                    table.ForeignKey(
                        name: "FK_t5_delegations_t14_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "t14_matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t5_delegations_t4_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "t4_teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t16_protocol_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    protocol_id = table.Column<int>(type: "integer", nullable: false),
                    changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status_at_moment = table.Column<short>(type: "smallint", nullable: true),
                    data_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t16_protocol_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_t16_protocol_history_t16_protocols_protocol_id",
                        column: x => x.protocol_id,
                        principalTable: "t16_protocols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t17a_substitutions",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    sub_out_player_id = table.Column<int>(type: "integer", nullable: false),
                    sub_in_player_id = table.Column<int>(type: "integer", nullable: false),
                    sub_type_code = table.Column<short>(type: "smallint", nullable: true),
                    sub_seq_in_set = table.Column<short>(type: "smallint", nullable: true),
                    is_libero_swap = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t17a_substitutions", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_t17a_substitutions_s10_substitution_types_sub_type_code",
                        column: x => x.sub_type_code,
                        principalTable: "s10_substitution_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t17a_substitutions_t17_events_event_id",
                        column: x => x.event_id,
                        principalTable: "t17_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t17a_substitutions_t6_players_sub_in_player_id",
                        column: x => x.sub_in_player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t17a_substitutions_t6_players_sub_out_player_id",
                        column: x => x.sub_out_player_id,
                        principalTable: "t6_players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t17b_timeouts",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    timeout_type_code = table.Column<short>(type: "smallint", nullable: false),
                    timeout_seq_in_set = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t17b_timeouts", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_t17b_timeouts_s11_timeout_types_timeout_type_code",
                        column: x => x.timeout_type_code,
                        principalTable: "s11_timeout_types",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t17b_timeouts_t17_events_event_id",
                        column: x => x.event_id,
                        principalTable: "t17_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_users_email",
                table: "app_users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_users_linked_coach_id",
                table: "app_users",
                column: "linked_coach_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_users_linked_organizer_id",
                table: "app_users",
                column: "linked_organizer_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_users_linked_player_id",
                table: "app_users",
                column: "linked_player_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_users_linked_referee_id",
                table: "app_users",
                column: "linked_referee_id");

            migrationBuilder.CreateIndex(
                name: "IX_r1_starting_lineups_match_id_team_id_set_number_player_id",
                table: "r1_starting_lineups",
                columns: new[] { "match_id", "team_id", "set_number", "player_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_r1_starting_lineups_player_id",
                table: "r1_starting_lineups",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_r1_starting_lineups_team_id",
                table: "r1_starting_lineups",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_r2_player_stats_player_id",
                table: "r2_player_stats",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_r2_player_stats_team_id",
                table: "r2_player_stats",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t10_tournaments_format_code",
                table: "t10_tournaments",
                column: "format_code");

            migrationBuilder.CreateIndex(
                name: "IX_t10_tournaments_name",
                table: "t10_tournaments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t10_tournaments_organizer_id",
                table: "t10_tournaments",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "IX_t10_tournaments_scoring_system_code",
                table: "t10_tournaments",
                column: "scoring_system_code");

            migrationBuilder.CreateIndex(
                name: "IX_t10_tournaments_season_id",
                table: "t10_tournaments",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_t11_groups_tournament_id_name",
                table: "t11_groups",
                columns: new[] { "tournament_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_coin_toss_choice_code",
                table: "t14_matches",
                column: "coin_toss_choice_code");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_group_id",
                table: "t14_matches",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_guest_team_id",
                table: "t14_matches",
                column: "guest_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_home_team_id",
                table: "t14_matches",
                column: "home_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_stage_code",
                table: "t14_matches",
                column: "stage_code");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_status_code",
                table: "t14_matches",
                column: "status_code");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_tournament_id",
                table: "t14_matches",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "IX_t14_matches_venue_id",
                table: "t14_matches",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_t15_referee_assignments_match_id_referee_id",
                table: "t15_referee_assignments",
                columns: new[] { "match_id", "referee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t15_referee_assignments_referee_id",
                table: "t15_referee_assignments",
                column: "referee_id");

            migrationBuilder.CreateIndex(
                name: "IX_t15_referee_assignments_role_code",
                table: "t15_referee_assignments",
                column: "role_code");

            migrationBuilder.CreateIndex(
                name: "IX_t16_protocol_history_protocol_id",
                table: "t16_protocol_history",
                column: "protocol_id");

            migrationBuilder.CreateIndex(
                name: "IX_t16_protocols_match_id",
                table: "t16_protocols",
                column: "match_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t16_protocols_organizer_id",
                table: "t16_protocols",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "IX_t16_protocols_status_code",
                table: "t16_protocols",
                column: "status_code");

            migrationBuilder.CreateIndex(
                name: "IX_t17_events_event_type_code",
                table: "t17_events",
                column: "event_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_t17_events_match_id_set_number_global_seq_in_set",
                table: "t17_events",
                columns: new[] { "match_id", "set_number", "global_seq_in_set" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t17_events_team_id",
                table: "t17_events",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t17a_substitutions_sub_in_player_id",
                table: "t17a_substitutions",
                column: "sub_in_player_id");

            migrationBuilder.CreateIndex(
                name: "IX_t17a_substitutions_sub_out_player_id",
                table: "t17a_substitutions",
                column: "sub_out_player_id");

            migrationBuilder.CreateIndex(
                name: "IX_t17a_substitutions_sub_type_code",
                table: "t17a_substitutions",
                column: "sub_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_t17b_timeouts_timeout_type_code",
                table: "t17b_timeouts",
                column: "timeout_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_delay_violation_code",
                table: "t18_sanctions",
                column: "delay_violation_code");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_match_id",
                table: "t18_sanctions",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_player_id",
                table: "t18_sanctions",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_recipient_type_code",
                table: "t18_sanctions",
                column: "recipient_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_sanction_kind_code",
                table: "t18_sanctions",
                column: "sanction_kind_code");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_sanction_type_code",
                table: "t18_sanctions",
                column: "sanction_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_t18_sanctions_team_id",
                table: "t18_sanctions",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t20_awards_award_type_code",
                table: "t20_awards",
                column: "award_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_t20_awards_player_id",
                table: "t20_awards",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_t20_awards_team_id",
                table: "t20_awards",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t20_awards_tournament_id",
                table: "t20_awards",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "IX_t21_match_captains_player_id",
                table: "t21_match_captains",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_t21_match_captains_team_id",
                table: "t21_match_captains",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t4_teams_head_coach_id",
                table: "t4_teams",
                column: "head_coach_id");

            migrationBuilder.CreateIndex(
                name: "IX_t4_teams_home_venue_id",
                table: "t4_teams",
                column: "home_venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_t4_teams_name",
                table: "t4_teams",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t4_teams_region_oktmo",
                table: "t4_teams",
                column: "region_oktmo");

            migrationBuilder.CreateIndex(
                name: "IX_t5_delegations_match_id",
                table: "t5_delegations",
                column: "match_id");

            migrationBuilder.CreateIndex(
                name: "IX_t5_delegations_team_id",
                table: "t5_delegations",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t6_players_amplua_code",
                table: "t6_players",
                column: "amplua_code");

            migrationBuilder.CreateIndex(
                name: "IX_t6_players_status_code",
                table: "t6_players",
                column: "status_code");

            migrationBuilder.CreateIndex(
                name: "IX_t6_players_team_id",
                table: "t6_players",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_t7_applications_status_code",
                table: "t7_applications",
                column: "status_code");

            migrationBuilder.CreateIndex(
                name: "IX_t7_applications_team_id_tournament_id",
                table: "t7_applications",
                columns: new[] { "team_id", "tournament_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t7_applications_tournament_id",
                table: "t7_applications",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "IX_t8_application_composition_application_id_jersey_number_in_~",
                table: "t8_application_composition",
                columns: new[] { "application_id", "jersey_number_in_app" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t8_application_composition_player_id",
                table: "t8_application_composition",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_t9_seasons_name",
                table: "t9_seasons",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_users");

            migrationBuilder.DropTable(
                name: "r1_starting_lineups");

            migrationBuilder.DropTable(
                name: "r2_player_stats");

            migrationBuilder.DropTable(
                name: "r3_sets");

            migrationBuilder.DropTable(
                name: "t15_referee_assignments");

            migrationBuilder.DropTable(
                name: "t16_protocol_history");

            migrationBuilder.DropTable(
                name: "t17a_substitutions");

            migrationBuilder.DropTable(
                name: "t17b_timeouts");

            migrationBuilder.DropTable(
                name: "t18_sanctions");

            migrationBuilder.DropTable(
                name: "t20_awards");

            migrationBuilder.DropTable(
                name: "t21_match_captains");

            migrationBuilder.DropTable(
                name: "t5_delegations");

            migrationBuilder.DropTable(
                name: "t8_application_composition");

            migrationBuilder.DropTable(
                name: "s8_referee_roles");

            migrationBuilder.DropTable(
                name: "t12_referees");

            migrationBuilder.DropTable(
                name: "t16_protocols");

            migrationBuilder.DropTable(
                name: "s10_substitution_types");

            migrationBuilder.DropTable(
                name: "s11_timeout_types");

            migrationBuilder.DropTable(
                name: "t17_events");

            migrationBuilder.DropTable(
                name: "s12_sanction_types");

            migrationBuilder.DropTable(
                name: "s13_delay_violations");

            migrationBuilder.DropTable(
                name: "s15_sanction_kinds");

            migrationBuilder.DropTable(
                name: "s16_recipient_types");

            migrationBuilder.DropTable(
                name: "s14_award_types");

            migrationBuilder.DropTable(
                name: "t6_players");

            migrationBuilder.DropTable(
                name: "t7_applications");

            migrationBuilder.DropTable(
                name: "s7_protocol_statuses");

            migrationBuilder.DropTable(
                name: "s9_event_types");

            migrationBuilder.DropTable(
                name: "t14_matches");

            migrationBuilder.DropTable(
                name: "s1_amplua");

            migrationBuilder.DropTable(
                name: "s2_player_statuses");

            migrationBuilder.DropTable(
                name: "s3_application_statuses");

            migrationBuilder.DropTable(
                name: "s17_coin_toss_options");

            migrationBuilder.DropTable(
                name: "s5_tournament_stages");

            migrationBuilder.DropTable(
                name: "s6_match_statuses");

            migrationBuilder.DropTable(
                name: "t11_groups");

            migrationBuilder.DropTable(
                name: "t4_teams");

            migrationBuilder.DropTable(
                name: "t10_tournaments");

            migrationBuilder.DropTable(
                name: "t1_regions");

            migrationBuilder.DropTable(
                name: "t2_venues");

            migrationBuilder.DropTable(
                name: "t3_coaches");

            migrationBuilder.DropTable(
                name: "s18_scoring_systems");

            migrationBuilder.DropTable(
                name: "s4_tournament_formats");

            migrationBuilder.DropTable(
                name: "t13_organizers");

            migrationBuilder.DropTable(
                name: "t9_seasons");
        }
    }
}

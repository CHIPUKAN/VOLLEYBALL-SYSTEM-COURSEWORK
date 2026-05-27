using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyballIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingSchemaFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Баг 19.2 — формально добавить колонку comment в t7_applications (была добавлена рантайм-костылём)
            migrationBuilder.Sql(@"
                ALTER TABLE t7_applications
                ADD COLUMN IF NOT EXISTS comment TEXT;
            ");

            // Баг 19.1 — добавить отсутствующий FK на coin_toss_winner_team_id
            migrationBuilder.Sql(@"
                DO $$ BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.table_constraints
                        WHERE constraint_name = 'FK_t14_matches_t4_teams_coin_toss_winner_team_id'
                          AND table_name = 't14_matches'
                    ) THEN
                        ALTER TABLE t14_matches
                        ADD CONSTRAINT ""FK_t14_matches_t4_teams_coin_toss_winner_team_id""
                        FOREIGN KEY (coin_toss_winner_team_id)
                        REFERENCES t4_teams(id)
                        ON DELETE RESTRICT;
                    END IF;
                END $$;
            ");

            // Баг 19.1 — добавить отсутствующий FK на first_serve_team_id
            migrationBuilder.Sql(@"
                DO $$ BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.table_constraints
                        WHERE constraint_name = 'FK_t14_matches_t4_teams_first_serve_team_id'
                          AND table_name = 't14_matches'
                    ) THEN
                        ALTER TABLE t14_matches
                        ADD CONSTRAINT ""FK_t14_matches_t4_teams_first_serve_team_id""
                        FOREIGN KEY (first_serve_team_id)
                        REFERENCES t4_teams(id)
                        ON DELETE RESTRICT;
                    END IF;
                END $$;
            ");

            // Баг 20.2 — CHECK-ограничение для роли делегации
            migrationBuilder.Sql(@"
                DO $$ BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.table_constraints
                        WHERE constraint_name = 't5_delegations_role_type_check'
                          AND table_name = 't5_delegations'
                    ) THEN
                        ALTER TABLE t5_delegations
                        ADD CONSTRAINT t5_delegations_role_type_check
                        CHECK (role_type IN ('помощник тренера', 'массажист', 'врач'));
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE t14_matches DROP CONSTRAINT IF EXISTS ""FK_t14_matches_t4_teams_first_serve_team_id"";");
            migrationBuilder.Sql(@"ALTER TABLE t14_matches DROP CONSTRAINT IF EXISTS ""FK_t14_matches_t4_teams_coin_toss_winner_team_id"";");
            migrationBuilder.Sql(@"ALTER TABLE t5_delegations DROP CONSTRAINT IF EXISTS t5_delegations_role_type_check;");
        }
    }
}

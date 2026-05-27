using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyballIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBestOf3Support : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "sets_to_win",
                table: "t10_tournaments",
                type: "smallint",
                nullable: false,
                defaultValue: (short)3);

            migrationBuilder.AddColumn<short>(
                name: "tiebreak_score_target",
                table: "t10_tournaments",
                type: "smallint",
                nullable: false,
                defaultValue: (short)15);

            migrationBuilder.Sql(@"
                ALTER TABLE t10_tournaments
                ADD CONSTRAINT t10_tournaments_sets_to_win_check
                CHECK (sets_to_win IN (1, 2, 3));
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE t10_tournaments
                ADD CONSTRAINT t10_tournaments_tiebreak_score_check
                CHECK (tiebreak_score_target >= 5 AND tiebreak_score_target <= 30);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE t10_tournaments DROP CONSTRAINT IF EXISTS t10_tournaments_sets_to_win_check;");
            migrationBuilder.Sql("ALTER TABLE t10_tournaments DROP CONSTRAINT IF EXISTS t10_tournaments_tiebreak_score_check;");

            migrationBuilder.DropColumn(
                name: "sets_to_win",
                table: "t10_tournaments");

            migrationBuilder.DropColumn(
                name: "tiebreak_score_target",
                table: "t10_tournaments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class AddMultiplierForRankings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "MultiplierRankingAssists",
                table: "server_settings",
                nullable: false,
                defaultValue: 25f);

            migrationBuilder.AddColumn<float>(
                name: "MultiplierRankingDamage",
                table: "server_settings",
                nullable: false,
                defaultValue: 1f);

            migrationBuilder.AddColumn<float>(
                name: "MultiplierRankingKills",
                table: "server_settings",
                nullable: false,
                defaultValue: 75f);

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                columns: new[] { "MultiplierRankingAssists", "MultiplierRankingDamage", "MultiplierRankingKills" },
                values: new object[] { 25f, 1f, 75f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultiplierRankingAssists",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "MultiplierRankingDamage",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "MultiplierRankingKills",
                table: "server_settings");
        }
    }
}

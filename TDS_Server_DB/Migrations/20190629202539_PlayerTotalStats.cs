using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class PlayerTotalStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MostAssistsInADay",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostAssistsInARound",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostDamageInADay",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostDamageInARound",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostKillsInADay",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MostKillsInARound",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalMapsBought",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRounds",
                table: "player_lobby_stats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "player_total_stats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    Money = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_total_stats_pkey", x => x.PlayerID);
                    table.ForeignKey(
                        name: "player_total_stats_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_total_stats");

            migrationBuilder.DropColumn(
                name: "MostAssistsInADay",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "MostAssistsInARound",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "MostDamageInADay",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "MostDamageInARound",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "MostKillsInADay",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "MostKillsInARound",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "TotalMapsBought",
                table: "player_lobby_stats");

            migrationBuilder.DropColumn(
                name: "TotalRounds",
                table: "player_lobby_stats");
        }
    }
}

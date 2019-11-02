using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class ShowRanking_To_LobbyRoundSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowRanking",
                table: "lobbies");

            migrationBuilder.AddColumn<bool>(
                name: "ShowRanking",
                table: "lobby_round_settings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowRanking",
                table: "lobby_round_settings");

            migrationBuilder.AddColumn<bool>(
                name: "ShowRanking",
                table: "lobbies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

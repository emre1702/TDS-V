using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerSettings_GWSettings_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountPlayersAllowedInGangwarTeamBeforeCountCheck",
                table: "server_settings",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<bool>(
                name: "GangwarAttackerCanBeMore",
                table: "server_settings",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "GangwarOwnerCanBeMore",
                table: "server_settings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPlayersAllowedInGangwarTeamBeforeCountCheck",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "GangwarAttackerCanBeMore",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "GangwarOwnerCanBeMore",
                table: "server_settings");
        }
    }
}

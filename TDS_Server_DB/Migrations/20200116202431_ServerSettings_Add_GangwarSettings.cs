using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerSettings_Add_GangwarSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "gangwar_target_radius",
                table: "server_settings",
                nullable: false,
                defaultValue: 5.0);

            migrationBuilder.AddColumn<int>(
                name: "gangwar_target_without_attacker_max_seconds",
                table: "server_settings",
                nullable: false,
                defaultValue: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gangwar_target_radius",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "gangwar_target_without_attacker_max_seconds",
                table: "server_settings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;

namespace TDS_Server_DB.Migrations
{
    public partial class GangwarTimeMsToSecs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gangwar_action_time_ms",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "gangwar_preparation_time_ms",
                table: "server_settings");

            migrationBuilder.AddColumn<long>(
                name: "gangwar_action_time",
                table: "server_settings",
                nullable: false,
                defaultValue: 900L);

            migrationBuilder.AddColumn<long>(
                name: "gangwar_preparation_time",
                table: "server_settings",
                nullable: false,
                defaultValue: 180L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gangwar_action_time",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "gangwar_preparation_time",
                table: "server_settings");

            migrationBuilder.AddColumn<long>(
                name: "gangwar_action_time_ms",
                table: "server_settings",
                type: "bigint",
                nullable: false,
                defaultValue: 900000L);

            migrationBuilder.AddColumn<long>(
                name: "gangwar_preparation_time_ms",
                table: "server_settings",
                type: "bigint",
                nullable: false,
                defaultValue: 180000L);
        }
    }
}

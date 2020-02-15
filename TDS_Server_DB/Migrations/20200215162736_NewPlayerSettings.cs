using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class NewPlayerSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "afk_kick_after_sec",
                table: "server_settings");

            migrationBuilder.AddColumn<int>(
                name: "afk_kick_after_seconds",
                table: "player_settings",
                nullable: false,
                defaultValue: 25);

            migrationBuilder.AddColumn<int>(
                name: "afk_kick_show_warning_last_seconds",
                table: "player_settings",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<bool>(
                name: "check_afk",
                table: "player_settings",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "nametag_armor_empty_color",
                table: "player_settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nametag_armor_full_color",
                table: "player_settings",
                nullable: true,
                defaultValue: "rgba(255, 255, 255, 1)");

            migrationBuilder.AddColumn<string>(
                name: "nametag_dead_color",
                table: "player_settings",
                nullable: true,
                defaultValue: "rgba(0, 0, 0, 1)");

            migrationBuilder.AddColumn<string>(
                name: "nametag_health_empty_color",
                table: "player_settings",
                nullable: true,
                defaultValue: "rgba(50, 0, 0, 1)");

            migrationBuilder.AddColumn<string>(
                name: "nametag_health_full_color",
                table: "player_settings",
                nullable: true,
                defaultValue: "rgba(0, 255, 0, 1)");

            migrationBuilder.AddColumn<int>(
                name: "show_floating_damage_info_duration_ms",
                table: "player_settings",
                nullable: false,
                defaultValue: 1000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "afk_kick_after_seconds",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "afk_kick_show_warning_last_seconds",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "check_afk",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "nametag_armor_empty_color",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "nametag_armor_full_color",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "nametag_dead_color",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "nametag_health_empty_color",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "nametag_health_full_color",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "show_floating_damage_info_duration_ms",
                table: "player_settings");

            migrationBuilder.AddColumn<int>(
                name: "afk_kick_after_sec",
                table: "server_settings",
                type: "integer",
                nullable: false,
                defaultValue: 25);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class PlayerSettings_Add_Cooldowns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bloodscreen_cooldown_ms",
                table: "player_settings",
                nullable: false,
                defaultValue: 150);

            migrationBuilder.AddColumn<int>(
                name: "hud_ammo_update_cooldown_ms",
                table: "player_settings",
                nullable: false,
                defaultValue: 100);

            migrationBuilder.AddColumn<int>(
                name: "hud_health_update_cooldown_ms",
                table: "player_settings",
                nullable: false,
                defaultValue: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bloodscreen_cooldown_ms",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "hud_ammo_update_cooldown_ms",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "hud_health_update_cooldown_ms",
                table: "player_settings");
        }
    }
}

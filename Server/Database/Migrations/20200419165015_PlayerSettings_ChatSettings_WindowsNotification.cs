using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerSettings_ChatSettings_WindowsNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "chat_font_size",
                table: "player_settings",
                nullable: false,
                defaultValue: 1.4f);

            migrationBuilder.AddColumn<float>(
                name: "chat_max_height",
                table: "player_settings",
                nullable: false,
                defaultValue: 35f);

            migrationBuilder.AddColumn<float>(
                name: "chat_width",
                table: "player_settings",
                nullable: false,
                defaultValue: 30f);

            migrationBuilder.AddColumn<bool>(
                name: "hide_dirty_chat",
                table: "player_settings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_cursor_on_chat_open",
                table: "player_settings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "windows_notifications",
                table: "player_settings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chat_font_size",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "chat_max_height",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "chat_width",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "hide_dirty_chat",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "show_cursor_on_chat_open",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "windows_notifications",
                table: "player_settings");
        }
    }
}

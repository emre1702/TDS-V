using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class PlayerSettings_Add_DateTimeFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time_zone",
                table: "player_settings",
                newName: "timezone");

            migrationBuilder.AddColumn<string>(
                name: "date_time_format",
                table: "player_settings",
                nullable: false,
                defaultValue: "yyyy'-'MM'-'dd HH':'mm':'ss");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_time_format",
                table: "player_settings");

            migrationBuilder.RenameColumn(
                name: "timezone",
                table: "player_settings",
                newName: "time_zone");
        }
    }
}

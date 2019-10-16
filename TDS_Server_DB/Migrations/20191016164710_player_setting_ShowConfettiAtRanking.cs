using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class player_setting_ShowConfettiAtRanking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowConfettiAtRanking",
                table: "player_settings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowConfettiAtRanking",
                table: "player_settings");
        }
    }
}

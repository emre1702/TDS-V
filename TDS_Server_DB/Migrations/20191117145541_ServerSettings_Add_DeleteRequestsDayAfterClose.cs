using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerSettings_Add_DeleteRequestsDayAfterClose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeleteOfflineMessagesAfterDays",
                table: "server_settings",
                nullable: false,
                defaultValue: 60);

            migrationBuilder.AddColumn<int>(
                name: "DefendCount",
                table: "gangwar_areas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteOfflineMessagesAfterDays",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "DefendCount",
                table: "gangwar_areas");
        }
    }
}

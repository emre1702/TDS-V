using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerSettings_DeleteRequestsDaysAfterClose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeleteRequestsDaysAfterClose",
                table: "server_settings",
                nullable: false,
                defaultValue: 30L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteRequestsDaysAfterClose",
                table: "server_settings");
        }
    }
}

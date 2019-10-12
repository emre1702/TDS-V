using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class DeactivatedMapsPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NeedCheckMapsPath",
                table: "server_settings",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "NeedCheckMapsPath",
                value: "bridge/resources/tds/needcheckmaps/");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedCheckMapsPath",
                table: "server_settings");
        }
    }
}

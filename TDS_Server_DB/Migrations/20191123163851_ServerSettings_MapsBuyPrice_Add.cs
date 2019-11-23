using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerSettings_MapsBuyPrice_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapBuyBasePrice",
                table: "server_settings",
                nullable: false,
                defaultValue: 1000);

            migrationBuilder.AddColumn<float>(
                name: "MapBuyCounterMultiplicator",
                table: "server_settings",
                nullable: false,
                defaultValue: 1f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapBuyBasePrice",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "MapBuyCounterMultiplicator",
                table: "server_settings");
        }
    }
}

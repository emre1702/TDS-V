using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class SeedData_Arena_ShowRanking_True : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "lobby_round_settings",
                keyColumn: "lobby_id",
                keyValue: -1,
                column: "show_ranking",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "lobby_round_settings",
                keyColumn: "lobby_id",
                keyValue: -1,
                column: "show_ranking",
                value: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class NametagMaxDistance_80 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "NametagMaxDistance",
                value: 80f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "NametagMaxDistance",
                value: 625f);
        }
    }
}

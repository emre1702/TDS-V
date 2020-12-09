using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class ServerStatsRefresh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Short",
                table: "Gangs",
                newName: "NameShort");

            migrationBuilder.AlterColumn<int>(
                name: "RefreshServerStatsFrequencySec",
                table: "BonusbotSettings",
                type: "integer",
                nullable: false,
                defaultValue: 300,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 60);

            migrationBuilder.UpdateData(
                table: "BonusbotSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshServerStatsFrequencySec",
                value: 300);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameShort",
                table: "Gangs",
                newName: "Short");

            migrationBuilder.AlterColumn<int>(
                name: "RefreshServerStatsFrequencySec",
                table: "BonusbotSettings",
                type: "integer",
                nullable: false,
                defaultValue: 60,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 300);

            migrationBuilder.UpdateData(
                table: "BonusbotSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "RefreshServerStatsFrequencySec",
                value: 0);
        }
    }
}

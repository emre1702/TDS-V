using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class MapLimitTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DieAfterOutsideMapLimitTime",
                table: "lobbies");

            migrationBuilder.AddColumn<int>(
                name: "MapLimitTime",
                table: "lobby_map_settings",
                nullable: false,
                defaultValueSql: "10");

            migrationBuilder.AlterColumn<float>(
                name: "DefaultSpawnZ",
                table: "lobbies",
                nullable: false,
                defaultValueSql: "9000",
                oldClrType: typeof(float),
                oldDefaultValueSql: "900");

            migrationBuilder.UpdateData(
                table: "lobby_map_settings",
                keyColumn: "LobbyID",
                keyValue: 1,
                column: "MapLimitTime",
                value: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapLimitTime",
                table: "lobby_map_settings");

            migrationBuilder.AlterColumn<float>(
                name: "DefaultSpawnZ",
                table: "lobbies",
                nullable: false,
                defaultValueSql: "900",
                oldClrType: typeof(float),
                oldDefaultValueSql: "9000");

            migrationBuilder.AddColumn<int>(
                name: "DieAfterOutsideMapLimitTime",
                table: "lobbies",
                nullable: false,
                defaultValueSql: "10");

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "ID",
                keyValue: 1,
                column: "DieAfterOutsideMapLimitTime",
                value: 10);
        }
    }
}

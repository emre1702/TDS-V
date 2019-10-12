using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class MapCreateLobby_dummy_lobby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "ID", "AmountLifes", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "SpawnAgainAfterDeathMs", "Type" },
                values: new object[] { 3, (short)1, true, false, "MapCreateLobby", 0, null, 400, ELobbyType.MapCreateLobby });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "lobbies",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}

using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Database.Migrations
{
    public partial class Add_DamageTestLobby_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lobbies",
                columns: new[] { "Id", "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "Type" },
                values: new object[] { -6, -365.425f, -131.809f, 37.873f, true, false, "DamageTestLobby", -1, null, LobbyType.DamageTestLobby });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lobbies",
                keyColumn: "Id",
                keyValue: -6);
        }
    }
}

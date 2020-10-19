using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_DamageTestLobby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE lobby_type ADD VALUE 'damage_test_lobby' AFTER 'gang_action_lobby'");

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

            migrationBuilder.Sql("ALTER TYPE lobby_type ADD VALUE 'damage_test_lobby' AFTER 'gang_action_lobby'");
        }
    }
}

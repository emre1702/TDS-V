using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_DamageTestLobby_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                 table: "LobbyFightSettings",
                 column: "LobbyId",
                 value: -6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LobbyFightSettings",
                keyColumn: "LobbyId",
                keyValue: -6);
        }
    }
}

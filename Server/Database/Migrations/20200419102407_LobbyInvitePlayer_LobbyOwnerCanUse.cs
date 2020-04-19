using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class LobbyInvitePlayer_LobbyOwnerCanUse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "commands",
                keyColumn: "id",
                keyValue: (short)25,
                column: "lobby_owner_can_use",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "commands",
                keyColumn: "id",
                keyValue: (short)25,
                column: "lobby_owner_can_use",
                value: false);
        }
    }
}

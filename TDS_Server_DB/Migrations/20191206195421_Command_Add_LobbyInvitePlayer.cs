using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class Command_Add_LobbyInvitePlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "ID", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[] { (short)25, "LobbyInvitePlayer", false, null, null, false });

            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "LobbyInvite", (short)25 },
                    { "InviteLobby", (short)25 },
                    { "InvitePlayerLobby", (short)25 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "ID", "Language", "Info" },
                values: new object[,]
                {
                    { (short)25, ELanguage.German, "Ladet einen Spieler in die eigene Lobby ein (falls möglich)." },
                    { (short)25, ELanguage.English, "Invite a player to your lobby (if possible)." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "command_alias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "InviteLobby", (short)25 });

            migrationBuilder.DeleteData(
                table: "command_alias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "InvitePlayerLobby", (short)25 });

            migrationBuilder.DeleteData(
                table: "command_alias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "LobbyInvite", (short)25 });

            migrationBuilder.DeleteData(
                table: "command_infos",
                keyColumns: new[] { "ID", "Language" },
                keyValues: new object[] { (short)25, ELanguage.German });

            migrationBuilder.DeleteData(
                table: "command_infos",
                keyColumns: new[] { "ID", "Language" },
                keyValues: new object[] { (short)25, ELanguage.English });

            migrationBuilder.DeleteData(
                table: "commands",
                keyColumn: "ID",
                keyValue: (short)25);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;

namespace TDS_Server_DB.Migrations
{
    public partial class AdminCommands_Add_Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "command_infos",
                keyColumns: new[] { "id", "language" },
                keyValues: new object[] { (short)25, ELanguage.English },
                column: "info",
                value: "Invites a player to your lobby (if possible).");

            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "id", "command", "lobby_owner_can_use", "needed_admin_level", "needed_donation", "vip_can_use" },
                values: new object[] { (short)26, "Test", false, 3, null, false });

         
            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "id", "language", "info" },
                values: new object[,]
                {
                    { (short)26, ELanguage.German, "Befehl zum schnellen Testen von Codes." },
                    { (short)26, ELanguage.English, "Command for quick testing of codes." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "command_infos",
                keyColumns: new[] { "id", "language" },
                keyValues: new object[] { (short)26, ELanguage.German });

            migrationBuilder.DeleteData(
                table: "command_infos",
                keyColumns: new[] { "id", "language" },
                keyValues: new object[] { (short)26, ELanguage.English });

            migrationBuilder.DeleteData(
                table: "commands",
                keyColumn: "id",
                keyValue: (short)26);

            migrationBuilder.UpdateData(
                table: "command_infos",
                keyColumns: new[] { "id", "language" },
                keyValues: new object[] { (short)25, ELanguage.English },
                column: "info",
                value: "Invite a player to your lobby (if possible).");

        }
    }
}

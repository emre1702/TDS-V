using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Migrations
{
    public partial class MapCreator_Rewards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapCreatorRewardBought",
                table: "ServerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "MapCreatorRewardRandomlySelected",
                table: "ServerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MapCreatorRewardVoted",
                table: "ServerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Language", "Answer", "Question" },
                values: new object[,]
                {
                    { 3, Language.English, "The map creator gets following rewards (only official lobbies):\n$1 - map got selected randomly\n$5 - map got voted\n$15 - map got bought", "What are the rewards for created maps?" },
                    { 3, Language.German, "Der Karten-Ersteller bekommt folgende Belohnungen (nur in offiziellen Lobbies):\n$1 - Karte wurde zufällig ausgewählt\n$5 - Karte wurde per Abstimmung gewählt\n$15 - Karte wurde gekauft", "Was sind die Belohnungen für erstelle Karten?" }
                });

            migrationBuilder.UpdateData(
                table: "ServerSettings",
                keyColumn: "Id",
                keyValue: (short)1,
                columns: new[] { "MapCreatorRewardBought", "MapCreatorRewardRandomlySelected", "MapCreatorRewardVoted" },
                values: new object[] { 15, 1, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 3, Language.German });

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 3, Language.English });

            migrationBuilder.DropColumn(
                name: "MapCreatorRewardBought",
                table: "ServerSettings");

            migrationBuilder.DropColumn(
                name: "MapCreatorRewardRandomlySelected",
                table: "ServerSettings");

            migrationBuilder.DropColumn(
                name: "MapCreatorRewardVoted",
                table: "ServerSettings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Migrations
{
    public partial class CharCreatorClothes2Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FAQs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 3, Language.German },
                column: "Question",
                value: "Was sind die Belohnungen für erstellte Karten?");

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Language", "Answer", "Question" },
                values: new object[,]
                {
                    { 4, Language.German, "Die Farbe der Shirts gibt dir diese zwei Informationen:\n1. Farbe der Ärmel: Feind (rot) oder Freund (grün)\n2. Rest: Team-Farbe", "Warum sind die Shirts in z.B. Arena verschieden farbig?" },
                    { 4, Language.English, "The color of the shirts gives you these two information:\n1. Color of the sleeves: Enemy (red) or friend (green)\n2. Rest: Team color", "Why are the shirts in e.g. Arena differently colored?" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 4, Language.German });

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 4, Language.English });

            migrationBuilder.UpdateData(
                table: "FAQs",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { 3, Language.German },
                column: "Question",
                value: "Was sind die Belohnungen für erstelle Karten?");
        }
    }
}

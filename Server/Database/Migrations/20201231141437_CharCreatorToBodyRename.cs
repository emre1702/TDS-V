using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class CharCreatorToBodyRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PlayerCharAppearanceDatas",
                newName: "PlayerBodyAppearanceDatas");

            migrationBuilder.RenameTable(
                name: "PlayerCharFeaturesDatas",
                newName: "PlayerBodyFeaturesDatas");

            migrationBuilder.RenameTable(
                name: "PlayerCharGeneralDatas",
                newName: "PlayerBodyGeneralDatas");

            migrationBuilder.RenameTable(
                name: "PlayerCharHairAndColorsDatas",
                newName: "PlayerBodyHairAndColorsDatas");

            migrationBuilder.RenameTable(
                name: "PlayerCharHeritageDatas",
                newName: "PlayerBodyHeritageDatas");

            migrationBuilder.RenameTable(
                name: "PlayerCharDatas",
                newName: "PlayerBodyDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
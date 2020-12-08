using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
#pragma warning disable
    public partial class FixPlayerCharHasOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharAppearanceDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharHeritageDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharHairAndColorsDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharGeneralDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharFeaturesDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharAppearanceDatas",
                type: "smallint",
                nullable: true);
        }
    }
}

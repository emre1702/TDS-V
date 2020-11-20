using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class FixPlayerCharHasOne_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyncedData_PlayerId",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_PlayerId",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_PlayerId",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_PlayerId",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_PlayerId",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_PlayerId",
                table: "PlayerCharAppearanceDatas");

            migrationBuilder.DropColumn(
                name: "SyncedData_Slot",
                table: "PlayerCharAppearanceDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SyncedData_PlayerId",
                table: "PlayerCharHeritageDatas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharHeritageDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncedData_PlayerId",
                table: "PlayerCharHairAndColorsDatas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharHairAndColorsDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncedData_PlayerId",
                table: "PlayerCharGeneralDatas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharGeneralDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncedData_PlayerId",
                table: "PlayerCharFeaturesDatas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharFeaturesDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncedData_PlayerId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharDatas",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SyncedData_PlayerId",
                table: "PlayerCharAppearanceDatas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SyncedData_Slot",
                table: "PlayerCharAppearanceDatas",
                type: "smallint",
                nullable: true);
        }
    }
}

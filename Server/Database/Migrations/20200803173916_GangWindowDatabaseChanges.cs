using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class GangWindowDatabaseChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Gangs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "GangRanks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetRanks",
                table: "GangRankPermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GangRankPermissions",
                keyColumn: "GangId",
                keyValue: -1,
                columns: new[] { "InviteMembers", "KickMembers", "ManagePermissions", "ManageRanks", "StartGangwar" },
                values: new object[] { 1, 1, 1, 1, 1 });

            migrationBuilder.UpdateData(
                table: "GangRanks",
                keyColumns: new[] { "GangId", "Rank" },
                keyValues: new object[] { -1, (short)0 },
                column: "Color",
                value: "rgb(255,255,255)");          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Gangs");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "GangRanks");

            migrationBuilder.DropColumn(
                name: "SetRanks",
                table: "GangRankPermissions");


            migrationBuilder.UpdateData(
                table: "GangRankPermissions",
                keyColumn: "GangId",
                keyValue: -1,
                columns: new[] { "InviteMembers", "KickMembers", "ManagePermissions", "ManageRanks", "StartGangwar" },
                values: new object[] { 5, 5, 5, 5, 5 });
        }
    }
}

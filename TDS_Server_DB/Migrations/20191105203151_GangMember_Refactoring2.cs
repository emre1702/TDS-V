using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class GangMember_Refactoring2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "gangs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<short>(
                name: "Rank",
                table: "gang_members",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.UpdateData(
                table: "gangs",
                keyColumn: "ID",
                keyValue: -1,
                column: "OwnerId",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "gangs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Rank",
                table: "gang_members",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: (short)0);

            migrationBuilder.UpdateData(
                table: "gangs",
                keyColumn: "ID",
                keyValue: -1,
                column: "OwnerId",
                value: 0);
        }
    }
}

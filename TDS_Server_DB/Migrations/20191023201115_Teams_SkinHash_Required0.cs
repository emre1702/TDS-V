using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Teams_SkinHash_Required0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SkinHash",
                table: "teams",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 1,
                column: "SkinHash",
                value: 0);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 4,
                column: "SkinHash",
                value: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SkinHash",
                table: "teams",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 1,
                column: "SkinHash",
                value: null);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 4,
                column: "SkinHash",
                value: null);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class KillingSpreeMaxSecondsUntilNextKill_DataType_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "KillingSpreeMaxSecondsUntilNextKill",
                table: "server_settings",
                nullable: false,
                defaultValue: 18,
                oldClrType: typeof(float),
                oldDefaultValue: 18f);

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "KillingSpreeMaxSecondsUntilNextKill",
                value: 18);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "KillingSpreeMaxSecondsUntilNextKill",
                table: "server_settings",
                nullable: false,
                defaultValue: 18f,
                oldClrType: typeof(int),
                oldDefaultValue: 18);

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                column: "KillingSpreeMaxSecondsUntilNextKill",
                value: 18f);
        }
    }
}

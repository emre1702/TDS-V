using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class AddGlobalToAlias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "Alias", "Command" },
                values: new object[] { "Global", (short)12 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "command_alias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "Global", (short)12 });
        }
    }
}

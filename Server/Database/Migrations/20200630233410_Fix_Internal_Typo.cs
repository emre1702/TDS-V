using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Fix_Internal_Typo : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "InternalChat", (short)2 });

            migrationBuilder.UpdateData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)2, Language.English },
                column: "Info",
                value: "Writes intern to admins only.");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommandAlias",
                columns: new[] { "Alias", "Command" },
                values: new object[] { "InternalChat", (short)2 });

            migrationBuilder.UpdateData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)2, Language.English },
                column: "Info",
                value: "Writes internally to admins only.");
        }

        #endregion Protected Methods
    }
}

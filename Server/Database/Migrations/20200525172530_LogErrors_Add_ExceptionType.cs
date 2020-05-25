using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class LogErrors_Add_ExceptionType : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionType",
                table: "LogErrors");

            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "LogErrors",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "LogErrors",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionType",
                table: "LogErrors",
                nullable: true);
        }

        #endregion Protected Methods
    }
}

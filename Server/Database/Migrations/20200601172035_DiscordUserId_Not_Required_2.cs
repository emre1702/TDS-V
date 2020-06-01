using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class DiscordUserId_Not_Required_2 : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscordUserId",
                table: "PlayerSettings",
                type: "numeric(20,0)",
                nullable: true,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscordUserId",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldNullable: true,
                oldDefaultValue: 0m);
        }

        #endregion Protected Methods
    }
}

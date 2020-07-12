using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerThemeSettings_RemoveOpacity : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ThemeBackgroundAlphaPercentage",
                table: "PlayerThemeSettings",
                type: "real",
                nullable: false,
                defaultValue: 87f);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeBackgroundAlphaPercentage",
                table: "PlayerThemeSettings");
        }

        #endregion Protected Methods
    }
}

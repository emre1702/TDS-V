using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerSettings_Add_ThemeSettings : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeBackgroundAlphaPercentage",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ThemeBackgroundDarkColor",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ThemeBackgroundLightColor",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ThemeMainColor",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ThemeSecondaryColor",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ThemeWarnColor",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "UseDarkTheme",
                table: "PlayerSettings");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ThemeBackgroundAlphaPercentage",
                table: "PlayerSettings",
                maxLength: 50,
                nullable: false,
                defaultValue: 87f);

            migrationBuilder.AddColumn<string>(
                name: "ThemeBackgroundDarkColor",
                table: "PlayerSettings",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(48, 48, 48, 0.87)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeBackgroundLightColor",
                table: "PlayerSettings",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(250, 250, 250, 0.87)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeMainColor",
                table: "PlayerSettings",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(0,0,77,1)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeSecondaryColor",
                table: "PlayerSettings",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(255,152,0,1)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeWarnColor",
                table: "PlayerSettings",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(244,67,54,1)");

            migrationBuilder.AddColumn<bool>(
                name: "UseDarkTheme",
                table: "PlayerSettings",
                nullable: false,
                defaultValue: false);
        }

        #endregion Protected Methods
    }
}

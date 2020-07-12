using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_PlayerThemeSettings : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerThemeSettings");

            migrationBuilder.AddColumn<float>(
                name: "ThemeBackgroundAlphaPercentage",
                table: "PlayerSettings",
                type: "real",
                nullable: false,
                defaultValue: 87f);

            migrationBuilder.AddColumn<string>(
                name: "ThemeBackgroundDarkColor",
                table: "PlayerSettings",
                type: "text",
                nullable: true,
                defaultValue: "linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeBackgroundLightColor",
                table: "PlayerSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(250, 250, 250, 0.87)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeMainColor",
                table: "PlayerSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(0,0,77,1)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeSecondaryColor",
                table: "PlayerSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(255,152,0,1)");

            migrationBuilder.AddColumn<string>(
                name: "ThemeWarnColor",
                table: "PlayerSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(244,67,54,1)");

            migrationBuilder.AddColumn<bool>(
                name: "UseDarkTheme",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "PlayerThemeSettings",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    ThemeBackgroundAlphaPercentage = table.Column<float>(nullable: false, defaultValue: 87f),
                    ThemeBackgroundDarkColor = table.Column<string>(nullable: true, defaultValue: "linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)"),
                    ThemeBackgroundLightColor = table.Column<string>(nullable: true, defaultValue: "rgba(250, 250, 250, 0.87)"),
                    ThemeMainColor = table.Column<string>(nullable: true, defaultValue: "rgba(0,0,77,1)"),
                    ThemeSecondaryColor = table.Column<string>(nullable: true, defaultValue: "rgba(255,152,0,1)"),
                    ThemeWarnColor = table.Column<string>(nullable: true, defaultValue: "rgba(244,67,54,1)"),
                    ToolbarDesign = table.Column<int>(nullable: false, defaultValue: 1),
                    UseDarkTheme = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerThemeSettings", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerThemeSettings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        #endregion Protected Methods
    }
}

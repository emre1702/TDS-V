using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class DefaultWarnColor_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThemeWarnColor",
                table: "PlayerThemeSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgb(150,0,0)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "rgba(244,67,54,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeSecondaryColor",
                table: "PlayerThemeSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgb(255,152,0)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "rgba(255,152,0,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeMainColor",
                table: "PlayerThemeSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgb(0,0,77)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "rgba(0,0,77,1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThemeWarnColor",
                table: "PlayerThemeSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(244,67,54,1)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "rgb(150,0,0)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeSecondaryColor",
                table: "PlayerThemeSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(255,152,0,1)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "rgb(255,152,0)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeMainColor",
                table: "PlayerThemeSettings",
                type: "text",
                nullable: true,
                defaultValue: "rgba(0,0,77,1)",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "rgb(0,0,77)");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class UpdateThemeSettings : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThemeWarnColor",
                table: "PlayerSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(244,67,54,1)",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "rgba(244,67,54,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeSecondaryColor",
                table: "PlayerSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(255,152,0,1)",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "rgba(255,152,0,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeMainColor",
                table: "PlayerSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(0,0,77,1)",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "rgba(0,0,77,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeBackgroundLightColor",
                table: "PlayerSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(250, 250, 250, 0.87)",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "rgba(250, 250, 250, 0.87)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeBackgroundDarkColor",
                table: "PlayerSettings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "rgba(48, 48, 48, 0.87)",
                oldClrType: typeof(string),
                oldNullable: true,
                oldDefaultValue: "linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThemeWarnColor",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: "rgba(244,67,54,1)",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "rgba(244,67,54,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeSecondaryColor",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: "rgba(255,152,0,1)",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "rgba(255,152,0,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeMainColor",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: "rgba(0,0,77,1)",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "rgba(0,0,77,1)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeBackgroundLightColor",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: "rgba(250, 250, 250, 0.87)",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "rgba(250, 250, 250, 0.87)");

            migrationBuilder.AlterColumn<string>(
                name: "ThemeBackgroundDarkColor",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: "linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "rgba(48, 48, 48, 0.87)");
        }

        #endregion Protected Methods
    }
}

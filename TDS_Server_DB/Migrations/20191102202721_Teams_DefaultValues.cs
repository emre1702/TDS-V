using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Teams_DefaultValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "teams",
                maxLength: 100,
                nullable: false,
                defaultValue: "Spectator",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<short>(
                name: "ColorR",
                table: "teams",
                nullable: false,
                defaultValue: (short)255,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "ColorG",
                table: "teams",
                nullable: false,
                defaultValue: (short)255,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "ColorB",
                table: "teams",
                nullable: false,
                defaultValue: (short)255,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "BlipColor",
                table: "teams",
                nullable: false,
                defaultValue: (short)4,
                oldClrType: typeof(short),
                oldType: "smallint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "teams",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldDefaultValue: "Spectator");

            migrationBuilder.AlterColumn<short>(
                name: "ColorR",
                table: "teams",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: (short)255);

            migrationBuilder.AlterColumn<short>(
                name: "ColorG",
                table: "teams",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: (short)255);

            migrationBuilder.AlterColumn<short>(
                name: "ColorB",
                table: "teams",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: (short)255);

            migrationBuilder.AlterColumn<short>(
                name: "BlipColor",
                table: "teams",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: (short)4);
        }
    }
}

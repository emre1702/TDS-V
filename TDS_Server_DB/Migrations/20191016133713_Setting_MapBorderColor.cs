using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Setting_MapBorderColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "AllowDataTransfer",
                table: "player_settings",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MapBorderColor",
                table: "player_settings",
                nullable: true,
                defaultValue: "rgba(150,0,0,0.35)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapBorderColor",
                table: "player_settings");

            migrationBuilder.AlterColumn<bool>(
                name: "AllowDataTransfer",
                table: "player_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));
        }
    }
}

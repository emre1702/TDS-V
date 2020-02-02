using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerSettings_GangwarTargetWithoutAttackerMaxSeconds_DefaultValueTo10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "gangwar_target_without_attacker_max_seconds",
                table: "server_settings",
                nullable: false,
                defaultValue: 10,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "gangwar_target_without_attacker_max_seconds",
                table: "server_settings",
                type: "integer",
                nullable: false,
                defaultValue: 5,
                oldClrType: typeof(int),
                oldDefaultValue: 10);
        }
    }
}

using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Database.Migrations
{
    public partial class KillInfoSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "FreeroamDefaultVehicle",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PlayerKillInfoSettings",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    ShowIcon = table.Column<bool>(nullable: false),
                    FontWidth = table.Column<float>(nullable: false, defaultValue: 1.4f),
                    IconWidth = table.Column<int>(nullable: false, defaultValue: 60),
                    Spacing = table.Column<int>(nullable: false, defaultValue: 15)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerKillInfoSettings", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerKillInfoSettings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerKillInfoSettings");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "FreeroamDefaultVehicle",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true); 
        }
    }
}

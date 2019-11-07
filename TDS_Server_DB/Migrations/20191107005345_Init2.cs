using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "players_AdminLvl_fkey",
                table: "players");

            migrationBuilder.AddForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "players_AdminLvl_fkey",
                table: "players",
                column: "AdminLvl",
                principalTable: "admin_levels",
                principalColumn: "Level",
                onDelete: ReferentialAction.SetNull,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "players_AdminLvl_fkey",
                table: "players");

            migrationBuilder.AddForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "players_AdminLvl_fkey",
                table: "players",
                column: "AdminLvl",
                principalTable: "admin_levels",
                principalColumn: "Level",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

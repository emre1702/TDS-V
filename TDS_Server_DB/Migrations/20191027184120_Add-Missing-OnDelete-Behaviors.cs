using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class AddMissingOnDeleteBehaviors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commands_admin_levels",
                table: "commands");

            migrationBuilder.DropForeignKey(
                name: "player_bans_PlayerID_fkey",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players");

            migrationBuilder.AddForeignKey(
                name: "FK_commands_admin_levels",
                table: "commands",
                column: "NeededAdminLevel",
                principalTable: "admin_levels",
                principalColumn: "Level",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "player_bans_PlayerID_fkey",
                table: "player_bans",
                column: "PlayerId",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commands_admin_levels",
                table: "commands");

            migrationBuilder.DropForeignKey(
                name: "player_bans_PlayerID_fkey",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players");

            migrationBuilder.AddForeignKey(
                name: "FK_commands_admin_levels",
                table: "commands",
                column: "NeededAdminLevel",
                principalTable: "admin_levels",
                principalColumn: "Level",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "player_bans_PlayerID_fkey",
                table: "player_bans",
                column: "PlayerId",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

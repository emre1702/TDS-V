using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class FixPrimaryKeyLobbyWeapons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "lobby_weapons_pkey",
                table: "lobby_weapons");

            migrationBuilder.AddPrimaryKey(
                name: "lobby_weapons_pkey",
                table: "lobby_weapons",
                columns: new[] { "Hash", "Lobby" });

            migrationBuilder.CreateIndex(
                name: "IX_lobby_weapons_Hash",
                table: "lobby_weapons",
                column: "Hash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "lobby_weapons_pkey",
                table: "lobby_weapons");

            migrationBuilder.DropIndex(
                name: "IX_lobby_weapons_Hash",
                table: "lobby_weapons");

            migrationBuilder.AddPrimaryKey(
                name: "lobby_weapons_pkey",
                table: "lobby_weapons",
                column: "Hash");
        }
    }
}

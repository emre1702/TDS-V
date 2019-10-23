using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class LobbyWeapons_RemoveUniqueHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lobby_weapons_Hash",
                table: "lobby_weapons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_lobby_weapons_Hash",
                table: "lobby_weapons",
                column: "Hash",
                unique: true);
        }
    }
}

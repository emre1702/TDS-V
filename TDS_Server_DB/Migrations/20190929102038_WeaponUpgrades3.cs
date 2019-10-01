using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class WeaponUpgrades3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_player_weapon_components",
                table: "player_weapon_components");

            migrationBuilder.AddPrimaryKey(
                name: "PK_player_weapon_components",
                table: "player_weapon_components",
                columns: new[] { "PlayerID", "WeaponHash", "ComponentHash" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_player_weapon_components",
                table: "player_weapon_components");

            migrationBuilder.AddPrimaryKey(
                name: "PK_player_weapon_components",
                table: "player_weapon_components",
                columns: new[] { "PlayerID", "WeaponHash" });
        }
    }
}

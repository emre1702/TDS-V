using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class SeedData_LobbyWeapons_AddBullpuprifleMk2_AddSpecialCarbineMk2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "damage",
                table: "lobby_weapons",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "lobby_weapons",
                columns: new[] { "hash", "lobby", "ammo", "damage", "head_multiplicator" },
                values: new object[,]
                {
                    { WeaponHash.Bullpuprifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Specialcarbine_mk2, -1, 9999, null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "lobby_weapons",
                keyColumns: new[] { "hash", "lobby" },
                keyValues: new object[] { WeaponHash.Bullpuprifle_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "lobby_weapons",
                keyColumns: new[] { "hash", "lobby" },
                keyValues: new object[] { WeaponHash.Specialcarbine_mk2, -1 });

            migrationBuilder.AlterColumn<short>(
                name: "damage",
                table: "lobby_weapons",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}

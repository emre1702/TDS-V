using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server.Database.Migrations
{
    public partial class SeeData_Weapons_AddBullpuprifleMk2_AddSpecialCarbineMk2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Bullpuprifle_mk2, 32f, 1f, EWeaponType.AssaultRifle });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Specialcarbine_mk2, 32f, 1f, EWeaponType.AssaultRifle });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpuprifle_mk2);

            migrationBuilder.DeleteData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Specialcarbine_mk2);
        }
    }
}

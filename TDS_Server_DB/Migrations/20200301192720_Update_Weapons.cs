using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class Update_Weapons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Fireextinguisher,
                column: "type",
                value: EWeaponType.Rest);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Petrolcan,
                column: "type",
                value: EWeaponType.Rest);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bottle,
                column: "type",
                value: EWeaponType.Melee);

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Stone_hatchet, 50f, 1f, EWeaponType.Melee });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.CeramicPistol, 20f, 1f, EWeaponType.Handgun });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.NavyRevolver, 40f, 1f, EWeaponType.Handgun });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.HazardCan, 1f, EWeaponType.Rest });

            migrationBuilder.InsertData(
                table: "lobby_weapons",
                columns: new[] { "hash", "lobby", "ammo", "damage", "head_multiplicator" },
                values: new object[,]
                {
                    { WeaponHash.Stone_hatchet, -1, 9999, null, null },
                    { WeaponHash.CeramicPistol, -1, 9999, null, null },
                    { WeaponHash.NavyRevolver, -1, 9999, null, null },
                    { WeaponHash.HazardCan, -1, 9999, null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "lobby_weapons",
                keyColumns: new[] { "hash", "lobby" },
                keyValues: new object[] { WeaponHash.CeramicPistol, -1 });

            migrationBuilder.DeleteData(
                table: "lobby_weapons",
                keyColumns: new[] { "hash", "lobby" },
                keyValues: new object[] { WeaponHash.Stone_hatchet, -1 });

            migrationBuilder.DeleteData(
                table: "lobby_weapons",
                keyColumns: new[] { "hash", "lobby" },
                keyValues: new object[] { WeaponHash.NavyRevolver, -1 });

            migrationBuilder.DeleteData(
                table: "lobby_weapons",
                keyColumns: new[] { "hash", "lobby" },
                keyValues: new object[] { WeaponHash.HazardCan, -1 });

            migrationBuilder.DeleteData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.CeramicPistol);

            migrationBuilder.DeleteData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stone_hatchet);

            migrationBuilder.DeleteData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.NavyRevolver);

            migrationBuilder.DeleteData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.HazardCan);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Fireextinguisher,
                column: "type",
                value: EWeaponType.ThrownWeapon);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Petrolcan,
                column: "type",
                value: EWeaponType.ThrownWeapon);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bottle,
                column: "type",
                value: EWeaponType.ThrownWeapon);
        }
    }
}

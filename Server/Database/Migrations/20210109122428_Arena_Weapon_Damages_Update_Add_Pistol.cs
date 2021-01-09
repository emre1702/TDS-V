using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class Arena_Weapon_Damages_Update_Add_Pistol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Sawnoffshotgun, -1 });

            migrationBuilder.InsertData(
                table: "LobbyWeapons",
                columns: new[] { "Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator" },
                values: new object[] { WeaponHash.Pistol, -1, 9999, null, null });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Fireextinguisher,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Compactlauncher,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snowball,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Vintagepistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatpdw,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 15f, 1.5f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Autoshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Microsmg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Wrench,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 18f, 1.5f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pumpshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Appistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Ball,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Molotov,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.CeramicPistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smg,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 12f, 1.5f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stickybomb,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Petrolcan,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stungun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stone_hatchet,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultrifle_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavyshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Minigun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Golfclub,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Raycarbine,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flaregun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flare,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenadelauncher_smoke,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hammer,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pumpshotgun_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatpistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Gusenberg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Compactrifle,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hominglauncher,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Nightstick,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Railgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Sawnoffshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smg_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpuprifle,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Firework,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatmg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Carbinerifle,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Crowbar,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpuprifle_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snspistol_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flashlight,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Proximine,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.NavyRevolver,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Dagger,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenade,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Poolcue,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bat,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Specialcarbine_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Doubleaction,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol50,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 35f, 1.5f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Knife,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Mg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpupshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bzgas,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Unarmed,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenadelauncher,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Advancedrifle,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Raypistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Rpg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Rayminigun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pipebomb,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.HazardCan,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Minismg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snspistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultrifle,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 20f, 1.5f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Specialcarbine,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Revolver,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 60f, 1.5f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Revolver_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Battleaxe,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavypistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Knuckle,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Machinepistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatmg_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Marksmanpistol,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Machete,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Switchblade,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Dbshotgun,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultsmg,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hatchet,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bottle,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Carbinerifle_mk2,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Parachute,
                column: "HeadShotDamageModifier",
                value: 1.5f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smokegrenade,
                column: "HeadShotDamageModifier",
                value: 1.5f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Pistol, -1 });

            migrationBuilder.InsertData(
                table: "LobbyWeapons",
                columns: new[] { "Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator" },
                values: new object[] { WeaponHash.Sawnoffshotgun, -1, 9999, null, null });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Fireextinguisher,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Compactlauncher,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snowball,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Vintagepistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatpdw,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Autoshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Microsmg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Wrench,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 26f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pumpshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Appistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Ball,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Molotov,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.CeramicPistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smg,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stickybomb,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Petrolcan,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stungun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Stone_hatchet,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultrifle_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavyshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Minigun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Golfclub,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Raycarbine,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flaregun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flare,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenadelauncher_smoke,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hammer,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pumpshotgun_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatpistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Gusenberg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Compactrifle,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hominglauncher,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Nightstick,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Railgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Sawnoffshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smg_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpuprifle,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Firework,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatmg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Carbinerifle,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Crowbar,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpuprifle_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snspistol_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Flashlight,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Proximine,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.NavyRevolver,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Dagger,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenade,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Poolcue,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bat,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Specialcarbine_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Doubleaction,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol50,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 51f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Knife,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Mg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bullpupshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bzgas,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Unarmed,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Grenadelauncher,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Advancedrifle,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Raypistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Rpg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Rayminigun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pipebomb,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.HazardCan,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Minismg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Snspistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Pistol_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultrifle,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Specialcarbine,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Revolver,
                columns: new[] { "Damage", "HeadShotDamageModifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Revolver_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Battleaxe,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Heavypistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Knuckle,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Machinepistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Combatmg_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Marksmanpistol,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Machete,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Switchblade,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Dbshotgun,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Assaultsmg,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Hatchet,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Bottle,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Carbinerifle_mk2,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Parachute,
                column: "HeadShotDamageModifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "Weapons",
                keyColumn: "Hash",
                keyValue: WeaponHash.Smokegrenade,
                column: "HeadShotDamageModifier",
                value: 1f);
        }
    }
}

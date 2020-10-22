using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Arena_Weapons_Reducing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Sniperrifle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Fireextinguisher, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Compactlauncher, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Snowball, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Vintagepistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Heavysniper_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Heavysniper, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Autoshotgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Microsmg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Wrench, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Pistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Pumpshotgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Appistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Ball, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Molotov, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.CeramicPistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Stickybomb, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Petrolcan, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Stungun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Stone_hatchet, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Assaultrifle_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Heavyshotgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Minigun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Golfclub, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Raycarbine, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Flaregun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Flare, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Grenadelauncher_smoke, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Hammer, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Pumpshotgun_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Combatpistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Gusenberg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Compactrifle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Hominglauncher, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Nightstick, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Marksmanrifle_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Railgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Smg_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Bullpuprifle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Firework, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Combatmg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Carbinerifle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Crowbar, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Bullpuprifle_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Snspistol_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Flashlight, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Proximine, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.NavyRevolver, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Dagger, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Grenade, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Poolcue, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Bat, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Specialcarbine_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Doubleaction, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Knife, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Mg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Bullpupshotgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Bzgas, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Grenadelauncher, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Advancedrifle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Raypistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Rpg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Rayminigun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Pipebomb, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.HazardCan, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Minismg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Snspistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Pistol_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Specialcarbine, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Marksmanrifle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Revolver_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Battleaxe, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Heavypistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Knuckle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Machinepistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Combatmg_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Marksmanpistol, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Machete, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Switchblade, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Assaultshotgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Dbshotgun, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Assaultsmg, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Hatchet, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Bottle, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Carbinerifle_mk2, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Parachute, -1 });

            migrationBuilder.DeleteData(
                table: "LobbyWeapons",
                keyColumns: new[] { "Hash", "Lobby" },
                keyValues: new object[] { WeaponHash.Smokegrenade, -1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LobbyWeapons",
                columns: new[] { "Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator" },
                values: new object[,]
                {
                    { WeaponHash.Assaultshotgun, -1, 9999, null, null },
                    { WeaponHash.Specialcarbine, -1, 9999, null, null },
                    { WeaponHash.Revolver_mk2, -1, 9999, null, null },
                    { WeaponHash.Doubleaction, -1, 9999, null, null },
                    { WeaponHash.Marksmanrifle, -1, 9999, null, null },
                    { WeaponHash.Marksmanrifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Battleaxe, -1, 9999, null, null },
                    { WeaponHash.Heavypistol, -1, 9999, null, null },
                    { WeaponHash.Knuckle, -1, 9999, null, null },
                    { WeaponHash.Machinepistol, -1, 9999, null, null },
                    { WeaponHash.Marksmanpistol, -1, 9999, null, null },
                    { WeaponHash.Machete, -1, 9999, null, null },
                    { WeaponHash.Switchblade, -1, 9999, null, null },
                    { WeaponHash.Dbshotgun, -1, 9999, null, null },
                    { WeaponHash.NavyRevolver, -1, 9999, null, null },
                    { WeaponHash.Hatchet, -1, 9999, null, null },
                    { WeaponHash.Bottle, -1, 9999, null, null },
                    { WeaponHash.Parachute, -1, 9999, null, null },
                    { WeaponHash.Raypistol, -1, 9999, null, null },
                    { WeaponHash.Raycarbine, -1, 9999, null, null },
                    { WeaponHash.Carbinerifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Rayminigun, -1, 9999, null, null },
                    { WeaponHash.Bullpuprifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Specialcarbine_mk2, -1, 9999, null, null },
                    { WeaponHash.Smokegrenade, -1, 9999, null, null },
                    { WeaponHash.CeramicPistol, -1, 9999, null, null },
                    { WeaponHash.Assaultrifle_mk2, -1, 9999, null, null },
                    { WeaponHash.HazardCan, -1, 9999, null, null },
                    { WeaponHash.Stone_hatchet, -1, 9999, null, null },
                    { WeaponHash.Assaultsmg, -1, 9999, null, null },
                    { WeaponHash.Snspistol_mk2, -1, 9999, null, null },
                    { WeaponHash.Sniperrifle, -1, 9999, null, null },
                    { WeaponHash.Minismg, -1, 9999, null, null },
                    { WeaponHash.Flaregun, -1, 9999, null, null },
                    { WeaponHash.Golfclub, -1, 9999, null, null },
                    { WeaponHash.Fireextinguisher, -1, 9999, null, null },
                    { WeaponHash.Minigun, -1, 9999, null, null },
                    { WeaponHash.Compactlauncher, -1, 9999, null, null },
                    { WeaponHash.Snowball, -1, 9999, null, null },
                    { WeaponHash.Vintagepistol, -1, 9999, null, null },
                    { WeaponHash.Heavysniper, -1, 9999, null, null },
                    { WeaponHash.Heavysniper_mk2, -1, 9999, null, null },
                    { WeaponHash.Autoshotgun, -1, 9999, null, null },
                    { WeaponHash.Microsmg, -1, 9999, null, null },
                    { WeaponHash.Flare, -1, 9999, null, null },
                    { WeaponHash.Wrench, -1, 9999, null, null },
                    { WeaponHash.Pistol_mk2, -1, 9999, null, null },
                    { WeaponHash.Pumpshotgun, -1, 9999, null, null },
                    { WeaponHash.Pumpshotgun_mk2, -1, 9999, null, null },
                    { WeaponHash.Appistol, -1, 9999, null, null },
                    { WeaponHash.Ball, -1, 9999, null, null },
                    { WeaponHash.Molotov, -1, 9999, null, null },
                    { WeaponHash.Smg_mk2, -1, 9999, null, null },
                    { WeaponHash.Stickybomb, -1, 9999, null, null },
                    { WeaponHash.Petrolcan, -1, 9999, null, null },
                    { WeaponHash.Stungun, -1, 9999, null, null },
                    { WeaponHash.Heavyshotgun, -1, 9999, null, null },
                    { WeaponHash.Pistol, -1, 9999, null, null },
                    { WeaponHash.Grenadelauncher_smoke, -1, 9999, null, null },
                    { WeaponHash.Snspistol, -1, 9999, null, null },
                    { WeaponHash.Combatpistol, -1, 9999, null, null },
                    { WeaponHash.Hammer, -1, 9999, null, null },
                    { WeaponHash.Firework, -1, 9999, null, null },
                    { WeaponHash.Combatmg, -1, 9999, null, null },
                    { WeaponHash.Combatmg_mk2, -1, 9999, null, null },
                    { WeaponHash.Carbinerifle, -1, 9999, null, null },
                    { WeaponHash.Crowbar, -1, 9999, null, null },
                    { WeaponHash.Flashlight, -1, 9999, null, null },
                    { WeaponHash.Dagger, -1, 9999, null, null },
                    { WeaponHash.Grenade, -1, 9999, null, null },
                    { WeaponHash.Railgun, -1, 9999, null, null },
                    { WeaponHash.Poolcue, -1, 9999, null, null },
                    { WeaponHash.Knife, -1, 9999, null, null },
                    { WeaponHash.Mg, -1, 9999, null, null },
                    { WeaponHash.Bullpupshotgun, -1, 9999, null, null },
                    { WeaponHash.Bzgas, -1, 9999, null, null },
                    { WeaponHash.Grenadelauncher, -1, 9999, null, null },
                    { WeaponHash.Proximine, -1, 9999, null, null },
                    { WeaponHash.Advancedrifle, -1, 9999, null, null },
                    { WeaponHash.Rpg, -1, 9999, null, null },
                    { WeaponHash.Pipebomb, -1, 9999, null, null },
                    { WeaponHash.Bat, -1, 9999, null, null },
                    { WeaponHash.Nightstick, -1, 9999, null, null },
                    { WeaponHash.Bullpuprifle, -1, 9999, null, null },
                    { WeaponHash.Compactrifle, -1, 9999, null, null },
                    { WeaponHash.Hominglauncher, -1, 9999, null, null },
                    { WeaponHash.Gusenberg, -1, 9999, null, null }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class BonusbotSettings_Add_BansInfoChannelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "bans_info_channel_id",
                table: "bonusbot_settings",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "bonusbot_settings",
                keyColumn: "id",
                keyValue: 1,
                column: "bans_info_channel_id",
                value: 659705941771550730m);

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -3,
                column: "spawn_again_after_death_ms",
                value: 400);

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -2,
                column: "spawn_again_after_death_ms",
                value: 400);

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -1,
                column: "spawn_again_after_death_ms",
                value: 400);

            migrationBuilder.UpdateData(
                table: "lobby_map_settings",
                keyColumn: "lobby_id",
                keyValue: -1,
                column: "map_limit_time",
                value: 10);

            migrationBuilder.UpdateData(
                table: "lobby_round_settings",
                keyColumn: "lobby_id",
                keyValue: -1,
                columns: new[] { "bomb_defuse_time_ms", "bomb_detonate_time_ms", "bomb_plant_time_ms", "countdown_time", "round_time" },
                values: new object[] { 8000, 45000, 3000, 5, 240 });

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "id",
                keyValue: (short)1,
                columns: new[] { "give_money_fee", "give_money_min_amount", "killing_spree_max_seconds_until_next_kill", "map_rating_amount_for_check", "min_map_rating_for_new_maps", "multiplier_ranking_assists", "multiplier_ranking_damage", "multiplier_ranking_kills", "nametag_max_distance" },
                values: new object[] { 0.05f, 100, 18, 10, 3f, 25f, 1f, 75f, 80f });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -5,
                columns: new[] { "blip_color", "color_b", "color_g", "color_r", "name" },
                values: new object[] { (short)4, (short)255, (short)255, (short)255, "None" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -4,
                columns: new[] { "blip_color", "color_r", "name", "skin_hash" },
                values: new object[] { (short)1, (short)150, "Terrorist", 275618457 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -3,
                columns: new[] { "blip_color", "color_g", "name", "skin_hash" },
                values: new object[] { (short)52, (short)150, "SWAT", -1920001264 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -2,
                columns: new[] { "blip_color", "color_b", "color_g", "color_r", "name" },
                values: new object[] { (short)4, (short)255, (short)255, (short)255, "Spectator" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -1,
                columns: new[] { "blip_color", "color_b", "color_g", "color_r", "name", "skin_hash" },
                values: new object[] { (short)4, (short)255, (short)255, (short)255, "Spectator", 1004114196 });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SniperRifle,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.FireExtinguisher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CompactGrenadeLauncher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Snowball,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.VintagePistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatPDW,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavySniperMk2,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavySniper,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SweeperShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MicroSMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Wrench,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Pistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PumpShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.APPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Baseball,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Molotov,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.StickyBomb,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PetrolCan,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.StunGun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultRifleMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Minigun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.GolfClub,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.UnholyHellbringer,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.FlareGun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Flare,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.GrenadeLauncherSmoke,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Hammer,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PumpShotgunMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Gusenberg,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CompactRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HomingLauncher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Nightstick,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MarksmanRifleMk2,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Railgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SawnOffShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SMGMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BullpupRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Firework,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CarbineRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Crowbar,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SNSPistolMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Flashlight,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Dagger,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Grenade,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PoolCue,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Bat,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.DoubleActionRevolver,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Pistol50,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Knife,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BullpupShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BZGas,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Unarmed,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.GrenadeLauncher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.NightVision,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Musket,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.ProximityMine,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AdvancedRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.UpnAtomizer,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.RPG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Widowmaker,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PipeBomb,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MiniSMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SNSPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PistolMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SpecialCarbine,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyRevolver,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MarksmanRifle,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyRevolverMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BattleAxe,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.KnuckleDuster,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MachinePistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatMGMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MarksmanPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Machete,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SwitchBlade,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.DoubleBarrelShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultSMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Hatchet,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Bottle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CarbineRifleMK2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Parachute,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SmokeGrenade,
                column: "default_head_multiplicator",
                value: 1f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bans_info_channel_id",
                table: "bonusbot_settings");

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -3,
                column: "spawn_again_after_death_ms",
                value: 400);

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -2,
                column: "spawn_again_after_death_ms",
                value: 400);

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -1,
                column: "spawn_again_after_death_ms",
                value: 400);

            migrationBuilder.UpdateData(
                table: "lobby_map_settings",
                keyColumn: "lobby_id",
                keyValue: -1,
                column: "map_limit_time",
                value: 10);

            migrationBuilder.UpdateData(
                table: "lobby_round_settings",
                keyColumn: "lobby_id",
                keyValue: -1,
                columns: new[] { "bomb_defuse_time_ms", "bomb_detonate_time_ms", "bomb_plant_time_ms", "countdown_time", "round_time" },
                values: new object[] { 8000, 45000, 3000, 5, 240 });

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "id",
                keyValue: (short)1,
                columns: new[] { "give_money_fee", "give_money_min_amount", "killing_spree_max_seconds_until_next_kill", "map_rating_amount_for_check", "min_map_rating_for_new_maps", "multiplier_ranking_assists", "multiplier_ranking_damage", "multiplier_ranking_kills", "nametag_max_distance" },
                values: new object[] { 0.05f, 100, 18, 10, 3f, 25f, 1f, 75f, 80f });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -5,
                columns: new[] { "blip_color", "color_b", "color_g", "color_r", "name" },
                values: new object[] { (short)4, (short)255, (short)255, (short)255, "None" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -4,
                columns: new[] { "blip_color", "color_r", "name", "skin_hash" },
                values: new object[] { (short)1, (short)150, "Terrorist", 275618457 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -3,
                columns: new[] { "blip_color", "color_g", "name", "skin_hash" },
                values: new object[] { (short)52, (short)150, "SWAT", -1920001264 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -2,
                columns: new[] { "blip_color", "color_b", "color_g", "color_r", "name" },
                values: new object[] { (short)4, (short)255, (short)255, (short)255, "Spectator" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -1,
                columns: new[] { "blip_color", "color_b", "color_g", "color_r", "name", "skin_hash" },
                values: new object[] { (short)4, (short)255, (short)255, (short)255, "Spectator", 1004114196 });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SniperRifle,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.FireExtinguisher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CompactGrenadeLauncher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Snowball,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.VintagePistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatPDW,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavySniperMk2,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavySniper,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SweeperShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MicroSMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Wrench,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Pistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PumpShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.APPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Baseball,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Molotov,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.StickyBomb,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PetrolCan,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.StunGun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultRifleMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Minigun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.GolfClub,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.UnholyHellbringer,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.FlareGun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Flare,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.GrenadeLauncherSmoke,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Hammer,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PumpShotgunMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Gusenberg,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CompactRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HomingLauncher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Nightstick,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MarksmanRifleMk2,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Railgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SawnOffShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SMGMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BullpupRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Firework,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CarbineRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Crowbar,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SNSPistolMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Flashlight,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Dagger,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Grenade,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PoolCue,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Bat,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.DoubleActionRevolver,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Pistol50,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Knife,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BullpupShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BZGas,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Unarmed,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.GrenadeLauncher,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.NightVision,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Musket,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.ProximityMine,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AdvancedRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.UpnAtomizer,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.RPG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Widowmaker,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PipeBomb,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MiniSMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SNSPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.PistolMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultRifle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SpecialCarbine,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyRevolver,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MarksmanRifle,
                column: "default_head_multiplicator",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyRevolverMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.BattleAxe,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.HeavyPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.KnuckleDuster,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MachinePistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CombatMGMk2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.MarksmanPistol,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Machete,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SwitchBlade,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.DoubleBarrelShotgun,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.AssaultSMG,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Hatchet,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Bottle,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.CarbineRifleMK2,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.Parachute,
                column: "default_head_multiplicator",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: EWeaponHash.SmokeGrenade,
                column: "default_head_multiplicator",
                value: 1f);
        }
    }
}

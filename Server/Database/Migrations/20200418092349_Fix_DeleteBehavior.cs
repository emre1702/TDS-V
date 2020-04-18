using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Fix_DeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admin_level_names_admin_levels_level_navigation_level",
                table: "admin_level_names");

            migrationBuilder.DropForeignKey(
                name: "fk_application_answers_applications_application_id",
                table: "application_answers");

            migrationBuilder.DropForeignKey(
                name: "fk_application_answers_application_questions_question_id",
                table: "application_answers");

            migrationBuilder.DropForeignKey(
                name: "fk_application_invitations_players_admin_id",
                table: "application_invitations");

            migrationBuilder.DropForeignKey(
                name: "fk_application_invitations_applications_application_id",
                table: "application_invitations");

            migrationBuilder.DropForeignKey(
                name: "fk_application_questions_players_admin_id",
                table: "application_questions");

            migrationBuilder.DropForeignKey(
                name: "fk_applications_players_player_id",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "fk_command_alias_commands_command_navigation_id",
                table: "command_alias");

            migrationBuilder.DropForeignKey(
                name: "fk_command_infos_commands_id_navigation_id",
                table: "command_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_commands_admin_levels_needed_admin_level_navigation_level",
                table: "commands");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_members_gangs_gang_id",
                table: "gang_members");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_members_players_player_id",
                table: "gang_members");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_rank_permissions_gangs_gang_id",
                table: "gang_rank_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_ranks_gangs_gang_id",
                table: "gang_ranks");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_stats_gangs_gang_id",
                table: "gang_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_gangs_gang_houses_house_id",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "fk_gangs_players_owner_id",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "fk_gangs_teams_team_id",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "fk_gangwar_areas_maps_map_id",
                table: "gangwar_areas");

            migrationBuilder.DropForeignKey(
                name: "fk_gangwar_areas_gangs_owner_gang_id",
                table: "gangwar_areas");

            migrationBuilder.DropForeignKey(
                name: "fk_killingspree_rewards_lobbies_lobby_id",
                table: "killingspree_rewards");

            migrationBuilder.DropForeignKey(
                name: "fk_lobbies_players_owner_id",
                table: "lobbies");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_fight_settings_lobbies_lobby_id",
                table: "lobby_fight_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_map_settings_lobbies_lobby_id",
                table: "lobby_map_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_maps_lobbies_lobby_id",
                table: "lobby_maps");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_maps_maps_map_id",
                table: "lobby_maps");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_rewards_lobbies_lobby_id",
                table: "lobby_rewards");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_round_settings_lobbies_lobby_id",
                table: "lobby_round_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_weapons_weapons_hash",
                table: "lobby_weapons");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_weapons_lobbies_lobby_navigation_id",
                table: "lobby_weapons");

            migrationBuilder.DropForeignKey(
                name: "fk_maps_players_creator_id",
                table: "maps");

            migrationBuilder.DropForeignKey(
                name: "fk_offlinemessages_players_source_id",
                table: "offlinemessages");

            migrationBuilder.DropForeignKey(
                name: "fk_offlinemessages_players_target_id",
                table: "offlinemessages");

            migrationBuilder.DropForeignKey(
                name: "fk_player_bans_players_admin_id",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "fk_player_bans_lobbies_lobby_id",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "fk_player_bans_players_player_id",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "fk_player_challenges_players_player_id",
                table: "player_challenges");

            migrationBuilder.DropForeignKey(
                name: "fk_player_clothes_players_player_id",
                table: "player_clothes");

            migrationBuilder.DropForeignKey(
                name: "fk_player_lobby_stats_lobbies_lobby_id",
                table: "player_lobby_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_player_lobby_stats_players_player_id",
                table: "player_lobby_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_favourites_maps_map_id",
                table: "player_map_favourites");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_favourites_players_player_id",
                table: "player_map_favourites");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_ratings_maps_map_id",
                table: "player_map_ratings");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_ratings_players_player_id",
                table: "player_map_ratings");

            migrationBuilder.DropForeignKey(
                name: "fk_player_relations_players_player_id",
                table: "player_relations");

            migrationBuilder.DropForeignKey(
                name: "fk_player_relations_players_target_id",
                table: "player_relations");

            migrationBuilder.DropForeignKey(
                name: "fk_player_settings_players_player_id",
                table: "player_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_player_stats_players_player_id",
                table: "player_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_player_total_stats_players_player_id",
                table: "player_total_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_players_players_admin_leader_id",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "fk_players_admin_levels_admin_lvl_navigation_level",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "fk_rule_texts_rules_rule_id",
                table: "rule_texts");

            migrationBuilder.DropForeignKey(
                name: "fk_support_request_messages_players_author_id",
                table: "support_request_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_support_request_messages_support_requests_request_id",
                table: "support_request_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_players_author_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_teams_lobbies_lobby_navigation_id",
                table: "teams");

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Kills, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 150, 75 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Assists, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100, 50 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Damage, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100000, 20000 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.PlayTime, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1500, 300 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.RoundPlayed, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100, 50 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BombDefuse, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BombPlant, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Killstreak, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 7, 3 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BuyMaps, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 500, 500 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ReviewMaps, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 10, 10 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ReadTheRules, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ReadTheFAQ, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ChangeSettings, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.JoinDiscordServer, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.WriteHelpfulIssue, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.CreatorOfAcceptedMap, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BeHelpfulEnough, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

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
                columns: new[] { "blip_color", "name" },
                values: new object[] { (byte)4, "None" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -4,
                columns: new[] { "blip_color", "name", "skin_hash" },
                values: new object[] { (byte)1, "Terrorist", 275618457 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -3,
                columns: new[] { "blip_color", "name", "skin_hash" },
                values: new object[] { (byte)52, "SWAT", -1920001264 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -2,
                columns: new[] { "blip_color", "name" },
                values: new object[] { (byte)4, "Spectator" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -1,
                columns: new[] { "blip_color", "name", "skin_hash" },
                values: new object[] { (byte)4, "Spectator", 1004114196 });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Sniperrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 101f, 1000f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Fireextinguisher,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Compactlauncher,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Snowball,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 10f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Vintagepistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 34f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatpdw,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavysniper_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 216f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavysniper,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 216f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Autoshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 162f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Microsmg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 21f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Wrench,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 26f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pumpshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 58f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Appistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Ball,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Molotov,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 10f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.CeramicPistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 20f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Smg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stickybomb,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Petrolcan,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stungun,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stone_hatchet,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultrifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavyshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 117f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Minigun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Golfclub,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Raycarbine,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 23f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Flaregun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Flare,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Grenadelauncher_smoke,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Hammer,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pumpshotgun_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 58f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatpistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 27f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Gusenberg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 34f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Compactrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 34f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Hominglauncher,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 150f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Nightstick,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 35f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Marksmanrifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 65f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Railgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Sawnoffshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 160f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Smg_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpuprifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Firework,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatmg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Carbinerifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Crowbar,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpuprifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Snspistol_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Flashlight,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Proximine,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.NavyRevolver,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Dagger,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 45f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Grenade,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Poolcue,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bat,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Specialcarbine_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Doubleaction,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pistol50,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 51f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Knife,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 45f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Mg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpupshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 112f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bzgas,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Unarmed,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 15f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Grenadelauncher,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Musket,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 165f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Advancedrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Raypistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 80f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Rpg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Rayminigun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pipebomb,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.HazardCan,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Minismg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Snspistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pistol_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 26f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Specialcarbine,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Revolver,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Marksmanrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 65f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Revolver_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Battleaxe,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavypistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Knuckle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Machinepistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 20f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatmg_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Marksmanpistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 150f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Machete,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 45f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Switchblade,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 192f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Dbshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 166f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultsmg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 23f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Hatchet,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bottle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 10f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Carbinerifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Parachute,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Smokegrenade,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.AddForeignKey(
                name: "fk_admin_level_names_admin_levels_level_navigation_level",
                table: "admin_level_names",
                column: "level",
                principalTable: "admin_levels",
                principalColumn: "level",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_application_answers_applications_application_id",
                table: "application_answers",
                column: "application_id",
                principalTable: "applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_application_answers_application_questions_question_id",
                table: "application_answers",
                column: "question_id",
                principalTable: "application_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_application_invitations_players_admin_id",
                table: "application_invitations",
                column: "admin_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_application_invitations_applications_application_id",
                table: "application_invitations",
                column: "application_id",
                principalTable: "applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_application_questions_players_admin_id",
                table: "application_questions",
                column: "admin_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_applications_players_player_id",
                table: "applications",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_command_alias_commands_command_navigation_id",
                table: "command_alias",
                column: "command",
                principalTable: "commands",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_command_infos_commands_id_navigation_id",
                table: "command_infos",
                column: "id",
                principalTable: "commands",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_commands_admin_levels_needed_admin_level_navigation_level",
                table: "commands",
                column: "needed_admin_level",
                principalTable: "admin_levels",
                principalColumn: "level",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_members_gangs_gang_id",
                table: "gang_members",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_members_players_player_id",
                table: "gang_members",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_rank_permissions_gangs_gang_id",
                table: "gang_rank_permissions",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_ranks_gangs_gang_id",
                table: "gang_ranks",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_stats_gangs_gang_id",
                table: "gang_stats",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gangs_gang_houses_house_id",
                table: "gangs",
                column: "house_id",
                principalTable: "gang_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_gangs_players_owner_id",
                table: "gangs",
                column: "owner_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_gangs_teams_team_id",
                table: "gangs",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gangwar_areas_maps_map_id",
                table: "gangwar_areas",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gangwar_areas_gangs_owner_gang_id",
                table: "gangwar_areas",
                column: "owner_gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_killingspree_rewards_lobbies_lobby_id",
                table: "killingspree_rewards",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobbies_players_owner_id",
                table: "lobbies",
                column: "owner_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_fight_settings_lobbies_lobby_id",
                table: "lobby_fight_settings",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_map_settings_lobbies_lobby_id",
                table: "lobby_map_settings",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_maps_lobbies_lobby_id",
                table: "lobby_maps",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_maps_maps_map_id",
                table: "lobby_maps",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_rewards_lobbies_lobby_id",
                table: "lobby_rewards",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_round_settings_lobbies_lobby_id",
                table: "lobby_round_settings",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_weapons_weapons_hash",
                table: "lobby_weapons",
                column: "hash",
                principalTable: "weapons",
                principalColumn: "hash",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_weapons_lobbies_lobby_navigation_id",
                table: "lobby_weapons",
                column: "lobby",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_maps_players_creator_id",
                table: "maps",
                column: "creator_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_offlinemessages_players_source_id",
                table: "offlinemessages",
                column: "source_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_offlinemessages_players_target_id",
                table: "offlinemessages",
                column: "target_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_bans_players_admin_id",
                table: "player_bans",
                column: "admin_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_player_bans_lobbies_lobby_id",
                table: "player_bans",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_bans_players_player_id",
                table: "player_bans",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_player_challenges_players_player_id",
                table: "player_challenges",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_clothes_players_player_id",
                table: "player_clothes",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_lobby_stats_lobbies_lobby_id",
                table: "player_lobby_stats",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_lobby_stats_players_player_id",
                table: "player_lobby_stats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_favourites_maps_map_id",
                table: "player_map_favourites",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_favourites_players_player_id",
                table: "player_map_favourites",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_ratings_maps_map_id",
                table: "player_map_ratings",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_ratings_players_player_id",
                table: "player_map_ratings",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_relations_players_player_id",
                table: "player_relations",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_relations_players_target_id",
                table: "player_relations",
                column: "target_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_settings_players_player_id",
                table: "player_settings",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_stats_players_player_id",
                table: "player_stats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_player_total_stats_players_player_id",
                table: "player_total_stats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_players_players_admin_leader_id",
                table: "players",
                column: "admin_leader_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_players_admin_levels_admin_lvl_navigation_level",
                table: "players",
                column: "admin_lvl",
                principalTable: "admin_levels",
                principalColumn: "level",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_rule_texts_rules_rule_id",
                table: "rule_texts",
                column: "rule_id",
                principalTable: "rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_request_messages_players_author_id",
                table: "support_request_messages",
                column: "author_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_request_messages_support_requests_request_id",
                table: "support_request_messages",
                column: "request_id",
                principalTable: "support_requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_players_author_id",
                table: "support_requests",
                column: "author_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_teams_lobbies_lobby_navigation_id",
                table: "teams",
                column: "lobby",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_admin_level_names_admin_levels_level_navigation_level",
                table: "admin_level_names");

            migrationBuilder.DropForeignKey(
                name: "fk_application_answers_applications_application_id",
                table: "application_answers");

            migrationBuilder.DropForeignKey(
                name: "fk_application_answers_application_questions_question_id",
                table: "application_answers");

            migrationBuilder.DropForeignKey(
                name: "fk_application_invitations_players_admin_id",
                table: "application_invitations");

            migrationBuilder.DropForeignKey(
                name: "fk_application_invitations_applications_application_id",
                table: "application_invitations");

            migrationBuilder.DropForeignKey(
                name: "fk_application_questions_players_admin_id",
                table: "application_questions");

            migrationBuilder.DropForeignKey(
                name: "fk_applications_players_player_id",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "fk_command_alias_commands_command_navigation_id",
                table: "command_alias");

            migrationBuilder.DropForeignKey(
                name: "fk_command_infos_commands_id_navigation_id",
                table: "command_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_commands_admin_levels_needed_admin_level_navigation_level",
                table: "commands");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_members_gangs_gang_id",
                table: "gang_members");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_members_players_player_id",
                table: "gang_members");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_rank_permissions_gangs_gang_id",
                table: "gang_rank_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_ranks_gangs_gang_id",
                table: "gang_ranks");

            migrationBuilder.DropForeignKey(
                name: "fk_gang_stats_gangs_gang_id",
                table: "gang_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_gangs_gang_houses_house_id",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "fk_gangs_players_owner_id",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "fk_gangs_teams_team_id",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "fk_gangwar_areas_maps_map_id",
                table: "gangwar_areas");

            migrationBuilder.DropForeignKey(
                name: "fk_gangwar_areas_gangs_owner_gang_id",
                table: "gangwar_areas");

            migrationBuilder.DropForeignKey(
                name: "fk_killingspree_rewards_lobbies_lobby_id",
                table: "killingspree_rewards");

            migrationBuilder.DropForeignKey(
                name: "fk_lobbies_players_owner_id",
                table: "lobbies");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_fight_settings_lobbies_lobby_id",
                table: "lobby_fight_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_map_settings_lobbies_lobby_id",
                table: "lobby_map_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_maps_lobbies_lobby_id",
                table: "lobby_maps");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_maps_maps_map_id",
                table: "lobby_maps");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_rewards_lobbies_lobby_id",
                table: "lobby_rewards");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_round_settings_lobbies_lobby_id",
                table: "lobby_round_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_weapons_weapons_hash",
                table: "lobby_weapons");

            migrationBuilder.DropForeignKey(
                name: "fk_lobby_weapons_lobbies_lobby_navigation_id",
                table: "lobby_weapons");

            migrationBuilder.DropForeignKey(
                name: "fk_maps_players_creator_id",
                table: "maps");

            migrationBuilder.DropForeignKey(
                name: "fk_offlinemessages_players_source_id",
                table: "offlinemessages");

            migrationBuilder.DropForeignKey(
                name: "fk_offlinemessages_players_target_id",
                table: "offlinemessages");

            migrationBuilder.DropForeignKey(
                name: "fk_player_bans_players_admin_id",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "fk_player_bans_lobbies_lobby_id",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "fk_player_bans_players_player_id",
                table: "player_bans");

            migrationBuilder.DropForeignKey(
                name: "fk_player_challenges_players_player_id",
                table: "player_challenges");

            migrationBuilder.DropForeignKey(
                name: "fk_player_clothes_players_player_id",
                table: "player_clothes");

            migrationBuilder.DropForeignKey(
                name: "fk_player_lobby_stats_lobbies_lobby_id",
                table: "player_lobby_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_player_lobby_stats_players_player_id",
                table: "player_lobby_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_favourites_maps_map_id",
                table: "player_map_favourites");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_favourites_players_player_id",
                table: "player_map_favourites");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_ratings_maps_map_id",
                table: "player_map_ratings");

            migrationBuilder.DropForeignKey(
                name: "fk_player_map_ratings_players_player_id",
                table: "player_map_ratings");

            migrationBuilder.DropForeignKey(
                name: "fk_player_relations_players_player_id",
                table: "player_relations");

            migrationBuilder.DropForeignKey(
                name: "fk_player_relations_players_target_id",
                table: "player_relations");

            migrationBuilder.DropForeignKey(
                name: "fk_player_settings_players_player_id",
                table: "player_settings");

            migrationBuilder.DropForeignKey(
                name: "fk_player_stats_players_player_id",
                table: "player_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_player_total_stats_players_player_id",
                table: "player_total_stats");

            migrationBuilder.DropForeignKey(
                name: "fk_players_players_admin_leader_id",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "fk_players_admin_levels_admin_lvl_navigation_level",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "fk_rule_texts_rules_rule_id",
                table: "rule_texts");

            migrationBuilder.DropForeignKey(
                name: "fk_support_request_messages_players_author_id",
                table: "support_request_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_support_request_messages_support_requests_request_id",
                table: "support_request_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_support_requests_players_author_id",
                table: "support_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_teams_lobbies_lobby_navigation_id",
                table: "teams");

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Kills, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 150, 75 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Assists, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100, 50 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Damage, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100000, 20000 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.PlayTime, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1500, 300 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.RoundPlayed, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100, 50 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BombDefuse, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BombPlant, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 10, 5 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.Killstreak, ChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 7, 3 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BuyMaps, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 500, 500 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ReviewMaps, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 10, 10 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ReadTheRules, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ReadTheFAQ, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.ChangeSettings, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.JoinDiscordServer, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.WriteHelpfulIssue, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.CreatorOfAcceptedMap, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { ChallengeType.BeHelpfulEnough, ChallengeFrequency.Forever },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 1, 1 });

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
                columns: new[] { "blip_color", "name" },
                values: new object[] { (byte)4, "None" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -4,
                columns: new[] { "blip_color", "name", "skin_hash" },
                values: new object[] { (byte)1, "Terrorist", 275618457 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -3,
                columns: new[] { "blip_color", "name", "skin_hash" },
                values: new object[] { (byte)52, "SWAT", -1920001264 });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -2,
                columns: new[] { "blip_color", "name" },
                values: new object[] { (byte)4, "Spectator" });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "id",
                keyValue: -1,
                columns: new[] { "blip_color", "name", "skin_hash" },
                values: new object[] { (byte)4, "Spectator", 1004114196 });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Sniperrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 101f, 1000f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Fireextinguisher,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Compactlauncher,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Snowball,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 10f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Vintagepistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 34f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatpdw,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavysniper_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 216f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavysniper,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 216f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Autoshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 162f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Microsmg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 21f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Wrench,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 26f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pumpshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 58f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Appistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Ball,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Molotov,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 10f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.CeramicPistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 20f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Smg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stickybomb,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Petrolcan,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stungun,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Stone_hatchet,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultrifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavyshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 117f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Minigun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Golfclub,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Raycarbine,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 23f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Flaregun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Flare,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Grenadelauncher_smoke,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Hammer,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pumpshotgun_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 58f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatpistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 27f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Gusenberg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 34f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Compactrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 34f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Hominglauncher,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 150f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Nightstick,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 35f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Marksmanrifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 65f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Railgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Sawnoffshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 160f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Smg_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpuprifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Firework,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatmg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Carbinerifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Crowbar,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpuprifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Snspistol_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Flashlight,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Proximine,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.NavyRevolver,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Dagger,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 45f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Grenade,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Poolcue,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bat,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Specialcarbine_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Doubleaction,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pistol50,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 51f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Knife,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 45f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Mg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bullpupshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 112f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bzgas,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Unarmed,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 15f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Grenadelauncher,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Musket,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 165f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Advancedrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Raypistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 80f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Rpg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Rayminigun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pipebomb,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 100f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.HazardCan,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Minismg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 22f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Snspistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Pistol_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 26f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Specialcarbine,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Revolver,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Marksmanrifle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 65f, 2f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Revolver_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 110f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Battleaxe,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Heavypistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 40f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Knuckle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 30f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Machinepistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 20f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Combatmg_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 28f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Marksmanpistol,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 150f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Machete,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 45f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Switchblade,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 192f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Dbshotgun,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 166f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Assaultsmg,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 23f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Hatchet,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 50f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Bottle,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 10f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Carbinerifle_mk2,
                columns: new[] { "damage", "head_shot_damage_modifier" },
                values: new object[] { 32f, 1f });

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Parachute,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "weapons",
                keyColumn: "hash",
                keyValue: WeaponHash.Smokegrenade,
                column: "head_shot_damage_modifier",
                value: 1f);

            migrationBuilder.AddForeignKey(
                name: "fk_admin_level_names_admin_levels_level_navigation_level",
                table: "admin_level_names",
                column: "level",
                principalTable: "admin_levels",
                principalColumn: "level",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_application_answers_applications_application_id",
                table: "application_answers",
                column: "application_id",
                principalTable: "applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_application_answers_application_questions_question_id",
                table: "application_answers",
                column: "question_id",
                principalTable: "application_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_application_invitations_players_admin_id",
                table: "application_invitations",
                column: "admin_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_application_invitations_applications_application_id",
                table: "application_invitations",
                column: "application_id",
                principalTable: "applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_application_questions_players_admin_id",
                table: "application_questions",
                column: "admin_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_applications_players_player_id",
                table: "applications",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_command_alias_commands_command_navigation_id",
                table: "command_alias",
                column: "command",
                principalTable: "commands",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_command_infos_commands_id_navigation_id",
                table: "command_infos",
                column: "id",
                principalTable: "commands",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_commands_admin_levels_needed_admin_level_navigation_level",
                table: "commands",
                column: "needed_admin_level",
                principalTable: "admin_levels",
                principalColumn: "level",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_members_gangs_gang_id",
                table: "gang_members",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_members_players_player_id",
                table: "gang_members",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_rank_permissions_gangs_gang_id",
                table: "gang_rank_permissions",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_ranks_gangs_gang_id",
                table: "gang_ranks",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gang_stats_gangs_gang_id",
                table: "gang_stats",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gangs_gang_houses_house_id",
                table: "gangs",
                column: "house_id",
                principalTable: "gang_houses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gangs_players_owner_id",
                table: "gangs",
                column: "owner_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gangs_teams_team_id",
                table: "gangs",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gangwar_areas_maps_map_id",
                table: "gangwar_areas",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gangwar_areas_gangs_owner_gang_id",
                table: "gangwar_areas",
                column: "owner_gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_killingspree_rewards_lobbies_lobby_id",
                table: "killingspree_rewards",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobbies_players_owner_id",
                table: "lobbies",
                column: "owner_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_fight_settings_lobbies_lobby_id",
                table: "lobby_fight_settings",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_map_settings_lobbies_lobby_id",
                table: "lobby_map_settings",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_maps_lobbies_lobby_id",
                table: "lobby_maps",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_maps_maps_map_id",
                table: "lobby_maps",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_rewards_lobbies_lobby_id",
                table: "lobby_rewards",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_round_settings_lobbies_lobby_id",
                table: "lobby_round_settings",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_weapons_weapons_hash",
                table: "lobby_weapons",
                column: "hash",
                principalTable: "weapons",
                principalColumn: "hash",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lobby_weapons_lobbies_lobby_navigation_id",
                table: "lobby_weapons",
                column: "lobby",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_maps_players_creator_id",
                table: "maps",
                column: "creator_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_offlinemessages_players_source_id",
                table: "offlinemessages",
                column: "source_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_offlinemessages_players_target_id",
                table: "offlinemessages",
                column: "target_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_bans_players_admin_id",
                table: "player_bans",
                column: "admin_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_bans_lobbies_lobby_id",
                table: "player_bans",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_bans_players_player_id",
                table: "player_bans",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_challenges_players_player_id",
                table: "player_challenges",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_clothes_players_player_id",
                table: "player_clothes",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_lobby_stats_lobbies_lobby_id",
                table: "player_lobby_stats",
                column: "lobby_id",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_lobby_stats_players_player_id",
                table: "player_lobby_stats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_favourites_maps_map_id",
                table: "player_map_favourites",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_favourites_players_player_id",
                table: "player_map_favourites",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_ratings_maps_map_id",
                table: "player_map_ratings",
                column: "map_id",
                principalTable: "maps",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_map_ratings_players_player_id",
                table: "player_map_ratings",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_relations_players_player_id",
                table: "player_relations",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_relations_players_target_id",
                table: "player_relations",
                column: "target_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_settings_players_player_id",
                table: "player_settings",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_stats_players_player_id",
                table: "player_stats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_total_stats_players_player_id",
                table: "player_total_stats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_players_players_admin_leader_id",
                table: "players",
                column: "admin_leader_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_players_admin_levels_admin_lvl_navigation_level",
                table: "players",
                column: "admin_lvl",
                principalTable: "admin_levels",
                principalColumn: "level",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_rule_texts_rules_rule_id",
                table: "rule_texts",
                column: "rule_id",
                principalTable: "rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_request_messages_players_author_id",
                table: "support_request_messages",
                column: "author_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_request_messages_support_requests_request_id",
                table: "support_request_messages",
                column: "request_id",
                principalTable: "support_requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_support_requests_players_author_id",
                table: "support_requests",
                column: "author_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_teams_lobbies_lobby_navigation_id",
                table: "teams",
                column: "lobby",
                principalTable: "lobbies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

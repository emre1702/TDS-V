using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Database.Migrations
{
    public partial class DbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .Annotation("Npgsql:Enum:challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .Annotation("Npgsql:Enum:freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:language", "german,english")
                .Annotation("Npgsql:Enum:lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby")
                .Annotation("Npgsql:Enum:log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto,remove_map,voice_mute")
                .Annotation("Npgsql:Enum:map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .Annotation("Npgsql:Enum:player_relation", "none,block,friend")
                .Annotation("Npgsql:Enum:rule_category", "general,chat")
                .Annotation("Npgsql:Enum:rule_target", "user,admin,vip")
                .Annotation("Npgsql:Enum:support_type", "question,help,compliment,complaint")
                .Annotation("Npgsql:Enum:userpanel_admin_question_answer_type", "text,check,number")
                .Annotation("Npgsql:Enum:vehicle_hash", "chimera,carbonrs,hermes,virgo3,hotknife,tiptruck,faggio2,youga,glendale,dominator,rebla,bf400,kalahari,trophytruck,coquette,btype,boxville3,baller2,ardent,miljet,phantom3,freightcar,elegy,speedo4,cheetah2,sabregt2,speeder,reaper,buffalo3,freightcont2,tempesta,dinghy2,tropic,gburrito2,stryder,hexer,dynasty,stafford,crusader,cogcabrio,vacca,formula,pbus2,gauntlet2,chino,polmav,proptrailer,cargoplane,slamvan5,surano,hauler2,cliffhanger,raketrailer,jb7002,squalo,turismor,outlaw,avenger2,kanjo,kuruma2,infernus,nightshark,speeder2,boxville4,handler,imperator,volatol,cruiser,policet,tornado,lguard,baller5,mixer2,lynx,huntley,fusilade,swinger,dinghy3,utillitruck,voodoo2,boattrailer,moonbeam,dune2,zr380,khamelion,apc,packer,tankercar,hustler,luxor,romero,fcr,issi4,barracks3,banshee2,baller4,lectro,freightgrain,comet5,baller6,bruiser,cerberus2,boxville5,contender,drafter,surfer,cog552,supervolito,trailersmall,ruston,gauntlet3,dukes,speedo2,slamvan,sadler2,buffalo2,vagrant,gargoyle,pranger,sovereign,frogger,buzzard2,ztype,alpha,submersible,ruiner3,rhino,coquette3,sanchez,buzzard,sheava,defiler,slamvan2,annihilator,z190,monster4,rhapsody,bjxl,biff,jetmax,dinghy4,pariah,metrotrain,sultan2,sentinel2,habanero,oppressor,stromberg,intruder,utillitruck2,burrito4,savestra,mule,cog55,toro2,mesa,rrocket,xa21,freightcont1,issi3,ruiner2,casco,pony2,furia,duster,hydra,sultan,tampa,sugoi,voltic2,sandking2,impaler2,coquette2,graintrailer,youga2,freight,tezeract,ninef,dinghy,nero,nokota,blista2,tula,thrax,skylift,michelli,jb700,jet,bobcatxl,toro,barracks2,specter2,swift2,velum2,pigalle,nero2,verlierer2,sentinel3,hotring,asbo,slamvan3,superd,bfinjection,fbi,tribike,bmx,burrito5,caddy,rumpo,ambulance,dubsta,technical2,akula,xls,seminole,marshall,gp1,issi6,caracara,hakuchou,landstalker,bestiagts,deviant,airbus,zhaba,emerus,serrano,vestra,oracle,sentinel,sc1,flatbed,technical3,warrener,cyclone,cargobob3,paragon2,fmj,tractor3,tropic2,vstr,tulip,rumpo3,deluxo,paradise,thruster,schafter4,sanctus,forklift,trailerlarge,torero,picador,hauler,tornado2,issi5,scarab2,valkyrie2,stinger,brioso,airtug,comet4,molotok,windsor,deveste,fagaloa,cargobob2,imperator2,tractor,rancherxl,pounder2,t20,akuma,dilettante2,strikeforce,yosemite2,monster3,stratum,rapidgt2,bison3,streiter,slamvan6,stockade,enduro,tornado3,dloader,washington,mower,tr3,diablous2,besra,peyote,thrust,retinue,issi7,zion3,yosemite,ratbike,camper,baller3,specter,bulldozer,tropos,moonbeam2,dune3,fugitive,gb200,cerberus3,police3,trash,schafter6,sheriff2,stalion,rancherxl2,gauntlet4,firetruk,vagner,tourbus,mule4,frogger2,taco,tanker2,titan,osiris,daemon,voodoo,cavalcade,trailerlogs,futo,cargobob4,retinue2,esskey,brutus3,menacer,police,rapidgt3,benson,tyrus,lurcher,oppressor2,insurgent2,bison2,carbonizzare,tr2,tr4,pounder,prototipo,utillitruck3,rocoto,brutus,bagger,docktrailer,phantom,dump,blazer,manana,stunt,entity2,faction,avenger,avarus,guardian,rallytruck,tug,stingergt,technical,impaler,phoenix,gt500,tractor2,coach,mesa3,slamvan4,trailers3,mule3,italigtb,rebel2,bruiser3,primo2,faction3,tornado4,cognoscenti,comet3,pbus,feltzer2,terbyte,boxville,havok,police4,stretch,formula2,nightshade,rapidgt,windsor2,impaler3,insurgent3,wastelander,asterope,surge,brutus2,premier,emperor2,trailersmall2,insurgent,neon,volatus,faggio,pfister811,novak,deathbike2,asea,asea2,peyote2,gauntlet,tornado5,faction2,policeold2,rumpo2,granger,tvtrailer,microlight,penetrator,seven70,everon,mammatus,gburrito,impaler4,burrito3,rubble,starling,scrap,bullet,bruiser2,riot2,sabregt,sheriff,velum,supervolito2,double,dune,mamba,maverick,radi,phantom2,fbi2,armytrailer2,police2,voltic,neo,nightblade,valkyrie,blazer5,trailers2,feltzer3,clique,tornado6,gresley,policeold1,riata,raiden,alphaz1,manchez,schafter3,brawler,zr3803,stanier,armytrailer,ninef2,sanchez2,prairie,bodhi2,khanjali,infernus2,daemon2,zentorno,pyro,dominator5,deathbike3,kuruma,chino2,vindicator,trflat,caracara2,burrito,towtruck,surfer2,cheetah,jester,nimbus,dominator6,entityxf,ingot,faggio3,lazer,blazer3,ellie,flashgt,trash2,schafter2,minitank,vigilante,emperor3,dubsta3,tribike2,le7b,adder,shamal,luxor2,tampa3,rebel,armytanker,blade,riot,zion2,sandking,issi2,toros,primo,scarab,fq2,taipan,imorgon,dilettante,minivan2,zion,jester2,zr3802,trailers4,rentalbus,furoregt,tampa2,submersible2,mule2,comet2,marquis,banshee,seashark,buccaneer2,zombiea,tailgater,howard,cutter,visione,cheburek,dominator3,turismo2,rogue,cablecar,taxi,tiptruck2,locust,dominator2,pcj,burrito2,dodo,virgo2,ruffian,bati2,schafter5,docktug,nebula,trailers,ripley,monster,fixter,komoda,btype2,dune4,vigero,barracks,speedo,baller,patriot,cerberus,cavalcade2,mixer,freighttrailer,omnis,caddy3,fcr2,imperator3,mogul,mesa2,schwarzer,tanker,seasparrow,monster5,bus,chernobog,dominator4,emperor,buccaneer,zorrusso,raptor,ratloader,krieger,trophytruck2,cuban800,scramjet,nemesis,massacro2,jackal,wolfsbane,seashark2,blimp2,vortex,cognoscenti2,btype3,sadler,blista3,f620,ratloader2,scarab3,zombieb,elegy2,caddy2,oracle2,schlagen,virgo,predator,italigtb2,paragon,towtruck2,blazer4,monroe,xls2,panto,patriot2,revolter,shotaro,stalion2,tribike3,baletrailer,dubsta2,seabreeze,viseris,felon,penumbra,tyrant,hellion,bifta,blista,swift,italigto,dukes2,s80,autarch,dune5,seashark3,minivan,blimp3,brickade,buffalo,sultanrs,rcbandito,suntrap,hakuchou2,diablous,boxville2,ruiner,jester3,stockade3,barrage,jugular,scorcher,innovation,blimp,massacro,vader,kamacho,journey,pony,limo2,bati,felon2,savage,freecrawler,cargobob,vamos,blazer2,hunter,policeb,bombushka,halftrack,deathbike,bison,regina,exemplar")
                .Annotation("Npgsql:Enum:weapon_hash", "sniperrifle,fireextinguisher,compactlauncher,snowball,vintagepistol,combatpdw,heavysniper_mk2,heavysniper,autoshotgun,microsmg,wrench,pistol,pumpshotgun,appistol,ball,molotov,ceramic_pistol,smg,stickybomb,petrolcan,stungun,stone_hatchet,assaultrifle_mk2,heavyshotgun,minigun,golfclub,raycarbine,flaregun,flare,grenadelauncher_smoke,hammer,pumpshotgun_mk2,combatpistol,gusenberg,compactrifle,hominglauncher,nightstick,marksmanrifle_mk2,railgun,sawnoffshotgun,smg_mk2,bullpuprifle,firework,combatmg,carbinerifle,crowbar,bullpuprifle_mk2,snspistol_mk2,flashlight,proximine,navy_revolver,dagger,grenade,poolcue,bat,specialcarbine_mk2,doubleaction,pistol50,knife,mg,bullpupshotgun,bzgas,unarmed,grenadelauncher,musket,advancedrifle,raypistol,rpg,rayminigun,pipebomb,hazard_can,minismg,snspistol,pistol_mk2,assaultrifle,specialcarbine,revolver,marksmanrifle,revolver_mk2,battleaxe,heavypistol,knuckle,machinepistol,combatmg_mk2,marksmanpistol,machete,switchblade,assaultshotgun,dbshotgun,assaultsmg,hatchet,bottle,carbinerifle_mk2,parachute,smokegrenade")
                .Annotation("Npgsql:Enum:weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .Annotation("Npgsql:PostgresExtension:tsm_system_rows", ",,");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "admin_levels",
                columns: table => new
                {
                    level = table.Column<short>(nullable: false),
                    color_r = table.Column<short>(nullable: false),
                    color_g = table.Column<short>(nullable: false),
                    color_b = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admin_levels", x => x.level);
                });

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_announcements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bonusbot_settings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guild_id = table.Column<decimal>(nullable: true),
                    admin_applications_channel_id = table.Column<decimal>(nullable: true),
                    server_infos_channel_id = table.Column<decimal>(nullable: true),
                    support_requests_channel_id = table.Column<decimal>(nullable: true),
                    actions_info_channel_id = table.Column<decimal>(nullable: true),
                    bans_info_channel_id = table.Column<decimal>(nullable: true),
                    error_logs_channel_id = table.Column<decimal>(nullable: true),
                    send_private_message_on_ban = table.Column<bool>(nullable: false),
                    send_private_message_on_offline_message = table.Column<bool>(nullable: false),
                    refresh_server_stats_frequency_sec = table.Column<int>(nullable: false, defaultValue: 60)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bonusbot_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "challenge_settings",
                columns: table => new
                {
                    type = table.Column<ChallengeType>(nullable: false),
                    frequency = table.Column<ChallengeFrequency>(nullable: false),
                    min_number = table.Column<int>(nullable: false, defaultValue: 1),
                    max_number = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_challenge_settings", x => new { x.type, x.frequency });
                });

            migrationBuilder.CreateTable(
                name: "fa_qs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    language = table.Column<Language>(nullable: false),
                    question = table.Column<string>(nullable: true),
                    answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fa_qs", x => new { x.id, x.language });
                });

            migrationBuilder.CreateTable(
                name: "freeroam_default_vehicle",
                columns: table => new
                {
                    vehicle_type = table.Column<FreeroamVehicleType>(nullable: false),
                    vehicle_hash = table.Column<VehicleHash>(nullable: false),
                    note = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_freeroam_default_vehicle", x => x.vehicle_type);
                });

            migrationBuilder.CreateTable(
                name: "log_admins",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    type = table.Column<LogType>(nullable: false),
                    source = table.Column<int>(nullable: false),
                    target = table.Column<int>(nullable: true),
                    lobby = table.Column<int>(nullable: true),
                    as_donator = table.Column<bool>(nullable: false),
                    as_vip = table.Column<bool>(nullable: false),
                    reason = table.Column<string>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    length_or_end_time = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_admins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_chats",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    source = table.Column<int>(nullable: false),
                    target = table.Column<int>(nullable: true),
                    message = table.Column<string>(nullable: false),
                    lobby = table.Column<int>(nullable: true),
                    is_admin_chat = table.Column<bool>(nullable: false),
                    is_team_chat = table.Column<bool>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_chats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_errors",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    source = table.Column<int>(nullable: true),
                    info = table.Column<string>(nullable: false),
                    stack_trace = table.Column<string>(nullable: true),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_errors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_kills",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    killer_id = table.Column<int>(nullable: false),
                    dead_id = table.Column<int>(nullable: false),
                    weapon_id = table.Column<long>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_kills", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_rests",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    type = table.Column<LogType>(nullable: false),
                    source = table.Column<int>(nullable: false),
                    serial = table.Column<string>(maxLength: 200, nullable: true),
                    ip = table.Column<IPAddress>(nullable: true),
                    lobby = table.Column<int>(nullable: true),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_rests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    target = table.Column<RuleTarget>(nullable: false),
                    category = table.Column<RuleCategory>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "server_daily_stats",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    player_peak = table.Column<short>(nullable: false, defaultValue: (short)0),
                    arena_rounds_played = table.Column<int>(nullable: false, defaultValue: 0),
                    custom_arena_rounds_played = table.Column<int>(nullable: false, defaultValue: 0),
                    amount_logins = table.Column<int>(nullable: false, defaultValue: 0),
                    amount_registrations = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_daily_stats", x => x.date);
                });

            migrationBuilder.CreateTable(
                name: "server_settings",
                columns: table => new
                {
                    id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gamemode_name = table.Column<string>(maxLength: 50, nullable: false),
                    error_to_player_on_non_existent_command = table.Column<bool>(nullable: false),
                    to_chat_on_non_existent_command = table.Column<bool>(nullable: false),
                    distance_to_spot_to_plant = table.Column<float>(nullable: false),
                    distance_to_spot_to_defuse = table.Column<float>(nullable: false),
                    save_player_data_cooldown_minutes = table.Column<int>(nullable: false),
                    save_logs_cooldown_minutes = table.Column<int>(nullable: false),
                    save_seasons_cooldown_minutes = table.Column<int>(nullable: false),
                    team_order_cooldown_ms = table.Column<int>(nullable: false),
                    arena_new_map_probability_percent = table.Column<float>(nullable: false),
                    killing_spree_max_seconds_until_next_kill = table.Column<int>(nullable: false, defaultValue: 18),
                    map_rating_amount_for_check = table.Column<int>(nullable: false, defaultValue: 10),
                    min_map_rating_for_new_maps = table.Column<float>(nullable: false, defaultValue: 3f),
                    give_money_fee = table.Column<float>(nullable: false, defaultValue: 0.05f),
                    give_money_min_amount = table.Column<int>(nullable: false, defaultValue: 100),
                    nametag_max_distance = table.Column<float>(nullable: false, defaultValue: 625f),
                    show_nametag_only_on_aiming = table.Column<bool>(nullable: false),
                    multiplier_ranking_kills = table.Column<float>(nullable: false, defaultValue: 75f),
                    multiplier_ranking_assists = table.Column<float>(nullable: false, defaultValue: 25f),
                    multiplier_ranking_damage = table.Column<float>(nullable: false, defaultValue: 1f),
                    close_application_after_days = table.Column<int>(nullable: false, defaultValue: 7),
                    delete_application_after_days = table.Column<int>(nullable: false, defaultValue: 14),
                    gangwar_preparation_time = table.Column<long>(nullable: false, defaultValue: 180L),
                    gangwar_action_time = table.Column<long>(nullable: false, defaultValue: 900L),
                    delete_requests_days_after_close = table.Column<long>(nullable: false, defaultValue: 30L),
                    delete_offline_messages_after_days = table.Column<int>(nullable: false, defaultValue: 60),
                    min_players_online_for_gangwar = table.Column<int>(nullable: false, defaultValue: 3),
                    gangwar_area_attack_cooldown_minutes = table.Column<int>(nullable: false, defaultValue: 60),
                    amount_players_allowed_in_gangwar_team_before_count_check = table.Column<int>(nullable: false, defaultValue: 3),
                    gangwar_attacker_can_be_more = table.Column<bool>(nullable: false, defaultValue: true),
                    gangwar_owner_can_be_more = table.Column<bool>(nullable: false, defaultValue: false),
                    gangwar_target_radius = table.Column<double>(nullable: false, defaultValue: 5.0),
                    gangwar_target_without_attacker_max_seconds = table.Column<int>(nullable: false, defaultValue: 10),
                    reduce_maps_bought_counter_after_minute = table.Column<int>(nullable: false, defaultValue: 60),
                    map_buy_base_price = table.Column<int>(nullable: false, defaultValue: 1000),
                    map_buy_counter_multiplicator = table.Column<float>(nullable: false, defaultValue: 1f),
                    username_change_cost = table.Column<int>(nullable: false, defaultValue: 20000),
                    username_change_cooldown_days = table.Column<int>(nullable: false, defaultValue: 60),
                    amount_weekly_challenges = table.Column<int>(nullable: false, defaultValue: 3),
                    reload_server_bans_every_minutes = table.Column<int>(nullable: false, defaultValue: 5)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "server_total_stats",
                columns: table => new
                {
                    id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_peak = table.Column<short>(nullable: false, defaultValue: (short)0),
                    arena_rounds_played = table.Column<long>(nullable: false, defaultValue: 0L),
                    custom_arena_rounds_played = table.Column<long>(nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_server_total_stats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    hash = table.Column<WeaponHash>(nullable: false),
                    type = table.Column<WeaponType>(nullable: false),
                    clip_size = table.Column<int>(nullable: false, defaultValue: 0),
                    min_head_shot_distance = table.Column<float>(nullable: false, defaultValue: 0f),
                    max_head_shot_distance = table.Column<float>(nullable: false, defaultValue: 0f),
                    head_shot_damage_modifier = table.Column<float>(nullable: false, defaultValue: 0f),
                    damage = table.Column<float>(nullable: false, defaultValue: 0f),
                    hit_limbs_damage_modifier = table.Column<float>(nullable: false, defaultValue: 0f),
                    reload_time = table.Column<float>(nullable: false, defaultValue: 0f),
                    time_between_shots = table.Column<float>(nullable: false, defaultValue: 0f),
                    range = table.Column<float>(nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_weapons", x => x.hash);
                });

            migrationBuilder.CreateTable(
                name: "admin_level_names",
                columns: table => new
                {
                    level = table.Column<short>(nullable: false),
                    language = table.Column<Language>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admin_level_names", x => new { x.level, x.language });
                    table.ForeignKey(
                        name: "fk_admin_level_names_admin_levels_level_navigation_level",
                        column: x => x.level,
                        principalTable: "admin_levels",
                        principalColumn: "level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commands",
                columns: table => new
                {
                    id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    command = table.Column<string>(maxLength: 50, nullable: false),
                    needed_admin_level = table.Column<short>(nullable: true),
                    needed_donation = table.Column<short>(nullable: true),
                    vip_can_use = table.Column<bool>(nullable: false),
                    lobby_owner_can_use = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_commands", x => x.id);
                    table.ForeignKey(
                        name: "fk_commands_admin_levels_needed_admin_level_navigation_level",
                        column: x => x.needed_admin_level,
                        principalTable: "admin_levels",
                        principalColumn: "level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sc_name = table.Column<string>(maxLength: 255, nullable: false),
                    sc_id = table.Column<decimal>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    password = table.Column<string>(maxLength: 100, nullable: false),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    admin_lvl = table.Column<short>(nullable: false, defaultValue: (short)0),
                    admin_leader_id = table.Column<int>(nullable: true),
                    is_vip = table.Column<bool>(nullable: false),
                    donation = table.Column<short>(nullable: false, defaultValue: (short)0),
                    register_timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                    table.ForeignKey(
                        name: "fk_players_players_admin_leader_id",
                        column: x => x.admin_leader_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_players_admin_levels_admin_lvl_navigation_level",
                        column: x => x.admin_lvl,
                        principalTable: "admin_levels",
                        principalColumn: "level",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "rule_texts",
                columns: table => new
                {
                    rule_id = table.Column<int>(nullable: false),
                    language = table.Column<Language>(nullable: false),
                    rule_str = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rule_texts", x => new { x.rule_id, x.language });
                    table.ForeignKey(
                        name: "fk_rule_texts_rules_rule_id",
                        column: x => x.rule_id,
                        principalTable: "rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "command_alias",
                columns: table => new
                {
                    alias = table.Column<string>(maxLength: 100, nullable: false),
                    command = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_command_alias", x => new { x.alias, x.command });
                    table.ForeignKey(
                        name: "fk_command_alias_commands_command_navigation_id",
                        column: x => x.command,
                        principalTable: "commands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "command_infos",
                columns: table => new
                {
                    id = table.Column<short>(nullable: false),
                    language = table.Column<Language>(nullable: false),
                    info = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_command_infos", x => new { x.id, x.language });
                    table.ForeignKey(
                        name: "fk_command_infos_commands_id_navigation_id",
                        column: x => x.id,
                        principalTable: "commands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_questions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    admin_id = table.Column<int>(nullable: false),
                    question = table.Column<string>(nullable: true),
                    answer_type = table.Column<UserpanelAdminQuestionAnswerType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_questions_players_admin_id",
                        column: x => x.admin_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    player_id = table.Column<int>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_applications", x => x.id);
                    table.ForeignKey(
                        name: "fk_applications_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobbies",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    owner_id = table.Column<int>(nullable: false),
                    type = table.Column<LobbyType>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    password = table.Column<string>(maxLength: 100, nullable: true),
                    default_spawn_x = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    default_spawn_y = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    default_spawn_z = table.Column<float>(nullable: false, defaultValueSql: "9000"),
                    around_spawn_point = table.Column<float>(nullable: false, defaultValueSql: "3"),
                    default_spawn_rotation = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    is_temporary = table.Column<bool>(nullable: false),
                    is_official = table.Column<bool>(nullable: false),
                    create_timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobbies", x => x.id);
                    table.ForeignKey(
                        name: "fk_lobbies_players_owner_id",
                        column: x => x.owner_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(nullable: false),
                    creator_id = table.Column<int>(nullable: true),
                    create_timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maps", x => x.id);
                    table.ForeignKey(
                        name: "fk_maps_players_creator_id",
                        column: x => x.creator_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "offlinemessages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    target_id = table.Column<int>(nullable: false),
                    source_id = table.Column<int>(nullable: false),
                    message = table.Column<string>(nullable: false),
                    seen = table.Column<bool>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offlinemessages", x => x.id);
                    table.ForeignKey(
                        name: "fk_offlinemessages_players_source_id",
                        column: x => x.source_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_offlinemessages_players_target_id",
                        column: x => x.target_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_challenges",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    challenge = table.Column<ChallengeType>(nullable: false),
                    frequency = table.Column<ChallengeFrequency>(nullable: false),
                    amount = table.Column<int>(nullable: false, defaultValue: 1),
                    current_amount = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_challenges", x => new { x.player_id, x.challenge, x.frequency });
                    table.ForeignKey(
                        name: "fk_player_challenges_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_clothes",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    is_male = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_clothes", x => x.player_id);
                    table.ForeignKey(
                        name: "fk_player_clothes_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_relations",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    target_id = table.Column<int>(nullable: false),
                    relation = table.Column<PlayerRelation>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_relations", x => new { x.player_id, x.target_id });
                    table.ForeignKey(
                        name: "fk_player_relations_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_relations_players_target_id",
                        column: x => x.target_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_settings",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    language = table.Column<Language>(nullable: false, defaultValue: Language.English),
                    allow_data_transfer = table.Column<bool>(nullable: false),
                    show_confetti_at_ranking = table.Column<bool>(nullable: false),
                    timezone = table.Column<string>(nullable: true, defaultValue: "UTC"),
                    date_time_format = table.Column<string>(nullable: true, defaultValue: "yyyy'-'MM'-'dd HH':'mm':'ss"),
                    discord_user_id = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    hitsound = table.Column<bool>(nullable: false),
                    bloodscreen = table.Column<bool>(nullable: false),
                    floating_damage_info = table.Column<bool>(nullable: false),
                    check_afk = table.Column<bool>(nullable: false, defaultValue: true),
                    voice3d = table.Column<bool>(nullable: false, defaultValue: false),
                    voice_auto_volume = table.Column<bool>(nullable: false, defaultValue: false),
                    voice_volume = table.Column<float>(nullable: false, defaultValue: 6f),
                    map_border_color = table.Column<string>(nullable: true, defaultValue: "rgba(150,0,0,0.35)"),
                    nametag_dead_color = table.Column<string>(nullable: true, defaultValue: "rgba(0, 0, 0, 1)"),
                    nametag_health_empty_color = table.Column<string>(nullable: true, defaultValue: "rgba(50, 0, 0, 1)"),
                    nametag_health_full_color = table.Column<string>(nullable: true, defaultValue: "rgba(0, 255, 0, 1)"),
                    nametag_armor_empty_color = table.Column<string>(nullable: true),
                    nametag_armor_full_color = table.Column<string>(nullable: true, defaultValue: "rgba(255, 255, 255, 1)"),
                    bloodscreen_cooldown_ms = table.Column<int>(nullable: false, defaultValue: 150),
                    hud_ammo_update_cooldown_ms = table.Column<int>(nullable: false, defaultValue: 100),
                    hud_health_update_cooldown_ms = table.Column<int>(nullable: false, defaultValue: 100),
                    afk_kick_after_seconds = table.Column<int>(nullable: false, defaultValue: 25),
                    afk_kick_show_warning_last_seconds = table.Column<int>(nullable: false, defaultValue: 10),
                    show_floating_damage_info_duration_ms = table.Column<int>(nullable: false, defaultValue: 1000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_settings", x => x.player_id);
                    table.ForeignKey(
                        name: "fk_player_settings_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_stats",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    money = table.Column<int>(nullable: false),
                    play_time = table.Column<int>(nullable: false),
                    mute_time = table.Column<int>(nullable: true),
                    voice_mute_time = table.Column<int>(nullable: true),
                    logged_in = table.Column<bool>(nullable: false),
                    last_login_timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    maps_bought_counter = table.Column<int>(nullable: false, defaultValue: 1),
                    last_maps_bought_counter_reduce = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    last_free_username_change = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_stats", x => x.player_id);
                    table.ForeignKey(
                        name: "fk_player_stats_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_total_stats",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    money = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_total_stats", x => x.player_id);
                    table.ForeignKey(
                        name: "fk_player_total_stats_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_requests",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    author_id = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 100, nullable: true),
                    type = table.Column<SupportType>(nullable: false),
                    atleast_admin_level = table.Column<int>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    close_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_support_requests_players_author_id",
                        column: x => x.author_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_answers",
                columns: table => new
                {
                    application_id = table.Column<int>(nullable: false),
                    question_id = table.Column<int>(nullable: false),
                    answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_answers", x => new { x.application_id, x.question_id });
                    table.ForeignKey(
                        name: "fk_application_answers_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_application_answers_application_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "application_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_invitations",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    application_id = table.Column<int>(nullable: false),
                    admin_id = table.Column<int>(nullable: false),
                    message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_invitations", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_invitations_players_admin_id",
                        column: x => x.admin_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_application_invitations_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "killingspree_rewards",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    kills_amount = table.Column<short>(nullable: false),
                    health_or_armor = table.Column<short>(nullable: true),
                    only_health = table.Column<short>(nullable: true),
                    only_armor = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_killingspree_rewards", x => new { x.lobby_id, x.kills_amount });
                    table.ForeignKey(
                        name: "fk_killingspree_rewards_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_fight_settings",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    start_health = table.Column<short>(nullable: false, defaultValue: (short)100),
                    start_armor = table.Column<short>(nullable: false, defaultValue: (short)100),
                    amount_lifes = table.Column<short>(nullable: false),
                    spawn_again_after_death_ms = table.Column<int>(nullable: false, defaultValue: 400)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobby_fight_settings", x => x.lobby_id);
                    table.ForeignKey(
                        name: "fk_lobby_fight_settings_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_map_settings",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    map_limit_time = table.Column<int>(nullable: false, defaultValueSql: "10"),
                    map_limit_type = table.Column<MapLimitType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobby_map_settings", x => x.lobby_id);
                    table.ForeignKey(
                        name: "fk_lobby_map_settings_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_rewards",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    money_per_kill = table.Column<double>(nullable: false),
                    money_per_assist = table.Column<double>(nullable: false),
                    money_per_damage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobby_rewards", x => x.lobby_id);
                    table.ForeignKey(
                        name: "fk_lobby_rewards_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_round_settings",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    round_time = table.Column<int>(nullable: false, defaultValueSql: "240"),
                    countdown_time = table.Column<int>(nullable: false, defaultValueSql: "5"),
                    bomb_detonate_time_ms = table.Column<int>(nullable: false, defaultValueSql: "45000"),
                    bomb_defuse_time_ms = table.Column<int>(nullable: false, defaultValueSql: "8000"),
                    bomb_plant_time_ms = table.Column<int>(nullable: false, defaultValueSql: "3000"),
                    mix_teams_after_round = table.Column<bool>(nullable: false),
                    show_ranking = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobby_round_settings", x => x.lobby_id);
                    table.ForeignKey(
                        name: "fk_lobby_round_settings_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_weapons",
                columns: table => new
                {
                    hash = table.Column<WeaponHash>(nullable: false),
                    lobby = table.Column<int>(nullable: false),
                    ammo = table.Column<int>(nullable: false),
                    damage = table.Column<float>(nullable: true),
                    head_multiplicator = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobby_weapons", x => new { x.hash, x.lobby });
                    table.ForeignKey(
                        name: "fk_lobby_weapons_weapons_hash",
                        column: x => x.hash,
                        principalTable: "weapons",
                        principalColumn: "hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_lobby_weapons_lobbies_lobby_navigation_id",
                        column: x => x.lobby,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_bans",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    lobby_id = table.Column<int>(nullable: false),
                    admin_id = table.Column<int>(nullable: true),
                    ip = table.Column<string>(nullable: true),
                    serial = table.Column<string>(nullable: true),
                    sc_name = table.Column<string>(nullable: true),
                    sc_id = table.Column<decimal>(nullable: true),
                    reason = table.Column<string>(nullable: false),
                    start_timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    end_timestamp = table.Column<DateTime>(nullable: true),
                    prevent_connection = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_bans", x => new { x.player_id, x.lobby_id });
                    table.ForeignKey(
                        name: "fk_player_bans_players_admin_id",
                        column: x => x.admin_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_player_bans_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_bans_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "player_lobby_stats",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    lobby_id = table.Column<int>(nullable: false),
                    kills = table.Column<int>(nullable: false),
                    assists = table.Column<int>(nullable: false),
                    deaths = table.Column<int>(nullable: false),
                    damage = table.Column<int>(nullable: false),
                    total_kills = table.Column<int>(nullable: false),
                    total_assists = table.Column<int>(nullable: false),
                    total_deaths = table.Column<int>(nullable: false),
                    total_damage = table.Column<int>(nullable: false),
                    total_rounds = table.Column<int>(nullable: false),
                    most_kills_in_a_round = table.Column<int>(nullable: false),
                    most_damage_in_a_round = table.Column<int>(nullable: false),
                    most_assists_in_a_round = table.Column<int>(nullable: false),
                    total_maps_bought = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_lobby_stats", x => new { x.player_id, x.lobby_id });
                    table.ForeignKey(
                        name: "fk_player_lobby_stats_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_lobby_stats_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    index = table.Column<short>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false, defaultValue: "Spectator"),
                    lobby = table.Column<int>(nullable: false),
                    color_r = table.Column<short>(nullable: false),
                    color_g = table.Column<short>(nullable: false),
                    color_b = table.Column<short>(nullable: false),
                    blip_color = table.Column<short>(nullable: false, defaultValue: (short)4),
                    skin_hash = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teams", x => x.id);
                    table.ForeignKey(
                        name: "fk_teams_lobbies_lobby_navigation_id",
                        column: x => x.lobby,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_maps",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    map_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lobby_maps", x => new { x.lobby_id, x.map_id });
                    table.ForeignKey(
                        name: "fk_lobby_maps_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_lobby_maps_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_map_favourites",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    map_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_map_favourites", x => new { x.player_id, x.map_id });
                    table.ForeignKey(
                        name: "fk_player_map_favourites_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_map_favourites_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_map_ratings",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    map_id = table.Column<int>(nullable: false),
                    rating = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_map_ratings", x => new { x.player_id, x.map_id });
                    table.ForeignKey(
                        name: "fk_player_map_ratings_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_map_ratings_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_request_messages",
                columns: table => new
                {
                    request_id = table.Column<int>(nullable: false),
                    message_index = table.Column<int>(nullable: false),
                    author_id = table.Column<int>(nullable: false),
                    text = table.Column<string>(maxLength: 300, nullable: true),
                    create_time = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_support_request_messages", x => new { x.request_id, x.message_index });
                    table.ForeignKey(
                        name: "fk_support_request_messages_players_author_id",
                        column: x => x.author_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_support_request_messages_support_requests_request_id",
                        column: x => x.request_id,
                        principalTable: "support_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gangs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    team_id = table.Column<int>(nullable: false),
                    @short = table.Column<string>(name: "short", maxLength: 5, nullable: false),
                    owner_id = table.Column<int>(nullable: true),
                    create_time = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gangs", x => x.id);
                    table.ForeignKey(
                        name: "fk_gangs_players_owner_id",
                        column: x => x.owner_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_gangs_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gang_rank_permissions",
                columns: table => new
                {
                    gang_id = table.Column<int>(nullable: false),
                    manage_permissions = table.Column<short>(nullable: false),
                    invite_members = table.Column<short>(nullable: false),
                    kick_members = table.Column<short>(nullable: false),
                    manage_ranks = table.Column<short>(nullable: false),
                    start_gangwar = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gang_rank_permissions", x => x.gang_id);
                    table.ForeignKey(
                        name: "fk_gang_rank_permissions_gangs_gang_id",
                        column: x => x.gang_id,
                        principalTable: "gangs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gang_ranks",
                columns: table => new
                {
                    gang_id = table.Column<int>(nullable: false),
                    rank = table.Column<short>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gang_ranks", x => new { x.gang_id, x.rank });
                    table.ForeignKey(
                        name: "fk_gang_ranks_gangs_gang_id",
                        column: x => x.gang_id,
                        principalTable: "gangs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gangwar_areas",
                columns: table => new
                {
                    map_id = table.Column<int>(nullable: false),
                    owner_gang_id = table.Column<int>(nullable: false),
                    last_attacked = table.Column<DateTime>(nullable: false, defaultValueSql: "'2019-1-1'::timestamp"),
                    attack_count = table.Column<int>(nullable: false, defaultValue: 0),
                    defend_count = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gangwar_areas", x => x.map_id);
                    table.ForeignKey(
                        name: "fk_gangwar_areas_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gangwar_areas_gangs_owner_gang_id",
                        column: x => x.owner_gang_id,
                        principalTable: "gangs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "gang_members",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    gang_id = table.Column<int>(nullable: false),
                    rank = table.Column<short>(nullable: false, defaultValue: (short)0),
                    join_time = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    rank_navigation_gang_id = table.Column<int>(nullable: true),
                    rank_navigation_rank = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gang_members", x => x.player_id);
                    table.ForeignKey(
                        name: "fk_gang_members_gangs_gang_id",
                        column: x => x.gang_id,
                        principalTable: "gangs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gang_members_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gang_members_gang_ranks_rank_navigation_gang_id_rank_naviga",
                        columns: x => new { x.rank_navigation_gang_id, x.rank_navigation_rank },
                        principalTable: "gang_ranks",
                        principalColumns: new[] { "gang_id", "rank" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "admin_levels",
                columns: new[] { "level", "color_b", "color_g", "color_r" },
                values: new object[,]
                {
                    { (short)0, (short)220, (short)220, (short)220 },
                    { (short)1, (short)113, (short)202, (short)113 },
                    { (short)2, (short)85, (short)132, (short)253 },
                    { (short)3, (short)50, (short)50, (short)222 }
                });

            migrationBuilder.InsertData(
                table: "bonusbot_settings",
                columns: new[] { "id", "actions_info_channel_id", "admin_applications_channel_id", "bans_info_channel_id", "error_logs_channel_id", "guild_id", "send_private_message_on_ban", "send_private_message_on_offline_message", "server_infos_channel_id", "support_requests_channel_id" },
                values: new object[] { 1, 659088752890871818m, 659072893526736896m, 659705941771550730m, 659073884796092426m, 320309924175282177m, true, true, 659073271911809037m, 659073029896142855m });

            migrationBuilder.InsertData(
                table: "challenge_settings",
                columns: new[] { "type", "frequency", "max_number", "min_number" },
                values: new object[,]
                {
                    { ChallengeType.WriteHelpfulIssue, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.RoundPlayed, ChallengeFrequency.Weekly, 100, 50 },
                    { ChallengeType.ReviewMaps, ChallengeFrequency.Forever, 10, 10 },
                    { ChallengeType.ReadTheRules, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.ReadTheFAQ, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.PlayTime, ChallengeFrequency.Weekly, 1500, 300 },
                    { ChallengeType.Killstreak, ChallengeFrequency.Weekly, 7, 3 },
                    { ChallengeType.JoinDiscordServer, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.Kills, ChallengeFrequency.Weekly, 150, 75 },
                    { ChallengeType.CreatorOfAcceptedMap, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.ChangeSettings, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.BuyMaps, ChallengeFrequency.Forever, 500, 500 },
                    { ChallengeType.BombPlant, ChallengeFrequency.Weekly, 10, 5 },
                    { ChallengeType.BombDefuse, ChallengeFrequency.Weekly, 10, 5 },
                    { ChallengeType.BeHelpfulEnough, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.Assists, ChallengeFrequency.Weekly, 100, 50 },
                    { ChallengeType.Damage, ChallengeFrequency.Weekly, 100000, 20000 }
                });

            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "id", "command", "lobby_owner_can_use", "needed_admin_level", "needed_donation", "vip_can_use" },
                values: new object[,]
                {
                    { (short)18, "PrivateMessage", false, null, null, false },
                    { (short)25, "LobbyInvitePlayer", false, null, null, false },
                    { (short)24, "GiveMoney", false, null, null, false },
                    { (short)21, "UnblockUser", false, null, null, false },
                    { (short)20, "BlockUser", false, null, null, false },
                    { (short)19, "UserId", false, null, null, false },
                    { (short)17, "OpenPrivateChat", false, null, null, false },
                    { (short)13, "TeamChat", false, null, null, false },
                    { (short)15, "Position", false, null, null, false },
                    { (short)14, "PrivateChat", false, null, null, false },
                    { (short)12, "GlobalChat", false, null, null, false },
                    { (short)11, "Suicide", false, null, null, false },
                    { (short)10, "LobbyLeave", false, null, null, false },
                    { (short)16, "ClosePrivateChat", false, null, null, false }
                });

            migrationBuilder.InsertData(
                table: "fa_qs",
                columns: new[] { "id", "language", "answer", "question" },
                values: new object[,]
                {
                    { 1, Language.English, "With the END key on your keyboard.", "How do I activate my cursor?" },
                    { 1, Language.German, "Mit der ENDE Taste auf deiner Tastatur.", "Wie aktiviere ich meinen Cursor?" },
                    { 2, Language.English, @"In case of a transfer of TDS-V, the database will also be transferred, but without the player data (for data protection reasons).
                However, if you want to keep your data, you must allow it in the user panel.
                The data does not contain any sensitive information - IPs are not stored, passwords are secure (hash + salt).", "What is the 'Allow data transfer' setting in the userpanel?" },
                    { 2, Language.German, @"Im Falle einer Übergabe von TDS-V wird die Datenbank auch übergeben, jedoch ohne die Spieler-Daten (aus Datenschutz-Gründen).
                Falls du jedoch deine Daten auch dann weiterhin behalten willst, musst du es im Userpanel erlauben.
                Die Daten beinhalten keine sensiblen Informationen - IPs werden nicht gespeichert, Passwörter sind sicher (Hash + Salt).", "Was ist die 'Erlaube Daten-Transfer' Einstellung im Userpanel?" }
                });

            migrationBuilder.InsertData(
                table: "freeroam_default_vehicle",
                columns: new[] { "vehicle_type", "note", "vehicle_hash" },
                values: new object[,]
                {
                    { FreeroamVehicleType.Boat, null, VehicleHash.Speeder2 },
                    { FreeroamVehicleType.Plane, null, VehicleHash.Pyro },
                    { FreeroamVehicleType.Helicopter, null, VehicleHash.Akula },
                    { FreeroamVehicleType.Car, null, VehicleHash.Pfister811 },
                    { FreeroamVehicleType.Bike, null, VehicleHash.Hakuchou2 }
                });

            migrationBuilder.InsertData(
                table: "rules",
                columns: new[] { "id", "category", "target" },
                values: new object[,]
                {
                    { 5, RuleCategory.General, RuleTarget.Admin },
                    { 4, RuleCategory.General, RuleTarget.Admin },
                    { 3, RuleCategory.General, RuleTarget.Admin },
                    { 2, RuleCategory.Chat, RuleTarget.User },
                    { 1, RuleCategory.General, RuleTarget.User },
                    { 6, RuleCategory.General, RuleTarget.VIP },
                    { 7, RuleCategory.General, RuleTarget.VIP }
                });

            migrationBuilder.InsertData(
                table: "server_settings",
                columns: new[] { "id", "arena_new_map_probability_percent", "distance_to_spot_to_defuse", "distance_to_spot_to_plant", "error_to_player_on_non_existent_command", "gamemode_name", "give_money_fee", "give_money_min_amount", "killing_spree_max_seconds_until_next_kill", "map_rating_amount_for_check", "min_map_rating_for_new_maps", "multiplier_ranking_assists", "multiplier_ranking_damage", "multiplier_ranking_kills", "nametag_max_distance", "save_logs_cooldown_minutes", "save_player_data_cooldown_minutes", "save_seasons_cooldown_minutes", "show_nametag_only_on_aiming", "team_order_cooldown_ms", "to_chat_on_non_existent_command" },
                values: new object[] { (short)1, 2f, 3f, 3f, true, "tdm", 0.05f, 100, 18, 10, 3f, 25f, 1f, 75f, 80f, 1, 1, 1, true, 3000, false });

            migrationBuilder.InsertData(
                table: "server_total_stats",
                column: "id",
                value: (short)1);

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Dbshotgun, 166f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Assaultshotgun, 192f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Switchblade, 50f, 1f, WeaponType.Melee },
                    { WeaponHash.Machete, 45f, 1f, WeaponType.Melee },
                    { WeaponHash.Marksmanpistol, 150f, 1f, WeaponType.Handgun },
                    { WeaponHash.Machinepistol, 20f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Knuckle, 30f, 1f, WeaponType.Melee },
                    { WeaponHash.Heavypistol, 40f, 1f, WeaponType.Handgun },
                    { WeaponHash.Battleaxe, 50f, 1f, WeaponType.Melee },
                    { WeaponHash.Marksmanrifle_mk2, 65f, 2f, WeaponType.SniperRifle },
                    { WeaponHash.Pistol, 26f, 1f, WeaponType.Handgun },
                    { WeaponHash.Marksmanrifle, 65f, 2f, WeaponType.SniperRifle },
                    { WeaponHash.Doubleaction, 110f, 1f, WeaponType.Handgun },
                    { WeaponHash.Revolver_mk2, 110f, 1f, WeaponType.Handgun },
                    { WeaponHash.Revolver, 110f, 1f, WeaponType.Handgun },
                    { WeaponHash.Specialcarbine, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Assaultrifle_mk2, 30f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Assaultrifle, 30f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Snspistol_mk2, 28f, 1f, WeaponType.Handgun },
                    { WeaponHash.Snspistol, 28f, 1f, WeaponType.Handgun },
                    { WeaponHash.Assaultsmg, 23f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Hatchet, 50f, 1f, WeaponType.Melee },
                    { WeaponHash.Bottle, 10f, 1f, WeaponType.Melee },
                    { WeaponHash.Minismg, 22f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Sniperrifle, 101f, 1000f, WeaponType.SniperRifle }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Fireextinguisher, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Compactlauncher, 100f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Snowball, 10f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Vintagepistol, 34f, 1f, WeaponType.Handgun },
                    { WeaponHash.Combatpdw, 28f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Heavysniper, 216f, 2f, WeaponType.SniperRifle },
                    { WeaponHash.Heavysniper_mk2, 216f, 2f, WeaponType.SniperRifle },
                    { WeaponHash.Autoshotgun, 162f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Stone_hatchet, 50f, 1f, WeaponType.Melee }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.HazardCan, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.CeramicPistol, 20f, 1f, WeaponType.Handgun });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Smokegrenade, 1f, WeaponType.ThrownWeapon });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Specialcarbine_mk2, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Bullpuprifle_mk2, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Rayminigun, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Carbinerifle_mk2, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Raycarbine, 23f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Raypistol, 80f, 1f, WeaponType.Handgun }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Parachute, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.NavyRevolver, 40f, 1f, WeaponType.Handgun },
                    { WeaponHash.Wrench, 40f, 1f, WeaponType.Melee },
                    { WeaponHash.Pipebomb, 100f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Advancedrifle, 30f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Gusenberg, 34f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Combatpistol, 27f, 1f, WeaponType.Handgun },
                    { WeaponHash.Hammer, 40f, 1f, WeaponType.Melee }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Grenadelauncher_smoke, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Flare, 1f, WeaponType.ThrownWeapon }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Flaregun, 50f, 1f, WeaponType.Handgun },
                    { WeaponHash.Golfclub, 40f, 1f, WeaponType.Melee },
                    { WeaponHash.Minigun, 30f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Heavyshotgun, 117f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Compactrifle, 34f, 1f, WeaponType.AssaultRifle }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Stungun, 1f, WeaponType.Handgun });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Stickybomb, 100f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Smg_mk2, 22f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Smg, 22f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Molotov, 10f, 1f, WeaponType.ThrownWeapon }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Ball, 1f, WeaponType.ThrownWeapon });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Appistol, 28f, 1f, WeaponType.Handgun },
                    { WeaponHash.Pumpshotgun_mk2, 58f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Pumpshotgun, 58f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Pistol_mk2, 26f, 1f, WeaponType.Handgun }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Petrolcan, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Microsmg, 21f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Nightstick, 35f, 1f, WeaponType.Melee },
                    { WeaponHash.Railgun, 50f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Proximine, 100f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Musket, 165f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Grenadelauncher, 100f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Unarmed, 15f, 1f, WeaponType.Melee }
                });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "head_shot_damage_modifier", "type" },
                values: new object[] { WeaponHash.Bzgas, 1f, WeaponType.ThrownWeapon });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "hash", "damage", "head_shot_damage_modifier", "type" },
                values: new object[,]
                {
                    { WeaponHash.Bullpupshotgun, 112f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Mg, 40f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Knife, 45f, 1f, WeaponType.Melee },
                    { WeaponHash.Pistol50, 51f, 1f, WeaponType.Handgun },
                    { WeaponHash.Bat, 40f, 1f, WeaponType.Melee },
                    { WeaponHash.Poolcue, 40f, 1f, WeaponType.Melee },
                    { WeaponHash.Grenade, 100f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Dagger, 45f, 1f, WeaponType.Melee },
                    { WeaponHash.Flashlight, 30f, 1f, WeaponType.Melee },
                    { WeaponHash.Crowbar, 40f, 1f, WeaponType.Melee },
                    { WeaponHash.Carbinerifle, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Combatmg_mk2, 28f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Combatmg, 28f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Firework, 100f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Bullpuprifle, 32f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Sawnoffshotgun, 160f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Rpg, 100f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Hominglauncher, 150f, 1f, WeaponType.HeavyWeapon }
                });

            migrationBuilder.InsertData(
                table: "admin_level_names",
                columns: new[] { "level", "language", "name" },
                values: new object[,]
                {
                    { (short)0, Language.English, "User" },
                    { (short)0, Language.German, "User" },
                    { (short)1, Language.English, "Supporter" },
                    { (short)1, Language.German, "Supporter" },
                    { (short)3, Language.German, "Projektleiter" },
                    { (short)3, Language.English, "Projectleader" },
                    { (short)2, Language.German, "Administrator" },
                    { (short)2, Language.English, "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "alias", "command" },
                values: new object[,]
                {
                    { "StopPrivateChat", (short)16 },
                    { "ClosePrivateSay", (short)16 },
                    { "ClosePM", (short)16 },
                    { "CPC", (short)16 },
                    { "Pos", (short)15 },
                    { "GetPos", (short)15 },
                    { "StopPrivateSay", (short)17 },
                    { "CurrentPos", (short)15 },
                    { "Coordinates", (short)15 },
                    { "Coordinate", (short)15 },
                    { "Coord", (short)15 },
                    { "PrivateSay", (short)14 },
                    { "PChat", (short)14 },
                    { "TSay", (short)13 },
                    { "GetPosition", (short)15 },
                    { "OpenPrivateSay", (short)17 },
                    { "PM", (short)18 },
                    { "OPC", (short)17 },
                    { "InvitePlayerLobby", (short)25 },
                    { "InviteLobby", (short)25 },
                    { "LobbyInvite", (short)25 },
                    { "MoneySend", (short)24 },
                    { "SendMoney", (short)24 },
                    { "MoneyGive", (short)24 },
                    { "OpenPM", (short)17 },
                    { "Unblock", (short)21 },
                    { "IgnoreUser", (short)20 },
                    { "Ignore", (short)20 },
                    { "UID", (short)19 },
                    { "PSay", (short)18 },
                    { "TeamSay", (short)13 },
                    { "MSG", (short)18 },
                    { "Block", (short)20 },
                    { "TChat", (short)13 },
                    { "CurrentPosition", (short)15 },
                    { "GChat", (short)12 },
                    { "Dead", (short)11 },
                    { "Kill", (short)11 },
                    { "Leave", (short)10 },
                    { "AllChat", (short)12 },
                    { "AllSay", (short)12 },
                    { "G", (short)12 },
                    { "Death", (short)11 },
                    { "Die", (short)11 },
                    { "Back", (short)10 },
                    { "Global", (short)12 },
                    { "GlobalSay", (short)12 },
                    { "Mainmenu", (short)10 },
                    { "LeaveLobby", (short)10 },
                    { "PublicChat", (short)12 },
                    { "PublicSay", (short)12 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "id", "language", "info" },
                values: new object[,]
                {
                    { (short)12, Language.English, "Global chat which can be read everywhere." },
                    { (short)19, Language.German, "Gibt dir deine User-Id aus." },
                    { (short)18, Language.German, "Private Nachricht an einen bestimmten Spieler." },
                    { (short)12, Language.German, "Globaler Chat, welcher überall gelesen werden kann." },
                    { (short)21, Language.German, "Entfernt das Ziel aus der Blockliste." },
                    { (short)20, Language.German, "Fügt das Ziel in deine Blocklist ein, sodass du keine Nachrichten mehr von ihm liest, er dich nicht einladen kann usw." },
                    { (short)20, Language.English, "Adds the target into your blocklist so you won't see messages from him, he can't invite you anymore etc." },
                    { (short)21, Language.English, "Removes the target from the blocklist." },
                    { (short)24, Language.German, "Gibt einem Spieler Geld." },
                    { (short)24, Language.English, "Gives money to a player." },
                    { (short)25, Language.German, "Ladet einen Spieler in die eigene Lobby ein (falls möglich)." },
                    { (short)25, Language.English, "Invites a player to your lobby (if possible)." },
                    { (short)19, Language.English, "Outputs your user-id to yourself." },
                    { (short)17, Language.English, "Sends a private chat request or accepts the request of another user." },
                    { (short)18, Language.English, "Private message to a specific player." },
                    { (short)10, Language.German, "Verlässt die jetzige Lobby." },
                    { (short)13, Language.German, "Sendet die Nachricht nur zum eigenen Team." },
                    { (short)13, Language.English, "Sends the message to the current team only." },
                    { (short)14, Language.German, "Gibt die Position des Spielers aus." },
                    { (short)17, Language.German, "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                    { (short)11, Language.English, "Kills the user (suicide)." },
                    { (short)11, Language.German, "Tötet den Nutzer (Selbstmord)." },
                    { (short)15, Language.German, "Sendet eine Nachricht im Privatchat." },
                    { (short)14, Language.English, "Outputs the position of the player." },
                    { (short)15, Language.English, "Sends a message in private chat." },
                    { (short)10, Language.English, "Leaves the current lobby." },
                    { (short)16, Language.English, "Closes a private chat or withdraws a private chat request." },
                    { (short)16, Language.German, "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." }
                });

            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "id", "command", "lobby_owner_can_use", "needed_admin_level", "needed_donation", "vip_can_use" },
                values: new object[,]
                {
                    { (short)22, "LoadMapOfOthers", false, (short)1, null, true },
                    { (short)26, "Test", false, (short)3, null, false },
                    { (short)23, "VoiceMute", false, (short)1, null, true },
                    { (short)1, "AdminSay", false, (short)1, null, false },
                    { (short)2, "AdminChat", false, (short)1, null, true },
                    { (short)4, "Goto", true, (short)2, null, false },
                    { (short)6, "LobbyBan", true, (short)1, null, true },
                    { (short)7, "LobbyKick", true, (short)1, null, true },
                    { (short)3, "Ban", false, (short)2, null, false },
                    { (short)8, "Mute", false, (short)1, null, true },
                    { (short)9, "NextMap", true, (short)1, null, true },
                    { (short)5, "Kick", false, (short)1, null, true }
                });

            migrationBuilder.InsertData(
                table: "players",
                columns: new[] { "id", "admin_leader_id", "admin_lvl", "email", "is_vip", "name", "password", "sc_id", "sc_name" },
                values: new object[] { -1, null, (short)0, null, false, "System", "", 0m, "System" });

            migrationBuilder.InsertData(
                table: "rule_texts",
                columns: new[] { "rule_id", "language", "rule_str" },
                values: new object[,]
                {
                    { 6, Language.German, "Alle Admin-Regeln mit Ausnahme von Aktivitäts-Pflicht sind auch gültig für VIPs." },
                    { 6, Language.English, "All admin rules with the exception of activity duty are also valid for VIPs." },
                    { 5, Language.German, @"Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,
                frage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit.
                Zu hohe Zeiten sind schlecht, zu niedrige kein Problem." },
                    { 5, Language.English, @"If you are not sure if the time for e.g. Mute or Bann could be too high,
                ask your team leader - if you can't reach someone quickly, choose a lower time.
                Too high times are bad, too low times are no problem." },
                    { 4, Language.German, @"Ausnutzung der Befehle ist strengstens verboten!
                Admin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden." },
                    { 4, Language.English, @"Exploitation of the commands is strictly forbidden!
                Admin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules." },
                    { 2, Language.German, @"Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine.
                Unter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich.
                Die hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby.
                Chats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden." },
                    { 3, Language.English, "Admins have to follow the same rules as players do." },
                    { 2, Language.English, @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none.
                By 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area.
                The chat rules listed here are ONLY for the normal chat in an official lobby.
                Chats in private lobbies can be freely monitored by the lobby owners." },
                    { 1, Language.German, @"Teamen mit gegnerischen Spielern ist strengstens verboten!
                Damit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint.
                Wird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert." },
                    { 1, Language.English, @"Teaming with opposing players is strictly forbidden!
                This means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members.
                If such behaviour is noticed, it can lead to severe penalties and is permanently noted." },
                    { 7, Language.English, "The VIPs are free to decide whether they want to use their rights or not." },
                    { 3, Language.German, "Admins haben genauso die Regeln zu befolgen wie auch die Spieler." },
                    { 7, Language.German, "Den VIPs ist es frei überlassen, ob sie ihre Rechte nutzen wollen oder nicht." }
                });

            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "alias", "command" },
                values: new object[,]
                {
                    { "Skip", (short)9 },
                    { "Permaban", (short)3 },
                    { "PBan", (short)3 },
                    { "PermaMute", (short)8 },
                    { "PMute", (short)8 },
                    { "RMute", (short)8 },
                    { "TimeMute", (short)8 },
                    { "TMute", (short)8 },
                    { "EndRound", (short)9 },
                    { "Next", (short)9 },
                    { "MuteVoice", (short)23 },
                    { "VoiceTMute", (short)23 },
                    { "VoiceTimeMute", (short)23 },
                    { "PermaVoiceMute", (short)23 },
                    { "PVoiceMute", (short)23 },
                    { "RVoiceMute", (short)23 },
                    { "TimeVoiceMute", (short)23 },
                    { "TVoiceMute", (short)23 },
                    { "PermaMuteVoice", (short)23 },
                    { "PMuteVoice", (short)23 },
                    { "RMuteVoice", (short)23 },
                    { "TimeMuteVoice", (short)23 },
                    { "TMuteVoice", (short)23 },
                    { "VoicePermaMute", (short)23 },
                    { "VoicePMute", (short)23 },
                    { "RBan", (short)3 },
                    { "KickLobby", (short)7 },
                    { "BanLobby", (short)6 },
                    { "XYZ", (short)4 },
                    { "WarpToPlayer", (short)4 },
                    { "WarpTo", (short)4 },
                    { "Warp", (short)4 },
                    { "GotoXYZ", (short)4 },
                    { "Announce", (short)1 },
                    { "Announcement", (short)1 },
                    { "ASay", (short)1 },
                    { "OChat", (short)1 },
                    { "OSay", (short)1 },
                    { "TBan", (short)3 },
                    { "GotoPlayer", (short)4 },
                    { "AChat", (short)2 },
                    { "ChatAdmin", (short)2 },
                    { "InternChat", (short)2 },
                    { "WriteAdmin", (short)2 },
                    { "UnBan", (short)3 },
                    { "RKick", (short)5 },
                    { "UBan", (short)3 },
                    { "Timeban", (short)3 },
                    { "VoiceRMute", (short)23 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "id", "language", "info" },
                values: new object[,]
                {
                    { (short)23, Language.German, "Mutet einen Spieler im Voice-Chat." },
                    { (short)23, Language.English, "Mutes a player in the voice-chat." },
                    { (short)3, Language.German, "Bannt einen Spieler vom gesamten Server." },
                    { (short)4, Language.German, "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                    { (short)4, Language.English, "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
                    { (short)3, Language.English, "Bans a player out of the server." },
                    { (short)26, Language.English, "Command for quick testing of codes." },
                    { (short)9, Language.German, "Beendet die jetzige Runde in der jeweiligen Lobby." },
                    { (short)1, Language.German, "Schreibt öffentlich als ein Admin." },
                    { (short)1, Language.English, "Writes public as an admin." },
                    { (short)9, Language.English, "Ends the current round in the lobby." },
                    { (short)2, Language.English, "Writes intern to admins only." },
                    { (short)5, Language.German, "Kickt einen Spieler vom Server." },
                    { (short)5, Language.English, "Kicks a player out of the server." },
                    { (short)2, Language.German, "Schreibt intern nur den Admins." },
                    { (short)6, Language.English, "Bans a player out of the lobby in which the command was used." },
                    { (short)7, Language.German, "Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                    { (short)7, Language.English, "Kicks a player out of the lobby in which the command was used." },
                    { (short)8, Language.German, "Mutet einen Spieler im normalen Chat." },
                    { (short)8, Language.English, "Mutes a player in the normal chat." },
                    { (short)26, Language.German, "Befehl zum schnellen Testen von Codes." },
                    { (short)6, Language.German, "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." }
                });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "id", "is_official", "is_temporary", "name", "owner_id", "password", "type" },
                values: new object[,]
                {
                    { -3, true, false, "MapCreateLobby", -1, null, LobbyType.MapCreateLobby },
                    { -2, true, false, "GangLobby", -1, null, LobbyType.GangLobby },
                    { -1, true, false, "Arena", -1, null, LobbyType.Arena },
                    { -4, true, false, "MainMenu", -1, null, LobbyType.MainMenu }
                });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "id", "creator_id", "name" },
                values: new object[,]
                {
                    { -1, -1, "All" },
                    { -2, -1, "All Normals" },
                    { -3, -1, "All Bombs" },
                    { -4, -1, "All Sniper" }
                });

            migrationBuilder.InsertData(
                table: "killingspree_rewards",
                columns: new[] { "lobby_id", "kills_amount", "health_or_armor", "only_armor", "only_health" },
                values: new object[,]
                {
                    { -1, (short)3, (short)30, null, null },
                    { -1, (short)5, (short)50, null, null },
                    { -1, (short)10, (short)100, null, null },
                    { -1, (short)15, (short)100, null, null }
                });

            migrationBuilder.InsertData(
                table: "lobby_fight_settings",
                columns: new[] { "lobby_id", "amount_lifes" },
                values: new object[] { -1, (short)0 });

            migrationBuilder.InsertData(
                table: "lobby_map_settings",
                columns: new[] { "lobby_id", "map_limit_time", "map_limit_type" },
                values: new object[] { -1, 10, MapLimitType.KillAfterTime });

            migrationBuilder.InsertData(
                table: "lobby_maps",
                columns: new[] { "lobby_id", "map_id" },
                values: new object[] { -1, -1 });

            migrationBuilder.InsertData(
                table: "lobby_rewards",
                columns: new[] { "lobby_id", "money_per_assist", "money_per_damage", "money_per_kill" },
                values: new object[,]
                {
                    { -2, 10.0, 0.10000000000000001, 20.0 },
                    { -1, 10.0, 0.10000000000000001, 20.0 }
                });

            migrationBuilder.InsertData(
                table: "lobby_round_settings",
                columns: new[] { "lobby_id", "bomb_defuse_time_ms", "bomb_detonate_time_ms", "bomb_plant_time_ms", "countdown_time", "mix_teams_after_round", "round_time", "show_ranking" },
                values: new object[] { -1, 8000, 45000, 3000, 5, true, 240, true });

            migrationBuilder.InsertData(
                table: "lobby_weapons",
                columns: new[] { "hash", "lobby", "ammo", "damage", "head_multiplicator" },
                values: new object[,]
                {
                    { WeaponHash.Snspistol_mk2, -1, 9999, null, null },
                    { WeaponHash.Marksmanrifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Marksmanrifle, -1, 9999, null, null },
                    { WeaponHash.Doubleaction, -1, 9999, null, null },
                    { WeaponHash.Revolver_mk2, -1, 9999, null, null },
                    { WeaponHash.Revolver, -1, 9999, null, null },
                    { WeaponHash.Specialcarbine, -1, 9999, null, null },
                    { WeaponHash.Assaultrifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Assaultrifle, -1, 9999, null, null },
                    { WeaponHash.Snspistol, -1, 9999, null, null },
                    { WeaponHash.Proximine, -1, 9999, null, null },
                    { WeaponHash.Pipebomb, -1, 9999, null, null },
                    { WeaponHash.Rpg, -1, 9999, null, null },
                    { WeaponHash.Advancedrifle, -1, 9999, null, null },
                    { WeaponHash.Battleaxe, -1, 9999, null, null },
                    { WeaponHash.Musket, -1, 9999, null, null },
                    { WeaponHash.Grenadelauncher, -1, 9999, null, null },
                    { WeaponHash.Bzgas, -1, 9999, null, null },
                    { WeaponHash.Bullpupshotgun, -1, 9999, null, null },
                    { WeaponHash.Mg, -1, 9999, null, null },
                    { WeaponHash.Minismg, -1, 9999, null, null },
                    { WeaponHash.Heavypistol, -1, 9999, null, null },
                    { WeaponHash.Switchblade, -1, 9999, null, null },
                    { WeaponHash.Machinepistol, -1, 9999, null, null },
                    { WeaponHash.Stone_hatchet, -1, 9999, null, null },
                    { WeaponHash.HazardCan, -1, 9999, null, null },
                    { WeaponHash.NavyRevolver, -1, 9999, null, null },
                    { WeaponHash.CeramicPistol, -1, 9999, null, null },
                    { WeaponHash.Smokegrenade, -1, 9999, null, null },
                    { WeaponHash.Specialcarbine_mk2, -1, 9999, null, null },
                    { WeaponHash.Bullpuprifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Rayminigun, -1, 9999, null, null },
                    { WeaponHash.Carbinerifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Knuckle, -1, 9999, null, null },
                    { WeaponHash.Raycarbine, -1, 9999, null, null },
                    { WeaponHash.Parachute, -1, 9999, null, null },
                    { WeaponHash.Bottle, -1, 9999, null, null },
                    { WeaponHash.Hatchet, -1, 9999, null, null },
                    { WeaponHash.Assaultsmg, -1, 9999, null, null },
                    { WeaponHash.Dbshotgun, -1, 9999, null, null },
                    { WeaponHash.Assaultshotgun, -1, 9999, null, null },
                    { WeaponHash.Knife, -1, 9999, null, null },
                    { WeaponHash.Machete, -1, 9999, null, null },
                    { WeaponHash.Marksmanpistol, -1, 9999, null, null },
                    { WeaponHash.Raypistol, -1, 9999, null, null },
                    { WeaponHash.Pistol50, -1, 9999, null, null },
                    { WeaponHash.Grenade, -1, 9999, null, null },
                    { WeaponHash.Poolcue, -1, 9999, null, null },
                    { WeaponHash.Stickybomb, -1, 9999, null, null },
                    { WeaponHash.Smg_mk2, -1, 9999, null, null },
                    { WeaponHash.Smg, -1, 9999, null, null },
                    { WeaponHash.Molotov, -1, 9999, null, null },
                    { WeaponHash.Ball, -1, 9999, null, null },
                    { WeaponHash.Appistol, -1, 9999, null, null },
                    { WeaponHash.Pumpshotgun_mk2, -1, 9999, null, null },
                    { WeaponHash.Pumpshotgun, -1, 9999, null, null },
                    { WeaponHash.Pistol_mk2, -1, 9999, null, null },
                    { WeaponHash.Pistol, -1, 9999, null, null },
                    { WeaponHash.Wrench, -1, 9999, null, null },
                    { WeaponHash.Microsmg, -1, 9999, null, null },
                    { WeaponHash.Autoshotgun, -1, 9999, null, null },
                    { WeaponHash.Heavysniper_mk2, -1, 9999, null, null },
                    { WeaponHash.Heavysniper, -1, 9999, null, null },
                    { WeaponHash.Combatpdw, -1, 9999, null, null },
                    { WeaponHash.Vintagepistol, -1, 9999, null, null },
                    { WeaponHash.Snowball, -1, 9999, null, null },
                    { WeaponHash.Compactlauncher, -1, 9999, null, null },
                    { WeaponHash.Fireextinguisher, -1, 9999, null, null },
                    { WeaponHash.Sniperrifle, -1, 9999, null, null },
                    { WeaponHash.Bat, -1, 9999, null, null },
                    { WeaponHash.Stungun, -1, 9999, null, null },
                    { WeaponHash.Petrolcan, -1, 9999, null, null },
                    { WeaponHash.Minigun, -1, 9999, null, null },
                    { WeaponHash.Dagger, -1, 9999, null, null },
                    { WeaponHash.Flashlight, -1, 9999, null, null },
                    { WeaponHash.Crowbar, -1, 9999, null, null },
                    { WeaponHash.Carbinerifle, -1, 9999, null, null },
                    { WeaponHash.Combatmg_mk2, -1, 9999, null, null },
                    { WeaponHash.Combatmg, -1, 9999, null, null },
                    { WeaponHash.Firework, -1, 9999, null, null },
                    { WeaponHash.Heavyshotgun, -1, 9999, null, null },
                    { WeaponHash.Sawnoffshotgun, -1, 9999, null, null },
                    { WeaponHash.Railgun, -1, 9999, null, null },
                    { WeaponHash.Bullpuprifle, -1, 9999, null, null },
                    { WeaponHash.Hominglauncher, -1, 9999, null, null },
                    { WeaponHash.Compactrifle, -1, 9999, null, null },
                    { WeaponHash.Gusenberg, -1, 9999, null, null },
                    { WeaponHash.Combatpistol, -1, 9999, null, null },
                    { WeaponHash.Hammer, -1, 9999, null, null },
                    { WeaponHash.Grenadelauncher_smoke, -1, 9999, null, null },
                    { WeaponHash.Flare, -1, 9999, null, null },
                    { WeaponHash.Flaregun, -1, 9999, null, null },
                    { WeaponHash.Golfclub, -1, 9999, null, null },
                    { WeaponHash.Nightstick, -1, 9999, null, null }
                });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "blip_color", "color_b", "color_g", "color_r", "index", "lobby", "name" },
                values: new object[] { -2, (short)4, (short)255, (short)255, (short)255, (short)0, -1, "Spectator" });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "blip_color", "color_b", "color_g", "color_r", "index", "lobby", "name", "skin_hash" },
                values: new object[,]
                {
                    { -3, (short)52, (short)0, (short)150, (short)0, (short)1, -1, "SWAT", -1920001264 },
                    { -4, (short)1, (short)0, (short)0, (short)150, (short)2, -1, "Terrorist", 275618457 }
                });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "blip_color", "color_b", "color_g", "color_r", "index", "lobby", "name" },
                values: new object[] { -5, (short)4, (short)255, (short)255, (short)255, (short)0, -2, "None" });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "blip_color", "color_b", "color_g", "color_r", "index", "lobby", "name", "skin_hash" },
                values: new object[] { -1, (short)4, (short)255, (short)255, (short)255, (short)0, -4, "Spectator", 1004114196 });

            migrationBuilder.InsertData(
                table: "gangs",
                columns: new[] { "id", "owner_id", "short", "team_id" },
                values: new object[] { -1, null, "-", -5 });

            migrationBuilder.InsertData(
                table: "gang_rank_permissions",
                columns: new[] { "gang_id", "invite_members", "kick_members", "manage_permissions", "manage_ranks", "start_gangwar" },
                values: new object[] { -1, (short)5, (short)5, (short)5, (short)5, (short)5 });

            migrationBuilder.InsertData(
                table: "gang_ranks",
                columns: new[] { "gang_id", "rank", "name" },
                values: new object[] { -1, (short)0, "-" });

            migrationBuilder.CreateIndex(
                name: "ix_application_answers_question_id",
                table: "application_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_invitations_admin_id",
                table: "application_invitations",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_invitations_application_id",
                table: "application_invitations",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_questions_admin_id",
                table: "application_questions",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "ix_applications_player_id",
                table: "applications",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_command_alias_command",
                table: "command_alias",
                column: "command");

            migrationBuilder.CreateIndex(
                name: "ix_commands_needed_admin_level",
                table: "commands",
                column: "needed_admin_level");

            migrationBuilder.CreateIndex(
                name: "ix_gang_members_gang_id",
                table: "gang_members",
                column: "gang_id");

            migrationBuilder.CreateIndex(
                name: "ix_gang_members_rank_navigation_gang_id_rank_navigation_rank",
                table: "gang_members",
                columns: new[] { "rank_navigation_gang_id", "rank_navigation_rank" });

            migrationBuilder.CreateIndex(
                name: "ix_gangs_owner_id",
                table: "gangs",
                column: "owner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gangs_team_id",
                table: "gangs",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_gangwar_areas_owner_gang_id",
                table: "gangwar_areas",
                column: "owner_gang_id");

            migrationBuilder.CreateIndex(
                name: "ix_lobbies_owner_id",
                table: "lobbies",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_lobby_maps_map_id",
                table: "lobby_maps",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "ix_lobby_weapons_lobby",
                table: "lobby_weapons",
                column: "lobby");

            migrationBuilder.CreateIndex(
                name: "ix_maps_creator_id",
                table: "maps",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_maps_name",
                table: "maps",
                column: "name")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "ix_offlinemessages_source_id",
                table: "offlinemessages",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "ix_offlinemessages_target_id",
                table: "offlinemessages",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_bans_admin_id",
                table: "player_bans",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_bans_ip",
                table: "player_bans",
                column: "ip");

            migrationBuilder.CreateIndex(
                name: "ix_player_bans_lobby_id",
                table: "player_bans",
                column: "lobby_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_bans_sc_id",
                table: "player_bans",
                column: "sc_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_bans_sc_name",
                table: "player_bans",
                column: "sc_name");

            migrationBuilder.CreateIndex(
                name: "ix_player_bans_serial",
                table: "player_bans",
                column: "serial");

            migrationBuilder.CreateIndex(
                name: "ix_player_lobby_stats_lobby_id",
                table: "player_lobby_stats",
                column: "lobby_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_map_favourites_map_id",
                table: "player_map_favourites",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_map_ratings_map_id",
                table: "player_map_ratings",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_relations_target_id",
                table: "player_relations",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "ix_players_admin_leader_id",
                table: "players",
                column: "admin_leader_id");

            migrationBuilder.CreateIndex(
                name: "ix_players_admin_lvl",
                table: "players",
                column: "admin_lvl");

            migrationBuilder.CreateIndex(
                name: "ix_support_request_messages_author_id",
                table: "support_request_messages",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_support_requests_author_id",
                table: "support_requests",
                column: "author_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teams_lobby",
                table: "teams",
                column: "lobby");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_level_names");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "application_answers");

            migrationBuilder.DropTable(
                name: "application_invitations");

            migrationBuilder.DropTable(
                name: "bonusbot_settings");

            migrationBuilder.DropTable(
                name: "challenge_settings");

            migrationBuilder.DropTable(
                name: "command_alias");

            migrationBuilder.DropTable(
                name: "command_infos");

            migrationBuilder.DropTable(
                name: "fa_qs");

            migrationBuilder.DropTable(
                name: "freeroam_default_vehicle");

            migrationBuilder.DropTable(
                name: "gang_members");

            migrationBuilder.DropTable(
                name: "gang_rank_permissions");

            migrationBuilder.DropTable(
                name: "gangwar_areas");

            migrationBuilder.DropTable(
                name: "killingspree_rewards");

            migrationBuilder.DropTable(
                name: "lobby_fight_settings");

            migrationBuilder.DropTable(
                name: "lobby_map_settings");

            migrationBuilder.DropTable(
                name: "lobby_maps");

            migrationBuilder.DropTable(
                name: "lobby_rewards");

            migrationBuilder.DropTable(
                name: "lobby_round_settings");

            migrationBuilder.DropTable(
                name: "lobby_weapons");

            migrationBuilder.DropTable(
                name: "log_admins");

            migrationBuilder.DropTable(
                name: "log_chats");

            migrationBuilder.DropTable(
                name: "log_errors");

            migrationBuilder.DropTable(
                name: "log_kills");

            migrationBuilder.DropTable(
                name: "log_rests");

            migrationBuilder.DropTable(
                name: "offlinemessages");

            migrationBuilder.DropTable(
                name: "player_bans");

            migrationBuilder.DropTable(
                name: "player_challenges");

            migrationBuilder.DropTable(
                name: "player_clothes");

            migrationBuilder.DropTable(
                name: "player_lobby_stats");

            migrationBuilder.DropTable(
                name: "player_map_favourites");

            migrationBuilder.DropTable(
                name: "player_map_ratings");

            migrationBuilder.DropTable(
                name: "player_relations");

            migrationBuilder.DropTable(
                name: "player_settings");

            migrationBuilder.DropTable(
                name: "player_stats");

            migrationBuilder.DropTable(
                name: "player_total_stats");

            migrationBuilder.DropTable(
                name: "rule_texts");

            migrationBuilder.DropTable(
                name: "server_daily_stats");

            migrationBuilder.DropTable(
                name: "server_settings");

            migrationBuilder.DropTable(
                name: "server_total_stats");

            migrationBuilder.DropTable(
                name: "support_request_messages");

            migrationBuilder.DropTable(
                name: "application_questions");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "commands");

            migrationBuilder.DropTable(
                name: "gang_ranks");

            migrationBuilder.DropTable(
                name: "weapons");

            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "rules");

            migrationBuilder.DropTable(
                name: "support_requests");

            migrationBuilder.DropTable(
                name: "gangs");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "lobbies");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "admin_levels");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");
        }
    }
}

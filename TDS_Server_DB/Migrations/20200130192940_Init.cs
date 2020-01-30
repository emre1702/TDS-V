using System;
using System.Net;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;
using TDS_Common.Enum.Userpanel;

namespace TDS_Server_DB.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .Annotation("Npgsql:Enum:e_challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .Annotation("Npgsql:Enum:e_freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:e_language", "german,english")
                .Annotation("Npgsql:Enum:e_lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby")
                .Annotation("Npgsql:Enum:e_log_type", "kick,ban,mute,next,login,register,lobby__join,lobby__leave,lobby__kick,lobby__ban,goto,remove_map,voice_mute")
                .Annotation("Npgsql:Enum:e_map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .Annotation("Npgsql:Enum:e_player_relation", "none,block,friend")
                .Annotation("Npgsql:Enum:e_rule_category", "general,chat")
                .Annotation("Npgsql:Enum:e_rule_target", "user,admin,vip")
                .Annotation("Npgsql:Enum:e_support_type", "question,help,compliment,complaint")
                .Annotation("Npgsql:Enum:e_userpanel_admin_question_answer_type", "text,check,number")
                .Annotation("Npgsql:Enum:e_weapon_hash", "sniper_rifle,fire_extinguisher,compact_grenade_launcher,snowball,vintage_pistol,combat_pdw,heavy_sniper,heavy_sniper_mk2,sweeper_shotgun,micro_smg,wrench,pistol,pistol_mk2,pump_shotgun,pump_shotgun_mk2,ap_pistol,baseball,molotov,smg,smg_mk2,sticky_bomb,petrol_can,stun_gun,heavy_shotgun,minigun,golf_club,flare_gun,flare,grenade_launcher_smoke,hammer,combat_pistol,gusenberg,compact_rifle,homing_launcher,nightstick,railgun,sawn_off_shotgun,bullpup_rifle,firework,combat_mg,combat_mg_mk2,carbine_rifle,crowbar,flashlight,dagger,grenade,pool_cue,bat,pistol50,knife,mg,bullpup_shotgun,bz_gas,unarmed,grenade_launcher,night_vision,musket,proximity_mine,advanced_rifle,rpg,pipe_bomb,mini_smg,sns_pistol,sns_pistol_mk2,assault_rifle,assault_rifle_mk2,special_carbine,heavy_revolver,heavy_revolver_mk2,double_action_revolver,marksman_rifle,marksman_rifle_mk2,battle_axe,heavy_pistol,knuckle_duster,machine_pistol,marksman_pistol,machete,switch_blade,assault_shotgun,double_barrel_shotgun,assault_smg,hatchet,bottle,parachute,smoke_grenade,upn_atomizer,unholy_hellbringer,carbine_rifle_mk2,sepcial_carbine_mk2,bullpup_rifle_mk2,widowmaker")
                .Annotation("Npgsql:Enum:e_weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .Annotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust,minitank,retinue2,outlaw,yosemite2,stryder,jb7002,sultan2,everon,sugoi,zhaba,formula,formula2,rebla,vagrant,furia,vstr,komoda,asbo,kanjo,imorgon")
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
                    table.PrimaryKey("PK_admin_levels", x => x.level);
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
                    table.PrimaryKey("PK_announcements", x => x.id);
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
                    table.PrimaryKey("PK_bonusbot_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "challenge_settings",
                columns: table => new
                {
                    type = table.Column<EChallengeType>(nullable: false),
                    frequency = table.Column<EChallengeFrequency>(nullable: false),
                    min_number = table.Column<int>(nullable: false, defaultValue: 1),
                    max_number = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_challenge_settings", x => new { x.type, x.frequency });
                });

            migrationBuilder.CreateTable(
                name: "fa_qs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    language = table.Column<ELanguage>(nullable: false),
                    question = table.Column<string>(nullable: true),
                    answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fa_qs", x => new { x.id, x.language });
                });

            migrationBuilder.CreateTable(
                name: "freeroam_default_vehicle",
                columns: table => new
                {
                    vehicle_type = table.Column<EFreeroamVehicleType>(nullable: false),
                    vehicle_hash = table.Column<VehicleHash>(nullable: false),
                    note = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_freeroam_default_vehicle", x => x.vehicle_type);
                });

            migrationBuilder.CreateTable(
                name: "log_admins",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    type = table.Column<ELogType>(nullable: false),
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
                    table.PrimaryKey("PK_log_admins", x => x.id);
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
                    table.PrimaryKey("PK_log_chats", x => x.id);
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
                    table.PrimaryKey("PK_log_errors", x => x.id);
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
                    table.PrimaryKey("PK_log_kills", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_rests",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    type = table.Column<ELogType>(nullable: false),
                    source = table.Column<int>(nullable: false),
                    serial = table.Column<string>(maxLength: 200, nullable: true),
                    ip = table.Column<IPAddress>(nullable: true),
                    lobby = table.Column<int>(nullable: true),
                    timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_rests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    target = table.Column<ERuleTarget>(nullable: false),
                    category = table.Column<ERuleCategory>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rules", x => x.id);
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
                    table.PrimaryKey("PK_server_daily_stats", x => x.date);
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
                    afk_kick_after_sec = table.Column<int>(nullable: false, defaultValue: 25),
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
                    gangwar_target_without_attacker_max_seconds = table.Column<int>(nullable: false, defaultValue: 5),
                    reduce_maps_bought_counter_after_minute = table.Column<int>(nullable: false, defaultValue: 60),
                    map_buy_base_price = table.Column<int>(nullable: false, defaultValue: 1000),
                    map_buy_counter_multiplicator = table.Column<float>(nullable: false, defaultValue: 1f),
                    username_change_cost = table.Column<int>(nullable: false, defaultValue: 20000),
                    username_change_cooldown_days = table.Column<int>(nullable: false, defaultValue: 60),
                    amount_weekly_challenges = table.Column<int>(nullable: false, defaultValue: 3)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_settings", x => x.id);
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
                    table.PrimaryKey("PK_server_total_stats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    hash = table.Column<EWeaponHash>(nullable: false),
                    type = table.Column<EWeaponType>(nullable: false),
                    default_damage = table.Column<short>(nullable: false),
                    default_head_multiplicator = table.Column<float>(nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapons", x => x.hash);
                });

            migrationBuilder.CreateTable(
                name: "admin_level_names",
                columns: table => new
                {
                    level = table.Column<short>(nullable: false),
                    language = table.Column<ELanguage>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_level_names", x => new { x.level, x.language });
                    table.ForeignKey(
                        name: "FK_admin_level_names_admin_levels_level",
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
                    table.PrimaryKey("PK_commands", x => x.id);
                    table.ForeignKey(
                        name: "FK_commands_admin_levels_needed_admin_level",
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
                    table.PrimaryKey("PK_players", x => x.id);
                    table.ForeignKey(
                        name: "FK_players_players_admin_leader_id",
                        column: x => x.admin_leader_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_players_admin_levels_admin_lvl",
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
                    language = table.Column<ELanguage>(nullable: false),
                    rule_str = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rule_texts", x => new { x.rule_id, x.language });
                    table.ForeignKey(
                        name: "FK_rule_texts_rules_rule_id",
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
                    table.PrimaryKey("PK_command_alias", x => new { x.alias, x.command });
                    table.ForeignKey(
                        name: "FK_command_alias_commands_command",
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
                    language = table.Column<ELanguage>(nullable: false),
                    info = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_infos", x => new { x.id, x.language });
                    table.ForeignKey(
                        name: "FK_command_infos_commands_id",
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
                    answer_type = table.Column<EUserpanelAdminQuestionAnswerType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_application_questions_players_admin_id",
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
                    table.PrimaryKey("PK_applications", x => x.id);
                    table.ForeignKey(
                        name: "FK_applications_players_player_id",
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
                    type = table.Column<ELobbyType>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    password = table.Column<string>(maxLength: 100, nullable: true),
                    start_health = table.Column<short>(nullable: false, defaultValueSql: "100"),
                    start_armor = table.Column<short>(nullable: false, defaultValueSql: "100"),
                    amount_lifes = table.Column<short>(nullable: true),
                    default_spawn_x = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    default_spawn_y = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    default_spawn_z = table.Column<float>(nullable: false, defaultValueSql: "9000"),
                    around_spawn_point = table.Column<float>(nullable: false, defaultValueSql: "3"),
                    default_spawn_rotation = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    is_temporary = table.Column<bool>(nullable: false),
                    is_official = table.Column<bool>(nullable: false),
                    spawn_again_after_death_ms = table.Column<int>(nullable: false, defaultValueSql: "400"),
                    create_timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lobbies", x => x.id);
                    table.ForeignKey(
                        name: "FK_lobbies_players_owner_id",
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
                    table.PrimaryKey("PK_maps", x => x.id);
                    table.ForeignKey(
                        name: "FK_maps_players_creator_id",
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
                    table.PrimaryKey("PK_offlinemessages", x => x.id);
                    table.ForeignKey(
                        name: "FK_offlinemessages_players_source_id",
                        column: x => x.source_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_offlinemessages_players_target_id",
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
                    challenge = table.Column<EChallengeType>(nullable: false),
                    frequency = table.Column<EChallengeFrequency>(nullable: false),
                    amount = table.Column<int>(nullable: false, defaultValue: 1),
                    current_amount = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_challenges", x => new { x.player_id, x.challenge, x.frequency });
                    table.ForeignKey(
                        name: "FK_player_challenges_players_player_id",
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
                    table.PrimaryKey("PK_player_clothes", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_player_clothes_players_player_id",
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
                    relation = table.Column<EPlayerRelation>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_relations", x => new { x.player_id, x.target_id });
                    table.ForeignKey(
                        name: "FK_player_relations_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_relations_players_target_id",
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
                    language = table.Column<ELanguage>(nullable: false, defaultValue: ELanguage.English),
                    allow_data_transfer = table.Column<bool>(nullable: false),
                    show_confetti_at_ranking = table.Column<bool>(nullable: false),
                    timezone = table.Column<string>(nullable: true, defaultValue: "UTC"),
                    date_time_format = table.Column<string>(nullable: true, defaultValue: "yyyy'-'MM'-'dd HH':'mm':'ss"),
                    discord_user_id = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    hitsound = table.Column<bool>(nullable: false),
                    bloodscreen = table.Column<bool>(nullable: false),
                    floating_damage_info = table.Column<bool>(nullable: false),
                    voice3d = table.Column<bool>(nullable: false, defaultValue: false),
                    voice_auto_volume = table.Column<bool>(nullable: false, defaultValue: false),
                    voice_volume = table.Column<float>(nullable: false, defaultValue: 6f),
                    map_border_color = table.Column<string>(nullable: true, defaultValue: "rgba(150,0,0,0.35)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_settings", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_player_settings_players_player_id",
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
                    table.PrimaryKey("PK_player_stats", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_player_stats_players_player_id",
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
                    table.PrimaryKey("PK_player_total_stats", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_player_total_stats_players_player_id",
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
                    type = table.Column<ESupportType>(nullable: false),
                    atleast_admin_level = table.Column<int>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    close_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_support_requests_players_author_id",
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
                    table.PrimaryKey("PK_application_answers", x => new { x.application_id, x.question_id });
                    table.ForeignKey(
                        name: "FK_application_answers_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_answers_application_questions_question_id",
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
                    table.PrimaryKey("PK_application_invitations", x => x.id);
                    table.ForeignKey(
                        name: "FK_application_invitations_players_admin_id",
                        column: x => x.admin_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_invitations_applications_application_id",
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
                    table.PrimaryKey("PK_killingspree_rewards", x => new { x.lobby_id, x.kills_amount });
                    table.ForeignKey(
                        name: "FK_killingspree_rewards_lobbies_lobby_id",
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
                    map_limit_type = table.Column<EMapLimitType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lobby_map_settings", x => x.lobby_id);
                    table.ForeignKey(
                        name: "FK_lobby_map_settings_lobbies_lobby_id",
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
                    table.PrimaryKey("PK_lobby_rewards", x => x.lobby_id);
                    table.ForeignKey(
                        name: "FK_lobby_rewards_lobbies_lobby_id",
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
                    table.PrimaryKey("PK_lobby_round_settings", x => x.lobby_id);
                    table.ForeignKey(
                        name: "FK_lobby_round_settings_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_weapons",
                columns: table => new
                {
                    hash = table.Column<EWeaponHash>(nullable: false),
                    lobby = table.Column<int>(nullable: false),
                    ammo = table.Column<int>(nullable: false),
                    damage = table.Column<short>(nullable: true),
                    head_multiplicator = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lobby_weapons", x => new { x.hash, x.lobby });
                    table.ForeignKey(
                        name: "FK_lobby_weapons_weapons_hash",
                        column: x => x.hash,
                        principalTable: "weapons",
                        principalColumn: "hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lobby_weapons_lobbies_lobby",
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
                    table.PrimaryKey("PK_player_bans", x => new { x.player_id, x.lobby_id });
                    table.ForeignKey(
                        name: "FK_player_bans_players_admin_id",
                        column: x => x.admin_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_player_bans_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_bans_players_player_id",
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
                    table.PrimaryKey("PK_player_lobby_stats", x => new { x.player_id, x.lobby_id });
                    table.ForeignKey(
                        name: "FK_player_lobby_stats_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_lobby_stats_players_player_id",
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
                    table.PrimaryKey("PK_teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_teams_lobbies_lobby",
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
                    table.PrimaryKey("PK_lobby_maps", x => new { x.lobby_id, x.map_id });
                    table.ForeignKey(
                        name: "FK_lobby_maps_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lobby_maps_maps_map_id",
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
                    table.PrimaryKey("PK_player_map_favourites", x => new { x.player_id, x.map_id });
                    table.ForeignKey(
                        name: "FK_player_map_favourites_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_map_favourites_players_player_id",
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
                    table.PrimaryKey("PK_player_map_ratings", x => new { x.player_id, x.map_id });
                    table.ForeignKey(
                        name: "FK_player_map_ratings_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_map_ratings_players_player_id",
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
                    table.PrimaryKey("PK_support_request_messages", x => new { x.request_id, x.message_index });
                    table.ForeignKey(
                        name: "FK_support_request_messages_players_author_id",
                        column: x => x.author_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_support_request_messages_support_requests_request_id",
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
                    table.PrimaryKey("PK_gangs", x => x.id);
                    table.ForeignKey(
                        name: "FK_gangs_players_owner_id",
                        column: x => x.owner_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_gangs_teams_team_id",
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
                    table.PrimaryKey("PK_gang_rank_permissions", x => x.gang_id);
                    table.ForeignKey(
                        name: "FK_gang_rank_permissions_gangs_gang_id",
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
                    table.PrimaryKey("PK_gang_ranks", x => new { x.gang_id, x.rank });
                    table.ForeignKey(
                        name: "FK_gang_ranks_gangs_gang_id",
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
                    table.PrimaryKey("PK_gangwar_areas", x => x.map_id);
                    table.ForeignKey(
                        name: "FK_gangwar_areas_maps_map_id",
                        column: x => x.map_id,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gangwar_areas_gangs_owner_gang_id",
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
                    table.PrimaryKey("PK_gang_members", x => x.player_id);
                    table.ForeignKey(
                        name: "FK_gang_members_gangs_gang_id",
                        column: x => x.gang_id,
                        principalTable: "gangs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gang_members_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gang_members_gang_ranks_rank_navigation_gang_id_rank_naviga~",
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
                    { EChallengeType.WriteHelpfulIssue, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.RoundPlayed, EChallengeFrequency.Weekly, 100, 50 },
                    { EChallengeType.ReviewMaps, EChallengeFrequency.Forever, 10, 10 },
                    { EChallengeType.ReadTheRules, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.ReadTheFAQ, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.PlayTime, EChallengeFrequency.Weekly, 1500, 300 },
                    { EChallengeType.Killstreak, EChallengeFrequency.Weekly, 7, 3 },
                    { EChallengeType.JoinDiscordServer, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.Kills, EChallengeFrequency.Weekly, 150, 75 },
                    { EChallengeType.CreatorOfAcceptedMap, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.ChangeSettings, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.BuyMaps, EChallengeFrequency.Forever, 500, 500 },
                    { EChallengeType.BombPlant, EChallengeFrequency.Weekly, 10, 5 },
                    { EChallengeType.BombDefuse, EChallengeFrequency.Weekly, 10, 5 },
                    { EChallengeType.BeHelpfulEnough, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.Assists, EChallengeFrequency.Weekly, 100, 50 },
                    { EChallengeType.Damage, EChallengeFrequency.Weekly, 100000, 20000 }
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
                    { 1, ELanguage.English, "With the END key on your keyboard.", "How do I activate my cursor?" },
                    { 1, ELanguage.German, "Mit der ENDE Taste auf deiner Tastatur.", "Wie aktiviere ich meinen Cursor?" },
                    { 2, ELanguage.English, @"In case of a transfer of TDS-V, the database will also be transferred, but without the player data (for data protection reasons).
                However, if you want to keep your data, you must allow it in the user panel.
                The data does not contain any sensitive information - IPs are not stored, passwords are secure (hash + salt).", "What is the 'Allow data transfer' setting in the userpanel?" },
                    { 2, ELanguage.German, @"Im Falle einer Übergabe von TDS-V wird die Datenbank auch übergeben, jedoch ohne die Spieler-Daten (aus Datenschutz-Gründen).
                Falls du jedoch deine Daten auch dann weiterhin behalten willst, musst du es im Userpanel erlauben.
                Die Daten beinhalten keine sensiblen Informationen - IPs werden nicht gespeichert, Passwörter sind sicher (Hash + Salt).", "Was ist die 'Erlaube Daten-Transfer' Einstellung im Userpanel?" }
                });

            migrationBuilder.InsertData(
                table: "freeroam_default_vehicle",
                columns: new[] { "vehicle_type", "note", "vehicle_hash" },
                values: new object[,]
                {
                    { EFreeroamVehicleType.Boat, null, VehicleHash.Speeder2 },
                    { EFreeroamVehicleType.Plane, null, VehicleHash.Pyro },
                    { EFreeroamVehicleType.Helicopter, null, VehicleHash.Akula },
                    { EFreeroamVehicleType.Car, null, VehicleHash.Pfister811 },
                    { EFreeroamVehicleType.Bike, null, VehicleHash.Hakuchou2 }
                });

            migrationBuilder.InsertData(
                table: "rules",
                columns: new[] { "id", "category", "target" },
                values: new object[,]
                {
                    { 5, ERuleCategory.General, ERuleTarget.Admin },
                    { 4, ERuleCategory.General, ERuleTarget.Admin },
                    { 3, ERuleCategory.General, ERuleTarget.Admin },
                    { 2, ERuleCategory.Chat, ERuleTarget.User },
                    { 1, ERuleCategory.General, ERuleTarget.User },
                    { 6, ERuleCategory.General, ERuleTarget.VIP },
                    { 7, ERuleCategory.General, ERuleTarget.VIP }
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
                columns: new[] { "hash", "default_damage", "default_head_multiplicator", "type" },
                values: new object[,]
                {
                    { EWeaponHash.MarksmanPistol, (short)150, 1f, EWeaponType.Handgun },
                    { EWeaponHash.MachinePistol, (short)20, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.KnuckleDuster, (short)30, 1f, EWeaponType.Melee },
                    { EWeaponHash.HeavyPistol, (short)40, 1f, EWeaponType.Handgun },
                    { EWeaponHash.BattleAxe, (short)50, 1f, EWeaponType.Melee },
                    { EWeaponHash.MarksmanRifleMk2, (short)65, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.MarksmanRifle, (short)65, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.DoubleActionRevolver, (short)110, 1f, EWeaponType.Handgun },
                    { EWeaponHash.HeavyRevolverMk2, (short)110, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Pistol, (short)26, 1f, EWeaponType.Handgun },
                    { EWeaponHash.HeavyRevolver, (short)110, 1f, EWeaponType.Handgun },
                    { EWeaponHash.SpecialCarbine, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.AssaultRifleMk2, (short)30, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.AssaultRifle, (short)30, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.SNSPistolMk2, (short)28, 1f, EWeaponType.Handgun },
                    { EWeaponHash.SNSPistol, (short)28, 1f, EWeaponType.Handgun },
                    { EWeaponHash.MiniSMG, (short)22, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.PipeBomb, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.RPG, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Machete, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.SwitchBlade, (short)50, 1f, EWeaponType.Melee },
                    { EWeaponHash.AssaultShotgun, (short)192, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.DoubleBarrelShotgun, (short)166, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.SniperRifle, (short)101, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.FireExtinguisher, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.CompactGrenadeLauncher, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Snowball, (short)10, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.VintagePistol, (short)34, 1f, EWeaponType.Handgun },
                    { EWeaponHash.CombatPDW, (short)28, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.HeavySniper, (short)216, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.HeavySniperMk2, (short)216, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.AdvancedRifle, (short)30, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.SweeperShotgun, (short)162, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.CarbineRifleMK2, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.UnholyHellbringer, (short)23, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.UpnAtomizer, (short)80, 1f, EWeaponType.Handgun },
                    { EWeaponHash.SmokeGrenade, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Parachute, (short)0, 1f, EWeaponType.Rest },
                    { EWeaponHash.Bottle, (short)10, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Hatchet, (short)50, 1f, EWeaponType.Melee },
                    { EWeaponHash.AssaultSMG, (short)23, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Widowmaker, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.ProximityMine, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Musket, (short)165, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.NightVision, (short)0, 1f, EWeaponType.Rest },
                    { EWeaponHash.MicroSMG, (short)21, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Hammer, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.GrenadeLauncherSmoke, (short)0, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Flare, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.FlareGun, (short)50, 1f, EWeaponType.Handgun },
                    { EWeaponHash.GolfClub, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.Minigun, (short)30, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.HeavyShotgun, (short)117, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.StunGun, (short)0, 1f, EWeaponType.Handgun },
                    { EWeaponHash.PetrolCan, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.StickyBomb, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.SMGMk2, (short)22, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.SMG, (short)22, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Molotov, (short)10, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Baseball, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.APPistol, (short)28, 1f, EWeaponType.Handgun },
                    { EWeaponHash.PumpShotgunMk2, (short)58, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.PumpShotgun, (short)58, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.PistolMk2, (short)26, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Gusenberg, (short)34, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.CompactRifle, (short)34, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.HomingLauncher, (short)150, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Nightstick, (short)35, 1f, EWeaponType.Melee },
                    { EWeaponHash.GrenadeLauncher, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Unarmed, (short)15, 1f, EWeaponType.Melee },
                    { EWeaponHash.BZGas, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.BullpupShotgun, (short)112, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.MG, (short)40, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Knife, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.Pistol50, (short)51, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Bat, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.PoolCue, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.Wrench, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.Grenade, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Flashlight, (short)30, 1f, EWeaponType.Melee },
                    { EWeaponHash.Crowbar, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.CarbineRifle, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.CombatMGMk2, (short)28, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.CombatMG, (short)28, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Firework, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.BullpupRifle, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.SawnOffShotgun, (short)160, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.Railgun, (short)50, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Dagger, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.CombatPistol, (short)27, 1f, EWeaponType.Handgun }
                });

            migrationBuilder.InsertData(
                table: "admin_level_names",
                columns: new[] { "level", "language", "name" },
                values: new object[,]
                {
                    { (short)0, ELanguage.English, "User" },
                    { (short)0, ELanguage.German, "User" },
                    { (short)1, ELanguage.English, "Supporter" },
                    { (short)1, ELanguage.German, "Supporter" },
                    { (short)3, ELanguage.German, "Projektleiter" },
                    { (short)3, ELanguage.English, "Projectleader" },
                    { (short)2, ELanguage.German, "Administrator" },
                    { (short)2, ELanguage.English, "Administrator" }
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
                    { (short)12, ELanguage.English, "Global chat which can be read everywhere." },
                    { (short)19, ELanguage.German, "Gibt dir deine User-Id aus." },
                    { (short)18, ELanguage.German, "Private Nachricht an einen bestimmten Spieler." },
                    { (short)12, ELanguage.German, "Globaler Chat, welcher überall gelesen werden kann." },
                    { (short)21, ELanguage.German, "Entfernt das Ziel aus der Blockliste." },
                    { (short)20, ELanguage.German, "Fügt das Ziel in deine Blocklist ein, sodass du keine Nachrichten mehr von ihm liest, er dich nicht einladen kann usw." },
                    { (short)20, ELanguage.English, "Adds the target into your blocklist so you won't see messages from him, he can't invite you anymore etc." },
                    { (short)21, ELanguage.English, "Removes the target from the blocklist." },
                    { (short)24, ELanguage.German, "Gibt einem Spieler Geld." },
                    { (short)24, ELanguage.English, "Gives money to a player." },
                    { (short)25, ELanguage.German, "Ladet einen Spieler in die eigene Lobby ein (falls möglich)." },
                    { (short)25, ELanguage.English, "Invites a player to your lobby (if possible)." },
                    { (short)19, ELanguage.English, "Outputs your user-id to yourself." },
                    { (short)17, ELanguage.English, "Sends a private chat request or accepts the request of another user." },
                    { (short)18, ELanguage.English, "Private message to a specific player." },
                    { (short)10, ELanguage.German, "Verlässt die jetzige Lobby." },
                    { (short)13, ELanguage.German, "Sendet die Nachricht nur zum eigenen Team." },
                    { (short)13, ELanguage.English, "Sends the message to the current team only." },
                    { (short)14, ELanguage.German, "Gibt die Position des Spielers aus." },
                    { (short)17, ELanguage.German, "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                    { (short)11, ELanguage.English, "Kills the user (suicide)." },
                    { (short)11, ELanguage.German, "Tötet den Nutzer (Selbstmord)." },
                    { (short)15, ELanguage.German, "Sendet eine Nachricht im Privatchat." },
                    { (short)14, ELanguage.English, "Outputs the position of the player." },
                    { (short)15, ELanguage.English, "Sends a message in private chat." },
                    { (short)10, ELanguage.English, "Leaves the current lobby." },
                    { (short)16, ELanguage.English, "Closes a private chat or withdraws a private chat request." },
                    { (short)16, ELanguage.German, "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." }
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
                    { 6, ELanguage.German, "Alle Admin-Regeln mit Ausnahme von Aktivitäts-Pflicht sind auch gültig für VIPs." },
                    { 6, ELanguage.English, "All admin rules with the exception of activity duty are also valid for VIPs." },
                    { 5, ELanguage.German, @"Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,
                frage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit.
                Zu hohe Zeiten sind schlecht, zu niedrige kein Problem." },
                    { 5, ELanguage.English, @"If you are not sure if the time for e.g. Mute or Bann could be too high,
                ask your team leader - if you can't reach someone quickly, choose a lower time.
                Too high times are bad, too low times are no problem." },
                    { 4, ELanguage.German, @"Ausnutzung der Befehle ist strengstens verboten!
                Admin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden." },
                    { 4, ELanguage.English, @"Exploitation of the commands is strictly forbidden!
                Admin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules." },
                    { 2, ELanguage.German, @"Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine.
                Unter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich.
                Die hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby.
                Chats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden." },
                    { 3, ELanguage.English, "Admins have to follow the same rules as players do." },
                    { 2, ELanguage.English, @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none.
                By 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area.
                The chat rules listed here are ONLY for the normal chat in an official lobby.
                Chats in private lobbies can be freely monitored by the lobby owners." },
                    { 1, ELanguage.German, @"Teamen mit gegnerischen Spielern ist strengstens verboten!
                Damit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint.
                Wird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert." },
                    { 1, ELanguage.English, @"Teaming with opposing players is strictly forbidden!
                This means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members.
                If such behaviour is noticed, it can lead to severe penalties and is permanently noted." },
                    { 7, ELanguage.English, "The VIPs are free to decide whether they want to use their rights or not." },
                    { 3, ELanguage.German, "Admins haben genauso die Regeln zu befolgen wie auch die Spieler." },
                    { 7, ELanguage.German, "Den VIPs ist es frei überlassen, ob sie ihre Rechte nutzen wollen oder nicht." }
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
                    { (short)23, ELanguage.German, "Mutet einen Spieler im Voice-Chat." },
                    { (short)23, ELanguage.English, "Mutes a player in the voice-chat." },
                    { (short)3, ELanguage.German, "Bannt einen Spieler vom gesamten Server." },
                    { (short)4, ELanguage.German, "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                    { (short)4, ELanguage.English, "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
                    { (short)3, ELanguage.English, "Bans a player out of the server." },
                    { (short)26, ELanguage.English, "Command for quick testing of codes." },
                    { (short)9, ELanguage.German, "Beendet die jetzige Runde in der jeweiligen Lobby." },
                    { (short)1, ELanguage.German, "Schreibt öffentlich als ein Admin." },
                    { (short)1, ELanguage.English, "Writes public as an admin." },
                    { (short)9, ELanguage.English, "Ends the current round in the lobby." },
                    { (short)2, ELanguage.English, "Writes intern to admins only." },
                    { (short)5, ELanguage.German, "Kickt einen Spieler vom Server." },
                    { (short)5, ELanguage.English, "Kicks a player out of the server." },
                    { (short)2, ELanguage.German, "Schreibt intern nur den Admins." },
                    { (short)6, ELanguage.English, "Bans a player out of the lobby in which the command was used." },
                    { (short)7, ELanguage.German, "Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                    { (short)7, ELanguage.English, "Kicks a player out of the lobby in which the command was used." },
                    { (short)8, ELanguage.German, "Mutet einen Spieler im normalen Chat." },
                    { (short)8, ELanguage.English, "Mutes a player in the normal chat." },
                    { (short)26, ELanguage.German, "Befehl zum schnellen Testen von Codes." },
                    { (short)6, ELanguage.German, "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." }
                });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "id", "amount_lifes", "is_official", "is_temporary", "name", "owner_id", "password", "spawn_again_after_death_ms", "type" },
                values: new object[] { -3, (short)1, true, false, "MapCreateLobby", -1, null, 400, ELobbyType.MapCreateLobby });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "id", "amount_lifes", "is_official", "is_temporary", "name", "owner_id", "password", "spawn_again_after_death_ms", "type" },
                values: new object[] { -2, (short)1, true, false, "GangLobby", -1, null, 400, ELobbyType.GangLobby });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "id", "amount_lifes", "is_official", "is_temporary", "name", "owner_id", "password", "spawn_again_after_death_ms", "type" },
                values: new object[] { -1, (short)1, true, false, "Arena", -1, null, 400, ELobbyType.Arena });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "id", "amount_lifes", "is_official", "is_temporary", "name", "owner_id", "password", "type" },
                values: new object[] { -4, null, true, false, "MainMenu", -1, null, ELobbyType.MainMenu });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "id", "creator_id", "name" },
                values: new object[] { -1, -1, "All" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "id", "creator_id", "name" },
                values: new object[] { -2, -1, "All Normals" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "id", "creator_id", "name" },
                values: new object[] { -3, -1, "All Bombs" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "id", "creator_id", "name" },
                values: new object[] { -4, -1, "All Sniper" });

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
                table: "lobby_map_settings",
                columns: new[] { "lobby_id", "map_limit_time", "map_limit_type" },
                values: new object[] { -1, 10, EMapLimitType.KillAfterTime });

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
                values: new object[] { -1, 8000, 45000, 3000, 5, true, 240, false });

            migrationBuilder.InsertData(
                table: "lobby_weapons",
                columns: new[] { "hash", "lobby", "ammo", "damage", "head_multiplicator" },
                values: new object[,]
                {
                    { EWeaponHash.RPG, -1, 99999, null, null },
                    { EWeaponHash.SpecialCarbine, -1, 99999, null, null },
                    { EWeaponHash.AssaultRifleMk2, -1, 99999, null, null },
                    { EWeaponHash.AssaultRifle, -1, 99999, null, null },
                    { EWeaponHash.SNSPistolMk2, -1, 99999, null, null },
                    { EWeaponHash.SNSPistol, -1, 99999, null, null },
                    { EWeaponHash.MiniSMG, -1, 99999, null, null },
                    { EWeaponHash.PipeBomb, -1, 99999, null, null },
                    { EWeaponHash.AdvancedRifle, -1, 99999, null, null },
                    { EWeaponHash.BZGas, -1, 99999, null, null },
                    { EWeaponHash.Musket, -1, 99999, null, null },
                    { EWeaponHash.NightVision, -1, 99999, null, null },
                    { EWeaponHash.GrenadeLauncher, -1, 99999, null, null },
                    { EWeaponHash.HeavyRevolver, -1, 99999, null, null },
                    { EWeaponHash.BullpupShotgun, -1, 99999, null, null },
                    { EWeaponHash.MG, -1, 99999, null, null },
                    { EWeaponHash.Knife, -1, 99999, null, null },
                    { EWeaponHash.Pistol50, -1, 99999, null, null },
                    { EWeaponHash.Bat, -1, 99999, null, null },
                    { EWeaponHash.ProximityMine, -1, 99999, null, null },
                    { EWeaponHash.HeavyRevolverMk2, -1, 99999, null, null },
                    { EWeaponHash.BattleAxe, -1, 99999, null, null },
                    { EWeaponHash.MarksmanRifle, -1, 99999, null, null },
                    { EWeaponHash.Widowmaker, -1, 99999, null, null },
                    { EWeaponHash.CarbineRifleMK2, -1, 99999, null, null },
                    { EWeaponHash.UnholyHellbringer, -1, 99999, null, null },
                    { EWeaponHash.UpnAtomizer, -1, 99999, null, null },
                    { EWeaponHash.SmokeGrenade, -1, 99999, null, null },
                    { EWeaponHash.Parachute, -1, 99999, null, null },
                    { EWeaponHash.Bottle, -1, 99999, null, null },
                    { EWeaponHash.Hatchet, -1, 99999, null, null },
                    { EWeaponHash.AssaultSMG, -1, 99999, null, null },
                    { EWeaponHash.DoubleBarrelShotgun, -1, 99999, null, null },
                    { EWeaponHash.AssaultShotgun, -1, 99999, null, null },
                    { EWeaponHash.SwitchBlade, -1, 99999, null, null },
                    { EWeaponHash.Machete, -1, 99999, null, null },
                    { EWeaponHash.MarksmanPistol, -1, 99999, null, null },
                    { EWeaponHash.MachinePistol, -1, 99999, null, null },
                    { EWeaponHash.KnuckleDuster, -1, 99999, null, null },
                    { EWeaponHash.HeavyPistol, -1, 99999, null, null },
                    { EWeaponHash.PoolCue, -1, 99999, null, null },
                    { EWeaponHash.MarksmanRifleMk2, -1, 99999, null, null },
                    { EWeaponHash.DoubleActionRevolver, -1, 99999, null, null },
                    { EWeaponHash.Grenade, -1, 99999, null, null },
                    { EWeaponHash.Flashlight, -1, 99999, null, null },
                    { EWeaponHash.SMGMk2, -1, 99999, null, null },
                    { EWeaponHash.Dagger, -1, 99999, null, null },
                    { EWeaponHash.SMG, -1, 99999, null, null },
                    { EWeaponHash.Molotov, -1, 99999, null, null },
                    { EWeaponHash.Baseball, -1, 99999, null, null },
                    { EWeaponHash.APPistol, -1, 99999, null, null },
                    { EWeaponHash.PumpShotgunMk2, -1, 99999, null, null },
                    { EWeaponHash.PumpShotgun, -1, 99999, null, null },
                    { EWeaponHash.PistolMk2, -1, 99999, null, null },
                    { EWeaponHash.Pistol, -1, 99999, null, null },
                    { EWeaponHash.Wrench, -1, 99999, null, null },
                    { EWeaponHash.MicroSMG, -1, 99999, null, null },
                    { EWeaponHash.SweeperShotgun, -1, 99999, null, null },
                    { EWeaponHash.HeavySniperMk2, -1, 99999, null, null },
                    { EWeaponHash.HeavySniper, -1, 99999, null, null },
                    { EWeaponHash.CombatPDW, -1, 99999, null, null },
                    { EWeaponHash.VintagePistol, -1, 99999, null, null },
                    { EWeaponHash.Snowball, -1, 99999, null, null },
                    { EWeaponHash.CompactGrenadeLauncher, -1, 99999, null, null },
                    { EWeaponHash.FireExtinguisher, -1, 99999, null, null },
                    { EWeaponHash.StickyBomb, -1, 99999, null, null },
                    { EWeaponHash.SniperRifle, -1, 99999, null, null },
                    { EWeaponHash.PetrolCan, -1, 99999, null, null },
                    { EWeaponHash.HeavyShotgun, -1, 99999, null, null },
                    { EWeaponHash.Crowbar, -1, 99999, null, null },
                    { EWeaponHash.CarbineRifle, -1, 99999, null, null },
                    { EWeaponHash.CombatMGMk2, -1, 99999, null, null },
                    { EWeaponHash.CombatMG, -1, 99999, null, null },
                    { EWeaponHash.Firework, -1, 99999, null, null },
                    { EWeaponHash.BullpupRifle, -1, 99999, null, null },
                    { EWeaponHash.SawnOffShotgun, -1, 99999, null, null },
                    { EWeaponHash.Railgun, -1, 99999, null, null },
                    { EWeaponHash.Nightstick, -1, 99999, null, null },
                    { EWeaponHash.HomingLauncher, -1, 99999, null, null },
                    { EWeaponHash.CompactRifle, -1, 99999, null, null },
                    { EWeaponHash.Gusenberg, -1, 99999, null, null },
                    { EWeaponHash.CombatPistol, -1, 99999, null, null },
                    { EWeaponHash.Hammer, -1, 99999, null, null },
                    { EWeaponHash.GrenadeLauncherSmoke, -1, 99999, null, null },
                    { EWeaponHash.Flare, -1, 99999, null, null },
                    { EWeaponHash.FlareGun, -1, 99999, null, null },
                    { EWeaponHash.GolfClub, -1, 99999, null, null },
                    { EWeaponHash.Minigun, -1, 99999, null, null },
                    { EWeaponHash.StunGun, -1, 99999, null, null }
                });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "id", "blip_color", "color_b", "color_g", "color_r", "index", "lobby", "name", "skin_hash" },
                values: new object[,]
                {
                    { -4, (short)1, (short)0, (short)0, (short)150, (short)2, -1, "Terrorist", 275618457 },
                    { -5, (short)4, (short)255, (short)255, (short)255, (short)0, -2, "None", 0 },
                    { -2, (short)4, (short)255, (short)255, (short)255, (short)0, -1, "Spectator", 0 },
                    { -3, (short)52, (short)0, (short)150, (short)0, (short)1, -1, "SWAT", -1920001264 },
                    { -1, (short)4, (short)255, (short)255, (short)255, (short)0, -4, "Spectator", 1004114196 }
                });

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
                name: "IX_application_answers_question_id",
                table: "application_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_invitations_admin_id",
                table: "application_invitations",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_invitations_application_id",
                table: "application_invitations",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_questions_admin_id",
                table: "application_questions",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_applications_player_id",
                table: "applications",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_command_alias_command",
                table: "command_alias",
                column: "command");

            migrationBuilder.CreateIndex(
                name: "IX_commands_needed_admin_level",
                table: "commands",
                column: "needed_admin_level");

            migrationBuilder.CreateIndex(
                name: "IX_gang_members_gang_id",
                table: "gang_members",
                column: "gang_id");

            migrationBuilder.CreateIndex(
                name: "IX_gang_members_rank_navigation_gang_id_rank_navigation_rank",
                table: "gang_members",
                columns: new[] { "rank_navigation_gang_id", "rank_navigation_rank" });

            migrationBuilder.CreateIndex(
                name: "IX_gangs_owner_id",
                table: "gangs",
                column: "owner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gangs_team_id",
                table: "gangs",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_gangwar_areas_owner_gang_id",
                table: "gangwar_areas",
                column: "owner_gang_id");

            migrationBuilder.CreateIndex(
                name: "IX_lobbies_owner_id",
                table: "lobbies",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_lobby_maps_map_id",
                table: "lobby_maps",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "IX_lobby_weapons_lobby",
                table: "lobby_weapons",
                column: "lobby");

            migrationBuilder.CreateIndex(
                name: "IX_maps_creator_id",
                table: "maps",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_maps_name",
                table: "maps",
                column: "name")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_offlinemessages_source_id",
                table: "offlinemessages",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "IX_offlinemessages_target_id",
                table: "offlinemessages",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_admin_id",
                table: "player_bans",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_ip",
                table: "player_bans",
                column: "ip");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_lobby_id",
                table: "player_bans",
                column: "lobby_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_sc_id",
                table: "player_bans",
                column: "sc_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_sc_name",
                table: "player_bans",
                column: "sc_name");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_serial",
                table: "player_bans",
                column: "serial");

            migrationBuilder.CreateIndex(
                name: "IX_player_lobby_stats_lobby_id",
                table: "player_lobby_stats",
                column: "lobby_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_map_favourites_map_id",
                table: "player_map_favourites",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_map_ratings_map_id",
                table: "player_map_ratings",
                column: "map_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_relations_target_id",
                table: "player_relations",
                column: "target_id");

            migrationBuilder.CreateIndex(
                name: "IX_players_admin_leader_id",
                table: "players",
                column: "admin_leader_id");

            migrationBuilder.CreateIndex(
                name: "IX_players_admin_lvl",
                table: "players",
                column: "admin_lvl");

            migrationBuilder.CreateIndex(
                name: "IX_support_request_messages_author_id",
                table: "support_request_messages",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_support_requests_author_id",
                table: "support_requests",
                column: "author_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_lobby",
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

﻿using System;
using System.Net;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:e_language", "german,english")
                .Annotation("Npgsql:Enum:e_lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby")
                .Annotation("Npgsql:Enum:e_log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto")
                .Annotation("Npgsql:Enum:e_player_relation", "none,block,friend")
                .Annotation("Npgsql:Enum:e_weapon_hash", "sniper_rifle,fire_extinguisher,compact_grenade_launcher,snowball,vintage_pistol,combat_pdw,heavy_sniper,sweeper_shotgun,micro_smg,wrench,pistol,pump_shotgun,ap_pistol,ball,molotov,smg,sticky_bomb,petrol_can,stun_gun,heavy_shotgun,minigun,golf_club,flare_gun,flare,grenade_launcher_smoke,hammer,combat_pistol,gusenberg,compact_rifle,homing_launcher,nightstick,railgun,sawn_off_shotgun,bullpup_rifle,firework,combat_mg,carbine_rifle,crowbar,flashlight,dagger,grenade,pool_cue,bat,pistol50,knife,mg,bullpup_shotgun,bz_gas,unarmed,grenade_launcher,night_vision,musket,proximity_mine,advanced_rifle,rpg,pipe_bomb,mini_smg,sns_pistol,assault_rifle,special_carbine,revolver,marksman_rifle,battle_axe,heavy_pistol,knuckle_duster,machine_pistol,marksman_pistol,machete,switch_blade,assault_shotgun,double_barrel_shotgun,assault_smg,hatchet,bottle,parachute,smoke_grenade,upn_atomizer,unholy_hellbringer,carbine_rifle_m_k2,sepcial_carbine_m_k2,bullpup_rifle_m_k2")
                .Annotation("Npgsql:Enum:e_weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .Annotation("Npgsql:Enum:vehicle_hash", "adder,airbus,airtug,akuma,alpha,alpha_z1,ambulance,annihilator,apc,ardent,army_tanker,army_trailer,army_trailer2,asea,asea2,asterope,avarus,bagger,bale_trailer,baller,baller2,baller3,baller4,baller5,baller6,banshee,banshee2,barracks,barracks2,barracks3,bati,bati2,benson,besra,bestia_gts,b_f400,bf_injection,biff,bifta,bison,bison2,bison3,bjxl,blade,blazer,blazer2,blazer3,blazer4,blazer5,blimp,blimp2,blista,blista2,blista3,bmx,boat_trailer,bobcat_xl,bodhi2,bombushka,boxville,boxville2,boxville3,boxville4,boxville5,brawler,brickade,brioso,b_type,b_type2,b_type3,buccaneer,buccaneer2,buffalo,buffalo2,buffalo3,bulldozer,bullet,burrito,burrito2,burrito3,burrito4,burrito5,bus,buzzard,buzzard2,cable_car,caddy,caddy2,caddy3,camper,carbonizzare,carbon_rs,cargobob,cargobob2,cargobob3,cargobob4,cargo_plane,casco,cavalcade,cavalcade2,cheetah,cheetah2,chimera,chino,chino2,cliffhanger,coach,cog55,cog552,cog_cabrio,cognoscenti,cognoscenti2,comet2,comet3,contender,coquette,coquette2,coquette3,cruiser,crusader,cuban800,cutter,cyclone,daemon,daemon2,defiler,diablous,diablous2,dilettante,dilettante2,dinghy,dinghy2,dinghy3,dinghy4,d_loader,dock_trailer,docktug,dodo,dominator,dominator2,double,dubsta,dubsta2,dubsta3,dukes,dukes2,dump,dune,dune2,dune3,dune4,dune5,duster,elegy,elegy2,emperor,emperor2,emperor3,enduro,entity_xf,esskey,exemplar,f620,faction,faction2,faction3,faggio,faggio2,faggio3,fbi,fb_i2,fcr,fc_r2,felon,felon2,feltzer2,feltzer3,fire_truck,fixter,flatbed,forklift,fmj,f_q2,freight,freight_car,freight_cont1,freight_cont2,freight_grain,freight_trailer,frogger,frogger2,fugitive,furoregt,fusilade,futo,gargoyle,gauntlet,gauntlet2,g_burrito,g_burrito2,glendale,g_p1,grain_trailer,granger,gresley,guardian,habanero,hakuchou,hakuchou2,half_track,handler,hauler,hauler2,havok,hexer,hotknife,howard,hunter,huntley,hydra,infernus,infernus2,ingot,innovation,insurgent,insurgent2,insurgent3,intruder,issi2,itali_gtb,itali_gt_b2,jackal,j_b700,jester,jester2,jet,jetmax,journey,kalahari,khamelion,kuruma,kuruma2,landstalker,lazer,lectro,lguard,limo2,lurcher,luxor,luxor2,lynx,mamba,mammatus,manana,manchez,marquis,marshall,massacro,massacro2,maverick,mesa,mesa2,mesa3,metro_train,microlight,miljet,minivan,minivan2,mixer,mixer2,mogul,molotok,monroe,monster,moonbeam,moonbeam2,mower,mule,mule2,mule3,nemesis,nero,nero2,nightblade,nightshade,night_shark,nimbus,ninef,ninef2,nokota,omnis,oppressor,oracle,oracle2,osiris,packer,panto,paradise,patriot,p_bus,pcj,penetrator,penumbra,peyote,pfister811,phantom,phantom2,phantom3,phoenix,picador,pigalle,police,police2,police3,police4,policeb,police_old1,police_old2,police_t,polmav,pony,pony2,pounder,prairie,pranger,predator,premier,primo,primo2,prop_trailer,prototipo,pyro,radi,rake_trailer,rancher_xl,rancher_x_l2,rally_truck,rapid_gt,rapid_g_t2,rapid_g_t3,raptor,rat_bike,rat_loader,rat_loader2,r_e7_b,reaper,rebel,rebel2,regina,rental_bus,retinue,rhapsody,rhino,riot,ripley,rocoto,romero,rogue,rubble,ruffian,ruiner,ruiner2,ruiner3,rumpo,rumpo2,rumpo3,ruston,sabre_gt,sabre_g_t2,sadler,sadler2,sanchez,sanchez2,sanctus,sandking,sandking2,savage,schafter2,schafter3,schafter4,schafter5,schafter6,schwarzer,scorcher,scrap,seabreeze,seashark,seashark2,seashark3,seminole,sentinel,sentinel2,serrano,seven70,shamal,sheava,sheriff,sheriff2,shotaro,skylift,slam_van,slam_van2,slam_van3,sovereign,specter,specter2,speeder,speeder2,speedo,speedo2,squalo,stalion,stalion2,stanier,starling,stinger,stinger_gt,stockade,stockade3,stratum,stretch,stunt,submersible,submersible2,sultan,sultan_rs,suntrap,superd,supervolito,supervolito2,surano,surfer,surfer2,surge,swift2,swift,t20,taco,tailgater,tampa,tampa2,tampa3,tanker,tanker2,tanker_car,taxi,technical,technical2,technical3,tempesta,thrust,tip_truck,tip_truck2,titan,torero,tornado,tornado2,tornado3,tornado4,tornado5,tornado6,toro,toro2,tourbus,tow_truck,tow_truck2,t_r2,t_r3,t_r4,tractor,tractor2,tractor3,trailer_logs,trailer_large,trailers,trailers2,trailers3,trailers4,trailer_small,trailer_small2,trash,trash2,tr_flat,tri_bike,tri_bike2,tri_bike3,trophy_truck,trophy_truck2,tropic,tropic2,tropos,tug,tula,turismor,turismo2,tv_trailer,tyrus,utilli_truck,utilli_truck2,utilli_truck3,vacca,vader,vagner,valkyrie,valkyrie2,velum,velum2,verlierer2,vestra,vigero,vigilante,vindicator,virgo,virgo2,virgo3,visione,volatus,voltic,voltic2,voodoo,voodoo2,vortex,warrener,washington,wastelander,windsor,windsor2,wolfsbane,x_a21,xls,xl_s2,youga,youga2,zentorno,zion,zion2,zombie_a,zombie_b,z_type,akula,autarch,avenger,avenge_r2,barrage,chernobog,come_t4,come_t5,deluxo,g_t500,hermes,hustler,kamacho,khanjali,neon,pariah,raiden,revolter,riata,rio_t2,savestra,s_c1,sentine_l3,streiter,stromberg,thruster,viseris,volatol,yosemite,z190,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2");

            migrationBuilder.CreateSequence<short>(
                name: "ID_ID_seq");

            migrationBuilder.CreateSequence<int>(
                name: "lobbies_ID_seq");

            migrationBuilder.CreateSequence<int>(
                name: "maps_ID_seq");

            migrationBuilder.CreateSequence<short>(
                name: "ServerSettings_ID_seq");

            migrationBuilder.CreateTable(
                name: "admin_levels",
                columns: table => new
                {
                    Level = table.Column<short>(nullable: false),
                    ColorR = table.Column<short>(nullable: false),
                    ColorG = table.Column<short>(nullable: false),
                    ColorB = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("admin_levels_pkey", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "freeroam_default_vehicle",
                columns: table => new
                {
                    VehicleType = table.Column<EFreeroamVehicleType>(nullable: false),
                    VehicleHash = table.Column<VehicleHash>(nullable: false),
                    Note = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_freeroam_default_vehicle", x => x.VehicleType);
                });

            migrationBuilder.CreateTable(
                name: "log_admins",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type = table.Column<ELogType>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: true),
                    Lobby = table.Column<int>(nullable: true),
                    AsDonator = table.Column<bool>(nullable: false),
                    AsVIP = table.Column<bool>(nullable: false),
                    Reason = table.Column<string>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_admins", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "log_chats",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Source = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    Lobby = table.Column<int>(nullable: true),
                    IsAdminChat = table.Column<bool>(nullable: false),
                    IsTeamChat = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_chats", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "log_errors",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Source = table.Column<int>(nullable: true),
                    Info = table.Column<string>(nullable: false),
                    StackTrace = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_errors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "log_rests",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type = table.Column<ELogType>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Serial = table.Column<string>(maxLength: 200, nullable: true),
                    IP = table.Column<IPAddress>(nullable: true),
                    Lobby = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_rests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "server_settings",
                columns: table => new
                {
                    ID = table.Column<short>(nullable: false, defaultValueSql: "nextval('\"ServerSettings_ID_seq\"'::regclass)"),
                    GamemodeName = table.Column<string>(maxLength: 50, nullable: false),
                    MapsPath = table.Column<string>(maxLength: 300, nullable: false),
                    NewMapsPath = table.Column<string>(maxLength: 300, nullable: false),
                    ErrorToPlayerOnNonExistentCommand = table.Column<bool>(nullable: false),
                    ToChatOnNonExistentCommand = table.Column<bool>(nullable: false),
                    DistanceToSpotToPlant = table.Column<float>(nullable: false),
                    DistanceToSpotToDefuse = table.Column<float>(nullable: false),
                    SavePlayerDataCooldownMinutes = table.Column<int>(nullable: false),
                    SaveLogsCooldownMinutes = table.Column<int>(nullable: false),
                    SaveSeasonsCooldownMinutes = table.Column<int>(nullable: false),
                    TeamOrderCooldownMs = table.Column<int>(nullable: false),
                    ArenaNewMapProbabilityPercent = table.Column<float>(nullable: false),
                    SavedMapsPath = table.Column<string>(maxLength: 300, nullable: false, defaultValueSql: "'bridge/resources/tds/savedmaps/'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "weapons",
                columns: table => new
                {
                    Hash = table.Column<EWeaponHash>(nullable: false),
                    Type = table.Column<EWeaponType>(nullable: false),
                    DefaultDamage = table.Column<short>(nullable: false),
                    DefaultHeadMultiplicator = table.Column<float>(nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("weapons_pkey", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "admin_level_names",
                columns: table => new
                {
                    Level = table.Column<short>(nullable: false),
                    Language = table.Column<ELanguage>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_level_names", x => new { x.Level, x.Language });
                    table.ForeignKey(
                        name: "FK_admin_level_names_admin_level",
                        column: x => x.Level,
                        principalTable: "admin_levels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commands",
                columns: table => new
                {
                    ID = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Command = table.Column<string>(maxLength: 50, nullable: false),
                    NeededAdminLevel = table.Column<short>(nullable: true),
                    NeededDonation = table.Column<short>(nullable: true),
                    VipCanUse = table.Column<bool>(nullable: false),
                    LobbyOwnerCanUse = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commands", x => x.ID);
                    table.ForeignKey(
                        name: "FK_commands_admin_levels",
                        column: x => x.NeededAdminLevel,
                        principalTable: "admin_levels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "command_alias",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 100, nullable: false),
                    Command = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("command_alias_pkey", x => new { x.Alias, x.Command });
                    table.ForeignKey(
                        name: "command_alias_Command_fkey",
                        column: x => x.Command,
                        principalTable: "commands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "command_infos",
                columns: table => new
                {
                    ID = table.Column<short>(nullable: false),
                    Language = table.Column<ELanguage>(nullable: false),
                    Info = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("command_infos_pkey", x => new { x.ID, x.Language });
                    table.ForeignKey(
                        name: "command_infos_ID_fkey",
                        column: x => x.ID,
                        principalTable: "commands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SCName = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    AdminLvl = table.Column<short>(nullable: false),
                    IsVIP = table.Column<bool>(nullable: false),
                    Donation = table.Column<short>(nullable: false),
                    GangId = table.Column<int>(nullable: true),
                    RegisterTimestamp = table.Column<DateTime>(type: "timestamp(4) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.ID);
                    table.ForeignKey(
                        name: "players_AdminLvl_fkey",
                        column: x => x.AdminLvl,
                        principalTable: "admin_levels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobbies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Owner = table.Column<int>(nullable: false),
                    Type = table.Column<ELobbyType>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    StartHealth = table.Column<short>(nullable: false, defaultValueSql: "100"),
                    StartArmor = table.Column<short>(nullable: false, defaultValueSql: "100"),
                    AmountLifes = table.Column<short>(nullable: true),
                    DefaultSpawnX = table.Column<float>(nullable: false),
                    DefaultSpawnY = table.Column<float>(nullable: false),
                    DefaultSpawnZ = table.Column<float>(nullable: false, defaultValueSql: "900"),
                    AroundSpawnPoint = table.Column<float>(nullable: false, defaultValueSql: "3"),
                    DefaultSpawnRotation = table.Column<float>(nullable: false),
                    IsTemporary = table.Column<bool>(nullable: false),
                    IsOfficial = table.Column<bool>(nullable: false),
                    SpawnAgainAfterDeathMs = table.Column<int>(nullable: false, defaultValueSql: "400"),
                    CreateTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DieAfterOutsideMapLimitTime = table.Column<int>(nullable: false, defaultValueSql: "10")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lobbies", x => x.Id);
                    table.ForeignKey(
                        name: "lobbies_Owner_fkey",
                        column: x => x.Owner,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"maps_ID_seq\"'::regclass)"),
                    Name = table.Column<string>(nullable: false),
                    CreatorId = table.Column<int>(nullable: true),
                    CreateTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maps", x => x.Id);
                    table.ForeignKey(
                        name: "maps_CreatorID_fkey",
                        column: x => x.CreatorId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "offlinemessages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TargetID = table.Column<int>(nullable: false),
                    SourceID = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offlinemessages", x => x.ID);
                    table.ForeignKey(
                        name: "offlinemessages_SourceID_fkey",
                        column: x => x.SourceID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "offlinemessages_TargetID_fkey",
                        column: x => x.TargetID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_relations",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    Relation = table.Column<EPlayerRelation>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_relations_pkey", x => new { x.PlayerId, x.TargetId });
                    table.ForeignKey(
                        name: "player_relations_PlayerId_fkey",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_relations_TargetId_fkey",
                        column: x => x.TargetId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_settings",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    Language = table.Column<ELanguage>(nullable: false),
                    Hitsound = table.Column<bool>(nullable: false),
                    Bloodscreen = table.Column<bool>(nullable: false),
                    FloatingDamageInfo = table.Column<bool>(nullable: false),
                    AllowDataTransfer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_settings_pkey", x => x.PlayerID);
                    table.ForeignKey(
                        name: "player_settings_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_stats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    Money = table.Column<int>(nullable: false),
                    PlayTime = table.Column<int>(nullable: false),
                    MuteTime = table.Column<int>(nullable: true),
                    LoggedIn = table.Column<bool>(nullable: false),
                    LastLoginTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_stats_pkey", x => x.PlayerID);
                    table.ForeignKey(
                        name: "player_stats_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "killingspree_rewards",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    KillsAmount = table.Column<short>(nullable: false),
                    HealthOrArmor = table.Column<short>(nullable: true),
                    OnlyHealth = table.Column<short>(nullable: true),
                    OnlyArmor = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("killingspree_rewards_pkey", x => new { x.LobbyId, x.KillsAmount });
                    table.ForeignKey(
                        name: "lobby_killingspree_rewards_LobbyID_fkey",
                        column: x => x.LobbyId,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_rewards",
                columns: table => new
                {
                    LobbyID = table.Column<int>(nullable: false),
                    MoneyPerKill = table.Column<double>(nullable: false),
                    MoneyPerAssist = table.Column<double>(nullable: false),
                    MoneyPerDamage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lobby_rewards_pkey", x => x.LobbyID);
                    table.ForeignKey(
                        name: "lobby_rewards_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_round_settings",
                columns: table => new
                {
                    LobbyID = table.Column<int>(nullable: false),
                    RoundTime = table.Column<int>(nullable: false, defaultValueSql: "240"),
                    CountdownTime = table.Column<int>(nullable: false, defaultValueSql: "5"),
                    BombDetonateTimeMs = table.Column<int>(nullable: false, defaultValueSql: "45000"),
                    BombDefuseTimeMs = table.Column<int>(nullable: false, defaultValueSql: "8000"),
                    BombPlantTimeMs = table.Column<int>(nullable: false, defaultValueSql: "3000"),
                    MixTeamsAfterRound = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lobby_round_infos_pkey", x => x.LobbyID);
                    table.ForeignKey(
                        name: "lobby_round_infos_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_weapons",
                columns: table => new
                {
                    Hash = table.Column<EWeaponHash>(nullable: false),
                    Lobby = table.Column<int>(nullable: false),
                    Ammo = table.Column<int>(nullable: false),
                    Damage = table.Column<short>(nullable: true),
                    HeadMultiplicator = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lobby_weapons_pkey", x => x.Hash);
                    table.ForeignKey(
                        name: "lobby_weapons_Hash_fkey",
                        column: x => x.Hash,
                        principalTable: "weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "lobby_weapons_Lobby_fkey",
                        column: x => x.Lobby,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_bans",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    LobbyId = table.Column<int>(nullable: false),
                    AdminId = table.Column<int>(nullable: true),
                    Reason = table.Column<string>(nullable: false),
                    StartTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    EndTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_bans_pkey", x => new { x.PlayerId, x.LobbyId });
                    table.ForeignKey(
                        name: "player_bans_AdminID_fkey",
                        column: x => x.AdminId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "player_bans_LobbyID_fkey",
                        column: x => x.LobbyId,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_bans_PlayerID_fkey",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_lobby_stats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    LobbyID = table.Column<int>(nullable: false),
                    Kills = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    Deaths = table.Column<int>(nullable: false),
                    Damage = table.Column<int>(nullable: false),
                    TotalKills = table.Column<int>(nullable: false),
                    TotalAssists = table.Column<int>(nullable: false),
                    TotalDeaths = table.Column<int>(nullable: false),
                    TotalDamage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_lobby_stats_pkey", x => new { x.PlayerID, x.LobbyID });
                    table.ForeignKey(
                        name: "player_lobby_stats_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_lobby_stats_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Lobby = table.Column<int>(nullable: false),
                    ColorR = table.Column<short>(nullable: false),
                    ColorG = table.Column<short>(nullable: false),
                    ColorB = table.Column<short>(nullable: false),
                    BlipColor = table.Column<short>(nullable: false),
                    SkinHash = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.ID);
                    table.ForeignKey(
                        name: "teams_Lobby_fkey",
                        column: x => x.Lobby,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_maps",
                columns: table => new
                {
                    LobbyID = table.Column<int>(nullable: false),
                    MapID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lobby_maps_pkey", x => new { x.LobbyID, x.MapID });
                    table.ForeignKey(
                        name: "lobby_maps_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lobby_maps_maps",
                        column: x => x.MapID,
                        principalTable: "maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_map_favourites",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    MapID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_map_favourites_pkey", x => new { x.PlayerID, x.MapID });
                    table.ForeignKey(
                        name: "player_map_favourites_MapID_fkey",
                        column: x => x.MapID,
                        principalTable: "maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_map_favourites_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_map_ratings",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    MapID = table.Column<int>(nullable: false),
                    Rating = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_map_ratings_pkey", x => new { x.PlayerID, x.MapID });
                    table.ForeignKey(
                        name: "player_map_ratings_MapID_fkey",
                        column: x => x.MapID,
                        principalTable: "maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_map_ratings_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gangs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeamID = table.Column<int>(nullable: false),
                    Short = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gangs", x => x.ID);
                    table.ForeignKey(
                        name: "gangs_TeamID_fkey",
                        column: x => x.TeamID,
                        principalTable: "teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "admin_levels",
                columns: new[] { "Level", "ColorB", "ColorG", "ColorR" },
                values: new object[,]
                {
                    { (short)0, (short)220, (short)220, (short)220 },
                    { (short)1, (short)113, (short)202, (short)113 },
                    { (short)2, (short)85, (short)132, (short)253 },
                    { (short)3, (short)50, (short)50, (short)222 }
                });

            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "ID", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[,]
                {
                    { (short)10, "LobbyLeave", false, null, null, false },
                    { (short)11, "Suicide", false, null, null, false },
                    { (short)12, "GlobalChat", false, null, null, false },
                    { (short)13, "TeamChat", false, null, null, false },
                    { (short)14, "PrivateChat", false, null, null, false },
                    { (short)15, "Position", false, null, null, false },
                    { (short)16, "ClosePrivateChat", false, null, null, false },
                    { (short)17, "OpenPrivateChat", false, null, null, false },
                    { (short)18, "PrivateMessage", false, null, null, false },
                    { (short)19, "UserId", false, null, null, false }
                });

            migrationBuilder.InsertData(
                table: "freeroam_default_vehicle",
                columns: new[] { "VehicleType", "Note", "VehicleHash" },
                values: new object[,]
                {
                    { EFreeroamVehicleType.Bike, null, VehicleHash.Hakuchou2 },
                    { EFreeroamVehicleType.Plane, null, VehicleHash.Pyro },
                    { EFreeroamVehicleType.Boat, null, VehicleHash.Speeder2 },
                    { EFreeroamVehicleType.Car, null, VehicleHash.Pfister811 },
                    { EFreeroamVehicleType.Helicopter, null, VehicleHash.AKULA }
                });

            migrationBuilder.InsertData(
                table: "server_settings",
                columns: new[] { "ID", "ArenaNewMapProbabilityPercent", "DistanceToSpotToDefuse", "DistanceToSpotToPlant", "ErrorToPlayerOnNonExistentCommand", "GamemodeName", "MapsPath", "NewMapsPath", "SaveLogsCooldownMinutes", "SavePlayerDataCooldownMinutes", "SaveSeasonsCooldownMinutes", "SavedMapsPath", "TeamOrderCooldownMs", "ToChatOnNonExistentCommand" },
                values: new object[] { (short)1, 2f, 3f, 3f, true, "tdm", "bridge/resources/tds/maps/", "bridge/resources/tds/newmaps/", 1, 1, 1, "bridge/resources/tds/savedmaps/", 3000, false });

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "Hash", "DefaultDamage", "DefaultHeadMultiplicator", "Type" },
                values: new object[,]
                {
                    { EWeaponHash.MiniSMG, (short)22, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.PipeBomb, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.RPG, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.AdvancedRifle, (short)30, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.ProximityMine, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Musket, (short)165, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.NightVision, (short)0, 1f, EWeaponType.Rest },
                    { EWeaponHash.GrenadeLauncher, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Unarmed, (short)15, 1f, EWeaponType.Melee },
                    { EWeaponHash.BullpupShotgun, (short)112, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.MG, (short)40, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Knife, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.Pistol50, (short)51, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Bat, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.PoolCue, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.SNSPistol, (short)28, 1f, EWeaponType.Handgun },
                    { EWeaponHash.BZGas, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.AssaultRifle, (short)30, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.SwitchBlade, (short)50, 1f, EWeaponType.Melee },
                    { EWeaponHash.Revolver, (short)110, 1f, EWeaponType.Handgun },
                    { EWeaponHash.UpnAtomizer, (short)80, 1f, EWeaponType.Handgun },
                    { EWeaponHash.SmokeGrenade, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Parachute, (short)0, 1f, EWeaponType.Rest },
                    { EWeaponHash.Bottle, (short)10, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Hatchet, (short)50, 1f, EWeaponType.Melee },
                    { EWeaponHash.AssaultSMG, (short)23, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.DoubleBarrelShotgun, (short)166, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.SpecialCarbine, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.AssaultShotgun, (short)192, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.Machete, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.MarksmanPistol, (short)150, 1f, EWeaponType.Handgun },
                    { EWeaponHash.MachinePistol, (short)20, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.KnuckleDuster, (short)30, 1f, EWeaponType.Melee },
                    { EWeaponHash.HeavyPistol, (short)40, 1f, EWeaponType.Handgun },
                    { EWeaponHash.BattleAxe, (short)50, 1f, EWeaponType.Melee },
                    { EWeaponHash.MarksmanRifle, (short)65, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.Grenade, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Dagger, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.HomingLauncher, (short)150, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Crowbar, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.SMG, (short)22, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Molotov, (short)10, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.Ball, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.APPistol, (short)28, 1f, EWeaponType.Handgun },
                    { EWeaponHash.PumpShotgun, (short)58, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.Pistol, (short)26, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Wrench, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.StickyBomb, (short)100, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.MicroSMG, (short)21, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.HeavySniper, (short)216, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.CombatPDW, (short)28, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.VintagePistol, (short)34, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Snowball, (short)10, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.CompactGrenadeLauncher, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.FireExtinguisher, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.SniperRifle, (short)101, 2f, EWeaponType.SniperRifle },
                    { EWeaponHash.SweeperShotgun, (short)162, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.PetrolCan, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.StunGun, (short)0, 1f, EWeaponType.Handgun },
                    { EWeaponHash.HeavyShotgun, (short)117, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.CarbineRifle, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.CombatMG, (short)28, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Firework, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.BullpupRifle, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.SawnOffShotgun, (short)160, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.Railgun, (short)50, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Nightstick, (short)35, 1f, EWeaponType.Melee },
                    { EWeaponHash.UnholyHellbringer, (short)23, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.CompactRifle, (short)34, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.Gusenberg, (short)34, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.CombatPistol, (short)27, 1f, EWeaponType.Handgun },
                    { EWeaponHash.Hammer, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.GrenadeLauncherSmoke, (short)0, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Flare, (short)0, 1f, EWeaponType.ThrownWeapon },
                    { EWeaponHash.FlareGun, (short)50, 1f, EWeaponType.Handgun },
                    { EWeaponHash.GolfClub, (short)40, 1f, EWeaponType.Melee },
                    { EWeaponHash.Minigun, (short)30, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Flashlight, (short)30, 1f, EWeaponType.Melee },
                    { EWeaponHash.CarbineRifleMK2, (short)32, 1f, EWeaponType.AssaultRifle }
                });

            migrationBuilder.InsertData(
                table: "admin_level_names",
                columns: new[] { "Level", "Language", "Name" },
                values: new object[,]
                {
                    { (short)0, ELanguage.English, "User" },
                    { (short)2, ELanguage.German, "Administrator" },
                    { (short)2, ELanguage.English, "Administrator" },
                    { (short)3, ELanguage.English, "Projectleader" },
                    { (short)3, ELanguage.German, "Projektleiter" },
                    { (short)1, ELanguage.English, "Supporter" },
                    { (short)0, ELanguage.German, "User" },
                    { (short)1, ELanguage.German, "Supporter" }
                });

            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "GChat", (short)12 },
                    { "GlobalSay", (short)12 },
                    { "PublicChat", (short)12 },
                    { "StopPrivateChat", (short)16 },
                    { "ClosePrivateSay", (short)16 },
                    { "TChat", (short)13 },
                    { "G", (short)12 },
                    { "TeamSay", (short)13 },
                    { "TSay", (short)13 },
                    { "PublicSay", (short)12 },
                    { "CPC", (short)16 },
                    { "AllSay", (short)12 },
                    { "PChat", (short)14 },
                    { "PrivateSay", (short)14 },
                    { "Coord", (short)15 },
                    { "Coordinate", (short)15 },
                    { "Coordinates", (short)15 },
                    { "CurrentPos", (short)15 },
                    { "CurrentPosition", (short)15 },
                    { "GetPos", (short)15 },
                    { "ClosePM", (short)16 },
                    { "AllChat", (short)12 },
                    { "Kill", (short)11 },
                    { "OPC", (short)17 },
                    { "UID", (short)19 },
                    { "PSay", (short)18 },
                    { "PM", (short)18 },
                    { "MSG", (short)18 },
                    { "OpenPM", (short)17 },
                    { "Back", (short)10 },
                    { "Leave", (short)10 },
                    { "Pos", (short)15 },
                    { "Mainmenu", (short)10 },
                    { "OpenPrivateSay", (short)17 },
                    { "StopPrivateSay", (short)17 },
                    { "Dead", (short)11 },
                    { "Death", (short)11 },
                    { "Die", (short)11 },
                    { "GetPosition", (short)15 },
                    { "LeaveLobby", (short)10 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "ID", "Language", "Info" },
                values: new object[,]
                {
                    { (short)16, ELanguage.German, "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." },
                    { (short)17, ELanguage.German, "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                    { (short)17, ELanguage.English, "Sends a private chat request or accepts the request of another user." },
                    { (short)15, ELanguage.English, "Sends a message in private chat." },
                    { (short)15, ELanguage.German, "Sendet eine Nachricht im Privatchat." },
                    { (short)18, ELanguage.German, "Private Nachricht an einen bestimmten Spieler." },
                    { (short)18, ELanguage.English, "Private message to a specific player." },
                    { (short)16, ELanguage.English, "Closes a private chat or withdraws a private chat request." },
                    { (short)12, ELanguage.English, "Global chat which can be read everywhere." },
                    { (short)14, ELanguage.German, "Gibt die Position des Spielers aus." },
                    { (short)14, ELanguage.English, "Outputs the position of the player." },
                    { (short)10, ELanguage.German, "Verlässt die jetzige Lobby." },
                    { (short)10, ELanguage.English, "Leaves the current lobby." },
                    { (short)11, ELanguage.German, "Tötet den Nutzer (Selbstmord)." },
                    { (short)19, ELanguage.English, "Outputs your user-id to yourself." },
                    { (short)12, ELanguage.German, "Globaler Chat, welcher überall gelesen werden kann." },
                    { (short)19, ELanguage.German, "Gibt dir deine User-Id aus." },
                    { (short)13, ELanguage.German, "Sendet die Nachricht nur zum eigenen Team." },
                    { (short)13, ELanguage.English, "Sends the message to the current team only." },
                    { (short)11, ELanguage.English, "Kills the user (suicide)." }
                });

            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "ID", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[,]
                {
                    { (short)4, "Goto", true, (short)2, null, false },
                    { (short)9, "NextMap", true, (short)1, null, true },
                    { (short)8, "Mute", false, (short)1, null, true },
                    { (short)7, "LobbyKick", true, (short)1, null, true },
                    { (short)6, "LobbyBan", true, (short)1, null, true },
                    { (short)5, "Kick", false, (short)1, null, true },
                    { (short)2, "AdminChat", false, (short)1, null, true },
                    { (short)1, "AdminSay", false, (short)1, null, false },
                    { (short)3, "Ban", false, (short)2, null, false }
                });

            migrationBuilder.InsertData(
                table: "players",
                columns: new[] { "ID", "AdminLvl", "Donation", "Email", "GangId", "IsVIP", "Name", "Password", "SCName" },
                values: new object[] { 0, (short)0, (short)0, null, null, false, "System", "", "System" });

            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "PermaMute", (short)8 },
                    { "Skip", (short)9 },
                    { "TMute", (short)8 },
                    { "TimeMute", (short)8 },
                    { "RMute", (short)8 },
                    { "PMute", (short)8 },
                    { "PBan", (short)3 },
                    { "Permaban", (short)3 },
                    { "KickLobby", (short)7 },
                    { "RBan", (short)3 },
                    { "TBan", (short)3 },
                    { "BanLobby", (short)6 },
                    { "Timeban", (short)3 },
                    { "UBan", (short)3 },
                    { "RKick", (short)5 },
                    { "EndRound", (short)9 },
                    { "UnBan", (short)3 },
                    { "Announcement", (short)1 },
                    { "XYZ", (short)4 },
                    { "WarpToPlayer", (short)4 },
                    { "WarpTo", (short)4 },
                    { "Warp", (short)4 },
                    { "GotoXYZ", (short)4 },
                    { "Announce", (short)1 },
                    { "WriteAdmin", (short)2 },
                    { "Next", (short)9 },
                    { "OChat", (short)1 },
                    { "OSay", (short)1 },
                    { "GotoPlayer", (short)4 },
                    { "AChat", (short)2 },
                    { "ChatAdmin", (short)2 },
                    { "InternChat", (short)2 },
                    { "ASay", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "ID", "Language", "Info" },
                values: new object[,]
                {
                    { (short)9, ELanguage.English, "Ends the current round in the lobby." },
                    { (short)3, ELanguage.English, "Bans a player out of the server." },
                    { (short)9, ELanguage.German, "Beendet die jetzige Runde in der jeweiligen Lobby." },
                    { (short)3, ELanguage.German, "Bannt einen Spieler vom gesamten Server." },
                    { (short)4, ELanguage.English, "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
                    { (short)8, ELanguage.German, "Mutet einen Spieler im normalen Chat." },
                    { (short)1, ELanguage.German, "Schreibt öffentlich als ein Admin." },
                    { (short)1, ELanguage.English, "Writes public as an admin." },
                    { (short)8, ELanguage.English, "Mutes a player in the normal chat." },
                    { (short)2, ELanguage.English, "Writes intern to admins only." },
                    { (short)5, ELanguage.German, "Kickt einen Spieler vom Server." },
                    { (short)2, ELanguage.German, "Schreibt intern nur den Admins." },
                    { (short)6, ELanguage.German, "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                    { (short)6, ELanguage.English, "Bans a player out of the lobby in which the command was used." },
                    { (short)7, ELanguage.German, "Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                    { (short)7, ELanguage.English, "Kicks a player out of the lobby in which the command was used." },
                    { (short)4, ELanguage.German, "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                    { (short)5, ELanguage.English, "Kicks a player out of the server." }
                });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "Id", "AmountLifes", "DefaultSpawnRotation", "DefaultSpawnX", "DefaultSpawnY", "IsOfficial", "IsTemporary", "Name", "Owner", "Password", "SpawnAgainAfterDeathMs", "Type" },
                values: new object[] { 2, (short)1, 0f, 0f, 0f, true, false, "GangLobby", 0, null, 400, ELobbyType.GangLobby });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "Id", "AmountLifes", "DefaultSpawnRotation", "DefaultSpawnX", "DefaultSpawnY", "DieAfterOutsideMapLimitTime", "IsOfficial", "IsTemporary", "Name", "Owner", "Password", "SpawnAgainAfterDeathMs", "Type" },
                values: new object[] { 1, (short)1, 0f, 0f, 0f, 10, true, false, "Arena", 0, null, 400, ELobbyType.Arena });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "Id", "AmountLifes", "DefaultSpawnRotation", "DefaultSpawnX", "DefaultSpawnY", "IsOfficial", "IsTemporary", "Name", "Owner", "Password", "Type" },
                values: new object[] { 0, null, 0f, 0f, 0f, true, false, "MainMenu", 0, null, ELobbyType.MainMenu });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "Id", "CreatorId", "Name" },
                values: new object[] { -1, 0, "All" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "Id", "CreatorId", "Name" },
                values: new object[] { -2, 0, "All Normals" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "Id", "CreatorId", "Name" },
                values: new object[] { -3, 0, "All Bombs" });

            migrationBuilder.InsertData(
                table: "killingspree_rewards",
                columns: new[] { "LobbyId", "KillsAmount", "HealthOrArmor", "OnlyArmor", "OnlyHealth" },
                values: new object[,]
                {
                    { 1, (short)3, (short)30, null, null },
                    { 1, (short)5, (short)50, null, null },
                    { 1, (short)10, (short)100, null, null },
                    { 1, (short)15, (short)100, null, null }
                });

            migrationBuilder.InsertData(
                table: "lobby_maps",
                columns: new[] { "LobbyID", "MapID" },
                values: new object[] { 1, -1 });

            migrationBuilder.InsertData(
                table: "lobby_rewards",
                columns: new[] { "LobbyID", "MoneyPerAssist", "MoneyPerDamage", "MoneyPerKill" },
                values: new object[,]
                {
                    { 1, 10.0, 0.10000000000000001, 20.0 },
                    { 2, 10.0, 0.10000000000000001, 20.0 }
                });

            migrationBuilder.InsertData(
                table: "lobby_round_settings",
                columns: new[] { "LobbyID", "BombDefuseTimeMs", "BombDetonateTimeMs", "BombPlantTimeMs", "CountdownTime", "MixTeamsAfterRound", "RoundTime" },
                values: new object[] { 1, 8000, 45000, 3000, 5, true, 240 });

            migrationBuilder.InsertData(
                table: "lobby_weapons",
                columns: new[] { "Hash", "Ammo", "Damage", "HeadMultiplicator", "Lobby" },
                values: new object[,]
                {
                    { EWeaponHash.CarbineRifleMK2, 2000, null, null, 1 },
                    { EWeaponHash.AssaultShotgun, 2000, null, null, 1 },
                    { EWeaponHash.UnholyHellbringer, 2000, null, null, 1 },
                    { EWeaponHash.SMG, 2000, null, null, 1 },
                    { EWeaponHash.UpnAtomizer, 500, null, null, 1 },
                    { EWeaponHash.Revolver, 500, null, null, 1 },
                    { EWeaponHash.AssaultRifle, 2000, null, null, 1 },
                    { EWeaponHash.MicroSMG, 2000, null, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "ID", "BlipColor", "ColorB", "ColorG", "ColorR", "Index", "Lobby", "Name", "SkinHash" },
                values: new object[,]
                {
                    { 4, (short)4, (short)255, (short)255, (short)255, (short)0, 2, "None", 1004114196 },
                    { 1, (short)4, (short)255, (short)255, (short)255, (short)0, 1, "Spectator", 1004114196 },
                    { 2, (short)52, (short)0, (short)150, (short)0, (short)1, 1, "SWAT", -1920001264 },
                    { 3, (short)1, (short)0, (short)0, (short)150, (short)2, 1, "Terrorist", 275618457 },
                    { 0, (short)4, (short)255, (short)255, (short)255, (short)0, 0, "Spectator", 1004114196 }
                });

            migrationBuilder.InsertData(
                table: "gangs",
                columns: new[] { "ID", "Short", "TeamID" },
                values: new object[] { 0, "-", 4 });

            migrationBuilder.CreateIndex(
                name: "IX_command_alias_Command",
                table: "command_alias",
                column: "Command");

            migrationBuilder.CreateIndex(
                name: "IX_commands_NeededAdminLevel",
                table: "commands",
                column: "NeededAdminLevel");

            migrationBuilder.CreateIndex(
                name: "IX_gangs_TeamID",
                table: "gangs",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_lobbies_Owner",
                table: "lobbies",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "fki_FK_lobby_maps_maps",
                table: "lobby_maps",
                column: "MapID");

            migrationBuilder.CreateIndex(
                name: "IX_lobby_weapons_Lobby",
                table: "lobby_weapons",
                column: "Lobby");

            migrationBuilder.CreateIndex(
                name: "IX_maps_CreatorId",
                table: "maps",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "Index_maps_name",
                table: "maps",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_offlinemessages_SourceID",
                table: "offlinemessages",
                column: "SourceID");

            migrationBuilder.CreateIndex(
                name: "IX_offlinemessages_TargetID",
                table: "offlinemessages",
                column: "TargetID");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_AdminId",
                table: "player_bans",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_player_bans_LobbyId",
                table: "player_bans",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_player_lobby_stats_LobbyID",
                table: "player_lobby_stats",
                column: "LobbyID");

            migrationBuilder.CreateIndex(
                name: "IX_player_map_favourites_MapID",
                table: "player_map_favourites",
                column: "MapID");

            migrationBuilder.CreateIndex(
                name: "IX_player_map_ratings_MapID",
                table: "player_map_ratings",
                column: "MapID");

            migrationBuilder.CreateIndex(
                name: "IX_player_relations_TargetId",
                table: "player_relations",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_players_AdminLvl",
                table: "players",
                column: "AdminLvl");

            migrationBuilder.CreateIndex(
                name: "IX_players_GangId",
                table: "players",
                column: "GangId");

            migrationBuilder.CreateIndex(
                name: "IX_teams_Lobby",
                table: "teams",
                column: "Lobby");

            migrationBuilder.AddForeignKey(
                name: "players_GangId_fkey",
                table: "players",
                column: "GangId",
                principalTable: "gangs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "players_AdminLvl_fkey",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "gangs_TeamID_fkey",
                table: "gangs");

            migrationBuilder.DropTable(
                name: "admin_level_names");

            migrationBuilder.DropTable(
                name: "command_alias");

            migrationBuilder.DropTable(
                name: "command_infos");

            migrationBuilder.DropTable(
                name: "freeroam_default_vehicle");

            migrationBuilder.DropTable(
                name: "killingspree_rewards");

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
                name: "log_rests");

            migrationBuilder.DropTable(
                name: "offlinemessages");

            migrationBuilder.DropTable(
                name: "player_bans");

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
                name: "server_settings");

            migrationBuilder.DropTable(
                name: "commands");

            migrationBuilder.DropTable(
                name: "weapons");

            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "admin_levels");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "lobbies");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "gangs");

            migrationBuilder.DropSequence(
                name: "ID_ID_seq");

            migrationBuilder.DropSequence(
                name: "lobbies_ID_seq");

            migrationBuilder.DropSequence(
                name: "maps_ID_seq");

            migrationBuilder.DropSequence(
                name: "ServerSettings_ID_seq");
        }
    }
}

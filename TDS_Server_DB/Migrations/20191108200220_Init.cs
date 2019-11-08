using System;
using System.Net;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Common.Enum;
using TDS_Common.Enum.Userpanel;

namespace TDS_Server_DB.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:e_language", "german,english")
                .Annotation("Npgsql:Enum:e_lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,gangwar_lobby")
                .Annotation("Npgsql:Enum:e_log_type", "kick,ban,mute,next,login,register,lobby__join,lobby__leave,lobby__kick,lobby__ban,goto,remove_map,voice_mute")
                .Annotation("Npgsql:Enum:e_map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .Annotation("Npgsql:Enum:e_player_relation", "none,block,friend")
                .Annotation("Npgsql:Enum:e_rule_category", "general,chat")
                .Annotation("Npgsql:Enum:e_rule_target", "user,admin,vip")
                .Annotation("Npgsql:Enum:e_userpanel_admin_question_answer_type", "text,check,number")
                .Annotation("Npgsql:Enum:e_weapon_hash", "sniper_rifle,fire_extinguisher,compact_grenade_launcher,snowball,vintage_pistol,combat_pdw,heavy_sniper,heavy_sniper_mk2,sweeper_shotgun,micro_smg,wrench,pistol,pistol_mk2,pump_shotgun,pump_shotgun_mk2,ap_pistol,baseball,molotov,smg,smg_mk2,sticky_bomb,petrol_can,stun_gun,heavy_shotgun,minigun,golf_club,flare_gun,flare,grenade_launcher_smoke,hammer,combat_pistol,gusenberg,compact_rifle,homing_launcher,nightstick,railgun,sawn_off_shotgun,bullpup_rifle,firework,combat_mg,combat_mg_mk2,carbine_rifle,crowbar,flashlight,dagger,grenade,pool_cue,bat,pistol50,knife,mg,bullpup_shotgun,bz_gas,unarmed,grenade_launcher,night_vision,musket,proximity_mine,advanced_rifle,rpg,pipe_bomb,mini_smg,sns_pistol,sns_pistol_mk2,assault_rifle,assault_rifle_mk2,special_carbine,heavy_revolver,heavy_revolver_mk2,double_action_revolver,marksman_rifle,marksman_rifle_mk2,battle_axe,heavy_pistol,knuckle_duster,machine_pistol,marksman_pistol,machete,switch_blade,assault_shotgun,double_barrel_shotgun,assault_smg,hatchet,bottle,parachute,smoke_grenade,upn_atomizer,unholy_hellbringer,carbine_rifle_mk2,sepcial_carbine_mk2,bullpup_rifle_mk2,widowmaker")
                .Annotation("Npgsql:Enum:e_weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .Annotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

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
                name: "faqs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Language = table.Column<ELanguage>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faqs", x => new { x.Id, x.Language });
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    Type = table.Column<ELogType>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: true),
                    Lobby = table.Column<int>(nullable: true),
                    AsDonator = table.Column<bool>(nullable: false),
                    AsVIP = table.Column<bool>(nullable: false),
                    Reason = table.Column<string>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    LengthOrEndTime = table.Column<string>(nullable: true)
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    Source = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    Lobby = table.Column<int>(nullable: true),
                    IsAdminChat = table.Column<bool>(nullable: false),
                    IsTeamChat = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    Source = table.Column<int>(nullable: true),
                    Info = table.Column<string>(nullable: false),
                    StackTrace = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    Type = table.Column<ELogType>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Serial = table.Column<string>(maxLength: 200, nullable: true),
                    IP = table.Column<IPAddress>(nullable: true),
                    Lobby = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_rests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "rules",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Target = table.Column<ERuleTarget>(nullable: false),
                    Category = table.Column<ERuleCategory>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rules", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "server_daily_stats",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    PlayerPeak = table.Column<short>(nullable: false, defaultValue: (short)0),
                    ArenaRoundsPlayed = table.Column<int>(nullable: false, defaultValue: 0),
                    CustomArenaRoundsPlayed = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountLogins = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountRegistrations = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("server_daily_stats_date_pkey", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "server_settings",
                columns: table => new
                {
                    ID = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GamemodeName = table.Column<string>(maxLength: 50, nullable: false),
                    ErrorToPlayerOnNonExistentCommand = table.Column<bool>(nullable: false),
                    ToChatOnNonExistentCommand = table.Column<bool>(nullable: false),
                    DistanceToSpotToPlant = table.Column<float>(nullable: false),
                    DistanceToSpotToDefuse = table.Column<float>(nullable: false),
                    SavePlayerDataCooldownMinutes = table.Column<int>(nullable: false),
                    SaveLogsCooldownMinutes = table.Column<int>(nullable: false),
                    SaveSeasonsCooldownMinutes = table.Column<int>(nullable: false),
                    TeamOrderCooldownMs = table.Column<int>(nullable: false),
                    ArenaNewMapProbabilityPercent = table.Column<float>(nullable: false),
                    KillingSpreeMaxSecondsUntilNextKill = table.Column<int>(nullable: false, defaultValue: 18),
                    MapRatingAmountForCheck = table.Column<int>(nullable: false, defaultValue: 10),
                    MinMapRatingForNewMaps = table.Column<float>(nullable: false, defaultValue: 3f),
                    GiveMoneyFee = table.Column<float>(nullable: false, defaultValue: 0.05f),
                    GiveMoneyMinAmount = table.Column<int>(nullable: false, defaultValue: 100),
                    NametagMaxDistance = table.Column<float>(nullable: false, defaultValue: 625f),
                    ShowNametagOnlyOnAiming = table.Column<bool>(nullable: false),
                    MultiplierRankingKills = table.Column<float>(nullable: false, defaultValue: 75f),
                    MultiplierRankingAssists = table.Column<float>(nullable: false, defaultValue: 25f),
                    MultiplierRankingDamage = table.Column<float>(nullable: false, defaultValue: 1f),
                    CloseApplicationAfterDays = table.Column<int>(nullable: false, defaultValue: 7),
                    DeleteApplicationAfterDays = table.Column<int>(nullable: false, defaultValue: 14),
                    GangwarPreparationTimeMs = table.Column<long>(nullable: false, defaultValue: 180000L),
                    GangwarActionTimeMs = table.Column<long>(nullable: false, defaultValue: 900000L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "server_total_stats",
                columns: table => new
                {
                    ID = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerPeak = table.Column<short>(nullable: false, defaultValue: (short)0),
                    ArenaRoundsPlayed = table.Column<long>(nullable: false, defaultValue: 0L),
                    CustomArenaRoundsPlayed = table.Column<long>(nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_total_stats", x => x.ID);
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    AdminLvl = table.Column<short>(nullable: false, defaultValue: (short)0),
                    AdminLeaderID = table.Column<int>(nullable: true),
                    IsVIP = table.Column<bool>(nullable: false, defaultValue: false),
                    Donation = table.Column<short>(nullable: false, defaultValue: (short)0),
                    RegisterTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.ID);
                    table.ForeignKey(
                        name: "FK_players_players_AdminLeaderID",
                        column: x => x.AdminLeaderID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "players_AdminLvl_fkey",
                        column: x => x.AdminLvl,
                        principalTable: "admin_levels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "rule_texts",
                columns: table => new
                {
                    RuleID = table.Column<int>(nullable: false),
                    Language = table.Column<ELanguage>(nullable: false),
                    RuleStr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rule_texts", x => new { x.RuleID, x.Language });
                    table.ForeignKey(
                        name: "FK_rule_texts_rules_RuleID",
                        column: x => x.RuleID,
                        principalTable: "rules",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                name: "application_questions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    AdminId = table.Column<int>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    AnswerType = table.Column<EUserpanelAdminQuestionAnswerType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_questions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_application_questions_players_AdminId",
                        column: x => x.AdminId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PlayerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_applications_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobbies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    Type = table.Column<ELobbyType>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    StartHealth = table.Column<short>(nullable: false, defaultValueSql: "100"),
                    StartArmor = table.Column<short>(nullable: false, defaultValueSql: "100"),
                    AmountLifes = table.Column<short>(nullable: true),
                    DefaultSpawnX = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    DefaultSpawnY = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    DefaultSpawnZ = table.Column<float>(nullable: false, defaultValueSql: "9000"),
                    AroundSpawnPoint = table.Column<float>(nullable: false, defaultValueSql: "3"),
                    DefaultSpawnRotation = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    IsTemporary = table.Column<bool>(nullable: false),
                    IsOfficial = table.Column<bool>(nullable: false),
                    SpawnAgainAfterDeathMs = table.Column<int>(nullable: false, defaultValueSql: "400"),
                    CreateTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lobbies", x => x.ID);
                    table.ForeignKey(
                        name: "lobbies_Owner_fkey",
                        column: x => x.OwnerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(nullable: false),
                    CreatorId = table.Column<int>(nullable: true),
                    CreateTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maps", x => x.ID);
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TargetID = table.Column<int>(nullable: false),
                    SourceID = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
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
                name: "player_clothes",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    IsMale = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_clothes", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_player_clothes_players_PlayerId",
                        column: x => x.PlayerId,
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
                    Language = table.Column<ELanguage>(nullable: false, defaultValue: ELanguage.English),
                    AllowDataTransfer = table.Column<bool>(nullable: false),
                    ShowConfettiAtRanking = table.Column<bool>(nullable: false),
                    TimeZone = table.Column<string>(nullable: true, defaultValue: "UTC"),
                    DiscordIdentity = table.Column<string>(nullable: true),
                    Hitsound = table.Column<bool>(nullable: false),
                    Bloodscreen = table.Column<bool>(nullable: false),
                    FloatingDamageInfo = table.Column<bool>(nullable: false),
                    Voice3D = table.Column<bool>(nullable: false, defaultValue: false),
                    VoiceAutoVolume = table.Column<bool>(nullable: false, defaultValue: false),
                    VoiceVolume = table.Column<float>(nullable: false, defaultValue: 6f),
                    MapBorderColor = table.Column<string>(nullable: true, defaultValue: "rgba(150,0,0,0.35)")
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
                    VoiceMuteTime = table.Column<int>(nullable: true),
                    LoggedIn = table.Column<bool>(nullable: false),
                    LastLoginTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
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
                name: "player_total_stats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    Money = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_total_stats_pkey", x => x.PlayerID);
                    table.ForeignKey(
                        name: "player_total_stats_PlayerID_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_answers",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(nullable: false),
                    QuestionID = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_answers", x => new { x.ApplicationID, x.QuestionID });
                    table.ForeignKey(
                        name: "FK_application_answers_applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "applications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_answers_application_questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "application_questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_invitations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ApplicationID = table.Column<int>(nullable: false),
                    AdminID = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_invitations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_application_invitations_players_AdminID",
                        column: x => x.AdminID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_invitations_applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "applications",
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lobby_map_settings",
                columns: table => new
                {
                    LobbyID = table.Column<int>(nullable: false),
                    MapLimitTime = table.Column<int>(nullable: false, defaultValueSql: "10"),
                    MapLimitType = table.Column<EMapLimitType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lobby_map_settings_pkey", x => x.LobbyID);
                    table.ForeignKey(
                        name: "lobby_map_settings_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "ID",
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
                        principalColumn: "ID",
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
                    MixTeamsAfterRound = table.Column<bool>(nullable: false),
                    ShowRanking = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lobby_round_infos_pkey", x => x.LobbyID);
                    table.ForeignKey(
                        name: "lobby_round_infos_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "ID",
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
                    table.PrimaryKey("lobby_weapons_pkey", x => new { x.Hash, x.Lobby });
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_bans",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    LobbyId = table.Column<int>(nullable: false),
                    AdminId = table.Column<int>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: false),
                    StartTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    EndTimestamp = table.Column<DateTime>(nullable: true)
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_bans_PlayerID_fkey",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
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
                    TotalDamage = table.Column<int>(nullable: false),
                    TotalRounds = table.Column<int>(nullable: false),
                    MostKillsInARound = table.Column<int>(nullable: false),
                    MostDamageInARound = table.Column<int>(nullable: false),
                    MostAssistsInARound = table.Column<int>(nullable: false),
                    TotalMapsBought = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_lobby_stats_pkey", x => new { x.PlayerID, x.LobbyID });
                    table.ForeignKey(
                        name: "player_lobby_stats_LobbyID_fkey",
                        column: x => x.LobbyID,
                        principalTable: "lobbies",
                        principalColumn: "ID",
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Index = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false, defaultValue: "Spectator"),
                    Lobby = table.Column<int>(nullable: false),
                    ColorR = table.Column<short>(nullable: false, defaultValue: (short)255),
                    ColorG = table.Column<short>(nullable: false, defaultValue: (short)255),
                    ColorB = table.Column<short>(nullable: false, defaultValue: (short)255),
                    BlipColor = table.Column<short>(nullable: false, defaultValue: (short)4),
                    SkinHash = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.ID);
                    table.ForeignKey(
                        name: "teams_Lobby_fkey",
                        column: x => x.Lobby,
                        principalTable: "lobbies",
                        principalColumn: "ID",
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lobby_maps_maps",
                        column: x => x.MapID,
                        principalTable: "maps",
                        principalColumn: "ID",
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
                        principalColumn: "ID",
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
                        principalColumn: "ID",
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TeamId = table.Column<int>(nullable: false),
                    Short = table.Column<string>(maxLength: 5, nullable: false),
                    OwnerId = table.Column<int>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gangs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_gangs_players_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "gangs_TeamId_fkey",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gang_rank_permissions",
                columns: table => new
                {
                    GangID = table.Column<int>(nullable: false),
                    ManagePermissions = table.Column<short>(nullable: false),
                    InviteMembers = table.Column<short>(nullable: false),
                    KickMembers = table.Column<short>(nullable: false),
                    ManageRanks = table.Column<short>(nullable: false),
                    StartGangwar = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gang_rank_permissions", x => x.GangID);
                    table.ForeignKey(
                        name: "FK_gang_rank_permissions_gangs_GangID",
                        column: x => x.GangID,
                        principalTable: "gangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gang_ranks",
                columns: table => new
                {
                    GangID = table.Column<int>(nullable: false),
                    Rank = table.Column<short>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gang_ranks", x => new { x.GangID, x.Rank });
                    table.ForeignKey(
                        name: "FK_gang_ranks_gangs_GangID",
                        column: x => x.GangID,
                        principalTable: "gangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gangwar_areas",
                columns: table => new
                {
                    MapID = table.Column<int>(nullable: false),
                    OwnerGangID = table.Column<int>(nullable: false),
                    LastAttacked = table.Column<DateTime>(nullable: false, defaultValueSql: "'1970-1-1'::timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gangwar_areas", x => x.MapID);
                    table.ForeignKey(
                        name: "FK_gangwar_areas_maps_MapID",
                        column: x => x.MapID,
                        principalTable: "maps",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gangwar_areas_gangs_OwnerGangID",
                        column: x => x.OwnerGangID,
                        principalTable: "gangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "gang_members",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    GangID = table.Column<int>(nullable: false),
                    Rank = table.Column<short>(nullable: false, defaultValue: (short)0),
                    JoinTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    RankNavigationGangId = table.Column<int>(nullable: true),
                    RankNavigationRank = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gang_members", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_gang_members_gangs_GangID",
                        column: x => x.GangID,
                        principalTable: "gangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gang_members_players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gang_members_gang_ranks_RankNavigationGangId_RankNavigation~",
                        columns: x => new { x.RankNavigationGangId, x.RankNavigationRank },
                        principalTable: "gang_ranks",
                        principalColumns: new[] { "GangID", "Rank" },
                        onDelete: ReferentialAction.Restrict);
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
                    { (short)24, "GiveMoney", false, null, null, false },
                    { (short)20, "BlockUser", false, null, null, false },
                    { (short)19, "UserId", false, null, null, false },
                    { (short)18, "PrivateMessage", false, null, null, false },
                    { (short)17, "OpenPrivateChat", false, null, null, false },
                    { (short)16, "ClosePrivateChat", false, null, null, false },
                    { (short)21, "UnblockUser", false, null, null, false },
                    { (short)14, "PrivateChat", false, null, null, false },
                    { (short)13, "TeamChat", false, null, null, false },
                    { (short)12, "GlobalChat", false, null, null, false },
                    { (short)11, "Suicide", false, null, null, false },
                    { (short)10, "LobbyLeave", false, null, null, false },
                    { (short)15, "Position", false, null, null, false }
                });

            migrationBuilder.InsertData(
                table: "faqs",
                columns: new[] { "Id", "Language", "Answer", "Question" },
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
                columns: new[] { "VehicleType", "Note", "VehicleHash" },
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
                columns: new[] { "ID", "Category", "Target" },
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
                columns: new[] { "ID", "ArenaNewMapProbabilityPercent", "DistanceToSpotToDefuse", "DistanceToSpotToPlant", "ErrorToPlayerOnNonExistentCommand", "GamemodeName", "GiveMoneyFee", "GiveMoneyMinAmount", "KillingSpreeMaxSecondsUntilNextKill", "MapRatingAmountForCheck", "MinMapRatingForNewMaps", "MultiplierRankingAssists", "MultiplierRankingDamage", "MultiplierRankingKills", "NametagMaxDistance", "SaveLogsCooldownMinutes", "SavePlayerDataCooldownMinutes", "SaveSeasonsCooldownMinutes", "ShowNametagOnlyOnAiming", "TeamOrderCooldownMs", "ToChatOnNonExistentCommand" },
                values: new object[] { (short)1, 2f, 3f, 3f, true, "tdm", 0.05f, 100, 18, 10, 3f, 25f, 1f, 75f, 80f, 1, 1, 1, true, 3000, false });

            migrationBuilder.InsertData(
                table: "server_total_stats",
                column: "ID",
                value: (short)1);

            migrationBuilder.InsertData(
                table: "weapons",
                columns: new[] { "Hash", "DefaultDamage", "DefaultHeadMultiplicator", "Type" },
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
                    { EWeaponHash.CombatPistol, (short)27, 1f, EWeaponType.Handgun },
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
                    { EWeaponHash.MicroSMG, (short)21, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.CombatMG, (short)28, 1f, EWeaponType.MachineGun },
                    { EWeaponHash.Firework, (short)100, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.BullpupRifle, (short)32, 1f, EWeaponType.AssaultRifle },
                    { EWeaponHash.SawnOffShotgun, (short)160, 1f, EWeaponType.Shotgun },
                    { EWeaponHash.Railgun, (short)50, 1f, EWeaponType.HeavyWeapon },
                    { EWeaponHash.Dagger, (short)45, 1f, EWeaponType.Melee },
                    { EWeaponHash.CombatMGMk2, (short)28, 1f, EWeaponType.MachineGun }
                });

            migrationBuilder.InsertData(
                table: "admin_level_names",
                columns: new[] { "Level", "Language", "Name" },
                values: new object[,]
                {
                    { (short)0, ELanguage.English, "User" },
                    { (short)3, ELanguage.English, "Projectleader" },
                    { (short)2, ELanguage.German, "Administrator" },
                    { (short)2, ELanguage.English, "Administrator" },
                    { (short)3, ELanguage.German, "Projektleiter" },
                    { (short)0, ELanguage.German, "User" },
                    { (short)1, ELanguage.English, "Supporter" },
                    { (short)1, ELanguage.German, "Supporter" }
                });

            migrationBuilder.InsertData(
                table: "command_alias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "SendMoney", (short)24 },
                    { "ClosePrivateSay", (short)16 },
                    { "ClosePM", (short)16 },
                    { "CPC", (short)16 },
                    { "Pos", (short)15 },
                    { "GetPosition", (short)15 },
                    { "GetPos", (short)15 },
                    { "CurrentPos", (short)15 },
                    { "Coordinate", (short)15 },
                    { "Coord", (short)15 },
                    { "PrivateSay", (short)14 },
                    { "PChat", (short)14 },
                    { "TSay", (short)13 },
                    { "CurrentPosition", (short)15 },
                    { "StopPrivateChat", (short)16 },
                    { "OpenPrivateSay", (short)17 },
                    { "OpenPM", (short)17 },
                    { "TeamSay", (short)13 },
                    { "OPC", (short)17 },
                    { "MSG", (short)18 },
                    { "MoneySend", (short)24 },
                    { "PM", (short)18 },
                    { "PSay", (short)18 },
                    { "UID", (short)19 },
                    { "Ignore", (short)20 },
                    { "IgnoreUser", (short)20 },
                    { "Block", (short)20 },
                    { "Unblock", (short)21 },
                    { "MoneyGive", (short)24 },
                    { "StopPrivateSay", (short)17 },
                    { "TChat", (short)13 },
                    { "Coordinates", (short)15 },
                    { "AllChat", (short)12 },
                    { "Leave", (short)10 },
                    { "Back", (short)10 },
                    { "Dead", (short)11 },
                    { "Death", (short)11 },
                    { "Die", (short)11 },
                    { "Kill", (short)11 },
                    { "PublicSay", (short)12 },
                    { "Mainmenu", (short)10 },
                    { "AllSay", (short)12 },
                    { "LeaveLobby", (short)10 },
                    { "G", (short)12 },
                    { "GChat", (short)12 },
                    { "Global", (short)12 },
                    { "GlobalSay", (short)12 },
                    { "PublicChat", (short)12 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "ID", "Language", "Info" },
                values: new object[,]
                {
                    { (short)21, ELanguage.English, "Removes the target from the blocklist." },
                    { (short)18, ELanguage.German, "Private Nachricht an einen bestimmten Spieler." },
                    { (short)17, ELanguage.English, "Sends a private chat request or accepts the request of another user." },
                    { (short)19, ELanguage.German, "Gibt dir deine User-Id aus." },
                    { (short)19, ELanguage.English, "Outputs your user-id to yourself." },
                    { (short)17, ELanguage.German, "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                    { (short)24, ELanguage.German, "Gibt einem Spieler Geld." },
                    { (short)21, ELanguage.German, "Entfernt das Ziel aus der Blockliste." },
                    { (short)18, ELanguage.English, "Private message to a specific player." },
                    { (short)12, ELanguage.German, "Globaler Chat, welcher überall gelesen werden kann." },
                    { (short)10, ELanguage.English, "Leaves the current lobby." },
                    { (short)16, ELanguage.German, "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." },
                    { (short)12, ELanguage.English, "Global chat which can be read everywhere." },
                    { (short)13, ELanguage.German, "Sendet die Nachricht nur zum eigenen Team." },
                    { (short)13, ELanguage.English, "Sends the message to the current team only." },
                    { (short)14, ELanguage.German, "Gibt die Position des Spielers aus." },
                    { (short)14, ELanguage.English, "Outputs the position of the player." },
                    { (short)24, ELanguage.English, "Gives money to a player." },
                    { (short)16, ELanguage.English, "Closes a private chat or withdraws a private chat request." },
                    { (short)20, ELanguage.German, "Fügt das Ziel in deine Blocklist ein, sodass du keine Nachrichten mehr von ihm liest, er dich nicht einladen kann usw." },
                    { (short)11, ELanguage.English, "Kills the user (suicide)." },
                    { (short)11, ELanguage.German, "Tötet den Nutzer (Selbstmord)." },
                    { (short)15, ELanguage.German, "Sendet eine Nachricht im Privatchat." },
                    { (short)15, ELanguage.English, "Sends a message in private chat." },
                    { (short)20, ELanguage.English, "Adds the target into your blocklist so you won't see messages from him, he can't invite you anymore etc." },
                    { (short)10, ELanguage.German, "Verlässt die jetzige Lobby." }
                });

            migrationBuilder.InsertData(
                table: "commands",
                columns: new[] { "ID", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[,]
                {
                    { (short)5, "Kick", false, (short)1, null, true },
                    { (short)1, "AdminSay", false, (short)1, null, false },
                    { (short)2, "AdminChat", false, (short)1, null, true },
                    { (short)3, "Ban", false, (short)2, null, false },
                    { (short)7, "LobbyKick", true, (short)1, null, true },
                    { (short)8, "Mute", false, (short)1, null, true },
                    { (short)9, "NextMap", true, (short)1, null, true },
                    { (short)22, "LoadMapOfOthers", false, (short)1, null, true },
                    { (short)23, "VoiceMute", false, (short)1, null, true },
                    { (short)4, "Goto", true, (short)2, null, false },
                    { (short)6, "LobbyBan", true, (short)1, null, true }
                });

            migrationBuilder.InsertData(
                table: "players",
                columns: new[] { "ID", "AdminLeaderID", "AdminLvl", "Email", "Name", "Password", "SCName" },
                values: new object[] { -1, null, (short)0, null, "System", "", "System" });

            migrationBuilder.InsertData(
                table: "rule_texts",
                columns: new[] { "RuleID", "Language", "RuleStr" },
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
                    { 2, ELanguage.English, @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none.
                By 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area.
                The chat rules listed here are ONLY for the normal chat in an official lobby.
                Chats in private lobbies can be freely monitored by the lobby owners." },
                    { 3, ELanguage.English, "Admins have to follow the same rules as players do." },
                    { 2, ELanguage.German, @"Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine.
                Unter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich.
                Die hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby.
                Chats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden." },
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
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "Next", (short)9 },
                    { "KickLobby", (short)7 },
                    { "PermaMute", (short)8 },
                    { "PMute", (short)8 },
                    { "RMute", (short)8 },
                    { "TimeMute", (short)8 },
                    { "TMute", (short)8 },
                    { "MuteVoice", (short)23 },
                    { "VoiceTMute", (short)23 },
                    { "EndRound", (short)9 },
                    { "VoiceTimeMute", (short)23 },
                    { "TMuteVoice", (short)23 },
                    { "Skip", (short)9 },
                    { "VoicePMute", (short)23 },
                    { "PermaVoiceMute", (short)23 },
                    { "PVoiceMute", (short)23 },
                    { "RVoiceMute", (short)23 },
                    { "TimeVoiceMute", (short)23 },
                    { "TVoiceMute", (short)23 },
                    { "PermaMuteVoice", (short)23 },
                    { "PMuteVoice", (short)23 },
                    { "RMuteVoice", (short)23 },
                    { "TimeMuteVoice", (short)23 },
                    { "VoiceRMute", (short)23 },
                    { "Permaban", (short)3 },
                    { "PBan", (short)3 },
                    { "RBan", (short)3 },
                    { "XYZ", (short)4 },
                    { "WarpToPlayer", (short)4 },
                    { "WarpTo", (short)4 },
                    { "Warp", (short)4 },
                    { "GotoXYZ", (short)4 },
                    { "GotoPlayer", (short)4 },
                    { "Announce", (short)1 },
                    { "Announcement", (short)1 },
                    { "ASay", (short)1 },
                    { "OChat", (short)1 },
                    { "BanLobby", (short)6 },
                    { "OSay", (short)1 },
                    { "UnBan", (short)3 },
                    { "AChat", (short)2 },
                    { "ChatAdmin", (short)2 },
                    { "InternChat", (short)2 },
                    { "WriteAdmin", (short)2 },
                    { "UBan", (short)3 },
                    { "Timeban", (short)3 },
                    { "RKick", (short)5 },
                    { "TBan", (short)3 },
                    { "VoicePermaMute", (short)23 }
                });

            migrationBuilder.InsertData(
                table: "command_infos",
                columns: new[] { "ID", "Language", "Info" },
                values: new object[,]
                {
                    { (short)3, ELanguage.English, "Bans a player out of the server." },
                    { (short)23, ELanguage.German, "Mutet einen Spieler im Voice-Chat." },
                    { (short)23, ELanguage.English, "Mutes a player in the voice-chat." },
                    { (short)3, ELanguage.German, "Bannt einen Spieler vom gesamten Server." },
                    { (short)4, ELanguage.English, "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
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
                    { (short)4, ELanguage.German, "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                    { (short)6, ELanguage.German, "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." }
                });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "ID", "AmountLifes", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "SpawnAgainAfterDeathMs", "Type" },
                values: new object[] { -3, (short)1, true, false, "MapCreateLobby", -1, null, 400, ELobbyType.MapCreateLobby });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "ID", "AmountLifes", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "SpawnAgainAfterDeathMs", "Type" },
                values: new object[] { -2, (short)1, true, false, "GangLobby", -1, null, 400, ELobbyType.GangLobby });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "ID", "AmountLifes", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "SpawnAgainAfterDeathMs", "Type" },
                values: new object[] { -1, (short)1, true, false, "Arena", -1, null, 400, ELobbyType.Arena });

            migrationBuilder.InsertData(
                table: "lobbies",
                columns: new[] { "ID", "AmountLifes", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "Type" },
                values: new object[] { -4, null, true, false, "MainMenu", -1, null, ELobbyType.MainMenu });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "ID", "CreatorId", "Name" },
                values: new object[] { -1, -1, "All" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "ID", "CreatorId", "Name" },
                values: new object[] { -2, -1, "All Normals" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "ID", "CreatorId", "Name" },
                values: new object[] { -3, -1, "All Bombs" });

            migrationBuilder.InsertData(
                table: "maps",
                columns: new[] { "ID", "CreatorId", "Name" },
                values: new object[] { -4, -1, "All Sniper" });

            migrationBuilder.InsertData(
                table: "killingspree_rewards",
                columns: new[] { "LobbyId", "KillsAmount", "HealthOrArmor", "OnlyArmor", "OnlyHealth" },
                values: new object[,]
                {
                    { -1, (short)3, (short)30, null, null },
                    { -1, (short)5, (short)50, null, null },
                    { -1, (short)10, (short)100, null, null },
                    { -1, (short)15, (short)100, null, null }
                });

            migrationBuilder.InsertData(
                table: "lobby_map_settings",
                columns: new[] { "LobbyID", "MapLimitTime", "MapLimitType" },
                values: new object[] { -1, 10, EMapLimitType.KillAfterTime });

            migrationBuilder.InsertData(
                table: "lobby_maps",
                columns: new[] { "LobbyID", "MapID" },
                values: new object[] { -1, -1 });

            migrationBuilder.InsertData(
                table: "lobby_rewards",
                columns: new[] { "LobbyID", "MoneyPerAssist", "MoneyPerDamage", "MoneyPerKill" },
                values: new object[,]
                {
                    { -2, 10.0, 0.10000000000000001, 20.0 },
                    { -1, 10.0, 0.10000000000000001, 20.0 }
                });

            migrationBuilder.InsertData(
                table: "lobby_round_settings",
                columns: new[] { "LobbyID", "BombDefuseTimeMs", "BombDetonateTimeMs", "BombPlantTimeMs", "CountdownTime", "MixTeamsAfterRound", "RoundTime", "ShowRanking" },
                values: new object[] { -1, 8000, 45000, 3000, 5, true, 240, false });

            migrationBuilder.InsertData(
                table: "lobby_weapons",
                columns: new[] { "Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator" },
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
                columns: new[] { "ID", "BlipColor", "ColorR", "Index", "Lobby", "Name", "SkinHash" },
                values: new object[] { -4, (short)1, (short)150, (short)2, -1, "Terrorist", 275618457 });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "ID", "BlipColor", "ColorB", "ColorG", "ColorR", "Index", "Lobby", "Name" },
                values: new object[] { -5, (short)4, (short)255, (short)255, (short)255, (short)0, -2, "None" });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "ID", "BlipColor", "ColorB", "ColorG", "ColorR", "Index", "Lobby", "Name" },
                values: new object[] { -2, (short)4, (short)255, (short)255, (short)255, (short)0, -1, "Spectator" });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "ID", "BlipColor", "ColorG", "Index", "Lobby", "Name", "SkinHash" },
                values: new object[] { -3, (short)52, (short)150, (short)1, -1, "SWAT", -1920001264 });

            migrationBuilder.InsertData(
                table: "teams",
                columns: new[] { "ID", "BlipColor", "ColorB", "ColorG", "ColorR", "Index", "Lobby", "Name", "SkinHash" },
                values: new object[] { -1, (short)4, (short)255, (short)255, (short)255, (short)0, -4, "Spectator", 1004114196 });

            migrationBuilder.InsertData(
                table: "gangs",
                columns: new[] { "ID", "OwnerId", "Short", "TeamId" },
                values: new object[] { -1, null, "-", -5 });

            migrationBuilder.InsertData(
                table: "gang_rank_permissions",
                columns: new[] { "GangID", "InviteMembers", "KickMembers", "ManagePermissions", "ManageRanks", "StartGangwar" },
                values: new object[] { -1, (short)5, (short)5, (short)5, (short)5, (short)5 });

            migrationBuilder.InsertData(
                table: "gang_ranks",
                columns: new[] { "GangID", "Rank", "Name" },
                values: new object[] { -1, (short)0, "-" });

            migrationBuilder.CreateIndex(
                name: "IX_application_answers_QuestionID",
                table: "application_answers",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_application_invitations_AdminID",
                table: "application_invitations",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_application_invitations_ApplicationID",
                table: "application_invitations",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_application_questions_AdminId",
                table: "application_questions",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_PlayerId",
                table: "applications",
                column: "PlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_command_alias_Command",
                table: "command_alias",
                column: "Command");

            migrationBuilder.CreateIndex(
                name: "IX_commands_NeededAdminLevel",
                table: "commands",
                column: "NeededAdminLevel");

            migrationBuilder.CreateIndex(
                name: "IX_gang_members_GangID",
                table: "gang_members",
                column: "GangID");

            migrationBuilder.CreateIndex(
                name: "IX_gang_members_RankNavigationGangId_RankNavigationRank",
                table: "gang_members",
                columns: new[] { "RankNavigationGangId", "RankNavigationRank" });

            migrationBuilder.CreateIndex(
                name: "IX_gangs_OwnerId",
                table: "gangs",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gangs_TeamId",
                table: "gangs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_gangwar_areas_OwnerGangID",
                table: "gangwar_areas",
                column: "OwnerGangID");

            migrationBuilder.CreateIndex(
                name: "IX_lobbies_OwnerId",
                table: "lobbies",
                column: "OwnerId");

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
                name: "IX_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_players_AdminLvl",
                table: "players",
                column: "AdminLvl");

            migrationBuilder.CreateIndex(
                name: "IX_teams_Lobby",
                table: "teams",
                column: "Lobby");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_level_names");

            migrationBuilder.DropTable(
                name: "application_answers");

            migrationBuilder.DropTable(
                name: "application_invitations");

            migrationBuilder.DropTable(
                name: "command_alias");

            migrationBuilder.DropTable(
                name: "command_infos");

            migrationBuilder.DropTable(
                name: "faqs");

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
                name: "log_rests");

            migrationBuilder.DropTable(
                name: "offlinemessages");

            migrationBuilder.DropTable(
                name: "player_bans");

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

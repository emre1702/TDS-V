using System;
using System.Net;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .Annotation("Npgsql:Enum:challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .Annotation("Npgsql:Enum:freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:language", "german,english")
                .Annotation("Npgsql:Enum:lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,char_create_lobby")
                .Annotation("Npgsql:Enum:log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto,remove_map,voice_mute")
                .Annotation("Npgsql:Enum:map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .Annotation("Npgsql:Enum:ped_body_part", "head,neck,torso,genital_region,arm,hand,leg,foot")
                .Annotation("Npgsql:Enum:player_relation", "none,block,friend")
                .Annotation("Npgsql:Enum:rule_category", "general,chat")
                .Annotation("Npgsql:Enum:rule_target", "user,admin,vip")
                .Annotation("Npgsql:Enum:scoreboard_player_sorting", "name,play_time,kills,assists,deaths,kills_deaths_ratio,kills_deaths_assists_ratio")
                .Annotation("Npgsql:Enum:support_type", "question,help,compliment,complaint")
                .Annotation("Npgsql:Enum:time_span_units_of_time", "second,minute,hour_minute,hour,day,week")
                .Annotation("Npgsql:Enum:userpanel_admin_question_answer_type", "text,check,number")
                .Annotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust,minitank,retinue2,outlaw,yosemite2,stryder,jb7002,sultan2,everon,sugoi,zhaba,formula,formula2,rebla,vagrant,furia,vstr,komoda,asbo,kanjo,imorgon")
                .Annotation("Npgsql:Enum:weapon_hash", "advancedrifle,appistol,assaultrifle,assaultrifle_mk2,assaultshotgun,assaultsmg,autoshotgun,ball,bat,battleaxe,bottle,bullpuprifle,bullpuprifle_mk2,bullpupshotgun,bzgas,carbinerifle,carbinerifle_mk2,combatmg,combatmg_mk2,combatpdw,combatpistol,compactlauncher,compactrifle,crowbar,dagger,dbshotgun,doubleaction,fireextinguisher,firework,flare,flaregun,flashlight,golfclub,grenade,grenadelauncher,grenadelauncher_smoke,gusenberg,hammer,hatchet,heavypistol,heavyshotgun,heavysniper,heavysniper_mk2,hominglauncher,knife,knuckle,machete,machinepistol,marksmanpistol,marksmanrifle,marksmanrifle_mk2,mg,microsmg,minigun,minismg,molotov,musket,nightstick,parachute,petrolcan,pipebomb,pistol,pistol_mk2,pistol50,poolcue,proximine,pumpshotgun,pumpshotgun_mk2,railgun,revolver,revolver_mk2,rpg,sawnoffshotgun,smg,smg_mk2,smokegrenade,sniperrifle,snowball,snspistol,snspistol_mk2,stone_hatchet,specialcarbine,specialcarbine_mk2,stickybomb,stungun,switchblade,unarmed,vintagepistol,wrench,raypistol,raycarbine,rayminigun,ceramic_pistol,hazard_can,navy_revolver")
                .Annotation("Npgsql:Enum:weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .Annotation("Npgsql:PostgresExtension:tsm_system_rows", ",,");

            migrationBuilder.CreateTable(
                name: "AdminLevels",
                columns: table => new
                {
                    Level = table.Column<short>(nullable: false),
                    ColorB = table.Column<short>(nullable: false),
                    ColorG = table.Column<short>(nullable: false),
                    ColorR = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLevels", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BonusbotSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionsInfoChannelId = table.Column<decimal>(nullable: true),
                    AdminApplicationsChannelId = table.Column<decimal>(nullable: true),
                    BansInfoChannelId = table.Column<decimal>(nullable: true),
                    ErrorLogsChannelId = table.Column<decimal>(nullable: true),
                    GuildId = table.Column<decimal>(nullable: true),
                    RefreshServerStatsFrequencySec = table.Column<int>(nullable: false, defaultValue: 60),
                    SendPrivateMessageOnBan = table.Column<bool>(nullable: false),
                    SendPrivateMessageOnOfflineMessage = table.Column<bool>(nullable: false),
                    ServerInfosChannelId = table.Column<decimal>(nullable: true),
                    SupportRequestsChannelId = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusbotSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeSettings",
                columns: table => new
                {
                    Frequency = table.Column<ChallengeFrequency>(nullable: false),
                    Type = table.Column<ChallengeType>(nullable: false),
                    MaxNumber = table.Column<int>(nullable: false, defaultValue: 1),
                    MinNumber = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeSettings", x => new { x.Type, x.Frequency });
                });

            migrationBuilder.CreateTable(
                name: "ChatInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Language = table.Column<Language>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Language = table.Column<Language>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => new { x.Id, x.Language });
                });

            migrationBuilder.CreateTable(
                name: "FreeroamDefaultVehicle",
                columns: table => new
                {
                    VehicleType = table.Column<FreeroamVehicleType>(nullable: false),
                    Note = table.Column<string>(type: "character varying", nullable: true),
                    VehicleHash = table.Column<VehicleHash>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeroamDefaultVehicle", x => x.VehicleType);
                });

            migrationBuilder.CreateTable(
                name: "GangLevelSettings",
                columns: table => new
                {
                    Level = table.Column<byte>(nullable: false),
                    CanChangeBlipColor = table.Column<bool>(nullable: false),
                    GangAreaSlots = table.Column<byte>(nullable: false, defaultValue: (byte)255),
                    HouseAreaRadius = table.Column<float>(nullable: false, defaultValue: 30f),
                    HousePrice = table.Column<int>(nullable: false, defaultValue: 2147483647),
                    NeededExperience = table.Column<int>(nullable: false, defaultValue: 2147483647),
                    PlayerSlots = table.Column<byte>(nullable: false, defaultValue: (byte)255),
                    RankSlots = table.Column<byte>(nullable: false, defaultValue: (byte)255),
                    UpgradePrice = table.Column<int>(nullable: false, defaultValue: 2147483647),
                    VehicleSlots = table.Column<byte>(nullable: false, defaultValue: (byte)255)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangLevelSettings", x => x.Level);
                });

            migrationBuilder.CreateTable(
                name: "LogAdmins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AsDonator = table.Column<bool>(nullable: false),
                    AsVip = table.Column<bool>(nullable: false),
                    LengthOrEndTime = table.Column<string>(nullable: true),
                    Lobby = table.Column<int>(nullable: true),
                    Reason = table.Column<string>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Type = table.Column<LogType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogChats",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAdminChat = table.Column<bool>(nullable: false),
                    IsTeamChat = table.Column<bool>(nullable: false),
                    Lobby = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogErrors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExceptionType = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    Source = table.Column<int>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogKills",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeadId = table.Column<int>(nullable: false),
                    KillerId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    WeaponId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogKills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogRests",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ip = table.Column<IPAddress>(nullable: true),
                    Lobby = table.Column<int>(nullable: true),
                    Serial = table.Column<string>(maxLength: 200, nullable: true),
                    Source = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Type = table.Column<LogType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogRests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<RuleCategory>(nullable: false),
                    Target = table.Column<RuleTarget>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerDailyStats",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    AmountLogins = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountRegistrations = table.Column<int>(nullable: false, defaultValue: 0),
                    ArenaRoundsPlayed = table.Column<int>(nullable: false, defaultValue: 0),
                    CustomArenaRoundsPlayed = table.Column<int>(nullable: false, defaultValue: 0),
                    PlayerPeak = table.Column<short>(nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerDailyStats", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "ServerSettings",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmountPlayersAllowedInGangwarTeamBeforeCountCheck = table.Column<int>(nullable: false, defaultValue: 3),
                    AmountWeeklyChallenges = table.Column<int>(nullable: false, defaultValue: 3),
                    ArenaNewMapProbabilityPercent = table.Column<float>(nullable: false),
                    CloseApplicationAfterDays = table.Column<int>(nullable: false, defaultValue: 7),
                    DeleteApplicationAfterDays = table.Column<int>(nullable: false, defaultValue: 14),
                    DeleteOfflineMessagesAfterDays = table.Column<int>(nullable: false, defaultValue: 60),
                    DeleteRequestsDaysAfterClose = table.Column<long>(nullable: false, defaultValue: 30L),
                    DistanceToSpotToDefuse = table.Column<float>(nullable: false),
                    DistanceToSpotToPlant = table.Column<float>(nullable: false),
                    ErrorToPlayerOnNonExistentCommand = table.Column<bool>(nullable: false),
                    GamemodeName = table.Column<string>(maxLength: 50, nullable: false),
                    GangwarActionTime = table.Column<long>(nullable: false, defaultValue: 900L),
                    GangwarAreaAttackCooldownMinutes = table.Column<int>(nullable: false, defaultValue: 60),
                    GangwarAttackerCanBeMore = table.Column<bool>(nullable: false),
                    GangwarOwnerCanBeMore = table.Column<bool>(nullable: false),
                    GangwarPreparationTime = table.Column<long>(nullable: false, defaultValue: 180L),
                    GangwarTargetRadius = table.Column<double>(nullable: false, defaultValue: 5.0),
                    GangwarTargetWithoutAttackerMaxSeconds = table.Column<int>(nullable: false, defaultValue: 10),
                    GiveMoneyFee = table.Column<float>(nullable: false, defaultValue: 0.05f),
                    GiveMoneyMinAmount = table.Column<int>(nullable: false, defaultValue: 100),
                    KillingSpreeMaxSecondsUntilNextKill = table.Column<int>(nullable: false, defaultValue: 18),
                    MapBuyBasePrice = table.Column<int>(nullable: false, defaultValue: 1000),
                    MapBuyCounterMultiplicator = table.Column<float>(nullable: false, defaultValue: 1f),
                    MapRatingAmountForCheck = table.Column<int>(nullable: false, defaultValue: 10),
                    MinMapRatingForNewMaps = table.Column<float>(nullable: false, defaultValue: 3f),
                    MinPlayersOnlineForGangwar = table.Column<int>(nullable: false, defaultValue: 3),
                    MultiplierRankingAssists = table.Column<float>(nullable: false, defaultValue: 25f),
                    MultiplierRankingDamage = table.Column<float>(nullable: false, defaultValue: 1f),
                    MultiplierRankingKills = table.Column<float>(nullable: false, defaultValue: 75f),
                    NametagMaxDistance = table.Column<float>(nullable: false, defaultValue: 625f),
                    ReduceMapsBoughtCounterAfterMinute = table.Column<int>(nullable: false, defaultValue: 60),
                    ReloadServerBansEveryMinutes = table.Column<int>(nullable: false, defaultValue: 5),
                    SaveLogsCooldownMinutes = table.Column<int>(nullable: false),
                    SavePlayerDataCooldownMinutes = table.Column<int>(nullable: false),
                    SaveSeasonsCooldownMinutes = table.Column<int>(nullable: false),
                    ShowNametagOnlyOnAiming = table.Column<bool>(nullable: false),
                    TeamOrderCooldownMs = table.Column<int>(nullable: false),
                    ToChatOnNonExistentCommand = table.Column<bool>(nullable: false),
                    UsernameChangeCooldownDays = table.Column<int>(nullable: false, defaultValue: 60),
                    UsernameChangeCost = table.Column<int>(nullable: false, defaultValue: 20000),
                    AmountCharSlots = table.Column<byte>(nullable: false, defaultValue: (byte)3)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerTotalStats",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArenaRoundsPlayed = table.Column<long>(nullable: false, defaultValue: 0L),
                    CustomArenaRoundsPlayed = table.Column<long>(nullable: false, defaultValue: 0L),
                    PlayerPeak = table.Column<short>(nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerTotalStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Hash = table.Column<WeaponHash>(nullable: false),
                    ClipSize = table.Column<int>(nullable: false, defaultValue: 0),
                    Damage = table.Column<float>(nullable: false, defaultValue: 0f),
                    DamageExpMult = table.Column<float>(nullable: false, defaultValue: 0f),
                    HeadShotDamageModifier = table.Column<float>(nullable: false, defaultValue: 0f),
                    HeadshotsExpMult = table.Column<float>(nullable: false, defaultValue: 0f),
                    HitLimbsDamageModifier = table.Column<float>(nullable: false, defaultValue: 0f),
                    HitsExpMult = table.Column<float>(nullable: false, defaultValue: 0f),
                    KillsExpMult = table.Column<float>(nullable: false, defaultValue: 0f),
                    MaxHeadShotDistance = table.Column<float>(nullable: false, defaultValue: 0f),
                    MinHeadShotDistance = table.Column<float>(nullable: false, defaultValue: 0f),
                    Range = table.Column<float>(nullable: false, defaultValue: 0f),
                    ReloadTime = table.Column<float>(nullable: false, defaultValue: 0f),
                    ShotsExpMult = table.Column<float>(nullable: false, defaultValue: 0f),
                    TimeBetweenShots = table.Column<float>(nullable: false, defaultValue: 0f),
                    Type = table.Column<WeaponType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "AdminLevelNames",
                columns: table => new
                {
                    Language = table.Column<Language>(nullable: false),
                    Level = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLevelNames", x => new { x.Level, x.Language });
                    table.ForeignKey(
                        name: "FK_AdminLevelNames_AdminLevels_Level",
                        column: x => x.Level,
                        principalTable: "AdminLevels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Command = table.Column<string>(maxLength: 50, nullable: false),
                    LobbyOwnerCanUse = table.Column<bool>(nullable: false),
                    NeededAdminLevel = table.Column<short>(nullable: true),
                    NeededDonation = table.Column<short>(nullable: true),
                    VipCanUse = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commands_AdminLevels_NeededAdminLevel",
                        column: x => x.NeededAdminLevel,
                        principalTable: "AdminLevels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdminLeaderId = table.Column<int>(nullable: true),
                    AdminLvl = table.Column<short>(nullable: false, defaultValue: (short)0),
                    Donation = table.Column<short>(nullable: false, defaultValue: (short)0),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    IsVip = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 500, nullable: false),
                    RegisterTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    SCId = table.Column<decimal>(nullable: false),
                    SCName = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Players_AdminLeaderId",
                        column: x => x.AdminLeaderId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Players_AdminLevels_AdminLvl",
                        column: x => x.AdminLvl,
                        principalTable: "AdminLevels",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RuleTexts",
                columns: table => new
                {
                    Language = table.Column<Language>(nullable: false),
                    RuleId = table.Column<int>(nullable: false),
                    RuleStr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleTexts", x => new { x.RuleId, x.Language });
                    table.ForeignKey(
                        name: "FK_RuleTexts_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandAlias",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 100, nullable: false),
                    Command = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandAlias", x => new { x.Alias, x.Command });
                    table.ForeignKey(
                        name: "FK_CommandAlias_Commands_Command",
                        column: x => x.Command,
                        principalTable: "Commands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandInfos",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false),
                    Language = table.Column<Language>(nullable: false),
                    Info = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandInfos", x => new { x.Id, x.Language });
                    table.ForeignKey(
                        name: "FK_CommandInfos_Commands_Id",
                        column: x => x.Id,
                        principalTable: "Commands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    AdminId = table.Column<int>(nullable: false),
                    AnswerType = table.Column<UserpanelAdminQuestionAnswerType>(nullable: false),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationQuestions_Players_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Closed = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GangHouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    LastBought = table.Column<DateTime>(nullable: true),
                    NeededGangLevel = table.Column<byte>(nullable: false),
                    PosX = table.Column<float>(nullable: false),
                    PosY = table.Column<float>(nullable: false),
                    PosZ = table.Column<float>(nullable: false),
                    Rot = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangHouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GangHouses_Players_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lobbies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    AroundSpawnPoint = table.Column<float>(nullable: false, defaultValueSql: "3"),
                    CreateTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    DefaultSpawnRotation = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    DefaultSpawnX = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    DefaultSpawnY = table.Column<float>(nullable: false, defaultValueSql: "0"),
                    DefaultSpawnZ = table.Column<float>(nullable: false, defaultValueSql: "9000"),
                    IsOfficial = table.Column<bool>(nullable: false),
                    IsTemporary = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<LobbyType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobbies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lobbies_Players_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CreateTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    CreatorId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Players_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Offlinemessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Message = table.Column<string>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offlinemessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offlinemessages_Players_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offlinemessages_Players_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerChallenges",
                columns: table => new
                {
                    Challenge = table.Column<ChallengeType>(nullable: false),
                    Frequency = table.Column<ChallengeFrequency>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false, defaultValue: 1),
                    CurrentAmount = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerChallenges", x => new { x.PlayerId, x.Challenge, x.Frequency });
                    table.ForeignKey(
                        name: "FK_PlayerChallenges_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SyncedData_Slot = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharDatas", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerCharDatas_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerClothes",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClothes", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerClothes_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCommands",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommandId = table.Column<short>(nullable: false),
                    CommandText = table.Column<string>(maxLength: 100, nullable: true),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCommands_Commands_CommandId",
                        column: x => x.CommandId,
                        principalTable: "Commands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerCommands_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRelations",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    Relation = table.Column<PlayerRelation>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRelations", x => new { x.PlayerId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_PlayerRelations_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerRelations_Players_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSettings",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    AFKKickAfterSeconds = table.Column<int>(nullable: false, defaultValue: 25),
                    AFKKickShowWarningLastSeconds = table.Column<int>(nullable: false, defaultValue: 10),
                    AllowDataTransfer = table.Column<bool>(nullable: false),
                    Bloodscreen = table.Column<bool>(nullable: false),
                    BloodscreenCooldownMs = table.Column<int>(nullable: false, defaultValue: 150),
                    ChatFontSize = table.Column<float>(nullable: false, defaultValue: 1.4f),
                    ChatInfoFontSize = table.Column<float>(nullable: false, defaultValue: 1f),
                    ChatInfoMoveTimeMs = table.Column<int>(nullable: false, defaultValue: 15000),
                    ChatMaxHeight = table.Column<float>(nullable: false, defaultValue: 35f),
                    ChatWidth = table.Column<float>(nullable: false, defaultValue: 30f),
                    CheckAFK = table.Column<bool>(nullable: false),
                    DateTimeFormat = table.Column<string>(nullable: true, defaultValue: "yyyy'-'MM'-'dd HH':'mm':'ss"),
                    DiscordUserId = table.Column<decimal>(nullable: true),
                    FloatingDamageInfo = table.Column<bool>(nullable: false),
                    HideChatInfo = table.Column<bool>(nullable: false),
                    HideDirtyChat = table.Column<bool>(nullable: false),
                    Hitsound = table.Column<bool>(nullable: false),
                    HudAmmoUpdateCooldownMs = table.Column<int>(nullable: false, defaultValue: 100),
                    HudHealthUpdateCooldownMs = table.Column<int>(nullable: false, defaultValue: 100),
                    MapBorderColor = table.Column<string>(nullable: true, defaultValue: "rgba(150,0,0,0.35)"),
                    NametagArmorEmptyColor = table.Column<string>(nullable: true),
                    NametagArmorFullColor = table.Column<string>(nullable: true, defaultValue: "rgba(255, 255, 255, 1)"),
                    NametagDeadColor = table.Column<string>(nullable: true, defaultValue: "rgba(0, 0, 0, 1)"),
                    NametagHealthEmptyColor = table.Column<string>(nullable: true, defaultValue: "rgba(50, 0, 0, 1)"),
                    NametagHealthFullColor = table.Column<string>(nullable: true, defaultValue: "rgba(0, 255, 0, 1)"),
                    ScoreboardPlayerSortingDesc = table.Column<bool>(nullable: false),
                    ScoreboardPlaytimeUnit = table.Column<TimeSpanUnitsOfTime>(nullable: false, defaultValue: TimeSpanUnitsOfTime.HourMinute),
                    ShowConfettiAtRanking = table.Column<bool>(nullable: false),
                    ShowCursorInfo = table.Column<bool>(nullable: false),
                    ShowCursorOnChatOpen = table.Column<bool>(nullable: false),
                    ShowFloatingDamageInfoDurationMs = table.Column<int>(nullable: false, defaultValue: 1000),
                    ShowLobbyLeaveInfo = table.Column<bool>(nullable: false),
                    Timezone = table.Column<string>(nullable: true, defaultValue: "UTC"),
                    Voice3D = table.Column<bool>(nullable: false),
                    VoiceAutoVolume = table.Column<bool>(nullable: false),
                    VoiceVolume = table.Column<float>(nullable: false, defaultValue: 6f),
                    WindowsNotifications = table.Column<bool>(nullable: false),
                    Language = table.Column<Language>(nullable: false, defaultValue: Language.English),
                    ScoreboardPlayerSorting = table.Column<ScoreboardPlayerSorting>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSettings", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerSettings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    LastFreeUsernameChange = table.Column<DateTime>(nullable: true),
                    LastLoginTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    LastMapsBoughtCounterReduce = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    LoggedIn = table.Column<bool>(nullable: false),
                    MapsBoughtCounter = table.Column<int>(nullable: false, defaultValue: 1),
                    Money = table.Column<int>(nullable: false),
                    MuteTime = table.Column<int>(nullable: true),
                    PlayTime = table.Column<int>(nullable: false),
                    VoiceMuteTime = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerThemeSettings",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    ThemeBackgroundDarkColor = table.Column<string>(nullable: true, defaultValue: "linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)"),
                    ThemeBackgroundLightColor = table.Column<string>(nullable: true, defaultValue: "rgba(250, 250, 250, 0.87)"),
                    ThemeMainColor = table.Column<string>(nullable: true, defaultValue: "rgba(0,0,77,1)"),
                    ThemeSecondaryColor = table.Column<string>(nullable: true, defaultValue: "rgba(255,152,0,1)"),
                    ThemeWarnColor = table.Column<string>(nullable: true, defaultValue: "rgba(244,67,54,1)"),
                    ToolbarDesign = table.Column<int>(nullable: false, defaultValue: 1),
                    UseDarkTheme = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerThemeSettings", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerThemeSettings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTotalStats",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Money = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTotalStats", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerTotalStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerWeaponBodypartStats",
                columns: table => new
                {
                    BodyPart = table.Column<PedBodyPart>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<WeaponHash>(nullable: false),
                    AmountHits = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialHits = table.Column<int>(nullable: false, defaultValue: 0),
                    DealtDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    DealtOfficialDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    Kills = table.Column<int>(nullable: false, defaultValue: 0),
                    OfficialKills = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerWeaponBodypartStats", x => new { x.PlayerId, x.WeaponHash, x.BodyPart });
                    table.ForeignKey(
                        name: "FK_PlayerWeaponBodypartStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerWeaponBodypartStats_Weapons_WeaponHash",
                        column: x => x.WeaponHash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerWeaponStats",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<WeaponHash>(nullable: false),
                    AmountHeadshots = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountHits = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialHeadshots = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialHits = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountOfficialShots = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountShots = table.Column<int>(nullable: false, defaultValue: 0),
                    DealtDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    DealtOfficialDamage = table.Column<long>(nullable: false, defaultValue: 0L),
                    Kills = table.Column<int>(nullable: false, defaultValue: 0),
                    OfficialKills = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerWeaponStats", x => new { x.PlayerId, x.WeaponHash });
                    table.ForeignKey(
                        name: "FK_PlayerWeaponStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerWeaponStats_Weapons_WeaponHash",
                        column: x => x.WeaponHash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtleastAdminLevel = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false),
                    CloseTime = table.Column<DateTime>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<SupportType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportRequests_Players_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationAnswers",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAnswers", x => new { x.ApplicationId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ApplicationAnswers_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationAnswers_ApplicationQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ApplicationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    AdminId = table.Column<int>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationInvitations_Players_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationInvitations_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KillingspreeRewards",
                columns: table => new
                {
                    KillsAmount = table.Column<short>(nullable: false),
                    LobbyId = table.Column<int>(nullable: false),
                    HealthOrArmor = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KillingspreeRewards", x => new { x.LobbyId, x.KillsAmount });
                    table.ForeignKey(
                        name: "FK_KillingspreeRewards_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyArmsRaceWeapons",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    AtKill = table.Column<short>(nullable: false),
                    WeaponHash = table.Column<WeaponHash>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyArmsRaceWeapons", x => new { x.LobbyId, x.AtKill });
                    table.ForeignKey(
                        name: "FK_LobbyArmsRaceWeapons_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyArmsRaceWeapons_Weapons_WeaponHash",
                        column: x => x.WeaponHash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyFightSettings",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    AmountLifes = table.Column<short>(nullable: false, defaultValue: (short)1),
                    SpawnAgainAfterDeathMs = table.Column<int>(nullable: false, defaultValue: 400),
                    StartArmor = table.Column<short>(nullable: false, defaultValue: (short)100),
                    StartHealth = table.Column<short>(nullable: false, defaultValue: (short)100)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyFightSettings", x => x.LobbyId);
                    table.ForeignKey(
                        name: "FK_LobbyFightSettings_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyMapSettings",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    MapLimitTime = table.Column<int>(nullable: false, defaultValueSql: "10"),
                    MapLimitType = table.Column<MapLimitType>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyMapSettings", x => x.LobbyId);
                    table.ForeignKey(
                        name: "FK_LobbyMapSettings_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyRewards",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    MoneyPerAssist = table.Column<double>(nullable: false),
                    MoneyPerDamage = table.Column<double>(nullable: false),
                    MoneyPerKill = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyRewards", x => x.LobbyId);
                    table.ForeignKey(
                        name: "FK_LobbyRewards_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyRoundSettings",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    BombDefuseTimeMs = table.Column<int>(nullable: false, defaultValueSql: "8000"),
                    BombDetonateTimeMs = table.Column<int>(nullable: false, defaultValueSql: "45000"),
                    BombPlantTimeMs = table.Column<int>(nullable: false, defaultValueSql: "3000"),
                    CountdownTime = table.Column<int>(nullable: false, defaultValueSql: "5"),
                    MixTeamsAfterRound = table.Column<bool>(nullable: false),
                    RoundTime = table.Column<int>(nullable: false, defaultValueSql: "240"),
                    ShowRanking = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyRoundSettings", x => x.LobbyId);
                    table.ForeignKey(
                        name: "FK_LobbyRoundSettings_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyWeapons",
                columns: table => new
                {
                    Lobby = table.Column<int>(nullable: false),
                    Hash = table.Column<WeaponHash>(nullable: false),
                    Ammo = table.Column<int>(nullable: false),
                    Damage = table.Column<float>(nullable: true),
                    HeadMultiplicator = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyWeapons", x => new { x.Hash, x.Lobby });
                    table.ForeignKey(
                        name: "FK_LobbyWeapons_Weapons_Hash",
                        column: x => x.Hash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyWeapons_Lobbies_Lobby",
                        column: x => x.Lobby,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBans",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    AdminId = table.Column<int>(nullable: true),
                    EndTimestamp = table.Column<DateTime>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    PreventConnection = table.Column<bool>(nullable: false),
                    Reason = table.Column<string>(nullable: false),
                    SCId = table.Column<decimal>(nullable: true),
                    SCName = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    StartTimestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBans", x => new { x.PlayerId, x.LobbyId });
                    table.ForeignKey(
                        name: "FK_PlayerBans_Players_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlayerBans_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerBans_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PlayerLobbyStats",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    Damage = table.Column<int>(nullable: false),
                    Deaths = table.Column<int>(nullable: false),
                    Kills = table.Column<int>(nullable: false),
                    MostAssistsInARound = table.Column<int>(nullable: false),
                    MostDamageInARound = table.Column<int>(nullable: false),
                    MostKillsInARound = table.Column<int>(nullable: false),
                    TotalAssists = table.Column<int>(nullable: false),
                    TotalDamage = table.Column<int>(nullable: false),
                    TotalDeaths = table.Column<int>(nullable: false),
                    TotalKills = table.Column<int>(nullable: false),
                    TotalMapsBought = table.Column<int>(nullable: false),
                    TotalRounds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerLobbyStats", x => new { x.PlayerId, x.LobbyId });
                    table.ForeignKey(
                        name: "FK_PlayerLobbyStats_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerLobbyStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BlipColor = table.Column<byte>(nullable: false, defaultValue: (byte)4),
                    ColorB = table.Column<short>(nullable: false),
                    ColorG = table.Column<short>(nullable: false),
                    ColorR = table.Column<short>(nullable: false),
                    Index = table.Column<short>(nullable: false),
                    Lobby = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false, defaultValue: "Spectator"),
                    SkinHash = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Lobbies_Lobby",
                        column: x => x.Lobby,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyMaps",
                columns: table => new
                {
                    LobbyId = table.Column<int>(nullable: false),
                    MapId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyMaps", x => new { x.LobbyId, x.MapId });
                    table.ForeignKey(
                        name: "FK_LobbyMaps_Lobbies_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyMaps_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMapFavourites",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMapFavourites", x => new { x.PlayerId, x.MapId });
                    table.ForeignKey(
                        name: "FK_PlayerMapFavourites_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMapFavourites_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMapRatings",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Rating = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMapRatings", x => new { x.PlayerId, x.MapId });
                    table.ForeignKey(
                        name: "FK_PlayerMapRatings_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMapRatings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharAppearanceDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SyncedData_Slot = table.Column<byte>(nullable: true),
                    SyncedData_Blemishes = table.Column<int>(nullable: true),
                    SyncedData_BlemishesOpacity = table.Column<float>(nullable: true),
                    SyncedData_FacialHair = table.Column<int>(nullable: true),
                    SyncedData_FacialHairOpacity = table.Column<float>(nullable: true),
                    SyncedData_Eyebrows = table.Column<int>(nullable: true),
                    SyncedData_EyebrowsOpacity = table.Column<float>(nullable: true),
                    SyncedData_Ageing = table.Column<int>(nullable: true),
                    SyncedData_AgeingOpacity = table.Column<float>(nullable: true),
                    SyncedData_Makeup = table.Column<int>(nullable: true),
                    SyncedData_MakeupOpacity = table.Column<float>(nullable: true),
                    SyncedData_Blush = table.Column<int>(nullable: true),
                    SyncedData_BlushOpacity = table.Column<float>(nullable: true),
                    SyncedData_Complexion = table.Column<int>(nullable: true),
                    SyncedData_ComplexionOpacity = table.Column<float>(nullable: true),
                    SyncedData_SunDamage = table.Column<int>(nullable: true),
                    SyncedData_SunDamageOpacity = table.Column<float>(nullable: true),
                    SyncedData_Lipstick = table.Column<int>(nullable: true),
                    SyncedData_LipstickOpacity = table.Column<float>(nullable: true),
                    SyncedData_MolesAndFreckles = table.Column<int>(nullable: true),
                    SyncedData_MolesAndFrecklesOpacity = table.Column<float>(nullable: true),
                    SyncedData_ChestHair = table.Column<int>(nullable: true),
                    SyncedData_ChestHairOpacity = table.Column<float>(nullable: true),
                    SyncedData_BodyBlemishes = table.Column<int>(nullable: true),
                    SyncedData_BodyBlemishesOpacity = table.Column<float>(nullable: true),
                    SyncedData_AddBodyBlemishes = table.Column<int>(nullable: true),
                    SyncedData_AddBodyBlemishesOpacity = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharAppearanceDatas", x => new { x.PlayerId, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerCharAppearanceDatas_PlayerCharDatas_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PlayerCharDatas",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharFeaturesDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SyncedData_Slot = table.Column<byte>(nullable: true),
                    SyncedData_NoseWidth = table.Column<float>(nullable: true),
                    SyncedData_NoseHeight = table.Column<float>(nullable: true),
                    SyncedData_NoseLength = table.Column<float>(nullable: true),
                    SyncedData_NoseBridge = table.Column<float>(nullable: true),
                    SyncedData_NoseTip = table.Column<float>(nullable: true),
                    SyncedData_NoseBridgeShift = table.Column<float>(nullable: true),
                    SyncedData_BrowHeight = table.Column<float>(nullable: true),
                    SyncedData_BrowWidth = table.Column<float>(nullable: true),
                    SyncedData_CheekboneHeight = table.Column<float>(nullable: true),
                    SyncedData_CheekboneWidth = table.Column<float>(nullable: true),
                    SyncedData_CheeksWidth = table.Column<float>(nullable: true),
                    SyncedData_Eyes = table.Column<float>(nullable: true),
                    SyncedData_Lips = table.Column<float>(nullable: true),
                    SyncedData_JawWidth = table.Column<float>(nullable: true),
                    SyncedData_JawHeight = table.Column<float>(nullable: true),
                    SyncedData_ChinLength = table.Column<float>(nullable: true),
                    SyncedData_ChinPosition = table.Column<float>(nullable: true),
                    SyncedData_ChinWidth = table.Column<float>(nullable: true),
                    SyncedData_ChinShape = table.Column<float>(nullable: true),
                    SyncedData_NeckWidth = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharFeaturesDatas", x => new { x.PlayerId, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerCharFeaturesDatas_PlayerCharDatas_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PlayerCharDatas",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharGeneralDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SyncedData_Slot = table.Column<byte>(nullable: true),
                    SyncedData_IsMale = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharGeneralDatas", x => new { x.PlayerId, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerCharGeneralDatas_PlayerCharDatas_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PlayerCharDatas",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharHairAndColorsDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SyncedData_Slot = table.Column<byte>(nullable: true),
                    SyncedData_Hair = table.Column<int>(nullable: true),
                    SyncedData_HairColor = table.Column<int>(nullable: true),
                    SyncedData_HairHighlightColor = table.Column<int>(nullable: true),
                    SyncedData_EyebrowColor = table.Column<int>(nullable: true),
                    SyncedData_FacialHairColor = table.Column<int>(nullable: true),
                    SyncedData_EyeColor = table.Column<int>(nullable: true),
                    SyncedData_BlushColor = table.Column<int>(nullable: true),
                    SyncedData_LipstickColor = table.Column<int>(nullable: true),
                    SyncedData_ChestHairColor = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharHairAndColorsDatas", x => new { x.PlayerId, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerCharHairAndColorsDatas_PlayerCharDatas_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PlayerCharDatas",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCharHeritageDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    Slot = table.Column<byte>(nullable: false),
                    SyncedData_Slot = table.Column<byte>(nullable: true),
                    SyncedData_FatherIndex = table.Column<int>(nullable: true),
                    SyncedData_MotherIndex = table.Column<int>(nullable: true),
                    SyncedData_ResemblancePercentage = table.Column<float>(nullable: true),
                    SyncedData_SkinTonePercentage = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharHeritageDatas", x => new { x.PlayerId, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerCharHeritageDatas_PlayerCharDatas_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PlayerCharDatas",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportRequestMessages",
                columns: table => new
                {
                    MessageIndex = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    Text = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportRequestMessages", x => new { x.RequestId, x.MessageIndex });
                    table.ForeignKey(
                        name: "FK_SupportRequestMessages_Players_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportRequestMessages_SupportRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "SupportRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BlipColor = table.Column<byte>(nullable: false, defaultValue: (byte)1),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    HouseId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: true),
                    Short = table.Column<string>(maxLength: 20, nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gangs_GangHouses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "GangHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Gangs_Players_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Gangs_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GangRankPermissions",
                columns: table => new
                {
                    GangId = table.Column<int>(nullable: false),
                    InviteMembers = table.Column<int>(nullable: false),
                    KickMembers = table.Column<int>(nullable: false),
                    ManagePermissions = table.Column<int>(nullable: false),
                    ManageRanks = table.Column<int>(nullable: false),
                    StartGangwar = table.Column<int>(nullable: false),
                    SetRanks = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangRankPermissions", x => x.GangId);
                    table.ForeignKey(
                        name: "FK_GangRankPermissions_Gangs_GangId",
                        column: x => x.GangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GangRanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GangId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Rank = table.Column<short>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GangRanks_Gangs_GangId",
                        column: x => x.GangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GangStats",
                columns: table => new
                {
                    GangId = table.Column<int>(nullable: false),
                    AmountAttacks = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountAttacksWon = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountDefends = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountDefendsWon = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountMembersSoFar = table.Column<int>(nullable: false, defaultValue: 0),
                    Experience = table.Column<int>(nullable: false, defaultValue: 0),
                    Money = table.Column<int>(nullable: false, defaultValue: 0),
                    PeakGangwarAreasOwned = table.Column<int>(nullable: false, defaultValue: 0),
                    TotalMoneySoFar = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangStats", x => x.GangId);
                    table.ForeignKey(
                        name: "FK_GangStats_Gangs_GangId",
                        column: x => x.GangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GangVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Color1 = table.Column<int>(nullable: false),
                    Color2 = table.Column<int>(nullable: false),
                    GangId = table.Column<int>(nullable: false),
                    Model = table.Column<VehicleHash>(nullable: false),
                    SpawnPosX = table.Column<float>(nullable: false),
                    SpawnPosY = table.Column<float>(nullable: false),
                    SpawnPosZ = table.Column<float>(nullable: false),
                    SpawnRotX = table.Column<float>(nullable: false),
                    SpawnRotY = table.Column<float>(nullable: false),
                    SpawnRotZ = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GangVehicles_Gangs_GangId",
                        column: x => x.GangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GangwarAreas",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false),
                    OwnerGangId = table.Column<int>(nullable: false),
                    AttackCount = table.Column<int>(nullable: false, defaultValue: 0),
                    DefendCount = table.Column<int>(nullable: false, defaultValue: 0),
                    LastAttacked = table.Column<DateTime>(nullable: false, defaultValueSql: "'2019-1-1'::timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangwarAreas", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_GangwarAreas_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GangwarAreas_Gangs_OwnerGangId",
                        column: x => x.OwnerGangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GangMembers",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    GangId = table.Column<int>(nullable: false),
                    JoinTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', now())"),
                    RankId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangMembers", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_GangMembers_Gangs_GangId",
                        column: x => x.GangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GangMembers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GangMembers_GangRanks_RankId",
                        column: x => x.RankId,
                        principalTable: "GangRanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdminLevels",
                columns: new[] { "Level", "ColorB", "ColorG", "ColorR" },
                values: new object[,]
                {
                    { (short)0, (short)220, (short)220, (short)220 },
                    { (short)1, (short)113, (short)202, (short)113 },
                    { (short)2, (short)85, (short)132, (short)253 },
                    { (short)3, (short)50, (short)50, (short)222 }
                });

            migrationBuilder.InsertData(
                table: "BonusbotSettings",
                columns: new[] { "Id", "ActionsInfoChannelId", "AdminApplicationsChannelId", "BansInfoChannelId", "ErrorLogsChannelId", "GuildId", "SendPrivateMessageOnBan", "SendPrivateMessageOnOfflineMessage", "ServerInfosChannelId", "SupportRequestsChannelId" },
                values: new object[] { 1, 659088752890871818m, 659072893526736896m, 659705941771550730m, 659073884796092426m, 320309924175282177m, true, true, 659073271911809037m, 659073029896142855m });

            migrationBuilder.InsertData(
                table: "ChallengeSettings",
                columns: new[] { "Type", "Frequency", "MaxNumber", "MinNumber" },
                values: new object[,]
                {
                    { ChallengeType.WriteHelpfulIssue, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.RoundPlayed, ChallengeFrequency.Weekly, 100, 50 },
                    { ChallengeType.ReviewMaps, ChallengeFrequency.Forever, 10, 10 },
                    { ChallengeType.ReadTheRules, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.ReadTheFAQ, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.Killstreak, ChallengeFrequency.Weekly, 7, 3 },
                    { ChallengeType.Kills, ChallengeFrequency.Weekly, 150, 75 },
                    { ChallengeType.JoinDiscordServer, ChallengeFrequency.Forever, 1, 1 },
                    { ChallengeType.PlayTime, ChallengeFrequency.Weekly, 1500, 300 },
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
                table: "ChatInfos",
                columns: new[] { "Id", "Language", "Message" },
                values: new object[,]
                {
                    { 1, Language.German, "Du kannst die Lobby mit \"/leave\" verlassen." },
                    { 2, Language.English, "You can leave the lobby with \"/leave.\"" },
                    { 3, Language.German, "VIPs sind keine Spender" },
                    { 4, Language.English, "VIPs are not donators" },
                    { 7, Language.German, "Der Projektleiter ernennt Administratoren. Die Administratoren ernennen Supporter." },
                    { 6, Language.English, "There are 3 admin ranks: Supporter, Administrator, Project Leader" },
                    { 8, Language.English, "The project leader appoints administrators. The administrators appoint supporters." },
                    { 5, Language.German, "Es gibt 3 Admin-Ränge: Supporter, Administrator, Projektleiter" }
                });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[,]
                {
                    { (short)24, "GiveMoney", false, null, null, false },
                    { (short)25, "LobbyInvitePlayer", true, null, null, false },
                    { (short)21, "UnblockUser", false, null, null, false },
                    { (short)17, "OpenPrivateChat", false, null, null, false },
                    { (short)19, "UserId", false, null, null, false },
                    { (short)18, "PrivateMessage", false, null, null, false },
                    { (short)16, "ClosePrivateChat", false, null, null, false },
                    { (short)15, "Position", false, null, null, false },
                    { (short)14, "PrivateChat", false, null, null, false },
                    { (short)13, "TeamChat", false, null, null, false },
                    { (short)12, "GlobalChat", false, null, null, false },
                    { (short)11, "Suicide", false, null, null, false },
                    { (short)10, "LobbyLeave", false, null, null, false },
                    { (short)20, "BlockUser", false, null, null, false }
                });

            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Language", "Answer", "Question" },
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
                table: "FreeroamDefaultVehicle",
                columns: new[] { "VehicleType", "Note", "VehicleHash" },
                values: new object[,]
                {
                    { FreeroamVehicleType.Plane, null, VehicleHash.Pyro },
                    { FreeroamVehicleType.Bike, null, VehicleHash.Hakuchou2 },
                    { FreeroamVehicleType.Helicopter, null, VehicleHash.Akula },
                    { FreeroamVehicleType.Car, null, VehicleHash.Pfister811 },
                    { FreeroamVehicleType.Boat, null, VehicleHash.Speeder2 }
                });

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "Id", "Category", "Target" },
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
                table: "ServerSettings",
                columns: new[] { "Id", "ArenaNewMapProbabilityPercent", "DistanceToSpotToDefuse", "DistanceToSpotToPlant", "ErrorToPlayerOnNonExistentCommand", "GamemodeName", "GangwarAttackerCanBeMore", "GangwarOwnerCanBeMore", "GiveMoneyFee", "GiveMoneyMinAmount", "KillingSpreeMaxSecondsUntilNextKill", "MapRatingAmountForCheck", "MinMapRatingForNewMaps", "MultiplierRankingAssists", "MultiplierRankingDamage", "MultiplierRankingKills", "NametagMaxDistance", "SaveLogsCooldownMinutes", "SavePlayerDataCooldownMinutes", "SaveSeasonsCooldownMinutes", "ShowNametagOnlyOnAiming", "TeamOrderCooldownMs", "ToChatOnNonExistentCommand" },
                values: new object[] { (short)1, 2f, 3f, 3f, true, "tdm", true, false, 0.05f, 100, 18, 10, 3f, 25f, 1f, 75f, 80f, 1, 1, 1, true, 3000, false });

            migrationBuilder.InsertData(
                table: "ServerTotalStats",
                column: "Id",
                value: (short)1);

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Dbshotgun, 166f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Assaultshotgun, 192f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Switchblade, 50f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Machete, 45f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Marksmanpistol, 150f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Machinepistol, 20f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Knuckle, 30f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Heavypistol, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Battleaxe, 50f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Marksmanrifle_mk2, 65f, 0.005f, 2f, 0.5f, 1f, WeaponType.SniperRifle },
                    { WeaponHash.Pistol, 26f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Marksmanrifle, 65f, 0.005f, 2f, 0.5f, 1f, WeaponType.SniperRifle },
                    { WeaponHash.Doubleaction, 110f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Revolver_mk2, 110f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Revolver, 110f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Specialcarbine, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Assaultrifle_mk2, 30f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Assaultrifle, 30f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Snspistol_mk2, 28f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Snspistol, 28f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Assaultsmg, 23f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Hatchet, 50f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Bottle, 10f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Minismg, 22f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Sniperrifle, 101f, 0.005f, 1000f, 0.5f, 1f, WeaponType.SniperRifle }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Fireextinguisher, 0.005f, 1f, 0.5f, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Compactlauncher, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Snowball, 10f, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Vintagepistol, 34f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Combatpdw, 28f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Heavysniper, 216f, 0.005f, 2f, 0.5f, 1f, WeaponType.SniperRifle },
                    { WeaponHash.Heavysniper_mk2, 216f, 0.005f, 2f, 0.5f, 1f, WeaponType.SniperRifle },
                    { WeaponHash.Autoshotgun, 162f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Stone_hatchet, 50f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.HazardCan, 0.005f, 1f, 0.5f, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.CeramicPistol, 20f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Smokegrenade, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Specialcarbine_mk2, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Bullpuprifle_mk2, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Rayminigun, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Carbinerifle_mk2, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Raycarbine, 23f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Raypistol, 80f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Parachute, 0.005f, 1f, 0.5f, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.NavyRevolver, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Wrench, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Pipebomb, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Advancedrifle, 30f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Gusenberg, 34f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Combatpistol, 27f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Microsmg, 21f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Grenadelauncher_smoke, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Flare, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Flaregun, 50f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Golfclub, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Minigun, 30f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Heavyshotgun, 117f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Compactrifle, 34f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Stungun, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Stickybomb, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Smg_mk2, 22f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Smg, 22f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Molotov, 10f, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Ball, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Appistol, 28f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Pumpshotgun_mk2, 58f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Pumpshotgun, 58f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Pistol_mk2, 26f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Petrolcan, 0.005f, 1f, 0.5f, 1f, WeaponType.Rest });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Hominglauncher, 150f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Nightstick, 35f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Railgun, 50f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Proximine, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Musket, 165f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Grenadelauncher, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Unarmed, 15f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[] { WeaponHash.Bzgas, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Hash", "Damage", "DamageExpMult", "HeadShotDamageModifier", "HeadshotsExpMult", "KillsExpMult", "Type" },
                values: new object[,]
                {
                    { WeaponHash.Bullpupshotgun, 112f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Mg, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Knife, 45f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Pistol50, 51f, 0.005f, 1f, 0.5f, 1f, WeaponType.Handgun },
                    { WeaponHash.Bat, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Poolcue, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Grenade, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.ThrownWeapon },
                    { WeaponHash.Dagger, 45f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Flashlight, 30f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Crowbar, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee },
                    { WeaponHash.Carbinerifle, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Combatmg_mk2, 28f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Combatmg, 28f, 0.005f, 1f, 0.5f, 1f, WeaponType.MachineGun },
                    { WeaponHash.Firework, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Bullpuprifle, 32f, 0.005f, 1f, 0.5f, 1f, WeaponType.AssaultRifle },
                    { WeaponHash.Sawnoffshotgun, 160f, 0.005f, 1f, 0.5f, 1f, WeaponType.Shotgun },
                    { WeaponHash.Rpg, 100f, 0.005f, 1f, 0.5f, 1f, WeaponType.HeavyWeapon },
                    { WeaponHash.Hammer, 40f, 0.005f, 1f, 0.5f, 1f, WeaponType.Melee }
                });

            migrationBuilder.InsertData(
                table: "AdminLevelNames",
                columns: new[] { "Level", "Language", "Name" },
                values: new object[,]
                {
                    { (short)0, Language.English, "User" },
                    { (short)3, Language.English, "Projectleader" },
                    { (short)2, Language.German, "Administrator" },
                    { (short)2, Language.English, "Administrator" },
                    { (short)3, Language.German, "Projektleiter" },
                    { (short)1, Language.English, "Supporter" },
                    { (short)0, Language.German, "User" },
                    { (short)1, Language.German, "Supporter" }
                });

            migrationBuilder.InsertData(
                table: "CommandAlias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "PrivateSay", (short)14 },
                    { "Coord", (short)15 },
                    { "Coordinate", (short)15 },
                    { "Coordinates", (short)15 },
                    { "CurrentPos", (short)15 },
                    { "GetPos", (short)15 },
                    { "StopPrivateSay", (short)17 },
                    { "GetPosition", (short)15 },
                    { "Pos", (short)15 },
                    { "CPC", (short)16 },
                    { "ClosePM", (short)16 },
                    { "TSay", (short)13 },
                    { "ClosePrivateSay", (short)16 },
                    { "StopPrivateChat", (short)16 },
                    { "PChat", (short)14 },
                    { "InviteLobby", (short)25 },
                    { "OpenPM", (short)17 },
                    { "OPC", (short)17 },
                    { "LobbyInvite", (short)25 },
                    { "MoneySend", (short)24 },
                    { "SendMoney", (short)24 },
                    { "MoneyGive", (short)24 },
                    { "Unblock", (short)21 },
                    { "Block", (short)20 },
                    { "OpenPrivateSay", (short)17 },
                    { "IgnoreUser", (short)20 },
                    { "InvitePlayerLobby", (short)25 },
                    { "UID", (short)19 },
                    { "PSay", (short)18 },
                    { "PM", (short)18 },
                    { "TeamSay", (short)13 },
                    { "MSG", (short)18 },
                    { "Ignore", (short)20 },
                    { "TChat", (short)13 },
                    { "CurrentPosition", (short)15 },
                    { "Back", (short)10 },
                    { "Die", (short)11 },
                    { "Kill", (short)11 },
                    { "Leave", (short)10 },
                    { "AllChat", (short)12 },
                    { "AllSay", (short)12 },
                    { "G", (short)12 },
                    { "Dead", (short)11 },
                    { "Death", (short)11 },
                    { "GChat", (short)12 },
                    { "Global", (short)12 },
                    { "Mainmenu", (short)10 },
                    { "LeaveLobby", (short)10 },
                    { "GlobalSay", (short)12 },
                    { "PublicChat", (short)12 },
                    { "PublicSay", (short)12 }
                });

            migrationBuilder.InsertData(
                table: "CommandInfos",
                columns: new[] { "Id", "Language", "Info" },
                values: new object[,]
                {
                    { (short)18, Language.English, "Private message to a specific player." },
                    { (short)19, Language.German, "Gibt dir deine User-Id aus." },
                    { (short)17, Language.English, "Sends a private chat request or accepts the request of another user." },
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
                    { (short)12, Language.English, "Global chat which can be read everywhere." },
                    { (short)18, Language.German, "Private Nachricht an einen bestimmten Spieler." },
                    { (short)16, Language.English, "Closes a private chat or withdraws a private chat request." },
                    { (short)13, Language.German, "Sendet die Nachricht nur zum eigenen Team." },
                    { (short)13, Language.English, "Sends the message to the current team only." },
                    { (short)14, Language.German, "Gibt die Position des Spielers aus." },
                    { (short)17, Language.German, "Sendet eine Anfrage für einen Privatchat oder nimmt die Anfrage eines Users an." },
                    { (short)11, Language.English, "Kills the user (suicide)." },
                    { (short)11, Language.German, "Tötet den Nutzer (Selbstmord)." },
                    { (short)15, Language.German, "Sendet eine Nachricht im Privatchat." },
                    { (short)14, Language.English, "Outputs the position of the player." },
                    { (short)10, Language.English, "Leaves the current lobby." },
                    { (short)16, Language.German, "Schließt den Privatchat oder nimmt eine Privatchat-Anfrage zurück." },
                    { (short)10, Language.German, "Verlässt die jetzige Lobby." },
                    { (short)15, Language.English, "Sends a message in private chat." }
                });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[,]
                {
                    { (short)23, "VoiceMute", false, (short)1, null, true },
                    { (short)1, "AdminSay", false, (short)1, null, false },
                    { (short)2, "AdminChat", false, (short)1, null, true },
                    { (short)4, "Goto", true, (short)2, null, false },
                    { (short)5, "Kick", false, (short)1, null, true },
                    { (short)7, "LobbyKick", true, (short)1, null, true },
                    { (short)26, "Test", false, (short)3, null, false },
                    { (short)8, "Mute", false, (short)1, null, true },
                    { (short)3, "Ban", false, (short)2, null, false },
                    { (short)9, "NextMap", true, (short)1, null, true },
                    { (short)22, "LoadMapOfOthers", false, (short)1, null, true },
                    { (short)6, "LobbyBan", true, (short)1, null, true },
                    { (short)27, "CreateHouse", false, (short)2, null, false }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "AdminLeaderId", "AdminLvl", "Email", "IsVip", "Name", "Password", "SCId", "SCName" },
                values: new object[] { -1, -1, (short)0, "", false, "System", "", 0m, "System" });

            migrationBuilder.InsertData(
                table: "RuleTexts",
                columns: new[] { "RuleId", "Language", "RuleStr" },
                values: new object[,]
                {
                    { 1, Language.English, @"Teaming with opposing players is strictly forbidden!
                This means the deliberate sparing, better treatment, letting or similar of certain opposing players without the permission of the own team members.
                If such behaviour is noticed, it can lead to severe penalties and is permanently noted." },
                    { 1, Language.German, @"Teamen mit gegnerischen Spielern ist strengstens verboten!
                Damit ist das absichtliche Verschonen, besser Behandeln, Lassen o.ä. von bestimmten gegnerischen Spielern ohne Erlaubnis der eigenen Team-Mitglieder gemeint.
                Wird ein solches Verhalten bemerkt, kann es zu starken Strafen führen und es wird permanent notiert." },
                    { 2, Language.English, @"The normal chat in an official lobby has rules, the other chats (private lobbies, dirty) none.
                By 'normal chat' we mean all chat methods (global, team, etc.) in the 'normal' chat area.
                The chat rules listed here are ONLY for the normal chat in an official lobby.
                Chats in private lobbies can be freely monitored by the lobby owners." },
                    { 2, Language.German, @"Der normale Chat in einer offiziellen Lobby hat Regeln, die restlichen Chats (private Lobbys, dirty) jedoch keine.
                Unter 'normaler Chat' versteht man alle Chats-Methode (global, team usw.) im 'normal' Chat-Bereich.
                Die hier aufgelisteten Chat-Regeln richten sich NUR an den normalen Chat in einer offiziellen Lobby.
                Chats in privaten Lobbys können frei von den Lobby-Besitzern überwacht werden." },
                    { 3, Language.English, "Admins have to follow the same rules as players do." },
                    { 3, Language.German, "Admins haben genauso die Regeln zu befolgen wie auch die Spieler." },
                    { 4, Language.English, @"Exploitation of the commands is strictly forbidden!
                Admin commands for 'punishing' (kick, mute, ban etc.) may only be used for violations of rules." },
                    { 4, Language.German, @"Ausnutzung der Befehle ist strengstens verboten!
                Admin-Befehle zum 'Bestrafen' (Kick, Mute, Ban usw.) dürfen auch nur bei Verstößen gegen Regeln genutzt werden." },
                    { 5, Language.English, @"If you are not sure if the time for e.g. Mute or Bann could be too high,
                ask your team leader - if you can't reach someone quickly, choose a lower time.
                Too high times are bad, too low times are no problem." },
                    { 5, Language.German, @"Wenn du dir nicht sicher bist, ob die Zeit für z.B. Mute oder Bann zu hoch sein könnte,
                frage deinen Team-Leiter - kannst du niemanden auf die Schnelle erreichen, so entscheide dich für eine niedrigere Zeit.
                Zu hohe Zeiten sind schlecht, zu niedrige kein Problem." },
                    { 6, Language.English, "All admin rules with the exception of activity duty are also valid for VIPs." },
                    { 6, Language.German, "Alle Admin-Regeln mit Ausnahme von Aktivitäts-Pflicht sind auch gültig für VIPs." },
                    { 7, Language.English, "The VIPs are free to decide whether they want to use their rights or not." },
                    { 7, Language.German, "Den VIPs ist es frei überlassen, ob sie ihre Rechte nutzen wollen oder nicht." }
                });

            migrationBuilder.InsertData(
                table: "CommandAlias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "Skip", (short)9 },
                    { "KickLobby", (short)7 },
                    { "TBan", (short)3 },
                    { "RBan", (short)3 },
                    { "PermaMute", (short)8 },
                    { "PMute", (short)8 },
                    { "RMute", (short)8 },
                    { "TimeMute", (short)8 },
                    { "TMute", (short)8 },
                    { "Permaban", (short)3 },
                    { "PBan", (short)3 },
                    { "EndRound", (short)9 },
                    { "Next", (short)9 },
                    { "VoiceTimeMute", (short)23 },
                    { "MuteVoice", (short)23 },
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
                    { "VoiceRMute", (short)23 },
                    { "PermaVoiceMute", (short)23 },
                    { "UBan", (short)3 },
                    { "Timeban", (short)3 },
                    { "UnBan", (short)3 },
                    { "HouseNew", (short)27 },
                    { "NewHouse", (short)27 },
                    { "HouseCreate", (short)27 },
                    { "XYZ", (short)4 },
                    { "WarpToPlayer", (short)4 },
                    { "WarpTo", (short)4 },
                    { "Announce", (short)1 },
                    { "Announcement", (short)1 },
                    { "ASay", (short)1 },
                    { "BanLobby", (short)6 },
                    { "OSay", (short)1 },
                    { "OChat", (short)1 },
                    { "GotoPlayer", (short)4 },
                    { "RKick", (short)5 },
                    { "Warp", (short)4 },
                    { "InternalChat", (short)2 },
                    { "WriteAdmin", (short)2 },
                    { "VoiceTMute", (short)23 },
                    { "ChatAdmin", (short)2 },
                    { "AChat", (short)2 },
                    { "GotoXYZ", (short)4 },
                    { "InternChat", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "CommandInfos",
                columns: new[] { "Id", "Language", "Info" },
                values: new object[,]
                {
                    { (short)4, Language.German, "Teleportiert den Nutzer zu einem Spieler (evtl. in sein Auto) oder zu den angegebenen Koordinaten." },
                    { (short)4, Language.English, "Warps the user to another player (maybe in his vehicle) or to the defined coordinates." },
                    { (short)3, Language.English, "Bans a player out of the server." },
                    { (short)23, Language.German, "Mutet einen Spieler im Voice-Chat." },
                    { (short)3, Language.German, "Bannt einen Spieler vom gesamten Server." },
                    { (short)27, Language.German, "Erstellt ein Haus in der Gang-Lobby." },
                    { (short)27, Language.English, "Creates a house in the gang lobby." },
                    { (short)23, Language.English, "Mutes a player in the voice-chat." },
                    { (short)26, Language.English, "Command for quick testing of codes." },
                    { (short)9, Language.German, "Beendet die jetzige Runde in der jeweiligen Lobby." },
                    { (short)1, Language.German, "Schreibt öffentlich als ein Admin." },
                    { (short)9, Language.English, "Ends the current round in the lobby." },
                    { (short)2, Language.German, "Schreibt intern nur den Admins." },
                    { (short)2, Language.English, "Writes internally to admins only." },
                    { (short)5, Language.German, "Kickt einen Spieler vom Server." },
                    { (short)5, Language.English, "Kicks a player out of the server." },
                    { (short)1, Language.English, "Writes public as an admin." },
                    { (short)6, Language.English, "Bans a player out of the lobby in which the command was used." },
                    { (short)7, Language.German, "Kickt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." },
                    { (short)7, Language.English, "Kicks a player out of the lobby in which the command was used." },
                    { (short)8, Language.German, "Mutet einen Spieler im normalen Chat." },
                    { (short)8, Language.English, "Mutes a player in the normal chat." },
                    { (short)26, Language.German, "Befehl zum schnellen Testen von Codes." },
                    { (short)6, Language.German, "Bannt einen Spieler aus der Lobby, in welchem der Befehl genutzt wurde." }
                });

            migrationBuilder.InsertData(
                table: "Lobbies",
                columns: new[] { "Id", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "Type" },
                values: new object[] { -1, true, false, "Arena", -1, "", LobbyType.Arena });

            migrationBuilder.InsertData(
                table: "Lobbies",
                columns: new[] { "Id", "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "Type" },
                values: new object[,]
                {
                    { -2, -365.425f, -131.809f, 37.873f, true, false, "GangLobby", -1, null, LobbyType.GangLobby },
                    { -3, -365.425f, -131.809f, 37.873f, true, false, "MapCreateLobby", -1, null, LobbyType.MapCreateLobby },
                    { -5, -425.2233f, 1126.9731f, 326.8f, true, false, "CharCreateLobby", -1, null, LobbyType.CharCreateLobby }
                });

            migrationBuilder.InsertData(
                table: "Lobbies",
                columns: new[] { "Id", "IsOfficial", "IsTemporary", "Name", "OwnerId", "Password", "Type" },
                values: new object[] { -4, true, false, "MainMenu", -1, "", LobbyType.MainMenu });

            migrationBuilder.InsertData(
                table: "Maps",
                columns: new[] { "Id", "CreatorId", "Name" },
                values: new object[,]
                {
                    { -5, -1, "All Gangwars" },
                    { -4, -1, "All Sniper" },
                    { -2, -1, "All Normals" },
                    { -1, -1, "All" },
                    { -6, -1, "All Arms Races" },
                    { -3, -1, "All Bombs" }
                });

            migrationBuilder.InsertData(
                table: "KillingspreeRewards",
                columns: new[] { "LobbyId", "KillsAmount", "HealthOrArmor" },
                values: new object[,]
                {
                    { -1, (short)15, (short)100 },
                    { -1, (short)5, (short)50 },
                    { -1, (short)3, (short)30 },
                    { -1, (short)10, (short)100 }
                });

            migrationBuilder.InsertData(
                table: "LobbyArmsRaceWeapons",
                columns: new[] { "LobbyId", "AtKill", "WeaponHash" },
                values: new object[,]
                {
                    { -1, (short)16, null },
                    { -1, (short)15, WeaponHash.Revolver },
                    { -1, (short)14, WeaponHash.Heavypistol },
                    { -1, (short)13, WeaponHash.Pistol50 },
                    { -1, (short)12, WeaponHash.Combatpistol },
                    { -1, (short)9, WeaponHash.Heavysniper },
                    { -1, (short)10, WeaponHash.Marksmanrifle },
                    { -1, (short)7, WeaponHash.Pumpshotgun },
                    { -1, (short)6, WeaponHash.Specialcarbine },
                    { -1, (short)5, WeaponHash.Advancedrifle },
                    { -1, (short)4, WeaponHash.Carbinerifle },
                    { -1, (short)3, WeaponHash.Assaultrifle },
                    { -1, (short)2, WeaponHash.Machinepistol },
                    { -1, (short)1, WeaponHash.Assaultsmg },
                    { -1, (short)0, WeaponHash.Microsmg },
                    { -1, (short)8, WeaponHash.Assaultshotgun },
                    { -1, (short)11, WeaponHash.Combatmg }
                });

            migrationBuilder.InsertData(
                table: "LobbyFightSettings",
                column: "LobbyId",
                value: -1);

            migrationBuilder.InsertData(
                table: "LobbyMapSettings",
                columns: new[] { "LobbyId", "MapLimitTime", "MapLimitType" },
                values: new object[] { -1, 10, MapLimitType.KillAfterTime });

            migrationBuilder.InsertData(
                table: "LobbyMaps",
                columns: new[] { "LobbyId", "MapId" },
                values: new object[] { -1, -1 });

            migrationBuilder.InsertData(
                table: "LobbyRewards",
                columns: new[] { "LobbyId", "MoneyPerAssist", "MoneyPerDamage", "MoneyPerKill" },
                values: new object[,]
                {
                    { -2, 10.0, 0.10000000000000001, 20.0 },
                    { -1, 10.0, 0.10000000000000001, 20.0 }
                });

            migrationBuilder.InsertData(
                table: "LobbyRoundSettings",
                columns: new[] { "LobbyId", "BombDefuseTimeMs", "BombDetonateTimeMs", "BombPlantTimeMs", "CountdownTime", "MixTeamsAfterRound", "RoundTime", "ShowRanking" },
                values: new object[] { -1, 8000, 45000, 3000, 5, true, 240, true });

            migrationBuilder.InsertData(
                table: "LobbyWeapons",
                columns: new[] { "Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator" },
                values: new object[,]
                {
                    { WeaponHash.Battleaxe, -1, 9999, null, null },
                    { WeaponHash.Marksmanrifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Marksmanrifle, -1, 9999, null, null },
                    { WeaponHash.Doubleaction, -1, 9999, null, null },
                    { WeaponHash.Revolver_mk2, -1, 9999, null, null },
                    { WeaponHash.Revolver, -1, 9999, null, null },
                    { WeaponHash.Specialcarbine, -1, 9999, null, null },
                    { WeaponHash.Assaultrifle_mk2, -1, 9999, null, null },
                    { WeaponHash.Assaultrifle, -1, 9999, null, null },
                    { WeaponHash.Snspistol_mk2, -1, 9999, null, null },
                    { WeaponHash.Snspistol, -1, 9999, null, null },
                    { WeaponHash.Pipebomb, -1, 9999, null, null },
                    { WeaponHash.Rpg, -1, 9999, null, null },
                    { WeaponHash.Advancedrifle, -1, 9999, null, null },
                    { WeaponHash.Proximine, -1, 9999, null, null },
                    { WeaponHash.Musket, -1, 9999, null, null },
                    { WeaponHash.Grenadelauncher, -1, 9999, null, null },
                    { WeaponHash.Bzgas, -1, 9999, null, null },
                    { WeaponHash.Bullpupshotgun, -1, 9999, null, null },
                    { WeaponHash.Mg, -1, 9999, null, null },
                    { WeaponHash.Minismg, -1, 9999, null, null },
                    { WeaponHash.Heavypistol, -1, 9999, null, null },
                    { WeaponHash.Raycarbine, -1, 9999, null, null },
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
                    { WeaponHash.Knife, -1, 9999, null, null },
                    { WeaponHash.Parachute, -1, 9999, null, null },
                    { WeaponHash.Bottle, -1, 9999, null, null },
                    { WeaponHash.Hatchet, -1, 9999, null, null },
                    { WeaponHash.Assaultsmg, -1, 9999, null, null },
                    { WeaponHash.Dbshotgun, -1, 9999, null, null },
                    { WeaponHash.Assaultshotgun, -1, 9999, null, null },
                    { WeaponHash.Switchblade, -1, 9999, null, null },
                    { WeaponHash.Machete, -1, 9999, null, null },
                    { WeaponHash.Marksmanpistol, -1, 9999, null, null },
                    { WeaponHash.Raypistol, -1, 9999, null, null },
                    { WeaponHash.Pistol50, -1, 9999, null, null },
                    { WeaponHash.Sawnoffshotgun, -1, 9999, null, null },
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
                    { WeaponHash.Grenade, -1, 9999, null, null },
                    { WeaponHash.Dagger, -1, 9999, null, null },
                    { WeaponHash.Flashlight, -1, 9999, null, null },
                    { WeaponHash.Crowbar, -1, 9999, null, null },
                    { WeaponHash.Carbinerifle, -1, 9999, null, null },
                    { WeaponHash.Combatmg_mk2, -1, 9999, null, null },
                    { WeaponHash.Combatmg, -1, 9999, null, null },
                    { WeaponHash.Firework, -1, 9999, null, null },
                    { WeaponHash.Heavyshotgun, -1, 9999, null, null },
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
                table: "Teams",
                columns: new[] { "Id", "BlipColor", "ColorB", "ColorG", "ColorR", "Index", "Lobby", "Name" },
                values: new object[,]
                {
                    { -5, (byte)4, (short)255, (short)255, (short)255, (short)0, -2, "None" },
                    { -2, (byte)4, (short)255, (short)255, (short)255, (short)0, -1, "Spectator" },
                    { -3, (byte)52, (short)0, (short)150, (short)0, (short)1, -1, "SWAT" },
                    { -4, (byte)1, (short)0, (short)0, (short)150, (short)2, -1, "Terrorist" },
                    { -1, (byte)4, (short)255, (short)255, (short)255, (short)0, -4, "Spectator" }
                });

            migrationBuilder.InsertData(
                table: "Gangs",
                columns: new[] { "Id", "Color", "HouseId", "Name", "OwnerId", "Short", "TeamId" },
                values: new object[] { -1, "rgb(255,255,255)", null, "System", null, "-", -5 });

            migrationBuilder.InsertData(
                table: "GangRankPermissions",
                columns: new[] { "GangId", "InviteMembers", "KickMembers", "ManagePermissions", "ManageRanks", "SetRanks", "StartGangwar" },
                values: new object[] { -1, 1, 1, 1, 1, 0, 1 });

            migrationBuilder.InsertData(
                table: "GangRanks",
                columns: new[] { "Id", "Color", "GangId", "Name", "Rank" },
                values: new object[] { -1, "rgb(255,255,255)", -1, "-", (short)0 });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAnswers_QuestionId",
                table: "ApplicationAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationInvitations_AdminId",
                table: "ApplicationInvitations",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationInvitations_ApplicationId",
                table: "ApplicationInvitations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationQuestions_AdminId",
                table: "ApplicationQuestions",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_PlayerId",
                table: "Applications",
                column: "PlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandAlias_Command",
                table: "CommandAlias",
                column: "Command");

            migrationBuilder.CreateIndex(
                name: "IX_Commands_NeededAdminLevel",
                table: "Commands",
                column: "NeededAdminLevel");

            migrationBuilder.CreateIndex(
                name: "IX_GangHouses_CreatorId",
                table: "GangHouses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GangMembers_GangId",
                table: "GangMembers",
                column: "GangId");

            migrationBuilder.CreateIndex(
                name: "IX_GangMembers_RankId",
                table: "GangMembers",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_GangRanks_GangId",
                table: "GangRanks",
                column: "GangId");

            migrationBuilder.CreateIndex(
                name: "IX_GangRanks_Rank",
                table: "GangRanks",
                column: "Rank");

            migrationBuilder.CreateIndex(
                name: "IX_Gangs_HouseId",
                table: "Gangs",
                column: "HouseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gangs_OwnerId",
                table: "Gangs",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gangs_TeamId",
                table: "Gangs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GangVehicles_GangId",
                table: "GangVehicles",
                column: "GangId");

            migrationBuilder.CreateIndex(
                name: "IX_GangwarAreas_OwnerGangId",
                table: "GangwarAreas",
                column: "OwnerGangId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_OwnerId",
                table: "Lobbies",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyArmsRaceWeapons_WeaponHash",
                table: "LobbyArmsRaceWeapons",
                column: "WeaponHash");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMaps_MapId",
                table: "LobbyMaps",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyWeapons_Lobby",
                table: "LobbyWeapons",
                column: "Lobby");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_CreatorId",
                table: "Maps",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_Name",
                table: "Maps",
                column: "Name")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_Offlinemessages_SourceId",
                table: "Offlinemessages",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Offlinemessages_TargetId",
                table: "Offlinemessages",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_AdminId",
                table: "PlayerBans",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_IP",
                table: "PlayerBans",
                column: "IP");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_LobbyId",
                table: "PlayerBans",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_SCId",
                table: "PlayerBans",
                column: "SCId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_SCName",
                table: "PlayerBans",
                column: "SCName");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBans_Serial",
                table: "PlayerBans",
                column: "Serial");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCommands_CommandId",
                table: "PlayerCommands",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCommands_PlayerId",
                table: "PlayerCommands",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLobbyStats_LobbyId",
                table: "PlayerLobbyStats",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMapFavourites_MapId",
                table: "PlayerMapFavourites",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMapRatings_MapId",
                table: "PlayerMapRatings",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRelations_TargetId",
                table: "PlayerRelations",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AdminLeaderId",
                table: "Players",
                column: "AdminLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AdminLvl",
                table: "Players",
                column: "AdminLvl");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSettings_DiscordUserId",
                table: "PlayerSettings",
                column: "DiscordUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerWeaponBodypartStats_WeaponHash",
                table: "PlayerWeaponBodypartStats",
                column: "WeaponHash");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerWeaponStats_WeaponHash",
                table: "PlayerWeaponStats",
                column: "WeaponHash");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequestMessages_AuthorId",
                table: "SupportRequestMessages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_AuthorId",
                table: "SupportRequests",
                column: "AuthorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Lobby",
                table: "Teams",
                column: "Lobby");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminLevelNames");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "ApplicationAnswers");

            migrationBuilder.DropTable(
                name: "ApplicationInvitations");

            migrationBuilder.DropTable(
                name: "BonusbotSettings");

            migrationBuilder.DropTable(
                name: "ChallengeSettings");

            migrationBuilder.DropTable(
                name: "ChatInfos");

            migrationBuilder.DropTable(
                name: "CommandAlias");

            migrationBuilder.DropTable(
                name: "CommandInfos");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "FreeroamDefaultVehicle");

            migrationBuilder.DropTable(
                name: "GangLevelSettings");

            migrationBuilder.DropTable(
                name: "GangMembers");

            migrationBuilder.DropTable(
                name: "GangRankPermissions");

            migrationBuilder.DropTable(
                name: "GangStats");

            migrationBuilder.DropTable(
                name: "GangVehicles");

            migrationBuilder.DropTable(
                name: "GangwarAreas");

            migrationBuilder.DropTable(
                name: "KillingspreeRewards");

            migrationBuilder.DropTable(
                name: "LobbyArmsRaceWeapons");

            migrationBuilder.DropTable(
                name: "LobbyFightSettings");

            migrationBuilder.DropTable(
                name: "LobbyMaps");

            migrationBuilder.DropTable(
                name: "LobbyMapSettings");

            migrationBuilder.DropTable(
                name: "LobbyRewards");

            migrationBuilder.DropTable(
                name: "LobbyRoundSettings");

            migrationBuilder.DropTable(
                name: "LobbyWeapons");

            migrationBuilder.DropTable(
                name: "LogAdmins");

            migrationBuilder.DropTable(
                name: "LogChats");

            migrationBuilder.DropTable(
                name: "LogErrors");

            migrationBuilder.DropTable(
                name: "LogKills");

            migrationBuilder.DropTable(
                name: "LogRests");

            migrationBuilder.DropTable(
                name: "Offlinemessages");

            migrationBuilder.DropTable(
                name: "PlayerBans");

            migrationBuilder.DropTable(
                name: "PlayerChallenges");

            migrationBuilder.DropTable(
                name: "PlayerCharAppearanceDatas");

            migrationBuilder.DropTable(
                name: "PlayerCharFeaturesDatas");

            migrationBuilder.DropTable(
                name: "PlayerCharGeneralDatas");

            migrationBuilder.DropTable(
                name: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropTable(
                name: "PlayerCharHeritageDatas");

            migrationBuilder.DropTable(
                name: "PlayerClothes");

            migrationBuilder.DropTable(
                name: "PlayerCommands");

            migrationBuilder.DropTable(
                name: "PlayerLobbyStats");

            migrationBuilder.DropTable(
                name: "PlayerMapFavourites");

            migrationBuilder.DropTable(
                name: "PlayerMapRatings");

            migrationBuilder.DropTable(
                name: "PlayerRelations");

            migrationBuilder.DropTable(
                name: "PlayerSettings");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "PlayerThemeSettings");

            migrationBuilder.DropTable(
                name: "PlayerTotalStats");

            migrationBuilder.DropTable(
                name: "PlayerWeaponBodypartStats");

            migrationBuilder.DropTable(
                name: "PlayerWeaponStats");

            migrationBuilder.DropTable(
                name: "RuleTexts");

            migrationBuilder.DropTable(
                name: "ServerDailyStats");

            migrationBuilder.DropTable(
                name: "ServerSettings");

            migrationBuilder.DropTable(
                name: "ServerTotalStats");

            migrationBuilder.DropTable(
                name: "SupportRequestMessages");

            migrationBuilder.DropTable(
                name: "ApplicationQuestions");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "GangRanks");

            migrationBuilder.DropTable(
                name: "PlayerCharDatas");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "SupportRequests");

            migrationBuilder.DropTable(
                name: "Gangs");

            migrationBuilder.DropTable(
                name: "GangHouses");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Lobbies");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "AdminLevels");
        }
    }
}

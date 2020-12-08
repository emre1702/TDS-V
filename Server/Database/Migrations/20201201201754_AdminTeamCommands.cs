﻿using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Migrations
{
#pragma warning disable
    public partial class AdminTeamCommands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .Annotation("Npgsql:Enum:challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .Annotation("Npgsql:Enum:freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:language", "german,english")
                .Annotation("Npgsql:Enum:lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,char_create_lobby,gang_action_lobby,damage_test_lobby")
                .Annotation("Npgsql:Enum:log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto,remove_map,voice_mute,reset_password,admin_team,set_admin_leader,set_vip")
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
                .Annotation("Npgsql:PostgresExtension:tsm_system_rows", ",,")
                .OldAnnotation("Npgsql:Enum:challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .OldAnnotation("Npgsql:Enum:challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .OldAnnotation("Npgsql:Enum:freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .OldAnnotation("Npgsql:Enum:language", "german,english")
                .OldAnnotation("Npgsql:Enum:lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,char_create_lobby,gang_action_lobby,damage_test_lobby")
                .OldAnnotation("Npgsql:Enum:log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto,remove_map,voice_mute,reset_password,admin_team")
                .OldAnnotation("Npgsql:Enum:map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .OldAnnotation("Npgsql:Enum:ped_body_part", "head,neck,torso,genital_region,arm,hand,leg,foot")
                .OldAnnotation("Npgsql:Enum:player_relation", "none,block,friend")
                .OldAnnotation("Npgsql:Enum:rule_category", "general,chat")
                .OldAnnotation("Npgsql:Enum:rule_target", "user,admin,vip")
                .OldAnnotation("Npgsql:Enum:scoreboard_player_sorting", "name,play_time,kills,assists,deaths,kills_deaths_ratio,kills_deaths_assists_ratio")
                .OldAnnotation("Npgsql:Enum:support_type", "question,help,compliment,complaint")
                .OldAnnotation("Npgsql:Enum:time_span_units_of_time", "second,minute,hour_minute,hour,day,week")
                .OldAnnotation("Npgsql:Enum:userpanel_admin_question_answer_type", "text,check,number")
                .OldAnnotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust,minitank,retinue2,outlaw,yosemite2,stryder,jb7002,sultan2,everon,sugoi,zhaba,formula,formula2,rebla,vagrant,furia,vstr,komoda,asbo,kanjo,imorgon")
                .OldAnnotation("Npgsql:Enum:weapon_hash", "advancedrifle,appistol,assaultrifle,assaultrifle_mk2,assaultshotgun,assaultsmg,autoshotgun,ball,bat,battleaxe,bottle,bullpuprifle,bullpuprifle_mk2,bullpupshotgun,bzgas,carbinerifle,carbinerifle_mk2,combatmg,combatmg_mk2,combatpdw,combatpistol,compactlauncher,compactrifle,crowbar,dagger,dbshotgun,doubleaction,fireextinguisher,firework,flare,flaregun,flashlight,golfclub,grenade,grenadelauncher,grenadelauncher_smoke,gusenberg,hammer,hatchet,heavypistol,heavyshotgun,heavysniper,heavysniper_mk2,hominglauncher,knife,knuckle,machete,machinepistol,marksmanpistol,marksmanrifle,marksmanrifle_mk2,mg,microsmg,minigun,minismg,molotov,musket,nightstick,parachute,petrolcan,pipebomb,pistol,pistol_mk2,pistol50,poolcue,proximine,pumpshotgun,pumpshotgun_mk2,railgun,revolver,revolver_mk2,rpg,sawnoffshotgun,smg,smg_mk2,smokegrenade,sniperrifle,snowball,snspistol,snspistol_mk2,stone_hatchet,specialcarbine,specialcarbine_mk2,stickybomb,stungun,switchblade,unarmed,vintagepistol,wrench,raypistol,raycarbine,rayminigun,ceramic_pistol,hazard_can,navy_revolver")
                .OldAnnotation("Npgsql:Enum:weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .OldAnnotation("Npgsql:PostgresExtension:tsm_system_rows", ",,");

            migrationBuilder.Sql("ALTER TYPE log_type RENAME VALUE 'admin_team' TO 'set_admin'");

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
                values: new object[,]
                {
                    { (short)30, "SetAdminLeader", false, (short)3, null, false },
                    { (short)31, "SetVip", false, (short)3, null, false },
                    { (short)32, "AdminLeader", false, null, null, false }
                });

            migrationBuilder.InsertData(
                table: "CommandAlias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "SetSupervisor", (short)30 },
                    { "SetVorgesetzter", (short)30 },
                    { "GiveVip", (short)31 },
                    { "Supervisor", (short)32 },
                    { "Vorgesetzter", (short)32 },
                    { "GetSupervisor", (short)32 },
                    { "GetAdminLeader", (short)32 }
                });

            migrationBuilder.InsertData(
                table: "CommandInfos",
                columns: new[] { "Id", "Language", "Info" },
                values: new object[,]
                {
                    { (short)30, Language.German, "Setzt den Vorgesetzten eines Spielers." },
                    { (short)30, Language.English, "Sets the supervisor of a player." },
                    { (short)31, Language.German, "Gibt einem Spieler den VIP-Rang." },
                    { (short)31, Language.English, "Gives a player the vip rank." },
                    { (short)32, Language.German, "Gibt den Vorgesetzten eines Admins aus." },
                    { (short)32, Language.English, "Outputs the supervisor of an admin." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "GetAdminLeader", (short)32 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "GetSupervisor", (short)32 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "GiveVip", (short)31 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "SetSupervisor", (short)30 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "SetVorgesetzter", (short)30 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "Supervisor", (short)32 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "Vorgesetzter", (short)32 });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)30, Language.German });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)30, Language.English });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)31, Language.German });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)31, Language.English });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)32, Language.German });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)32, Language.English });

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: (short)30);

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: (short)31);

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: (short)32);

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .Annotation("Npgsql:Enum:challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .Annotation("Npgsql:Enum:freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:language", "german,english")
                .Annotation("Npgsql:Enum:lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,char_create_lobby,gang_action_lobby,damage_test_lobby")
                .Annotation("Npgsql:Enum:log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto,remove_map,voice_mute,reset_password,set_admin")
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
                .Annotation("Npgsql:PostgresExtension:tsm_system_rows", ",,")
                .OldAnnotation("Npgsql:Enum:challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .OldAnnotation("Npgsql:Enum:challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .OldAnnotation("Npgsql:Enum:freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .OldAnnotation("Npgsql:Enum:language", "german,english")
                .OldAnnotation("Npgsql:Enum:lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,char_create_lobby,gang_action_lobby,damage_test_lobby")
                .OldAnnotation("Npgsql:Enum:log_type", "kick,ban,mute,next,login,register,lobby_join,lobby_leave,lobby_kick,lobby_ban,goto,remove_map,voice_mute,reset_password,set_admin,set_admin_leader,set_vip")
                .OldAnnotation("Npgsql:Enum:map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .OldAnnotation("Npgsql:Enum:ped_body_part", "head,neck,torso,genital_region,arm,hand,leg,foot")
                .OldAnnotation("Npgsql:Enum:player_relation", "none,block,friend")
                .OldAnnotation("Npgsql:Enum:rule_category", "general,chat")
                .OldAnnotation("Npgsql:Enum:rule_target", "user,admin,vip")
                .OldAnnotation("Npgsql:Enum:scoreboard_player_sorting", "name,play_time,kills,assists,deaths,kills_deaths_ratio,kills_deaths_assists_ratio")
                .OldAnnotation("Npgsql:Enum:support_type", "question,help,compliment,complaint")
                .OldAnnotation("Npgsql:Enum:time_span_units_of_time", "second,minute,hour_minute,hour,day,week")
                .OldAnnotation("Npgsql:Enum:userpanel_admin_question_answer_type", "text,check,number")
                .OldAnnotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust,minitank,retinue2,outlaw,yosemite2,stryder,jb7002,sultan2,everon,sugoi,zhaba,formula,formula2,rebla,vagrant,furia,vstr,komoda,asbo,kanjo,imorgon")
                .OldAnnotation("Npgsql:Enum:weapon_hash", "advancedrifle,appistol,assaultrifle,assaultrifle_mk2,assaultshotgun,assaultsmg,autoshotgun,ball,bat,battleaxe,bottle,bullpuprifle,bullpuprifle_mk2,bullpupshotgun,bzgas,carbinerifle,carbinerifle_mk2,combatmg,combatmg_mk2,combatpdw,combatpistol,compactlauncher,compactrifle,crowbar,dagger,dbshotgun,doubleaction,fireextinguisher,firework,flare,flaregun,flashlight,golfclub,grenade,grenadelauncher,grenadelauncher_smoke,gusenberg,hammer,hatchet,heavypistol,heavyshotgun,heavysniper,heavysniper_mk2,hominglauncher,knife,knuckle,machete,machinepistol,marksmanpistol,marksmanrifle,marksmanrifle_mk2,mg,microsmg,minigun,minismg,molotov,musket,nightstick,parachute,petrolcan,pipebomb,pistol,pistol_mk2,pistol50,poolcue,proximine,pumpshotgun,pumpshotgun_mk2,railgun,revolver,revolver_mk2,rpg,sawnoffshotgun,smg,smg_mk2,smokegrenade,sniperrifle,snowball,snspistol,snspistol_mk2,stone_hatchet,specialcarbine,specialcarbine_mk2,stickybomb,stungun,switchblade,unarmed,vintagepistol,wrench,raypistol,raycarbine,rayminigun,ceramic_pistol,hazard_can,navy_revolver")
                .OldAnnotation("Npgsql:Enum:weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .OldAnnotation("Npgsql:PostgresExtension:tsm_system_rows", ",,");

            migrationBuilder.Sql("ALTER TYPE log_type RENAME VALUE 'set_admin' TO 'admin_team'");
        }
    }
}

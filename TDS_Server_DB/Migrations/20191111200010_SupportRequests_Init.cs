using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Common.Enum.Userpanel;

namespace TDS_Server_DB.Migrations
{
    public partial class SupportRequests_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_support_type", "question,help,compliment,complaint");

            migrationBuilder.CreateTable(
                name: "support_requests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<ESupportType>(nullable: false),
                    AtleastAdminLevel = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)"),
                    CloseTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_requests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_support_requests_players_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_request_messages",
                columns: table => new
                {
                    RequestID = table.Column<int>(nullable: false),
                    MessageIndex = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 300, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "timezone('utc', CURRENT_DATE)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_request_messages", x => new { x.RequestID, x.MessageIndex });
                    table.ForeignKey(
                        name: "FK_support_request_messages_players_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_support_request_messages_support_requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "support_requests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_support_request_messages_AuthorId",
                table: "support_request_messages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_support_requests_AuthorId",
                table: "support_requests",
                column: "AuthorId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "support_request_messages");

            migrationBuilder.DropTable(
                name: "support_requests");

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
                .Annotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust")
                .OldAnnotation("Npgsql:Enum:e_freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .OldAnnotation("Npgsql:Enum:e_language", "german,english")
                .OldAnnotation("Npgsql:Enum:e_lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby,gangwar_lobby")
                .OldAnnotation("Npgsql:Enum:e_log_type", "kick,ban,mute,next,login,register,lobby__join,lobby__leave,lobby__kick,lobby__ban,goto,remove_map,voice_mute")
                .OldAnnotation("Npgsql:Enum:e_map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .OldAnnotation("Npgsql:Enum:e_player_relation", "none,block,friend")
                .OldAnnotation("Npgsql:Enum:e_rule_category", "general,chat")
                .OldAnnotation("Npgsql:Enum:e_rule_target", "user,admin,vip")
                .OldAnnotation("Npgsql:Enum:e_support_type", "question,help,compliment,complaint")
                .OldAnnotation("Npgsql:Enum:e_userpanel_admin_question_answer_type", "text,check,number")
                .OldAnnotation("Npgsql:Enum:e_weapon_hash", "sniper_rifle,fire_extinguisher,compact_grenade_launcher,snowball,vintage_pistol,combat_pdw,heavy_sniper,heavy_sniper_mk2,sweeper_shotgun,micro_smg,wrench,pistol,pistol_mk2,pump_shotgun,pump_shotgun_mk2,ap_pistol,baseball,molotov,smg,smg_mk2,sticky_bomb,petrol_can,stun_gun,heavy_shotgun,minigun,golf_club,flare_gun,flare,grenade_launcher_smoke,hammer,combat_pistol,gusenberg,compact_rifle,homing_launcher,nightstick,railgun,sawn_off_shotgun,bullpup_rifle,firework,combat_mg,combat_mg_mk2,carbine_rifle,crowbar,flashlight,dagger,grenade,pool_cue,bat,pistol50,knife,mg,bullpup_shotgun,bz_gas,unarmed,grenade_launcher,night_vision,musket,proximity_mine,advanced_rifle,rpg,pipe_bomb,mini_smg,sns_pistol,sns_pistol_mk2,assault_rifle,assault_rifle_mk2,special_carbine,heavy_revolver,heavy_revolver_mk2,double_action_revolver,marksman_rifle,marksman_rifle_mk2,battle_axe,heavy_pistol,knuckle_duster,machine_pistol,marksman_pistol,machete,switch_blade,assault_shotgun,double_barrel_shotgun,assault_smg,hatchet,bottle,parachute,smoke_grenade,upn_atomizer,unholy_hellbringer,carbine_rifle_mk2,sepcial_carbine_mk2,bullpup_rifle_mk2,widowmaker")
                .OldAnnotation("Npgsql:Enum:e_weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .OldAnnotation("Npgsql:Enum:vehicle_hash", "ninef,ninef2,blista,asea,asea2,boattrailer,bus,armytanker,armytrailer,armytrailer2,freighttrailer,coach,airbus,asterope,airtug,ambulance,barracks,barracks2,baller,baller2,bjxl,banshee,benson,bfinjection,biff,blazer,blazer2,blazer3,bison,bison2,bison3,boxville,boxville2,boxville3,bobcatxl,bodhi2,buccaneer,buffalo,buffalo2,bulldozer,bullet,blimp,burrito,burrito2,burrito3,burrito4,burrito5,cavalcade,cavalcade2,policet,gburrito,cablecar,caddy,caddy2,camper,carbonizzare,cheetah,comet2,cogcabrio,coquette,cutter,gresley,dilettante,dilettante2,dune,dune2,hotknife,dloader,dubsta,dubsta2,dump,rubble,docktug,dominator,emperor,emperor2,emperor3,entityxf,exemplar,elegy2,f620,fbi,fbi2,felon,felon2,feltzer2,firetruk,flatbed,forklift,fq2,fusilade,fugitive,futo,granger,gauntlet,habanero,hauler,handler,infernus,ingot,intruder,issi2,jackal,journey,jb700,khamelion,landstalker,lguard,manana,mesa,mesa2,mesa3,crusader,minivan,mixer,mixer2,monroe,mower,mule,mule2,oracle,oracle2,packer,patriot,pbus,penumbra,peyote,phantom,phoenix,picador,pounder,police,police4,police2,police3,policeold1,policeold2,pony,pony2,prairie,pranger,premier,primo,proptrailer,rancherxl,rancherxl2,rapidgt,rapidgt2,radi,ratloader,rebel,regina,rebel2,rentalbus,ruiner,rumpo,rumpo2,rhino,riot,ripley,rocoto,romero,sabregt,sadler,sadler2,sandking,sandking2,schafter2,schwarzer,scrap,seminole,sentinel,sentinel2,zion,zion2,serrano,sheriff,sheriff2,speedo,speedo2,stanier,stinger,stingergt,stockade,stockade3,stratum,sultan,superd,surano,surfer,surfer2,surge,taco,tailgater,taxi,trash,tractor,tractor2,tractor3,graintrailer,baletrailer,tiptruck,tiptruck2,tornado,tornado2,tornado3,tornado4,tourbus,towtruck,towtruck2,utillitruck,utillitruck2,utillitruck3,voodoo2,washington,stretch,youga,ztype,sanchez,sanchez2,scorcher,tribike,tribike2,tribike3,fixter,cruiser,bmx,policeb,akuma,carbonrs,bagger,bati,bati2,ruffian,daemon,double,pcj,vader,vigero,faggio2,hexer,annihilator,buzzard,buzzard2,cargobob,cargobob2,cargobob3,skylift,polmav,maverick,nemesis,frogger,frogger2,cuban800,duster,stunt,mammatus,jet,shamal,luxor,titan,lazer,cargoplane,squalo,marquis,dinghy,dinghy2,jetmax,predator,tropic,seashark,seashark2,submersible,freightcar,freight,freightcont1,freightcont2,freightgrain,tankercar,metrotrain,docktrailer,trailers,trailers2,trailers3,tvtrailer,raketrailer,tanker,trailerlogs,tr2,tr3,tr4,trflat,trailersmall,velum,adder,voltic,vacca,suntrap,impaler3,monster4,monster5,slamvan6,issi6,cerberus2,cerberus3,deathbike2,dominator6,deathbike3,impaler4,slamvan4,slamvan5,brutus,brutus2,brutus3,deathbike,dominator4,dominator5,bruiser,bruiser2,bruiser3,rcbandito,italigto,cerberus,impaler2,monster3,tulip,scarab,scarab2,scarab3,issi4,issi5,clique,deveste,vamos,imperator,imperator2,imperator3,toros,deviant,schlagen,impaler,zr380,zr3802,zr3803,nimbus,xls,xls2,seven70,fmj,bestiagts,pfister811,brickade,rumpo3,volatus,prototipo,reaper,tug,windsor2,trailers4,xa21,caddy3,vagner,phantom3,nightshark,cheetah2,torero,hauler2,trailerlarge,technical3,insurgent3,apc,tampa3,dune3,trailersmall2,halftrack,ardent,oppressor,mule3,velum2,tanker2,casco,boxville4,hydra,insurgent,insurgent2,gburrito2,technical,dinghy3,savage,enduro,guardian,lectro,kuruma,kuruma2,trash2,barracks3,valkyrie,slamvan2,rhapsody,warrener,blade,glendale,panto,dubsta3,pigalle,elegy,tempesta,italigtb,italigtb2,nero,nero2,specter,specter2,diablous,diablous2,blazer5,ruiner2,dune4,dune5,phantom2,voltic2,penetrator,boxville5,wastelander,technical2,fcr,fcr2,comet3,ruiner3,monster,sovereign,sultanrs,banshee2,faction3,minivan2,sabregt2,slamvan3,tornado5,virgo2,virgo3,innovation,hakuchou,furoregt,verlierer2,nightshade,mamba,limo2,schafter3,schafter4,schafter5,schafter6,cog55,cog552,cognoscenti,cognoscenti2,baller3,baller4,baller5,baller6,toro2,seashark3,dinghy4,tropic2,speeder2,cargobob4,supervolito,supervolito2,valkyrie2,swift2,luxor2,feltzer3,osiris,virgo,windsor,coquette3,vindicator,t20,brawler,toro,chino,miljet,besra,coquette2,swift,vigilante,bombushka,alphaz1,seabreeze,tula,havok,hunter,microlight,rogue,pyro,howard,mogul,starling,nokota,molotok,rapidgt3,retinue,cyclone,visione,lynx,gargoyle,tyrus,sheava,omnis,le7b,contender,trophytruck,trophytruck2,rallytruck,cliffhanger,bf400,tropos,brioso,tampa2,btype,submersible2,dukes,dukes2,buffalo3,dominator2,dodo,marshall,blimp2,gauntlet2,stalion,stalion2,blista2,blista3,entity2,cheburek,jester3,caracara,hotring,seasparrow,flashgt,ellie,michelli,fagaloa,dominator3,tyrant,tezeract,gb200,issi3,taipan,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2,tornado6,faggio3,faggio,raptor,vortex,avarus,sanctus,youga2,hakuchou2,nightblade,chimera,esskey,wolfsbane,zombiea,zombieb,defiler,daemon2,ratbike,shotaro,manchez,blazer4,jester2,massacro2,ratloader2,slamvan,z190,viseris,comet5,raiden,riata,sc1,autarch,savestra,gt500,comet4,neon,sentinel3,khanjali,barrage,volatol,akula,deluxo,stromberg,chernobog,riot2,avenger,avenger2,thruster,yosemite,hermes,hustler,streiter,revolter,pariah,kamacho,lurcher,btype2,faction,faction2,moonbeam,moonbeam2,primo2,chino2,buccaneer2,voodoo,turismo2,infernus2,gp1,ruston,btype3,paragon,paragon2,jugular,rrocket,neo,krieger,peyote2,gauntlet4,s80,caracara2,thrax,novak,zorrusso,issi7,locust,emerus,hellion,dynasty,gauntlet3,nebula,zion3,drafter,tampa,bifta,speeder,kalahari,paradise,jester,turismor,alpha,vestra,zentorno,massacro,huntley,thrust");
        }
    }
}

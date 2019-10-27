using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Common.Enum.Userpanel;

namespace TDS_Server_DB.Migrations
{
    public partial class Application_Init_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_userpanel_admin_question_answer_type", "text,check,number");

            migrationBuilder.AddColumn<int>(
                name: "AdminLeaderID",
                table: "players",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "application_questions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
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
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_answers_application_questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "application_questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_invitations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationID = table.Column<int>(nullable: false),
                    AdminID = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_invitations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_application_invitations_players_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_invitations_applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "applications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_application_answers_QuestionID",
                table: "application_answers",
                column: "QuestionID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players",
                column: "AdminLeaderID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_players_players_AdminLeaderID",
                table: "players");

            migrationBuilder.DropTable(
                name: "application_answers");

            migrationBuilder.DropTable(
                name: "application_invitations");

            migrationBuilder.DropTable(
                name: "application_questions");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropIndex(
                name: "IX_players_AdminLeaderID",
                table: "players");

            migrationBuilder.DropColumn(
                name: "AdminLeaderID",
                table: "players");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .Annotation("Npgsql:Enum:e_language", "german,english")
                .Annotation("Npgsql:Enum:e_lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby")
                .Annotation("Npgsql:Enum:e_log_type", "kick,ban,mute,next,login,register,lobby__join,lobby__leave,lobby__kick,lobby__ban,goto,remove_map,voice_mute")
                .Annotation("Npgsql:Enum:e_map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .Annotation("Npgsql:Enum:e_player_relation", "none,block,friend")
                .Annotation("Npgsql:Enum:e_rule_category", "general,chat")
                .Annotation("Npgsql:Enum:e_rule_target", "user,admin,vip")
                .Annotation("Npgsql:Enum:e_weapon_hash", "sniper_rifle,fire_extinguisher,compact_grenade_launcher,snowball,vintage_pistol,combat_pdw,heavy_sniper,heavy_sniper_mk2,sweeper_shotgun,micro_smg,wrench,pistol,pistol_mk2,pump_shotgun,pump_shotgun_mk2,ap_pistol,baseball,molotov,smg,smg_mk2,sticky_bomb,petrol_can,stun_gun,heavy_shotgun,minigun,golf_club,flare_gun,flare,grenade_launcher_smoke,hammer,combat_pistol,gusenberg,compact_rifle,homing_launcher,nightstick,railgun,sawn_off_shotgun,bullpup_rifle,firework,combat_mg,combat_mg_mk2,carbine_rifle,crowbar,flashlight,dagger,grenade,pool_cue,bat,pistol50,knife,mg,bullpup_shotgun,bz_gas,unarmed,grenade_launcher,night_vision,musket,proximity_mine,advanced_rifle,rpg,pipe_bomb,mini_smg,sns_pistol,sns_pistol_mk2,assault_rifle,assault_rifle_mk2,special_carbine,heavy_revolver,heavy_revolver_mk2,double_action_revolver,marksman_rifle,marksman_rifle_mk2,battle_axe,heavy_pistol,knuckle_duster,machine_pistol,marksman_pistol,machete,switch_blade,assault_shotgun,double_barrel_shotgun,assault_smg,hatchet,bottle,parachute,smoke_grenade,upn_atomizer,unholy_hellbringer,carbine_rifle_mk2,sepcial_carbine_mk2,bullpup_rifle_mk2,widowmaker")
                .Annotation("Npgsql:Enum:e_weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .Annotation("Npgsql:Enum:vehicle_hash", "adder,airbus,airtug,akuma,alpha,alpha_z1,ambulance,annihilator,apc,ardent,army_tanker,army_trailer,army_trailer2,asea,asea2,asterope,avarus,bagger,bale_trailer,baller,baller2,baller3,baller4,baller5,baller6,banshee,banshee2,barracks,barracks2,barracks3,bati,bati2,benson,besra,bestia_gts,bf400,bf_injection,biff,bifta,bison,bison2,bison3,bjxl,blade,blazer,blazer2,blazer3,blazer4,blazer5,blimp,blimp2,blista,blista2,blista3,bmx,boat_trailer,bobcat_xl,bodhi2,bombushka,boxville,boxville2,boxville3,boxville4,boxville5,brawler,brickade,brioso,b_type,b_type2,b_type3,buccaneer,buccaneer2,buffalo,buffalo2,buffalo3,bulldozer,bullet,burrito,burrito2,burrito3,burrito4,burrito5,bus,buzzard,buzzard2,cable_car,caddy,caddy2,caddy3,camper,carbonizzare,carbon_rs,cargobob,cargobob2,cargobob3,cargobob4,cargo_plane,casco,cavalcade,cavalcade2,cheetah,cheetah2,chimera,chino,chino2,cliffhanger,coach,cog55,cog552,cog_cabrio,cognoscenti,cognoscenti2,comet2,comet3,contender,coquette,coquette2,coquette3,cruiser,crusader,cuban800,cutter,cyclone,daemon,daemon2,defiler,diablous,diablous2,dilettante,dilettante2,dinghy,dinghy2,dinghy3,dinghy4,d_loader,dock_trailer,docktug,dodo,dominator,dominator2,double,dubsta,dubsta2,dubsta3,dukes,dukes2,dump,dune,dune2,dune3,dune4,dune5,duster,elegy,elegy2,emperor,emperor2,emperor3,enduro,entity_xf,esskey,exemplar,f620,faction,faction2,faction3,faggio,faggio2,faggio3,fbi,fbi2,fcr,fcr2,felon,felon2,feltzer2,feltzer3,fire_truck,fixter,flatbed,forklift,fmj,fq2,freight,freight_car,freight_cont1,freight_cont2,freight_grain,freight_trailer,frogger,frogger2,fugitive,furoregt,fusilade,futo,gargoyle,gauntlet,gauntlet2,g_burrito,g_burrito2,glendale,gp1,grain_trailer,granger,gresley,guardian,habanero,hakuchou,hakuchou2,half_track,handler,hauler,hauler2,havok,hexer,hotknife,howard,hunter,huntley,hydra,infernus,infernus2,ingot,innovation,insurgent,insurgent2,insurgent3,intruder,issi2,itali_gtb,itali_gtb2,jackal,jb700,jester,jester2,jet,jetmax,journey,kalahari,khamelion,kuruma,kuruma2,landstalker,lazer,lectro,lguard,limo2,lurcher,luxor,luxor2,lynx,mamba,mammatus,manana,manchez,marquis,marshall,massacro,massacro2,maverick,mesa,mesa2,mesa3,metro_train,microlight,miljet,minivan,minivan2,mixer,mixer2,mogul,molotok,monroe,monster,moonbeam,moonbeam2,mower,mule,mule2,mule3,nemesis,nero,nero2,nightblade,nightshade,night_shark,nimbus,ninef,ninef2,nokota,omnis,oppressor,oracle,oracle2,osiris,packer,panto,paradise,patriot,p_bus,pcj,penetrator,penumbra,peyote,pfister811,phantom,phantom2,phantom3,phoenix,picador,pigalle,police,police2,police3,police4,policeb,police_old1,police_old2,police_t,polmav,pony,pony2,pounder,prairie,pranger,predator,premier,primo,primo2,prop_trailer,prototipo,pyro,radi,rake_trailer,rancher_xl,rancher_xl2,rally_truck,rapid_gt,rapid_gt2,rapid_gt3,raptor,rat_bike,rat_loader,rat_loader2,re7b,reaper,rebel,rebel2,regina,rental_bus,retinue,rhapsody,rhino,riot,ripley,rocoto,romero,rogue,rubble,ruffian,ruiner,ruiner2,ruiner3,rumpo,rumpo2,rumpo3,ruston,sabre_gt,sabre_gt2,sadler,sadler2,sanchez,sanchez2,sanctus,sandking,sandking2,savage,schafter2,schafter3,schafter4,schafter5,schafter6,schwarzer,scorcher,scrap,seabreeze,seashark,seashark2,seashark3,seminole,sentinel,sentinel2,serrano,seven70,shamal,sheava,sheriff,sheriff2,shotaro,skylift,slam_van,slam_van2,slam_van3,sovereign,specter,specter2,speeder,speeder2,speedo,speedo2,squalo,stalion,stalion2,stanier,starling,stinger,stinger_gt,stockade,stockade3,stratum,stretch,stunt,submersible,submersible2,sultan,sultan_rs,suntrap,superd,supervolito,supervolito2,surano,surfer,surfer2,surge,swift2,swift,t20,taco,tailgater,tampa,tampa2,tampa3,tanker,tanker2,tanker_car,taxi,technical,technical2,technical3,tempesta,thrust,tip_truck,tip_truck2,titan,torero,tornado,tornado2,tornado3,tornado4,tornado5,tornado6,toro,toro2,tourbus,tow_truck,tow_truck2,tr2,tr3,tr4,tractor,tractor2,tractor3,trailer_logs,trailer_large,trailers,trailers2,trailers3,trailers4,trailer_small,trailer_small2,trash,trash2,tr_flat,tri_bike,tri_bike2,tri_bike3,trophy_truck,trophy_truck2,tropic,tropic2,tropos,tug,tula,turismor,turismo2,tv_trailer,tyrus,utilli_truck,utilli_truck2,utilli_truck3,vacca,vader,vagner,valkyrie,valkyrie2,velum,velum2,verlierer2,vestra,vigero,vigilante,vindicator,virgo,virgo2,virgo3,visione,volatus,voltic,voltic2,voodoo,voodoo2,vortex,warrener,washington,wastelander,windsor,windsor2,wolfsbane,xa21,xls,xls2,youga,youga2,zentorno,zion,zion2,zombie_a,zombie_b,z_type,akula,autarch,avenger,avenger2,barrage,chernobog,comet4,comet5,deluxo,gt500,hermes,hustler,kamacho,khanjali,neon,pariah,raiden,revolter,riata,riot2,savestra,sc1,sentinel3,streiter,stromberg,thruster,viseris,volatol,yosemite,z190,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2")
                .OldAnnotation("Npgsql:Enum:e_freeroam_vehicle_type", "car,helicopter,plane,bike,boat")
                .OldAnnotation("Npgsql:Enum:e_language", "german,english")
                .OldAnnotation("Npgsql:Enum:e_lobby_type", "main_menu,fight_lobby,arena,gang_lobby,map_create_lobby")
                .OldAnnotation("Npgsql:Enum:e_log_type", "kick,ban,mute,next,login,register,lobby__join,lobby__leave,lobby__kick,lobby__ban,goto,remove_map,voice_mute")
                .OldAnnotation("Npgsql:Enum:e_map_limit_type", "kill_after_time,teleport_back_after_time,block,display")
                .OldAnnotation("Npgsql:Enum:e_player_relation", "none,block,friend")
                .OldAnnotation("Npgsql:Enum:e_rule_category", "general,chat")
                .OldAnnotation("Npgsql:Enum:e_rule_target", "user,admin,vip")
                .OldAnnotation("Npgsql:Enum:e_userpanel_admin_question_answer_type", "text,check,number")
                .OldAnnotation("Npgsql:Enum:e_weapon_hash", "sniper_rifle,fire_extinguisher,compact_grenade_launcher,snowball,vintage_pistol,combat_pdw,heavy_sniper,heavy_sniper_mk2,sweeper_shotgun,micro_smg,wrench,pistol,pistol_mk2,pump_shotgun,pump_shotgun_mk2,ap_pistol,baseball,molotov,smg,smg_mk2,sticky_bomb,petrol_can,stun_gun,heavy_shotgun,minigun,golf_club,flare_gun,flare,grenade_launcher_smoke,hammer,combat_pistol,gusenberg,compact_rifle,homing_launcher,nightstick,railgun,sawn_off_shotgun,bullpup_rifle,firework,combat_mg,combat_mg_mk2,carbine_rifle,crowbar,flashlight,dagger,grenade,pool_cue,bat,pistol50,knife,mg,bullpup_shotgun,bz_gas,unarmed,grenade_launcher,night_vision,musket,proximity_mine,advanced_rifle,rpg,pipe_bomb,mini_smg,sns_pistol,sns_pistol_mk2,assault_rifle,assault_rifle_mk2,special_carbine,heavy_revolver,heavy_revolver_mk2,double_action_revolver,marksman_rifle,marksman_rifle_mk2,battle_axe,heavy_pistol,knuckle_duster,machine_pistol,marksman_pistol,machete,switch_blade,assault_shotgun,double_barrel_shotgun,assault_smg,hatchet,bottle,parachute,smoke_grenade,upn_atomizer,unholy_hellbringer,carbine_rifle_mk2,sepcial_carbine_mk2,bullpup_rifle_mk2,widowmaker")
                .OldAnnotation("Npgsql:Enum:e_weapon_type", "melee,handgun,machine_gun,assault_rifle,sniper_rifle,shotgun,heavy_weapon,thrown_weapon,rest")
                .OldAnnotation("Npgsql:Enum:vehicle_hash", "adder,airbus,airtug,akuma,alpha,alpha_z1,ambulance,annihilator,apc,ardent,army_tanker,army_trailer,army_trailer2,asea,asea2,asterope,avarus,bagger,bale_trailer,baller,baller2,baller3,baller4,baller5,baller6,banshee,banshee2,barracks,barracks2,barracks3,bati,bati2,benson,besra,bestia_gts,bf400,bf_injection,biff,bifta,bison,bison2,bison3,bjxl,blade,blazer,blazer2,blazer3,blazer4,blazer5,blimp,blimp2,blista,blista2,blista3,bmx,boat_trailer,bobcat_xl,bodhi2,bombushka,boxville,boxville2,boxville3,boxville4,boxville5,brawler,brickade,brioso,b_type,b_type2,b_type3,buccaneer,buccaneer2,buffalo,buffalo2,buffalo3,bulldozer,bullet,burrito,burrito2,burrito3,burrito4,burrito5,bus,buzzard,buzzard2,cable_car,caddy,caddy2,caddy3,camper,carbonizzare,carbon_rs,cargobob,cargobob2,cargobob3,cargobob4,cargo_plane,casco,cavalcade,cavalcade2,cheetah,cheetah2,chimera,chino,chino2,cliffhanger,coach,cog55,cog552,cog_cabrio,cognoscenti,cognoscenti2,comet2,comet3,contender,coquette,coquette2,coquette3,cruiser,crusader,cuban800,cutter,cyclone,daemon,daemon2,defiler,diablous,diablous2,dilettante,dilettante2,dinghy,dinghy2,dinghy3,dinghy4,d_loader,dock_trailer,docktug,dodo,dominator,dominator2,double,dubsta,dubsta2,dubsta3,dukes,dukes2,dump,dune,dune2,dune3,dune4,dune5,duster,elegy,elegy2,emperor,emperor2,emperor3,enduro,entity_xf,esskey,exemplar,f620,faction,faction2,faction3,faggio,faggio2,faggio3,fbi,fbi2,fcr,fcr2,felon,felon2,feltzer2,feltzer3,fire_truck,fixter,flatbed,forklift,fmj,fq2,freight,freight_car,freight_cont1,freight_cont2,freight_grain,freight_trailer,frogger,frogger2,fugitive,furoregt,fusilade,futo,gargoyle,gauntlet,gauntlet2,g_burrito,g_burrito2,glendale,gp1,grain_trailer,granger,gresley,guardian,habanero,hakuchou,hakuchou2,half_track,handler,hauler,hauler2,havok,hexer,hotknife,howard,hunter,huntley,hydra,infernus,infernus2,ingot,innovation,insurgent,insurgent2,insurgent3,intruder,issi2,itali_gtb,itali_gtb2,jackal,jb700,jester,jester2,jet,jetmax,journey,kalahari,khamelion,kuruma,kuruma2,landstalker,lazer,lectro,lguard,limo2,lurcher,luxor,luxor2,lynx,mamba,mammatus,manana,manchez,marquis,marshall,massacro,massacro2,maverick,mesa,mesa2,mesa3,metro_train,microlight,miljet,minivan,minivan2,mixer,mixer2,mogul,molotok,monroe,monster,moonbeam,moonbeam2,mower,mule,mule2,mule3,nemesis,nero,nero2,nightblade,nightshade,night_shark,nimbus,ninef,ninef2,nokota,omnis,oppressor,oracle,oracle2,osiris,packer,panto,paradise,patriot,p_bus,pcj,penetrator,penumbra,peyote,pfister811,phantom,phantom2,phantom3,phoenix,picador,pigalle,police,police2,police3,police4,policeb,police_old1,police_old2,police_t,polmav,pony,pony2,pounder,prairie,pranger,predator,premier,primo,primo2,prop_trailer,prototipo,pyro,radi,rake_trailer,rancher_xl,rancher_xl2,rally_truck,rapid_gt,rapid_gt2,rapid_gt3,raptor,rat_bike,rat_loader,rat_loader2,re7b,reaper,rebel,rebel2,regina,rental_bus,retinue,rhapsody,rhino,riot,ripley,rocoto,romero,rogue,rubble,ruffian,ruiner,ruiner2,ruiner3,rumpo,rumpo2,rumpo3,ruston,sabre_gt,sabre_gt2,sadler,sadler2,sanchez,sanchez2,sanctus,sandking,sandking2,savage,schafter2,schafter3,schafter4,schafter5,schafter6,schwarzer,scorcher,scrap,seabreeze,seashark,seashark2,seashark3,seminole,sentinel,sentinel2,serrano,seven70,shamal,sheava,sheriff,sheriff2,shotaro,skylift,slam_van,slam_van2,slam_van3,sovereign,specter,specter2,speeder,speeder2,speedo,speedo2,squalo,stalion,stalion2,stanier,starling,stinger,stinger_gt,stockade,stockade3,stratum,stretch,stunt,submersible,submersible2,sultan,sultan_rs,suntrap,superd,supervolito,supervolito2,surano,surfer,surfer2,surge,swift2,swift,t20,taco,tailgater,tampa,tampa2,tampa3,tanker,tanker2,tanker_car,taxi,technical,technical2,technical3,tempesta,thrust,tip_truck,tip_truck2,titan,torero,tornado,tornado2,tornado3,tornado4,tornado5,tornado6,toro,toro2,tourbus,tow_truck,tow_truck2,tr2,tr3,tr4,tractor,tractor2,tractor3,trailer_logs,trailer_large,trailers,trailers2,trailers3,trailers4,trailer_small,trailer_small2,trash,trash2,tr_flat,tri_bike,tri_bike2,tri_bike3,trophy_truck,trophy_truck2,tropic,tropic2,tropos,tug,tula,turismor,turismo2,tv_trailer,tyrus,utilli_truck,utilli_truck2,utilli_truck3,vacca,vader,vagner,valkyrie,valkyrie2,velum,velum2,verlierer2,vestra,vigero,vigilante,vindicator,virgo,virgo2,virgo3,visione,volatus,voltic,voltic2,voodoo,voodoo2,vortex,warrener,washington,wastelander,windsor,windsor2,wolfsbane,xa21,xls,xls2,youga,youga2,zentorno,zion,zion2,zombie_a,zombie_b,z_type,akula,autarch,avenger,avenger2,barrage,chernobog,comet4,comet5,deluxo,gt500,hermes,hustler,kamacho,khanjali,neon,pariah,raiden,revolter,riata,riot2,savestra,sc1,sentinel3,streiter,stromberg,thruster,viseris,volatol,yosemite,z190,stafford,scramjet,strikeforce,terbyte,pbus2,oppressor2,pounder2,speedo4,freecrawler,mule4,menacer,blimp3,swinger,patriot2");
        }
    }
}

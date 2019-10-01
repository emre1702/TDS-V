using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class WeaponUpgrades2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "component_player_weapon_components_ComponentHash_fkey",
                table: "player_weapon_components");

            migrationBuilder.DropPrimaryKey(
                name: "PK_weapon_components",
                table: "weapon_components");

            migrationBuilder.DropIndex(
                name: "IX_player_weapon_components_ComponentHash",
                table: "player_weapon_components");

            migrationBuilder.DropTable(
                name: "player_weapon_components");

            migrationBuilder.DropTable(
                name: "weapon_components");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_weapon_component", "railgun_clip01,combat_pistol_clip01,knuckle_varmod_player,at_ar_af_grip,heavy_pistol_clip01,micro_smg_clip02,grenade_launcher_clip01,marksman_rifle_varmod_luxe,revolver_varmod_boss,at_scope_large_fixed_zoom,gusenberg_clip01,pistol50clip01,ap_pistol_clip02,smg_clip01,smg_varmod_luxe,assault_smg_varmod_lowrider,db_shotgun_clip01,ap_pistol_clip01,heavy_shotgun_clip01,combat_pdw_clip02,vintage_pistol_clip02,smg_clip02,at_pi_flsh,at_pi_flsh02,advanced_rifle_varmod_luxe,at_scope_small02,at_scope_macro02,knuckle_varmod_love,sniper_rifle_varmod_luxe,combat_pdw_clip01,vintage_pistol_clip01,machine_pistol_clip01,heavy_sniper_clip01,micro_smg_varmod_luxe,rpg_clip01,assault_rifle_varmod_luxe,musket_clip01,knuckle_varmod_dollar,compact_rifle_clip01,compact_rifle_clip02,switchblade_varmod_var1,heavy_pistol_clip02,at_pi_supp02,special_carbine_clip03,combat_pdw_clip03,special_carbine_varmod_lowrider,at_rail_cover01,pistol50varmod_luxe,smg_clip03,heavy_pistol_varmod_luxe,knuckle_varmod_vagos,sns_pistol_clip02,at_ar_flsh,special_carbine_clip02,knuckle_varmod_hate,sns_pistol_varmod_lowrider,mg_clip02,at_ar_supp,sawnoff_shotgun_varmod_luxe,assault_shotgun_clip02,heavy_shotgun_clip03,assault_smg_clip01,advanced_rifle_clip02,carbine_rifle_clip02,switchblade_varmod_base,combat_mg_varmod_lowrider,flare_gun_clip01,revolver_varmod_goon,assault_shotgun_clip01,heavy_shotgun_clip02,knuckle_varmod_diamond,ap_pistol_varmod_luxe,sniper_rifle_clip01,at_scope_macro,carbine_rifle_clip01,at_scope_medium,pump_shotgun_varmod_lowrider,at_ar_supp02,bullpup_rifle_varmod_low,machine_pistol_clip03,at_scope_small,assault_rifle_clip02,bullpup_rifle_clip02,machine_pistol_clip02,carbine_rifle_clip03,assault_smg_clip02,at_scope_max,assault_rifle_clip01,at_pi_supp,bullpup_rifle_clip01,police_torch_flashlight,compact_rifle_clip03,knuckle_varmod_pimp,combat_pistol_varmod_lowrider,special_carbine_clip01,sawnoff_shotgun_clip01,minigun_clip01,bullpup_shotgun_clip01,micro_smg_clip01,marksman_pistol_clip01,marksman_rifle_clip02,pump_shotgun_clip01,at_scope_large,combat_pistol_clip02,combat_mg_clip02,mg_varmod_lowrider,pistol_varmod_luxe,marksman_rifle_clip01,carbine_rifle_varmod_luxe,pistol50clip02,assault_rifle_clip03,flashlight_light,combat_mg_clip01,knuckle_varmod_king,firework_clip01,at_sr_supp,switchblade_varmod_var2,revolver_clip01,gusenberg_clip02,pistol_clip02,knuckle_varmod_ballas,knuckle_varmod_base,mg_clip01,homing_launcher_clip01,sns_pistol_clip01,advanced_rifle_clip01,pistol_clip01,raypistol_varmos_xmas18,pistol_mk2clip01,pistol_mk2clip02,pistol_mk2clip_tracer,pistol_mk2clip_incendiary,pistol_mk2clip_hollowpoint,pistol_mk2clip_fmj,at_pi_rail,at_pi_comp,pistol_mk2camo,pistol_mk2camo02,pistol_mk2camo03,pistol_mk2camo04,pistol_mk2camo05,pistol_mk2camo06,pistol_mk2camo07,pistol_mk2camo08,pistol_mk2camo09,pistol_mk2camo10,pistol_mk2camo_ind01,pistol_mk2camo_slide,pistol_mk2camo02slide,pistol_mk2camo03slide,pistol_mk2camo04slide,pistol_mk2camo05slide,pistol_mk2camo06slide,pistol_mk2camo07slide,pistol_mk2camo08slide,pistol_mk2camo09slide,pistol_mk2camo10slide,pistol_mk2camo_ind01slide,sns_pistol_mk2clip01,sns_pistol_mk2clip02,sns_pistol_mk2clip_tracer,sns_pistol_mk2clip_incendiary,sns_pistol_mk2clip_hollowpoint,sns_pistol_mk2clip_fmj,at_pi_flsh03,at_pi_rail02,at_pi_comp02,sns_pistol_mk2camo,sns_pistol_mk2camo02,sns_pistol_mk2camo03,sns_pistol_mk2camo04,sns_pistol_mk2camo05,sns_pistol_mk2camo06,sns_pistol_mk2camo07,sns_pistol_mk2camo08,sns_pistol_mk2camo09,sns_pistol_mk2camo10,sns_pistol_mk2camo_ind01,sns_pistol_mk2camo_slide,sns_pistol_mk2camo02slide,sns_pistol_mk2camo03slide,sns_pistol_mk2camo04slide,sns_pistol_mk2camo05slide,sns_pistol_mk2camo06slide,sns_pistol_mk2camo07slide,sns_pistol_mk2camo08slide,sns_pistol_mk2camo09slide,sns_pistol_mk2camo10slide,sns_pistol_mk2camo_ind01slide,revolver_mk2clip01,revolver_mk2clip_tracer,revolver_mk2clip_incendiary,revolver_mk2clip_hollowpoint,revolver_mk2clip_fmj,at_sights,at_scope_macro_mk2,at_pi_comp03,revolver_mk2camo,revolver_mk2camo02,revolver_mk2camo03,revolver_mk2camo04,revolver_mk2camo05,revolver_mk2camo06,revolver_mk2camo07,revolver_mk2camo08,revolver_mk2camo09,revolver_mk2camo10,revolver_mk2camo_ind01,mini_smg_clip01,mini_smg_clip02,smg_mk2clip01,smg_mk2clip02,smg_mk2clip_tracer,smg_mk2clip_incendiary,smg_mk2clip_hollowpoint,smg_mk2clip_fmj,at_scope_macro02smg_mk2,at_scope_small_smg_mk2,at_sights_smg,at_muzzle01,at_muzzle02,at_muzzle03,at_muzzle04,at_muzzle05,at_muzzle06,at_muzzle07,at_sb_barrel01,at_sb_barrel02,smg_mk2camo,smg_mk2camo02,smg_mk2camo03,smg_mk2camo04,smg_mk2camo05,smg_mk2camo06,smg_mk2camo07,smg_mk2camo08,smg_mk2camo09,smg_mk2camo10,smg_mk2camo_ind01,invalid")
                .OldAnnotation("Npgsql:Enum:weapon_component", "advanced_rifle_clip01,advanced_rifle_clip02,advanced_rifle_varmod_luxe,ap_pistol_clip01,ap_pistol_clip02,ap_pistol_varmod_luxe,assault_rifle_clip01,assault_rifle_clip02,assault_rifle_clip03,assault_rifle_varmod_luxe,assault_smg_clip01,assault_smg_clip02,assault_smg_varmod_lowrider,assault_shotgun_clip01,assault_shotgun_clip02,at_ar_af_grip,at_ar_flsh,at_ar_supp,at_ar_supp02,at_pi_flsh,at_pi_supp,at_pi_supp02,at_rail_cover01,at_scope_large,at_scope_large_fixed_zoom,at_scope_macro,at_scope_macro02,at_scope_max,at_scope_medium,at_scope_small,at_scope_small02,at_sr_supp,bullpup_rifle_clip01,bullpup_rifle_clip02,bullpup_rifle_varmod_low,bullpup_shotgun_clip01,carbine_rifle_clip01,carbine_rifle_clip02,carbine_rifle_clip03,carbine_rifle_varmod_luxe,combat_mg_clip01,combat_mg_clip02,combat_mg_varmod_lowrider,combat_pdw_clip01,combat_pdw_clip02,combat_pdw_clip03,combat_pistol_clip01,combat_pistol_clip02,combat_pistol_varmod_lowrider,compact_rifle_clip01,compact_rifle_clip02,compact_rifle_clip03,db_shotgun_clip01,firework_clip01,flare_gun_clip01,flashlight_light,grenade_launcher_clip01,gusenberg_clip01,gusenberg_clip02,heavy_pistol_clip01,heavy_pistol_clip02,heavy_pistol_varmod_luxe,heavy_shotgun_clip01,heavy_shotgun_clip02,heavy_shotgun_clip03,heavy_sniper_clip01,homing_launcher_clip01,knuckle_varmod_ballas,knuckle_varmod_base,knuckle_varmod_diamond,knuckle_varmod_dollar,knuckle_varmod_hate,knuckle_varmod_king,knuckle_varmod_love,knuckle_varmod_pimp,knuckle_varmod_player,knuckle_varmod_vagos,mg_clip01,mg_clip02,mg_varmod_lowrider,machine_pistol_clip01,machine_pistol_clip02,machine_pistol_clip03,marksman_pistol_clip01,marksman_rifle_clip01,marksman_rifle_clip02,marksman_rifle_varmod_luxe,micro_smg_clip01,micro_smg_clip02,micro_smg_varmod_luxe,minigun_clip01,musket_clip01,pistol50clip01,pistol50clip02,pistol50varmod_luxe,pistol_clip01,pistol_clip02,pistol_varmod_luxe,police_torch_flashlight,pump_shotgun_clip01,pump_shotgun_varmod_lowrider,rpg_clip01,railgun_clip01,revolver_clip01,revolver_varmod_boss,revolver_varmod_goon,smg_clip01,smg_clip02,smg_clip03,smg_varmod_luxe,sns_pistol_clip01,sns_pistol_clip02,sns_pistol_varmod_lowrider,sawnoff_shotgun_clip01,sawnoff_shotgun_varmod_luxe,sniper_rifle_clip01,sniper_rifle_varmod_luxe,special_carbine_clip01,special_carbine_clip02,special_carbine_clip03,special_carbine_varmod_lowrider,switchblade_varmod_base,switchblade_varmod_var1,switchblade_varmod_var2,vintage_pistol_clip01,vintage_pistol_clip02,invalid");
            
            migrationBuilder.CreateTable(
                name: "weapon_components",
                columns: table => new
                {
                    Hash = table.Column<EWeaponComponent>(nullable: false),
                    WeaponHash = table.Column<EWeaponHash>(nullable: false),
                    Category = table.Column<EWeaponComponentCategory>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon_components", x => new { x.Hash, x.WeaponHash });
                    table.ForeignKey(
                        name: "weapon_weapon_components_WeaponHash_fkey",
                        column: x => x.WeaponHash,
                        principalTable: "weapons",
                        principalColumn: "Hash",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_weapon_components",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<EWeaponHash>(nullable: false),
                    ComponentHash = table.Column<EWeaponComponent>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_weapon_components", x => new { x.PlayerID, x.WeaponHash });
                    table.ForeignKey(
                        name: "component_player_weapon_components_ComponentHashWeaponHash_fkey",
                        columns: x => new { x.ComponentHash, x.WeaponHash },
                        principalTable: "weapon_components",
                        principalColumns: new[] { "Hash", "WeaponHash" },
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_player_weapon_components_PlayerId_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "weapon_player_weapon_components_WeaponHash_fkey",
                        column: x => x.WeaponHash,
                        principalTable: "weapons",
                        principalColumn: "Hash",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
               name: "IX_player_weapon_components_ComponentHash",
               table: "player_weapon_components",
               column: "ComponentHash");

            migrationBuilder.CreateIndex(
                name: "IX_player_weapon_components_WeaponHash",
                table: "player_weapon_components",
                column: "WeaponHash");           

            migrationBuilder.CreateIndex(
                name: "IX_weapon_components_WeaponHash",
                table: "weapon_components",
                column: "WeaponHash");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "weapon_components",
                nullable: true);

            migrationBuilder.InsertData(
                table: "weapon_components",
                columns: new[] { "Hash", "WeaponHash", "Category", "Name" },
                values: new object[,]
                {
                    { EWeaponComponent.KnuckleVarmodBase, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "Base Model" },
                    { EWeaponComponent.RaypistolVarmosXMAS18, EWeaponHash.UpnAtomizer, EWeaponComponentCategory.Varmod, "Festive tint" },
                    { EWeaponComponent.MicroSMGClip01, EWeaponHash.MicroSMG, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.MicroSMGClip02, EWeaponHash.MicroSMG, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.MicroSMG, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtScopeMacro, EWeaponHash.MicroSMG, EWeaponComponentCategory.Scope, "Scope" },
                    { EWeaponComponent.AtArSupp02, EWeaponHash.MicroSMG, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.MicroSMGVarmodLuxe, EWeaponHash.MicroSMG, EWeaponComponentCategory.Varmod, "Yusuf Amir Luxury Finish" },
                    { EWeaponComponent.SMGClip01, EWeaponHash.SMG, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.SMGClip02, EWeaponHash.SMG, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.SMGClip03, EWeaponHash.SMG, EWeaponComponentCategory.Clip, "Drum Magazine" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.SMG, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtScopeMacro02, EWeaponHash.SMG, EWeaponComponentCategory.Scope, "Scope" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.SMG, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.SMGVarmodLuxe, EWeaponHash.MicroSMG, EWeaponComponentCategory.Varmod, "Yusuf Amir Luxury Finish" },
                    { EWeaponComponent.AssaultSMGClip01, EWeaponHash.AssaultSMG, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.AssaultSMGClip02, EWeaponHash.AssaultSMG, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtArFlsh, EWeaponHash.AssaultSMG, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtScopeMacro, EWeaponHash.AssaultSMG, EWeaponComponentCategory.Scope, "Scope" },
                    { EWeaponComponent.AtArSupp02, EWeaponHash.AssaultSMG, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.AssaultSMGVarmodLowrider, EWeaponHash.AssaultSMG, EWeaponComponentCategory.Varmod, "Yusuf Amir Luxury Finish" },
                    { EWeaponComponent.MiniSMGClip01, EWeaponHash.MiniSMG, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.VintagePistol, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.VintagePistolClip02, EWeaponHash.VintagePistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.VintagePistolClip01, EWeaponHash.VintagePistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.PistolMk2CamoInd01Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Patriotic" },
                    { EWeaponComponent.AtPiSupp02, EWeaponHash.PistolMk2, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.AtPiComp, EWeaponHash.PistolMk2, EWeaponComponentCategory.Compensator, "Compensator" },
                    { EWeaponComponent.PistolMk2Camo, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Digital Camo" },
                    { EWeaponComponent.PistolMk2Camo02, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Brushstroke Camo" },
                    { EWeaponComponent.PistolMk2Camo03, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Woodland Camo" },
                    { EWeaponComponent.PistolMk2Camo04, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Skull" },
                    { EWeaponComponent.PistolMk2Camo05, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Sessanta Nove" },
                    { EWeaponComponent.PistolMk2Camo06, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Perseus" },
                    { EWeaponComponent.PistolMk2Camo07, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Leopard" },
                    { EWeaponComponent.PistolMk2Camo08, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Zebra" },
                    { EWeaponComponent.MiniSMGClip02, EWeaponHash.MiniSMG, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.PistolMk2Camo09, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Geometric" },
                    { EWeaponComponent.PistolMk2CamoInd01, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Patriotic" },
                    { EWeaponComponent.PistolMk2CamoSlide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Digital Camo" },
                    { EWeaponComponent.PistolMk2Camo02Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Brushstroke Camo" },
                    { EWeaponComponent.PistolMk2Camo03Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Woodland Camo" },
                    { EWeaponComponent.PistolMk2Camo05Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Sessanta Nove" },
                    { EWeaponComponent.PistolMk2Camo06Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Perseus" },
                    { EWeaponComponent.PistolMk2Camo07Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Leopard" },
                    { EWeaponComponent.PistolMk2Camo08Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Zebra" },
                    { EWeaponComponent.PistolMk2Camo09Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Geometric" },
                    { EWeaponComponent.PistolMk2Camo10Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Boom!" },
                    { EWeaponComponent.PistolMk2Camo10, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Boom!" },
                    { EWeaponComponent.AtPiFlsh02, EWeaponHash.PistolMk2, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.SMGMk2Clip01, EWeaponHash.SMGMk2, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.SMGMk2ClipTracer, EWeaponHash.SMGMk2, EWeaponComponentCategory.Clip, "Tracer Rounds" },
                    { EWeaponComponent.SMGMk2Camo10, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Boom!" },
                    { EWeaponComponent.SMGMk2CamoInd01, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Patriotic" },
                    { EWeaponComponent.MachinePistolClip01, EWeaponHash.MachinePistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.MachinePistolClip02, EWeaponHash.MachinePistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.MachinePistolClip03, EWeaponHash.MachinePistol, EWeaponComponentCategory.Clip, "Drum Magazine" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.MachinePistol, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.CombatPDWClip01, EWeaponHash.CombatPDW, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.CombatPDWClip02, EWeaponHash.CombatPDW, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.CombatPDWClip03, EWeaponHash.CombatPDW, EWeaponComponentCategory.Clip, "Drum Magazine" },
                    { EWeaponComponent.AtArFlsh, EWeaponHash.CombatPDW, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtArAfGrip, EWeaponHash.CombatPDW, EWeaponComponentCategory.Grip, "Grip" },
                    { EWeaponComponent.AtScopeSmall, EWeaponHash.CombatPDW, EWeaponComponentCategory.Scope, "Scope" },
                    { EWeaponComponent.AtArFlsh, EWeaponHash.PumpShotgun, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtSrSupp, EWeaponHash.PumpShotgun, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.PumpShotgunVarmodLowrider, EWeaponHash.PumpShotgun, EWeaponComponentCategory.Varmod, "Yusuf Amir Luxury Finish" },
                    { EWeaponComponent.SawnoffShotgunVarmodLuxe, EWeaponHash.SawnOffShotgun, EWeaponComponentCategory.Varmod, "Gilded Gun Metal Finish" },
                    { EWeaponComponent.AssaultShotgunClip01, EWeaponHash.AssaultShotgun, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.AssaultShotgunClip02, EWeaponHash.AssaultShotgun, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtArFlsh, EWeaponHash.AssaultShotgun, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtArSupp, EWeaponHash.AssaultShotgun, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.AtArAfGrip, EWeaponHash.AssaultShotgun, EWeaponComponentCategory.Grip, "Grip" },
                    { EWeaponComponent.SMGMk2Camo09, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Geometric" },
                    { EWeaponComponent.SMGMk2Camo08, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Zebra" },
                    { EWeaponComponent.SMGMk2Camo07, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Leopard" },
                    { EWeaponComponent.SMGMk2Camo06, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Perseus" },
                    { EWeaponComponent.SMGMk2ClipIncendiary, EWeaponHash.SMGMk2, EWeaponComponentCategory.Clip, "Incendiary Rounds" },
                    { EWeaponComponent.SMGMk2ClipHollowpoint, EWeaponHash.SMGMk2, EWeaponComponentCategory.Clip, "Hollow Point Rounds" },
                    { EWeaponComponent.SMGMk2ClipFMJ, EWeaponHash.SMGMk2, EWeaponComponentCategory.Clip, "Full Metal Jacket Rounds" },
                    { EWeaponComponent.AtArFlsh, EWeaponHash.SMGMk2, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtSightsSMG, EWeaponHash.SMGMk2, EWeaponComponentCategory.Scope, "Holographic Sight" },
                    { EWeaponComponent.AtScopeMacro02SMGMk2, EWeaponHash.SMGMk2, EWeaponComponentCategory.Scope, "Small Scope" },
                    { EWeaponComponent.AtScopeSmallSMGMk2, EWeaponHash.SMGMk2, EWeaponComponentCategory.Scope, "Medium Scope" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.SMGMk2, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.AtMuzzle01, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Flat Muzzle Brake" },
                    { EWeaponComponent.AtMuzzle02, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Tactical Muzzle Brake" },
                    { EWeaponComponent.SMGMk2Clip02, EWeaponHash.SMGMk2, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtMuzzle03, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Fat-End Muzzle Brake" },
                    { EWeaponComponent.AtMuzzle05, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Heavy Duty Muzzle Brake" },
                    { EWeaponComponent.AtMuzzle06, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Splanted Muzzle Brake" },
                    { EWeaponComponent.AtMuzzle07, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Split-End Muzzle Brake" },
                    { EWeaponComponent.AtSbBarrel01, EWeaponHash.SMGMk2, EWeaponComponentCategory.Barrel, "Default Barrel" },
                    { EWeaponComponent.AtSbBarrel02, EWeaponHash.SMGMk2, EWeaponComponentCategory.Barrel, "Heavy Barrel" },
                    { EWeaponComponent.SMGMk2Camo, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Digital Camo" },
                    { EWeaponComponent.SMGMk2Camo02, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Brushstroke Camo" },
                    { EWeaponComponent.SMGMk2Camo03, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Woodland Camo" },
                    { EWeaponComponent.SMGMk2Camo04, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Skull" },
                    { EWeaponComponent.SMGMk2Camo05, EWeaponHash.SMGMk2, EWeaponComponentCategory.Camouflag, "Sessanta Nove" },
                    { EWeaponComponent.AtMuzzle04, EWeaponHash.SMGMk2, EWeaponComponentCategory.Muzzle, "Precision Muzzle Brake" },
                    { EWeaponComponent.AtPiRail, EWeaponHash.PistolMk2, EWeaponComponentCategory.Scope, "Mounted Scope" },
                    { EWeaponComponent.PistolMk2Camo04Slide, EWeaponHash.PistolMk2, EWeaponComponentCategory.Camouflag, "Skull" },
                    { EWeaponComponent.PistolMk2ClipHollowpoint, EWeaponHash.PistolMk2, EWeaponComponentCategory.Clip, "Hollow Point Rounds" },
                    { EWeaponComponent.APPistolVarmodLuxe, EWeaponHash.APPistol, EWeaponComponentCategory.Varmod, "Gilded Gun Metal Finish" },
                    { EWeaponComponent.Pistol50Clip01, EWeaponHash.Pistol50, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.Pistol50Clip02, EWeaponHash.Pistol50, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.Pistol50, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtArSupp02, EWeaponHash.Pistol50, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.PistolMk2ClipFMJ, EWeaponHash.PistolMk2, EWeaponComponentCategory.Clip, "Full Metal Jacket Rounds" },
                    { EWeaponComponent.RevolverVarmodBoss, EWeaponHash.HeavyRevolver, EWeaponComponentCategory.Varmod, "VIP Variant" },
                    { EWeaponComponent.RevolverVarmodGoon, EWeaponHash.HeavyRevolver, EWeaponComponentCategory.Varmod, "Bodyguard Variant" },
                    { EWeaponComponent.RevolverClip01, EWeaponHash.HeavyRevolver, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.SNSPistolClip01, EWeaponHash.SNSPistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.APPistol, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.SNSPistolClip02, EWeaponHash.SNSPistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.HeavyPistolClip01, EWeaponHash.HeavyPistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.HeavyPistolClip02, EWeaponHash.HeavyPistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.HeavyPistol, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.HeavyPistol, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.HeavyPistolVarmodLuxe, EWeaponHash.HeavyPistol, EWeaponComponentCategory.Varmod, "Etched Wood Grip Finish" },
                    { EWeaponComponent.RevolverMk2Clip01, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.RevolverMk2ClipTracer, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Clip, "Tracer Rounds" },
                    { EWeaponComponent.RevolverMk2ClipIncendiary, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Clip, "Incendiary Rounds" },
                    { EWeaponComponent.RevolverMk2ClipHollowpoint, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Clip, "Hollow Point Rounds" },
                    { EWeaponComponent.RevolverMk2ClipFMJ, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Clip, "Full Metal Jacket Rounds" },
                    { EWeaponComponent.SNSPistolVarmodLowrider, EWeaponHash.SNSPistol, EWeaponComponentCategory.Varmod, "Etched Wood Grip Finish" },
                    { EWeaponComponent.AtSights, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Scope, "Holographic Sight" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.APPistol, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.APPistolClip01, EWeaponHash.APPistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.KnuckleVarmodPimp, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Pimp" },
                    { EWeaponComponent.KnuckleVarmodBallas, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Ballas" },
                    { EWeaponComponent.KnuckleVarmodDollar, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Hustler" },
                    { EWeaponComponent.KnuckleVarmodDiamond, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Rock" },
                    { EWeaponComponent.KnuckleVarmodHate, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Hater" },
                    { EWeaponComponent.KnuckleVarmodLove, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Lover" },
                    { EWeaponComponent.KnuckleVarmodPlayer, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Player" },
                    { EWeaponComponent.KnuckleVarmodKing, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The King" },
                    { EWeaponComponent.KnuckleVarmodVagos, EWeaponHash.KnuckleDuster, EWeaponComponentCategory.Varmod, "The Vagos" },
                    { EWeaponComponent.SwitchbladeVarmodBase, EWeaponHash.SwitchBlade, EWeaponComponentCategory.Varmod, "Default Handle" },
                    { EWeaponComponent.APPistolClip02, EWeaponHash.APPistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.SwitchbladeVarmodVar1, EWeaponHash.SwitchBlade, EWeaponComponentCategory.Varmod, "VIP Variant" },
                    { EWeaponComponent.PistolClip01, EWeaponHash.Pistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.PistolClip02, EWeaponHash.Pistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.Pistol, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtPiSupp02, EWeaponHash.Pistol, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.PistolVarmodLuxe, EWeaponHash.Pistol, EWeaponComponentCategory.Varmod, "Yusuf Amir Luxury Finish" },
                    { EWeaponComponent.CombatPistolClip01, EWeaponHash.CombatPistol, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.CombatPistolClip02, EWeaponHash.CombatPistol, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.CombatPistol, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtPiSupp, EWeaponHash.CombatPistol, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.CombatPistolVarmodLowrider, EWeaponHash.CombatPistol, EWeaponComponentCategory.Varmod, "Yusuf Amir Luxury Finish" },
                    { EWeaponComponent.SwitchbladeVarmodVar2, EWeaponHash.SwitchBlade, EWeaponComponentCategory.Varmod, "Bodyguard Variant" },
                    { EWeaponComponent.AtScopeMacroMk2, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Scope, "Small Scope" },
                    { EWeaponComponent.Pistol50VarmodLuxe, EWeaponHash.Pistol50, EWeaponComponentCategory.Varmod, "Platinum Pearl Deluxe Finish" },
                    { EWeaponComponent.AtPiComp03, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Compensator, "Compensator" },
                    { EWeaponComponent.SNSPistolMk2Camo06, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Perseus" },
                    { EWeaponComponent.SNSPistolMk2Camo07, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Leopard" },
                    { EWeaponComponent.SNSPistolMk2Camo08, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Zebra" },
                    { EWeaponComponent.SNSPistolMk2Camo09, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Geometric" },
                    { EWeaponComponent.SNSPistolMk2Camo10, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Boom!" },
                    { EWeaponComponent.SNSPistolMk2CamoInd01, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Patriotic" },
                    { EWeaponComponent.SNSPistolMk2CamoSlide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Digital Camo" },
                    { EWeaponComponent.SNSPistolMk2Camo02Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Brushstroke Camo" },
                    { EWeaponComponent.SNSPistolMk2Camo03Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Woodland Camo" },
                    { EWeaponComponent.SNSPistolMk2Camo04Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Skull" },
                    { EWeaponComponent.SNSPistolMk2Camo05Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Sessanta Nove" },
                    { EWeaponComponent.SNSPistolMk2Camo06Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Perseus" },
                    { EWeaponComponent.SNSPistolMk2Camo07Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Leopard" },
                    { EWeaponComponent.SNSPistolMk2Camo08Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Zebra" },
                    { EWeaponComponent.SNSPistolMk2Camo09Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Geometric" },
                    { EWeaponComponent.AtPiFlsh, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.SNSPistolMk2CamoInd01Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Patriotic" },
                    { EWeaponComponent.PistolMk2Clip01, EWeaponHash.PistolMk2, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.PistolMk2Clip02, EWeaponHash.PistolMk2, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.PistolMk2ClipTracer, EWeaponHash.PistolMk2, EWeaponComponentCategory.Clip, "Tracer Rounds" },
                    { EWeaponComponent.PistolMk2ClipIncendiary, EWeaponHash.PistolMk2, EWeaponComponentCategory.Clip, "Incendiary Rounds" },
                    { EWeaponComponent.SNSPistolMk2Camo05, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Sessanta Nove" },
                    { EWeaponComponent.SNSPistolMk2Camo04, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Skull" },
                    { EWeaponComponent.SNSPistolMk2Camo10Slide, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Boom!" },
                    { EWeaponComponent.SNSPistolMk2Camo02, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Brushstroke Camo" },
                    { EWeaponComponent.SNSPistolMk2Camo03, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Woodland Camo" },
                    { EWeaponComponent.RevolverMk2Camo, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Digital Camo" },
                    { EWeaponComponent.RevolverMk2Camo02, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Brushstroke Camo" },
                    { EWeaponComponent.RevolverMk2Camo03, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Woodland Camo" },
                    { EWeaponComponent.RevolverMk2Camo05, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Sessanta Nove" },
                    { EWeaponComponent.RevolverMk2Camo06, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Perseus" },
                    { EWeaponComponent.RevolverMk2Camo07, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Leopard" },
                    { EWeaponComponent.RevolverMk2Camo08, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Zebra" },
                    { EWeaponComponent.RevolverMk2Camo09, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Geometric" },
                    { EWeaponComponent.RevolverMk2Camo10, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Boom!" },
                    { EWeaponComponent.RevolverMk2CamoInd01, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Patriotic" },
                    { EWeaponComponent.RevolverMk2Camo04, EWeaponHash.HeavyRevolverMk2, EWeaponComponentCategory.Camouflag, "Skull" },
                    { EWeaponComponent.SNSPistolMk2Clip02, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Clip, "Extended Clip" },
                    { EWeaponComponent.SNSPistolMk2Clip01, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Clip, "Default Clip" },
                    { EWeaponComponent.AtPiComp02, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Compensator, "Compensator" },
                    { EWeaponComponent.SNSPistolMk2Camo, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Camouflag, "Digital Camo" },
                    { EWeaponComponent.AtPiFlsh03, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Flashlight, "Flashlight" },
                    { EWeaponComponent.AtPiRail02, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Scope, "Mounted Scope" },
                    { EWeaponComponent.AtPiSupp02, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Suppressor, "Suppressor" },
                    { EWeaponComponent.SNSPistolMk2ClipFMJ, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Clip, "Full Metal Jacket Rounds" },
                    { EWeaponComponent.SNSPistolMk2ClipHollowpoint, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Clip, "Hollow Point Rounds" },
                    { EWeaponComponent.SNSPistolMk2ClipIncendiary, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Clip, "Incendiary Rounds" },
                    { EWeaponComponent.SNSPistolMk2ClipTracer, EWeaponHash.SNSPistolMk2, EWeaponComponentCategory.Clip, "Tracer Rounds" }
                });

            migrationBuilder.InsertData(
                table: "weaponsTints",
                columns: new[] { "ID", "IsMK2", "Name" },
                values: new object[,]
                {
                    { 15, true, "Bold Orange" },
                    { 16, true, "Bold Green & Purple" },
                    { 17, true, "Bold Red Features" },
                    { 18, true, "Bold Green Features" },
                    { 19, true, "Bold Cyan Features" },
                    { 20, true, "Bold Yellow Features" },
                    { 14, true, "Bold Purple & Yellow" },
                    { 21, true, "Bold Red & White" },
                    { 25, true, "Metallic Gray & Lilac" },
                    { 23, true, "Metallic Gold" },
                    { 24, true, "Metallic Platinum" },
                    { 26, true, "Metallic Purple & Lime" },
                    { 27, true, "Metallic Red" },
                    { 28, true, "Metallic Green" },
                    { 13, true, "Bold Pink" },
                    { 29, true, "Metallic Blue" },
                    { 22, true, "Bold Blue & White" },
                    { 12, true, "Orange Contrast" },
                    { 7, false, "Platinum" },
                    { 10, true, "Blue Contrast" },
                    { 30, true, "Metallic White & Aqua" },
                    { 1, false, "Green" },
                    { 2, false, "Gold" },
                    { 3, false, "Pink" },
                    { 4, false, "Army" },
                    { 5, false, "LSPD" },
                    { 6, false, "Orange" },
                    { 11, true, "Yellow Contrast" },
                    { 1, true, "Classic Gray" },
                    { 3, true, "Classic White" },
                    { 4, true, "Classic Beige" },
                    { 5, true, "Classic Green" },
                    { 6, true, "Classic Blue" },
                    { 7, true, "Classic Earth" },
                    { 8, true, "Classic Brown & Black" },
                    { 9, true, "Red Contrast" },
                    { 2, true, "Classic Two-Tone" },
                    { 31, true, "Metallic Red & Yellow" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_player_weapon_components_ComponentHash_WeaponHash",
                table: "player_weapon_components",
                columns: new[] { "ComponentHash", "WeaponHash" });

            migrationBuilder.AddForeignKey(
                name: "component_player_weapon_components_ComponentHash_fkey",
                table: "player_weapon_components",
                columns: new[] { "ComponentHash", "WeaponHash" },
                principalTable: "weapon_components",
                principalColumns: new[] { "Hash", "WeaponHash" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "component_player_weapon_components_ComponentHash_fkey",
                table: "player_weapon_components");

            migrationBuilder.DropPrimaryKey(
                name: "PK_weapon_components",
                table: "weapon_components");

            migrationBuilder.DropIndex(
                name: "IX_player_weapon_components_ComponentHash_WeaponHash",
                table: "player_weapon_components");

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo02, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2ClipIncendiary, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Clip01, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo04, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle05, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeMacroMk2, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo09, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.CombatPistolClip01, EWeaponHash.CombatPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodPlayer, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo03, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2ClipFMJ, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArAfGrip, EWeaponHash.CombatPDW });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArAfGrip, EWeaponHash.AssaultShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.HeavyPistolClip01, EWeaponHash.HeavyPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2ClipFMJ, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MicroSMGClip02, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2ClipHollowpoint, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo05, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo02, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverVarmodBoss, EWeaponHash.HeavyRevolver });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo06, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo02Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2CamoInd01Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo10, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiComp, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo09, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.Pistol50Clip01, EWeaponHash.Pistol50 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo09Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo10, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.APPistolClip02, EWeaponHash.APPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo07, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo07, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2ClipTracer, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGClip01, EWeaponHash.SMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiComp03, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGVarmodLuxe, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AssaultSMGVarmodLowrider, EWeaponHash.AssaultSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo02Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo08Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2ClipIncendiary, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo04Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.APPistolClip01, EWeaponHash.APPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.CombatPDWClip02, EWeaponHash.CombatPDW });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.VintagePistolClip02, EWeaponHash.VintagePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle06, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGClip02, EWeaponHash.SMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo06, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.Pistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.APPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.SMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.CombatPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.Pistol50 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh, EWeaponHash.HeavyPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo08, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo02, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2CamoInd01, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2ClipHollowpoint, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo03Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeMacro02, EWeaponHash.SMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeSmallSMGMk2, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodLove, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtSights, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo07, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.CombatPDWClip01, EWeaponHash.CombatPDW });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo10Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh02, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2CamoInd01, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.VintagePistolClip01, EWeaponHash.VintagePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo10, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MachinePistolClip01, EWeaponHash.MachinePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiRail02, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MicroSMGVarmodLuxe, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo09, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo05, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiFlsh03, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2CamoInd01Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo03, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Clip01, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo08, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle07, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo06, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2ClipFMJ, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodDollar, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo08, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SwitchbladeVarmodVar1, EWeaponHash.SwitchBlade });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Clip02, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo09, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.HeavyPistolClip02, EWeaponHash.HeavyPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp02, EWeaponHash.Pistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp02, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp02, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo10, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo06Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.CombatPDWClip03, EWeaponHash.CombatPDW });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo03, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.Pistol50VarmodLuxe, EWeaponHash.Pistol50 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGClip03, EWeaponHash.SMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.HeavyPistolVarmodLuxe, EWeaponHash.HeavyPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodVagos, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolClip02, EWeaponHash.SNSPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArFlsh, EWeaponHash.CombatPDW });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArFlsh, EWeaponHash.PumpShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArFlsh, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArFlsh, EWeaponHash.AssaultShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArFlsh, EWeaponHash.AssaultSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodHate, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2ClipTracer, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolVarmodLowrider, EWeaponHash.SNSPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArSupp, EWeaponHash.AssaultShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MiniSMGClip01, EWeaponHash.MiniSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SawnoffShotgunVarmodLuxe, EWeaponHash.SawnOffShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2ClipHollowpoint, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AssaultShotgunClip02, EWeaponHash.AssaultShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo02, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo10Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2ClipHollowpoint, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AssaultSMGClip01, EWeaponHash.AssaultSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiRail, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2ClipTracer, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SwitchbladeVarmodBase, EWeaponHash.SwitchBlade });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2CamoInd01, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MiniSMGClip02, EWeaponHash.MiniSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverVarmodGoon, EWeaponHash.HeavyRevolver });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AssaultShotgunClip01, EWeaponHash.AssaultShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Clip01, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo03, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodDiamond, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.APPistolVarmodLuxe, EWeaponHash.APPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo05, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeMacro, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeMacro, EWeaponHash.AssaultSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo07Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtSightsSMG, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PumpShotgunVarmodLowrider, EWeaponHash.PumpShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo08, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtSbBarrel02, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArSupp02, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArSupp02, EWeaponHash.Pistol50 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtArSupp02, EWeaponHash.AssaultSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo04, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MachinePistolClip03, EWeaponHash.MachinePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeSmall, EWeaponHash.CombatPDW });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiComp02, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo08Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2CamoSlide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MachinePistolClip02, EWeaponHash.MachinePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Clip02, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle01, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Clip01, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo07, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AssaultSMGClip02, EWeaponHash.AssaultSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2ClipFMJ, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.VintagePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.APPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.SMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.CombatPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.HeavyPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtPiSupp, EWeaponHash.MachinePistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodPimp, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.CombatPistolVarmodLowrider, EWeaponHash.CombatPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo09Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2ClipTracer, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle02, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.MicroSMGClip01, EWeaponHash.MicroSMG });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo05Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Clip02, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.CombatPistolClip02, EWeaponHash.CombatPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolVarmodLuxe, EWeaponHash.Pistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RaypistolVarmosXMAS18, EWeaponHash.UpnAtomizer });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtSbBarrel01, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2CamoInd01, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2ClipIncendiary, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.Pistol50Clip02, EWeaponHash.Pistol50 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo06, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle03, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo05Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo07Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodKing, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo03Slide, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtScopeMacro02SMGMk2, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtSrSupp, EWeaponHash.PumpShotgun });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo04Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2ClipIncendiary, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SwitchbladeVarmodVar2, EWeaponHash.SwitchBlade });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2CamoSlide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverClip01, EWeaponHash.HeavyRevolver });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SMGMk2Camo04, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.AtMuzzle04, EWeaponHash.SMGMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolClip02, EWeaponHash.Pistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodBallas, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolMk2Camo05, EWeaponHash.PistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.RevolverMk2Camo04, EWeaponHash.HeavyRevolverMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.KnuckleVarmodBase, EWeaponHash.KnuckleDuster });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolClip01, EWeaponHash.SNSPistol });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.SNSPistolMk2Camo06Slide, EWeaponHash.SNSPistolMk2 });

            migrationBuilder.DeleteData(
                table: "weapon_components",
                keyColumns: new[] { "Hash", "WeaponHash" },
                keyValues: new object[] { EWeaponComponent.PistolClip01, EWeaponHash.Pistol });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 1, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 1, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 2, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 2, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 3, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 3, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 4, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 4, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 5, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 5, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 6, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 6, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 7, false });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 7, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 8, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 9, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 10, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 11, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 12, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 13, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 14, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 15, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 16, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 17, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 18, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 19, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 20, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 21, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 22, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 23, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 24, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 25, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 26, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 27, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 28, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 29, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 30, true });

            migrationBuilder.DeleteData(
                table: "weaponsTints",
                keyColumns: new[] { "ID", "IsMK2" },
                keyValues: new object[] { 31, true });

            migrationBuilder.DropColumn(
                name: "Name",
                table: "weapon_components");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:weapon_component", "advanced_rifle_clip01,advanced_rifle_clip02,advanced_rifle_varmod_luxe,ap_pistol_clip01,ap_pistol_clip02,ap_pistol_varmod_luxe,assault_rifle_clip01,assault_rifle_clip02,assault_rifle_clip03,assault_rifle_varmod_luxe,assault_smg_clip01,assault_smg_clip02,assault_smg_varmod_lowrider,assault_shotgun_clip01,assault_shotgun_clip02,at_ar_af_grip,at_ar_flsh,at_ar_supp,at_ar_supp02,at_pi_flsh,at_pi_supp,at_pi_supp02,at_rail_cover01,at_scope_large,at_scope_large_fixed_zoom,at_scope_macro,at_scope_macro02,at_scope_max,at_scope_medium,at_scope_small,at_scope_small02,at_sr_supp,bullpup_rifle_clip01,bullpup_rifle_clip02,bullpup_rifle_varmod_low,bullpup_shotgun_clip01,carbine_rifle_clip01,carbine_rifle_clip02,carbine_rifle_clip03,carbine_rifle_varmod_luxe,combat_mg_clip01,combat_mg_clip02,combat_mg_varmod_lowrider,combat_pdw_clip01,combat_pdw_clip02,combat_pdw_clip03,combat_pistol_clip01,combat_pistol_clip02,combat_pistol_varmod_lowrider,compact_rifle_clip01,compact_rifle_clip02,compact_rifle_clip03,db_shotgun_clip01,firework_clip01,flare_gun_clip01,flashlight_light,grenade_launcher_clip01,gusenberg_clip01,gusenberg_clip02,heavy_pistol_clip01,heavy_pistol_clip02,heavy_pistol_varmod_luxe,heavy_shotgun_clip01,heavy_shotgun_clip02,heavy_shotgun_clip03,heavy_sniper_clip01,homing_launcher_clip01,knuckle_varmod_ballas,knuckle_varmod_base,knuckle_varmod_diamond,knuckle_varmod_dollar,knuckle_varmod_hate,knuckle_varmod_king,knuckle_varmod_love,knuckle_varmod_pimp,knuckle_varmod_player,knuckle_varmod_vagos,mg_clip01,mg_clip02,mg_varmod_lowrider,machine_pistol_clip01,machine_pistol_clip02,machine_pistol_clip03,marksman_pistol_clip01,marksman_rifle_clip01,marksman_rifle_clip02,marksman_rifle_varmod_luxe,micro_smg_clip01,micro_smg_clip02,micro_smg_varmod_luxe,minigun_clip01,musket_clip01,pistol50clip01,pistol50clip02,pistol50varmod_luxe,pistol_clip01,pistol_clip02,pistol_varmod_luxe,police_torch_flashlight,pump_shotgun_clip01,pump_shotgun_varmod_lowrider,rpg_clip01,railgun_clip01,revolver_clip01,revolver_varmod_boss,revolver_varmod_goon,smg_clip01,smg_clip02,smg_clip03,smg_varmod_luxe,sns_pistol_clip01,sns_pistol_clip02,sns_pistol_varmod_lowrider,sawnoff_shotgun_clip01,sawnoff_shotgun_varmod_luxe,sniper_rifle_clip01,sniper_rifle_varmod_luxe,special_carbine_clip01,special_carbine_clip02,special_carbine_clip03,special_carbine_varmod_lowrider,switchblade_varmod_base,switchblade_varmod_var1,switchblade_varmod_var2,vintage_pistol_clip01,vintage_pistol_clip02,invalid")
                .OldAnnotation("Npgsql:Enum:e_weapon_component", "railgun_clip01,combat_pistol_clip01,knuckle_varmod_player,at_ar_af_grip,heavy_pistol_clip01,micro_smg_clip02,grenade_launcher_clip01,marksman_rifle_varmod_luxe,revolver_varmod_boss,at_scope_large_fixed_zoom,gusenberg_clip01,pistol50clip01,ap_pistol_clip02,smg_clip01,smg_varmod_luxe,assault_smg_varmod_lowrider,db_shotgun_clip01,ap_pistol_clip01,heavy_shotgun_clip01,combat_pdw_clip02,vintage_pistol_clip02,smg_clip02,at_pi_flsh,at_pi_flsh02,advanced_rifle_varmod_luxe,at_scope_small02,at_scope_macro02,knuckle_varmod_love,sniper_rifle_varmod_luxe,combat_pdw_clip01,vintage_pistol_clip01,machine_pistol_clip01,heavy_sniper_clip01,micro_smg_varmod_luxe,rpg_clip01,assault_rifle_varmod_luxe,musket_clip01,knuckle_varmod_dollar,compact_rifle_clip01,compact_rifle_clip02,switchblade_varmod_var1,heavy_pistol_clip02,at_pi_supp02,special_carbine_clip03,combat_pdw_clip03,special_carbine_varmod_lowrider,at_rail_cover01,pistol50varmod_luxe,smg_clip03,heavy_pistol_varmod_luxe,knuckle_varmod_vagos,sns_pistol_clip02,at_ar_flsh,special_carbine_clip02,knuckle_varmod_hate,sns_pistol_varmod_lowrider,mg_clip02,at_ar_supp,sawnoff_shotgun_varmod_luxe,assault_shotgun_clip02,heavy_shotgun_clip03,assault_smg_clip01,advanced_rifle_clip02,carbine_rifle_clip02,switchblade_varmod_base,combat_mg_varmod_lowrider,flare_gun_clip01,revolver_varmod_goon,assault_shotgun_clip01,heavy_shotgun_clip02,knuckle_varmod_diamond,ap_pistol_varmod_luxe,sniper_rifle_clip01,at_scope_macro,carbine_rifle_clip01,at_scope_medium,pump_shotgun_varmod_lowrider,at_ar_supp02,bullpup_rifle_varmod_low,machine_pistol_clip03,at_scope_small,assault_rifle_clip02,bullpup_rifle_clip02,machine_pistol_clip02,carbine_rifle_clip03,assault_smg_clip02,at_scope_max,assault_rifle_clip01,at_pi_supp,bullpup_rifle_clip01,police_torch_flashlight,compact_rifle_clip03,knuckle_varmod_pimp,combat_pistol_varmod_lowrider,special_carbine_clip01,sawnoff_shotgun_clip01,minigun_clip01,bullpup_shotgun_clip01,micro_smg_clip01,marksman_pistol_clip01,marksman_rifle_clip02,pump_shotgun_clip01,at_scope_large,combat_pistol_clip02,combat_mg_clip02,mg_varmod_lowrider,pistol_varmod_luxe,marksman_rifle_clip01,carbine_rifle_varmod_luxe,pistol50clip02,assault_rifle_clip03,flashlight_light,combat_mg_clip01,knuckle_varmod_king,firework_clip01,at_sr_supp,switchblade_varmod_var2,revolver_clip01,gusenberg_clip02,pistol_clip02,knuckle_varmod_ballas,knuckle_varmod_base,mg_clip01,homing_launcher_clip01,sns_pistol_clip01,advanced_rifle_clip01,pistol_clip01,raypistol_varmos_xmas18,pistol_mk2clip01,pistol_mk2clip02,pistol_mk2clip_tracer,pistol_mk2clip_incendiary,pistol_mk2clip_hollowpoint,pistol_mk2clip_fmj,at_pi_rail,at_pi_comp,pistol_mk2camo,pistol_mk2camo02,pistol_mk2camo03,pistol_mk2camo04,pistol_mk2camo05,pistol_mk2camo06,pistol_mk2camo07,pistol_mk2camo08,pistol_mk2camo09,pistol_mk2camo10,pistol_mk2camo_ind01,pistol_mk2camo_slide,pistol_mk2camo02slide,pistol_mk2camo03slide,pistol_mk2camo04slide,pistol_mk2camo05slide,pistol_mk2camo06slide,pistol_mk2camo07slide,pistol_mk2camo08slide,pistol_mk2camo09slide,pistol_mk2camo10slide,pistol_mk2camo_ind01slide,sns_pistol_mk2clip01,sns_pistol_mk2clip02,sns_pistol_mk2clip_tracer,sns_pistol_mk2clip_incendiary,sns_pistol_mk2clip_hollowpoint,sns_pistol_mk2clip_fmj,at_pi_flsh03,at_pi_rail02,at_pi_comp02,sns_pistol_mk2camo,sns_pistol_mk2camo02,sns_pistol_mk2camo03,sns_pistol_mk2camo04,sns_pistol_mk2camo05,sns_pistol_mk2camo06,sns_pistol_mk2camo07,sns_pistol_mk2camo08,sns_pistol_mk2camo09,sns_pistol_mk2camo10,sns_pistol_mk2camo_ind01,sns_pistol_mk2camo_slide,sns_pistol_mk2camo02slide,sns_pistol_mk2camo03slide,sns_pistol_mk2camo04slide,sns_pistol_mk2camo05slide,sns_pistol_mk2camo06slide,sns_pistol_mk2camo07slide,sns_pistol_mk2camo08slide,sns_pistol_mk2camo09slide,sns_pistol_mk2camo10slide,sns_pistol_mk2camo_ind01slide,revolver_mk2clip01,revolver_mk2clip_tracer,revolver_mk2clip_incendiary,revolver_mk2clip_hollowpoint,revolver_mk2clip_fmj,at_sights,at_scope_macro_mk2,at_pi_comp03,revolver_mk2camo,revolver_mk2camo02,revolver_mk2camo03,revolver_mk2camo04,revolver_mk2camo05,revolver_mk2camo06,revolver_mk2camo07,revolver_mk2camo08,revolver_mk2camo09,revolver_mk2camo10,revolver_mk2camo_ind01,mini_smg_clip01,mini_smg_clip02,smg_mk2clip01,smg_mk2clip02,smg_mk2clip_tracer,smg_mk2clip_incendiary,smg_mk2clip_hollowpoint,smg_mk2clip_fmj,at_scope_macro02smg_mk2,at_scope_small_smg_mk2,at_sights_smg,at_muzzle01,at_muzzle02,at_muzzle03,at_muzzle04,at_muzzle05,at_muzzle06,at_muzzle07,at_sb_barrel01,at_sb_barrel02,smg_mk2camo,smg_mk2camo02,smg_mk2camo03,smg_mk2camo04,smg_mk2camo05,smg_mk2camo06,smg_mk2camo07,smg_mk2camo08,smg_mk2camo09,smg_mk2camo10,smg_mk2camo_ind01,invalid");
               
            migrationBuilder.AlterColumn<long>(
                name: "Hash",
                table: "weapon_components",
                type: "weapon_component",
                nullable: false,
                oldClrType: typeof(EWeaponComponent));

            migrationBuilder.AlterColumn<long>(
                name: "ComponentHash",
                table: "player_weapon_components",
                type: "weapon_component",
                nullable: false,
                oldClrType: typeof(EWeaponComponent));

            migrationBuilder.AddPrimaryKey(
                name: "PK_weapon_components",
                table: "weapon_components",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_player_weapon_components_ComponentHash",
                table: "player_weapon_components",
                column: "ComponentHash");

            migrationBuilder.AddForeignKey(
                name: "component_player_weapon_components_ComponentHash_fkey",
                table: "player_weapon_components",
                column: "ComponentHash",
                principalTable: "weapon_components",
                principalColumn: "Hash",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

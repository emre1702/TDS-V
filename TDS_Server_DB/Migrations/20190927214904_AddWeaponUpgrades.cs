using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class AddWeaponUpgrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_weapon_component_category", "clip,flashlight,suppressor,varmod,scope,compensator,camouflag,muzzle,barrel,grip")
                .Annotation("Npgsql:Enum:weapon_component", "advanced_rifle_clip01,advanced_rifle_clip02,advanced_rifle_varmod_luxe,ap_pistol_clip01,ap_pistol_clip02,ap_pistol_varmod_luxe,assault_rifle_clip01,assault_rifle_clip02,assault_rifle_clip03,assault_rifle_varmod_luxe,assault_smg_clip01,assault_smg_clip02,assault_smg_varmod_lowrider,assault_shotgun_clip01,assault_shotgun_clip02,at_ar_af_grip,at_ar_flsh,at_ar_supp,at_ar_supp02,at_pi_flsh,at_pi_supp,at_pi_supp02,at_rail_cover01,at_scope_large,at_scope_large_fixed_zoom,at_scope_macro,at_scope_macro02,at_scope_max,at_scope_medium,at_scope_small,at_scope_small02,at_sr_supp,bullpup_rifle_clip01,bullpup_rifle_clip02,bullpup_rifle_varmod_low,bullpup_shotgun_clip01,carbine_rifle_clip01,carbine_rifle_clip02,carbine_rifle_clip03,carbine_rifle_varmod_luxe,combat_mg_clip01,combat_mg_clip02,combat_mg_varmod_lowrider,combat_pdw_clip01,combat_pdw_clip02,combat_pdw_clip03,combat_pistol_clip01,combat_pistol_clip02,combat_pistol_varmod_lowrider,compact_rifle_clip01,compact_rifle_clip02,compact_rifle_clip03,db_shotgun_clip01,firework_clip01,flare_gun_clip01,flashlight_light,grenade_launcher_clip01,gusenberg_clip01,gusenberg_clip02,heavy_pistol_clip01,heavy_pistol_clip02,heavy_pistol_varmod_luxe,heavy_shotgun_clip01,heavy_shotgun_clip02,heavy_shotgun_clip03,heavy_sniper_clip01,homing_launcher_clip01,knuckle_varmod_ballas,knuckle_varmod_base,knuckle_varmod_diamond,knuckle_varmod_dollar,knuckle_varmod_hate,knuckle_varmod_king,knuckle_varmod_love,knuckle_varmod_pimp,knuckle_varmod_player,knuckle_varmod_vagos,mg_clip01,mg_clip02,mg_varmod_lowrider,machine_pistol_clip01,machine_pistol_clip02,machine_pistol_clip03,marksman_pistol_clip01,marksman_rifle_clip01,marksman_rifle_clip02,marksman_rifle_varmod_luxe,micro_smg_clip01,micro_smg_clip02,micro_smg_varmod_luxe,minigun_clip01,musket_clip01,pistol50clip01,pistol50clip02,pistol50varmod_luxe,pistol_clip01,pistol_clip02,pistol_varmod_luxe,police_torch_flashlight,pump_shotgun_clip01,pump_shotgun_varmod_lowrider,rpg_clip01,railgun_clip01,revolver_clip01,revolver_varmod_boss,revolver_varmod_goon,smg_clip01,smg_clip02,smg_clip03,smg_varmod_luxe,sns_pistol_clip01,sns_pistol_clip02,sns_pistol_varmod_lowrider,sawnoff_shotgun_clip01,sawnoff_shotgun_varmod_luxe,sniper_rifle_clip01,sniper_rifle_varmod_luxe,special_carbine_clip01,special_carbine_clip02,special_carbine_clip03,special_carbine_varmod_lowrider,switchblade_varmod_base,switchblade_varmod_var1,switchblade_varmod_var2,vintage_pistol_clip01,vintage_pistol_clip02,invalid");

            migrationBuilder.CreateTable(
                name: "weapon_components",
                columns: table => new
                {
                    Hash = table.Column<WeaponComponent>(nullable: false),
                    WeaponHash = table.Column<EWeaponHash>(nullable: false),
                    Category = table.Column<EWeaponComponentCategory>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weapon_components", x => x.Hash);
                    table.ForeignKey(
                        name: "weapon_weapon_components_WeaponHash_fkey",
                        column: x => x.WeaponHash,
                        principalTable: "weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weaponsTints",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    IsMK2 = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weaponsTints", x => new { x.ID, x.IsMK2 });
                });

            migrationBuilder.CreateTable(
                name: "player_weapon_components",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<EWeaponHash>(nullable: false),
                    ComponentHash = table.Column<WeaponComponent>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_weapon_components", x => new { x.PlayerID, x.WeaponHash });
                    table.ForeignKey(
                        name: "component_player_weapon_components_ComponentHash_fkey",
                        column: x => x.ComponentHash,
                        principalTable: "weapon_components",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "player_player_weapon_components_PlayerId_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "weapon_player_weapon_components_WeaponHash_fkey",
                        column: x => x.WeaponHash,
                        principalTable: "weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_weapon_tints",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    WeaponHash = table.Column<EWeaponHash>(nullable: false),
                    TintID = table.Column<int>(nullable: false),
                    IsMK2 = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_weapon_tints", x => new { x.PlayerID, x.WeaponHash });
                    table.ForeignKey(
                        name: "player_player_weapon_tints_PlayerId_fkey",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "weapon_player_weapon_tints_WeaponHash_fkey",
                        column: x => x.WeaponHash,
                        principalTable: "weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "tint_player_weapon_tints_TintId_fkey",
                        columns: x => new { x.TintID, x.IsMK2 },
                        principalTable: "weaponsTints",
                        principalColumns: new[] { "ID", "IsMK2" },
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
                name: "IX_player_weapon_tints_WeaponHash",
                table: "player_weapon_tints",
                column: "WeaponHash");

            migrationBuilder.CreateIndex(
                name: "IX_player_weapon_tints_TintID_IsMK2",
                table: "player_weapon_tints",
                columns: new[] { "TintID", "IsMK2" });

            migrationBuilder.CreateIndex(
                name: "IX_weapon_components_WeaponHash",
                table: "weapon_components",
                column: "WeaponHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_weapon_components");

            migrationBuilder.DropTable(
                name: "player_weapon_tints");

            migrationBuilder.DropTable(
                name: "weapon_components");

            migrationBuilder.DropTable(
                name: "weaponsTints");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:e_weapon_component_category", "clip,flashlight,suppressor,varmod,scope,compensator,camouflag,muzzle,barrel,grip")
                .OldAnnotation("Npgsql:Enum:weapon_component", "advanced_rifle_clip01,advanced_rifle_clip02,advanced_rifle_varmod_luxe,ap_pistol_clip01,ap_pistol_clip02,ap_pistol_varmod_luxe,assault_rifle_clip01,assault_rifle_clip02,assault_rifle_clip03,assault_rifle_varmod_luxe,assault_smg_clip01,assault_smg_clip02,assault_smg_varmod_lowrider,assault_shotgun_clip01,assault_shotgun_clip02,at_ar_af_grip,at_ar_flsh,at_ar_supp,at_ar_supp02,at_pi_flsh,at_pi_supp,at_pi_supp02,at_rail_cover01,at_scope_large,at_scope_large_fixed_zoom,at_scope_macro,at_scope_macro02,at_scope_max,at_scope_medium,at_scope_small,at_scope_small02,at_sr_supp,bullpup_rifle_clip01,bullpup_rifle_clip02,bullpup_rifle_varmod_low,bullpup_shotgun_clip01,carbine_rifle_clip01,carbine_rifle_clip02,carbine_rifle_clip03,carbine_rifle_varmod_luxe,combat_mg_clip01,combat_mg_clip02,combat_mg_varmod_lowrider,combat_pdw_clip01,combat_pdw_clip02,combat_pdw_clip03,combat_pistol_clip01,combat_pistol_clip02,combat_pistol_varmod_lowrider,compact_rifle_clip01,compact_rifle_clip02,compact_rifle_clip03,db_shotgun_clip01,firework_clip01,flare_gun_clip01,flashlight_light,grenade_launcher_clip01,gusenberg_clip01,gusenberg_clip02,heavy_pistol_clip01,heavy_pistol_clip02,heavy_pistol_varmod_luxe,heavy_shotgun_clip01,heavy_shotgun_clip02,heavy_shotgun_clip03,heavy_sniper_clip01,homing_launcher_clip01,knuckle_varmod_ballas,knuckle_varmod_base,knuckle_varmod_diamond,knuckle_varmod_dollar,knuckle_varmod_hate,knuckle_varmod_king,knuckle_varmod_love,knuckle_varmod_pimp,knuckle_varmod_player,knuckle_varmod_vagos,mg_clip01,mg_clip02,mg_varmod_lowrider,machine_pistol_clip01,machine_pistol_clip02,machine_pistol_clip03,marksman_pistol_clip01,marksman_rifle_clip01,marksman_rifle_clip02,marksman_rifle_varmod_luxe,micro_smg_clip01,micro_smg_clip02,micro_smg_varmod_luxe,minigun_clip01,musket_clip01,pistol50clip01,pistol50clip02,pistol50varmod_luxe,pistol_clip01,pistol_clip02,pistol_varmod_luxe,police_torch_flashlight,pump_shotgun_clip01,pump_shotgun_varmod_lowrider,rpg_clip01,railgun_clip01,revolver_clip01,revolver_varmod_boss,revolver_varmod_goon,smg_clip01,smg_clip02,smg_clip03,smg_varmod_luxe,sns_pistol_clip01,sns_pistol_clip02,sns_pistol_varmod_lowrider,sawnoff_shotgun_clip01,sawnoff_shotgun_varmod_luxe,sniper_rifle_clip01,sniper_rifle_varmod_luxe,special_carbine_clip01,special_carbine_clip02,special_carbine_clip03,special_carbine_varmod_lowrider,switchblade_varmod_base,switchblade_varmod_var1,switchblade_varmod_var2,vintage_pistol_clip01,vintage_pistol_clip02,invalid");
        }
    }
}

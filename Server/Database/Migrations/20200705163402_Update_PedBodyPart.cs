using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Update_PedBodyPart : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerWeaponBodypartStats");

            migrationBuilder.Sql(
@"DROP TYPE public.ped_body_part;
CREATE TYPE public.ped_body_part AS ENUM ('head', 'neck', 'torso', 'genital_region', 'arm', 'hand', 'leg', 'foot');");

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
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerWeaponBodypartStats_Weapons_WeaponHash",
                        column: x => x.WeaponHash,
                        principalTable: "Weapons",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerWeaponBodypartStats_WeaponHash",
                table: "PlayerWeaponBodypartStats",
                column: "WeaponHash");
        }

        #endregion Protected Methods
    }
}

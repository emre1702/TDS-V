using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class UpdateDatabase2 : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LobbyArmsRaceWeapons");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LobbyArmsRaceWeapons",
                columns: table => new
                {
                    AtKill = table.Column<short>(nullable: false),
                    LobbyId = table.Column<int>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_LobbyArmsRaceWeapons_WeaponHash",
                table: "LobbyArmsRaceWeapons",
                column: "WeaponHash");
        }

        #endregion Protected Methods
    }
}

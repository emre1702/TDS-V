using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class GangActionSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GangwarAreas");

            migrationBuilder.RenameColumn(
                name: "MinPlayersOnlineForGangwar",
                table: "ServerSettings",
                newName: "MinPlayersOnlineForGangAction");

            migrationBuilder.RenameColumn(
                name: "GangwarTargetWithoutAttackerMaxSeconds",
                table: "ServerSettings",
                newName: "GangActionTargetWithoutAttackerMaxSeconds");

            migrationBuilder.RenameColumn(
                name: "GangwarTargetRadius",
                table: "ServerSettings",
                newName: "GangActionTargetRadius");

            migrationBuilder.RenameColumn(
                name: "GangwarPreparationTime",
                table: "ServerSettings",
                newName: "GangActionPreparationTime");

            migrationBuilder.RenameColumn(
                name: "GangwarOwnerCanBeMore",
                table: "ServerSettings",
                newName: "GangActionOwnerCanBeMore");

            migrationBuilder.RenameColumn(
                name: "GangwarAttackerCanBeMore",
                table: "ServerSettings",
                newName: "GangActionAttackerCanBeMore");

            migrationBuilder.RenameColumn(
                name: "GangwarAreaAttackCooldownMinutes",
                table: "ServerSettings",
                newName: "GangActionAreaAttackCooldownMinutes");

            migrationBuilder.RenameColumn(
                name: "GangwarActionTime",
                table: "ServerSettings",
                newName: "GangActionRoundTime");

            migrationBuilder.RenameColumn(
                name: "StartGangwar",
                table: "GangRankPermissions",
                newName: "StartGangAction");

            migrationBuilder.AddColumn<int>(
                name: "GangMaxGangActionAttacksPerDay",
                table: "ServerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GangActionAreas",
                columns: table => new
                {
                    MapId = table.Column<int>(type: "integer", nullable: false),
                    OwnerGangId = table.Column<int>(type: "integer", nullable: false),
                    AttackCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DefendCountSinceLastCapture = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastAttacked = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "'2019-1-1'::timestamp"),
                    CooldownStartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangActionAreas", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_GangActionAreas_Gangs_OwnerGangId",
                        column: x => x.OwnerGangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GangActionAreas_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ServerSettings",
                keyColumn: "Id",
                keyValue: (short)1,
                column: "GangMaxGangActionAttacksPerDay",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_GangActionAreas_OwnerGangId",
                table: "GangActionAreas",
                column: "OwnerGangId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GangActionAreas");

            migrationBuilder.DropColumn(
                name: "GangMaxGangActionAttacksPerDay",
                table: "ServerSettings");

            migrationBuilder.RenameColumn(
                name: "MinPlayersOnlineForGangAction",
                table: "ServerSettings",
                newName: "MinPlayersOnlineForGangwar");

            migrationBuilder.RenameColumn(
                name: "GangActionTargetWithoutAttackerMaxSeconds",
                table: "ServerSettings",
                newName: "GangwarTargetWithoutAttackerMaxSeconds");

            migrationBuilder.RenameColumn(
                name: "GangActionTargetRadius",
                table: "ServerSettings",
                newName: "GangwarTargetRadius");

            migrationBuilder.RenameColumn(
                name: "GangActionRoundTime",
                table: "ServerSettings",
                newName: "GangwarActionTime");

            migrationBuilder.RenameColumn(
                name: "GangActionPreparationTime",
                table: "ServerSettings",
                newName: "GangwarPreparationTime");

            migrationBuilder.RenameColumn(
                name: "GangActionOwnerCanBeMore",
                table: "ServerSettings",
                newName: "GangwarOwnerCanBeMore");

            migrationBuilder.RenameColumn(
                name: "GangActionAttackerCanBeMore",
                table: "ServerSettings",
                newName: "GangwarAttackerCanBeMore");

            migrationBuilder.RenameColumn(
                name: "GangActionAreaAttackCooldownMinutes",
                table: "ServerSettings",
                newName: "GangwarAreaAttackCooldownMinutes");

            migrationBuilder.RenameColumn(
                name: "StartGangAction",
                table: "GangRankPermissions",
                newName: "StartGangwar");

            migrationBuilder.CreateTable(
                name: "GangwarAreas",
                columns: table => new
                {
                    MapId = table.Column<int>(type: "integer", nullable: false),
                    AttackCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DefendCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastAttacked = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "'2019-1-1'::timestamp"),
                    OwnerGangId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GangwarAreas", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_GangwarAreas_Gangs_OwnerGangId",
                        column: x => x.OwnerGangId,
                        principalTable: "Gangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GangwarAreas_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GangwarAreas_OwnerGangId",
                table: "GangwarAreas",
                column: "OwnerGangId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class GangMember_Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "players_GangId_fkey",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_players_GangId",
                table: "players");

            migrationBuilder.DropColumn(
                name: "GangId",
                table: "players");

            migrationBuilder.AddColumn<int>(
                name: "GangsId",
                table: "players",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "gangs",
                nullable: false,
                defaultValueSql: "timezone('utc', now())");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "gangs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "gang_members",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false),
                    GangID = table.Column<int>(nullable: false),
                    Rank = table.Column<short>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gang_members_players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gang_members_gang_ranks_RankNavigationGangId_RankNavigation~",
                        columns: x => new { x.RankNavigationGangId, x.RankNavigationRank },
                        principalTable: "gang_ranks",
                        principalColumns: new[] { "GangID", "Rank" },
                        onDelete: ReferentialAction.SetDefault,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_players_GangsId",
                table: "players",
                column: "GangsId");

            migrationBuilder.CreateIndex(
                name: "IX_gangs_OwnerId",
                table: "gangs",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gang_members_GangID",
                table: "gang_members",
                column: "GangID");

            migrationBuilder.CreateIndex(
                name: "IX_gang_members_RankNavigationGangId_RankNavigationRank",
                table: "gang_members",
                columns: new[] { "RankNavigationGangId", "RankNavigationRank" });

            migrationBuilder.AddForeignKey(
                name: "FK_gangs_players_OwnerId",
                table: "gangs",
                column: "OwnerId",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetDefault,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_players_gangs_GangsId",
                table: "players",
                column: "GangsId",
                principalTable: "gangs",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetDefault,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gangs_players_OwnerId",
                table: "gangs");

            migrationBuilder.DropForeignKey(
                name: "FK_players_gangs_GangsId",
                table: "players");

            migrationBuilder.DropTable(
                name: "gang_members");

            migrationBuilder.DropIndex(
                name: "IX_players_GangsId",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_gangs_OwnerId",
                table: "gangs");

            migrationBuilder.DropColumn(
                name: "GangsId",
                table: "players");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "gangs");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "gangs");

            migrationBuilder.AddColumn<int>(
                name: "GangId",
                table: "players",
                type: "integer",
                nullable: true,
                defaultValue: -1);

            migrationBuilder.CreateIndex(
                name: "IX_players_GangId",
                table: "players",
                column: "GangId");

            migrationBuilder.AddForeignKey(
                name: "players_GangId_fkey",
                table: "players",
                column: "GangId",
                principalTable: "gangs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

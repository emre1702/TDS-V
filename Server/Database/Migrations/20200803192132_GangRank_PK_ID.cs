using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class GangRank_PK_ID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GangMembers_GangRanks_RankNavigationGangId_RankNavigationRa~",
                table: "GangMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GangRanks",
                table: "GangRanks");

            migrationBuilder.DropIndex(
                name: "IX_GangMembers_RankNavigationGangId_RankNavigationRank",
                table: "GangMembers");

            migrationBuilder.DeleteData(
                table: "GangRanks",
                keyColumns: new[] { "GangId", "Rank" },
                keyValues: new object[] { -1, (short)0 });

          
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "GangMembers");

            migrationBuilder.DropColumn(
                name: "RankNavigationGangId",
                table: "GangMembers");

            migrationBuilder.DropColumn(
                name: "RankNavigationRank",
                table: "GangMembers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GangRanks",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "GangMembers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GangRanks",
                table: "GangRanks",
                column: "Id");

            migrationBuilder.InsertData(
                table: "GangRanks",
                columns: new[] { "Id", "Color", "GangId", "Name", "Rank" },
                values: new object[] { -1, "rgb(255,255,255)", -1, "-", (short)0 });

            migrationBuilder.CreateIndex(
                name: "IX_GangRanks_GangId",
                table: "GangRanks",
                column: "GangId");

            migrationBuilder.CreateIndex(
                name: "IX_GangRanks_Rank",
                table: "GangRanks",
                column: "Rank");

            migrationBuilder.CreateIndex(
                name: "IX_GangMembers_RankId",
                table: "GangMembers",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_GangMembers_GangRanks_RankId",
                table: "GangMembers",
                column: "RankId",
                principalTable: "GangRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GangMembers_GangRanks_RankId",
                table: "GangMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GangRanks",
                table: "GangRanks");

            migrationBuilder.DropIndex(
                name: "IX_GangRanks_GangId",
                table: "GangRanks");

            migrationBuilder.DropIndex(
                name: "IX_GangRanks_Rank",
                table: "GangRanks");

            migrationBuilder.DropIndex(
                name: "IX_GangMembers_RankId",
                table: "GangMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GangRanks");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "GangMembers");

            migrationBuilder.AddColumn<short>(
                name: "Rank",
                table: "GangMembers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "RankNavigationGangId",
                table: "GangMembers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "RankNavigationRank",
                table: "GangMembers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GangRanks",
                table: "GangRanks",
                columns: new[] { "GangId", "Rank" });

            migrationBuilder.CreateIndex(
                name: "IX_GangMembers_RankNavigationGangId_RankNavigationRank",
                table: "GangMembers",
                columns: new[] { "RankNavigationGangId", "RankNavigationRank" });

            migrationBuilder.AddForeignKey(
                name: "FK_GangMembers_GangRanks_RankNavigationGangId_RankNavigationRa~",
                table: "GangMembers",
                columns: new[] { "RankNavigationGangId", "RankNavigationRank" },
                principalTable: "GangRanks",
                principalColumns: new[] { "GangId", "Rank" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

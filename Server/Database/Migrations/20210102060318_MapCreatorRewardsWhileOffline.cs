using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Migrations
{
    public partial class MapCreatorRewardsWhileOffline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:map_retrieve_type", "none,random,voted,bought");

            migrationBuilder.CreateTable(
                name: "PlayerMapCreatorRewardsWhileOffline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    MapId = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<MapRetrieveType>(type: "map_retrieve_type", nullable: false),
                    Reward = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMapCreatorRewardsWhileOffline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMapCreatorRewardsWhileOffline_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlayerMapCreatorRewardsWhileOffline_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMapCreatorRewardsWhileOffline_MapId",
                table: "PlayerMapCreatorRewardsWhileOffline",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMapCreatorRewardsWhileOffline_PlayerId",
                table: "PlayerMapCreatorRewardsWhileOffline",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData",
                columns: new[] { "PlayerId", "Slot" },
                principalTable: "PlayerClothesData",
                principalColumns: new[] { "PlayerId", "Slot" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData");

            migrationBuilder.DropTable(
                name: "PlayerMapCreatorRewardsWhileOffline");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:map_retrieve_type", "none,random,voted,bought");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData",
                columns: new[] { "PlayerId", "Slot" },
                principalTable: "PlayerClothesData",
                principalColumns: new[] { "PlayerId", "Slot" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
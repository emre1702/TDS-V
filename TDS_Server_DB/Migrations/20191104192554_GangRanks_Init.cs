using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TDS_Server_DB.Migrations
{
    public partial class GangRanks_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GangID",
                table: "gang_rank_permissions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "gang_ranks",
                columns: table => new
                {
                    GangID = table.Column<int>(nullable: false),
                    Rank = table.Column<short>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gang_ranks", x => new { x.GangID, x.Rank });
                    table.ForeignKey(
                        name: "FK_gang_ranks_gangs_GangID",
                        column: x => x.GangID,
                        principalTable: "gangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_gang_rank_permissions_gangs_GangID",
                table: "gang_rank_permissions",
                column: "GangID",
                principalTable: "gangs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gang_rank_permissions_gangs_GangID",
                table: "gang_rank_permissions");

            migrationBuilder.DropTable(
                name: "gang_ranks");

            migrationBuilder.AlterColumn<int>(
                name: "GangID",
                table: "gang_rank_permissions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}

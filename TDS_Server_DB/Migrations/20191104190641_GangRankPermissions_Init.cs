using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TDS_Server_DB.Migrations
{
    public partial class GangRankPermissions_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CloseApplicationAfterDays",
                table: "server_settings",
                nullable: false,
                defaultValue: 7);

            migrationBuilder.AddColumn<int>(
                name: "DeleteApplicationAfterDays",
                table: "server_settings",
                nullable: false,
                defaultValue: 14);

            migrationBuilder.AddColumn<long>(
                name: "GangwarActionTimeMs",
                table: "server_settings",
                nullable: false,
                defaultValue: 900000L);

            migrationBuilder.AddColumn<long>(
                name: "GangwarPreparationTimeMs",
                table: "server_settings",
                nullable: false,
                defaultValue: 180000L);

            migrationBuilder.CreateTable(
                name: "gang_rank_permissions",
                columns: table => new
                {
                    GangID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManagePermissions = table.Column<short>(nullable: false),
                    InviteMembers = table.Column<short>(nullable: false),
                    KickMembers = table.Column<short>(nullable: false),
                    ManageRanks = table.Column<short>(nullable: false),
                    StartGangwar = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gang_rank_permissions", x => x.GangID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gang_rank_permissions");

            migrationBuilder.DropColumn(
                name: "CloseApplicationAfterDays",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "DeleteApplicationAfterDays",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "GangwarActionTimeMs",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "GangwarPreparationTimeMs",
                table: "server_settings");
        }
    }
}

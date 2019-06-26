using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TDS_Server_DB.Migrations
{
    public partial class ServerStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "server_daily_stats",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    PlayerPeak = table.Column<short>(nullable: false, defaultValue: (short)0),
                    ArenaRoundsPlayed = table.Column<int>(nullable: false, defaultValue: 0),
                    CustomArenaRoundsPlayed = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountLogins = table.Column<int>(nullable: false, defaultValue: 0),
                    AmountRegistrations = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("server_daily_stats_date_pkey", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "server_total_stats",
                columns: table => new
                {
                    ID = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerPeak = table.Column<short>(nullable: false, defaultValue: (short)0),
                    ArenaRoundsPlayed = table.Column<long>(nullable: false, defaultValue: 0L),
                    CustomArenaRoundsPlayed = table.Column<long>(nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_total_stats", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "server_total_stats",
                column: "ID",
                value: (short)1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "server_daily_stats");

            migrationBuilder.DropTable(
                name: "server_total_stats");
        }
    }
}

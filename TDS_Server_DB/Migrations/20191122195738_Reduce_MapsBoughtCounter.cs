using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Reduce_MapsBoughtCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReduceMapsBoughtCounterAfterMinute",
                table: "server_settings",
                nullable: false,
                defaultValue: 60);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMapsBoughtCounterReduce",
                table: "player_stats",
                nullable: false,
                defaultValueSql: "timezone('utc', now())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReduceMapsBoughtCounterAfterMinute",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "LastMapsBoughtCounterReduce",
                table: "player_stats");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class GangwarAreas_Add_AttackCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GangwarAreaAttackCooldownMinutes",
                table: "server_settings",
                nullable: false,
                defaultValue: 60);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAttacked",
                table: "gangwar_areas",
                nullable: false,
                defaultValueSql: "'2019-1-1'::timestamp",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "'1970-1-1'::timestamp");

            migrationBuilder.AddColumn<int>(
                name: "AttackCount",
                table: "gangwar_areas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GangwarAreaAttackCooldownMinutes",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "AttackCount",
                table: "gangwar_areas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAttacked",
                table: "gangwar_areas",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "'1970-1-1'::timestamp",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "'2019-1-1'::timestamp");
        }
    }
}

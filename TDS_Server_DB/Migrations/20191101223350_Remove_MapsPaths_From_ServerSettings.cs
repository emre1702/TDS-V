using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Remove_MapsPaths_From_ServerSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapsPath",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "NeedCheckMapsPath",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "NewMapsPath",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "SavedMapsPath",
                table: "server_settings");

            migrationBuilder.CreateTable(
                name: "gangwar_areas",
                columns: table => new
                {
                    MapID = table.Column<int>(nullable: false),
                    OwnerGangID = table.Column<int>(nullable: false),
                    LastAttacked = table.Column<DateTime>(nullable: false, defaultValueSql: "'1970-1-1'::timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gangwar_areas", x => x.MapID);
                    table.ForeignKey(
                        name: "FK_gangwar_areas_maps_MapID",
                        column: x => x.MapID,
                        principalTable: "maps",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gangwar_areas_gangs_OwnerGangID",
                        column: x => x.OwnerGangID,
                        principalTable: "gangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gangwar_areas_OwnerGangID",
                table: "gangwar_areas",
                column: "OwnerGangID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gangwar_areas");

            migrationBuilder.AddColumn<string>(
                name: "MapsPath",
                table: "server_settings",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NeedCheckMapsPath",
                table: "server_settings",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewMapsPath",
                table: "server_settings",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SavedMapsPath",
                table: "server_settings",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "server_settings",
                keyColumn: "ID",
                keyValue: (short)1,
                columns: new[] { "MapsPath", "NeedCheckMapsPath", "NewMapsPath", "SavedMapsPath" },
                values: new object[] { "bridge/resources/tds/maps/", "bridge/resources/tds/needcheckmaps/", "bridge/resources/tds/newmaps/", "bridge/resources/tds/savedmaps/" });
        }
    }
}

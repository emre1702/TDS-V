using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class GangHouseAndMore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "house_area_radius",
                table: "gang_level_settings",
                nullable: false,
                defaultValue: 30f);

            migrationBuilder.CreateTable(
                name: "gang_vehicles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gang_id = table.Column<int>(nullable: false),
                    model = table.Column<VehicleHash>(nullable: false),
                    spawn_pos_x = table.Column<float>(nullable: false),
                    spawn_pos_y = table.Column<float>(nullable: false),
                    spawn_pos_z = table.Column<float>(nullable: false),
                    spawn_rot_x = table.Column<float>(nullable: false),
                    spawn_rot_y = table.Column<float>(nullable: false),
                    spawn_rot_z = table.Column<float>(nullable: false),
                    color1 = table.Column<int>(nullable: false),
                    color2 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gang_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_gang_vehicles_gangs_gang_id",
                        column: x => x.gang_id,
                        principalTable: "gangs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gang_vehicles_gang_id",
                table: "gang_vehicles",
                column: "gang_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gang_vehicles");

            migrationBuilder.DropColumn(
                name: "house_area_radius",
                table: "gang_level_settings");
        }
    }
}

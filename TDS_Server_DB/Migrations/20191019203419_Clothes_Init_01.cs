using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Clothes_Init_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SkinHash",
                table: "teams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "player_clothes",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false),
                    IsMale = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_clothes", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_player_clothes_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 1,
                column: "SkinHash",
                value: null);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 4,
                column: "SkinHash",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_clothes");

            migrationBuilder.AlterColumn<int>(
                name: "SkinHash",
                table: "teams",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 1,
                column: "SkinHash",
                value: 1004114196);

            migrationBuilder.UpdateData(
                table: "teams",
                keyColumn: "ID",
                keyValue: 4,
                column: "SkinHash",
                value: 1004114196);
        }
    }
}

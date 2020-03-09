using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_LobbyFightSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_lifes",
                table: "lobbies");

            migrationBuilder.DropColumn(
                name: "spawn_again_after_death_ms",
                table: "lobbies");

            migrationBuilder.DropColumn(
                name: "start_armor",
                table: "lobbies");

            migrationBuilder.DropColumn(
                name: "start_health",
                table: "lobbies");

            migrationBuilder.CreateTable(
                name: "lobby_fight_settings",
                columns: table => new
                {
                    lobby_id = table.Column<int>(nullable: false),
                    start_health = table.Column<short>(nullable: false, defaultValue: (short)100),
                    start_armor = table.Column<short>(nullable: false, defaultValue: (short)100),
                    amount_lifes = table.Column<short>(nullable: false),
                    spawn_again_after_death_ms = table.Column<int>(nullable: false, defaultValue: 400)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lobby_fight_settings", x => x.lobby_id);
                    table.ForeignKey(
                        name: "FK_lobby_fight_settings_lobbies_lobby_id",
                        column: x => x.lobby_id,
                        principalTable: "lobbies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "lobby_fight_settings",
                columns: new[] { "lobby_id", "amount_lifes" },
                values: new object[] { -1, (short)0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lobby_fight_settings");

            migrationBuilder.AddColumn<short>(
                name: "amount_lifes",
                table: "lobbies",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "spawn_again_after_death_ms",
                table: "lobbies",
                type: "integer",
                nullable: false,
                defaultValueSql: "400");

            migrationBuilder.AddColumn<short>(
                name: "start_armor",
                table: "lobbies",
                type: "smallint",
                nullable: false,
                defaultValueSql: "100");

            migrationBuilder.AddColumn<short>(
                name: "start_health",
                table: "lobbies",
                type: "smallint",
                nullable: false,
                defaultValueSql: "100");

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -3,
                columns: new[] { "amount_lifes", "spawn_again_after_death_ms" },
                values: new object[] { (short)1, 400 });

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -2,
                columns: new[] { "amount_lifes", "spawn_again_after_death_ms" },
                values: new object[] { (short)1, 400 });

            migrationBuilder.UpdateData(
                table: "lobbies",
                keyColumn: "id",
                keyValue: -1,
                columns: new[] { "amount_lifes", "spawn_again_after_death_ms" },
                values: new object[] { (short)1, 400 });
        }
    }
}

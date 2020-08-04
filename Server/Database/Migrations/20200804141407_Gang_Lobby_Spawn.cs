using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Gang_Lobby_Spawn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lobbies",
                keyColumn: "Id",
                keyValue: -5,
                columns: new[] { "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ" },
                values: new object[] { -425.2233f, 1126.9731f, 326.8f });

            migrationBuilder.UpdateData(
                table: "Lobbies",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ" },
                values: new object[] { -365.425f, -131.809f, 37.873f });

            migrationBuilder.UpdateData(
                table: "Lobbies",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ" },
                values: new object[] { -365.425f, -131.809f, 37.873f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lobbies",
                keyColumn: "Id",
                keyValue: -5,
                columns: new[] { "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ" },
                values: new object[] { -425.2233f, 1126.9731f, 326.8f });

            migrationBuilder.UpdateData(
                table: "Lobbies",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ" },
                values: new object[] { -365.425f, -131.809f, 37.873f });

           
        }
    }
}

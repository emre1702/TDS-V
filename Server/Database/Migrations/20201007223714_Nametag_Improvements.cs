using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Database.Migrations
{
    public partial class Nametag_Improvements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
               table: "ServerSettings",
               keyColumn: "Id",
               keyValue: (short)1,
               columns: new[] { "NametagMaxDistance" },
               values: new object[] { 1000f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
               table: "ServerSettings",
               keyColumn: "Id",
               keyValue: (short)1,
               columns: new[] { "NametagMaxDistance" },
               values: new object[] { 80f });
        }
    }
}

using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Database.Migrations
{
    public partial class ServerSettings_GitHubRepoOwnerAndName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GitHubRepoOwnerName",
                table: "ServerSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubRepoRepoName",
                table: "ServerSettings",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ServerSettings",
                keyColumn: "Id",
                keyValue: (short)1,
                columns: new[] { "GitHubRepoOwnerName", "GitHubRepoRepoName" },
                values: new object[] { "emre1702", "TDS-V" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubRepoOwnerName",
                table: "ServerSettings");

            migrationBuilder.DropColumn(
                name: "GitHubRepoRepoName",
                table: "ServerSettings");
        }
    }
}

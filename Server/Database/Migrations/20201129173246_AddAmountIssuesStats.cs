using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class AddAmountIssuesStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountHighPriorityIssues",
                table: "PlayerStats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountLowPriorityIssues",
                table: "PlayerStats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountMediumPriorityIssues",
                table: "PlayerStats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountUrgentPriorityIssues",
                table: "PlayerStats",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountHighPriorityIssues",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "AmountLowPriorityIssues",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "AmountMediumPriorityIssues",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "AmountUrgentPriorityIssues",
                table: "PlayerStats");
        }
    }
}

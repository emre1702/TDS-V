using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class LobbyKillingspreeRewards_RemoveOnlyHealthOnlyArmor : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "OnlyArmor",
                table: "KillingspreeRewards",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OnlyHealth",
                table: "KillingspreeRewards",
                type: "smallint",
                nullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlyArmor",
                table: "KillingspreeRewards");

            migrationBuilder.DropColumn(
                name: "OnlyHealth",
                table: "KillingspreeRewards");
        }

        #endregion Protected Methods
    }
}

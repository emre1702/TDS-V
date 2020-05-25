using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_Index_DiscordUserId : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerSettings_DiscordUserId",
                table: "PlayerSettings");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlayerSettings_DiscordUserId",
                table: "PlayerSettings",
                column: "DiscordUserId",
                unique: true);
        }

        #endregion Protected Methods
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerSettings_Infos : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowCursorInfo",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ShowLobbyLeaveInfo",
                table: "PlayerSettings");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowCursorInfo",
                table: "PlayerSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLobbyLeaveInfo",
                table: "PlayerSettings",
                nullable: false,
                defaultValue: false);
        }

        #endregion Protected Methods
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class ChatInfoSettings_Init : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatInfoFontSize",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "ChatInfoMoveTimeMs",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "HideChatInfo",
                table: "PlayerSettings");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ChatInfoFontSize",
                table: "PlayerSettings",
                nullable: false,
                defaultValue: 1f);

            migrationBuilder.AddColumn<int>(
                name: "ChatInfoMoveTimeMs",
                table: "PlayerSettings",
                nullable: false,
                defaultValue: 15000);

            migrationBuilder.AddColumn<bool>(
                name: "HideChatInfo",
                table: "PlayerSettings",
                nullable: false,
                defaultValue: false);
        }

        #endregion Protected Methods
    }
}

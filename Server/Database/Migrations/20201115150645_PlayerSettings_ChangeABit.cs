using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerSettings_ChangeABit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontWidth",
                table: "PlayerKillInfoSettings");

            migrationBuilder.AlterColumn<bool>(
                name: "WindowsNotifications",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<float>(
                name: "VoiceVolume",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 6f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 6f);

            migrationBuilder.AlterColumn<bool>(
                name: "VoiceAutoVolume",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "Voice3D",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLobbyLeaveInfo",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "ShowFloatingDamageInfoDurationMs",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 1000,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1000);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCursorOnChatOpen",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCursorInfo",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowConfettiAtRanking",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<TimeSpanUnitsOfTime>(
                name: "ScoreboardPlaytimeUnit",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: TimeSpanUnitsOfTime.HourMinute,
                oldClrType: typeof(TimeSpanUnitsOfTime),
                oldType: "time_span_units_of_time",
                oldDefaultValue: TimeSpanUnitsOfTime.HourMinute);

            migrationBuilder.AlterColumn<bool>(
                name: "ScoreboardPlayerSortingDesc",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<ScoreboardPlayerSorting>(
                name: "ScoreboardPlayerSorting",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: ScoreboardPlayerSorting.Name,
                oldClrType: typeof(ScoreboardPlayerSorting),
                oldType: "scoreboard_player_sorting");

            migrationBuilder.AlterColumn<Language>(
                name: "Language",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: Language.English,
                oldClrType: typeof(Language),
                oldType: "language",
                oldDefaultValue: Language.English);

            migrationBuilder.AlterColumn<int>(
                name: "HudHealthUpdateCooldownMs",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 100,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 100);

            migrationBuilder.AlterColumn<int>(
                name: "HudAmmoUpdateCooldownMs",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 100,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "Hitsound",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "HideDirtyChat",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "HideChatInfo",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "FloatingDamageInfo",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "CheckAFK",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<float>(
                name: "ChatWidth",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 30f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 30f);

            migrationBuilder.AlterColumn<float>(
                name: "ChatMaxHeight",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 35f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 35f);

            migrationBuilder.AlterColumn<int>(
                name: "ChatInfoMoveTimeMs",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 15000,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 15000);

            migrationBuilder.AlterColumn<float>(
                name: "ChatInfoFontSize",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 1f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 1f);

            migrationBuilder.AlterColumn<float>(
                name: "ChatFontSize",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 1.4f,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 1.4f);

            migrationBuilder.AlterColumn<int>(
                name: "BloodscreenCooldownMs",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 150,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "Bloodscreen",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "AllowDataTransfer",
                table: "PlayerSettings",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "AFKKickShowWarningLastSeconds",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 10,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 10);

            migrationBuilder.AlterColumn<int>(
                name: "AFKKickAfterSeconds",
                table: "PlayerSettings",
                nullable: true,
                defaultValue: 25,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 25);

            migrationBuilder.AddColumn<int>(
                name: "PlayerSettingsPlayerId",
                table: "PlayerSettings",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Duration",
                table: "PlayerKillInfoSettings",
                nullable: false,
                defaultValue: 10f);

            migrationBuilder.AddColumn<float>(
                name: "FontSize",
                table: "PlayerKillInfoSettings",
                nullable: false,
                defaultValue: 1.4f);

            migrationBuilder.AddColumn<int>(
                name: "IconHeight",
                table: "PlayerKillInfoSettings",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSettings_PlayerSettingsPlayerId",
                table: "PlayerSettings",
                column: "PlayerSettingsPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSettings_PlayerSettings_PlayerSettingsPlayerId",
                table: "PlayerSettings",
                column: "PlayerSettingsPlayerId",
                principalTable: "PlayerSettings",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSettings_PlayerSettings_PlayerSettingsPlayerId",
                table: "PlayerSettings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerSettings_PlayerSettingsPlayerId",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "PlayerSettingsPlayerId",
                table: "PlayerSettings");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "PlayerKillInfoSettings");

            migrationBuilder.DropColumn(
                name: "FontSize",
                table: "PlayerKillInfoSettings");

            migrationBuilder.DropColumn(
                name: "IconHeight",
                table: "PlayerKillInfoSettings");

            migrationBuilder.AlterColumn<float>(
                name: "VoiceVolume",
                table: "PlayerSettings",
                type: "real",
                nullable: false,
                defaultValue: 6f,
                oldClrType: typeof(float),
                oldNullable: true,
                oldDefaultValue: 6f);

            migrationBuilder.AlterColumn<bool>(
                name: "VoiceAutoVolume",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Voice3D",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpanUnitsOfTime>(
                name: "ScoreboardPlaytimeUnit",
                table: "PlayerSettings",
                type: "time_span_units_of_time",
                nullable: false,
                defaultValue: TimeSpanUnitsOfTime.HourMinute,
                oldClrType: typeof(TimeSpanUnitsOfTime),
                oldNullable: true,
                oldDefaultValue: TimeSpanUnitsOfTime.HourMinute);

            migrationBuilder.AlterColumn<bool>(
                name: "ScoreboardPlayerSortingDesc",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<ScoreboardPlayerSorting>(
                name: "ScoreboardPlayerSorting",
                table: "PlayerSettings",
                type: "scoreboard_player_sorting",
                nullable: false,
                oldClrType: typeof(ScoreboardPlayerSorting),
                oldNullable: true,
                oldDefaultValue: ScoreboardPlayerSorting.Name);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLobbyLeaveInfo",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCursorInfo",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "WindowsNotifications",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowConfettiAtRanking",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<Language>(
                name: "Language",
                table: "PlayerSettings",
                type: "language",
                nullable: false,
                defaultValue: Language.English,
                oldClrType: typeof(Language),
                oldNullable: true,
                oldDefaultValue: Language.English);

            migrationBuilder.AlterColumn<bool>(
                name: "CheckAFK",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowDataTransfer",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Hitsound",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "FloatingDamageInfo",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Bloodscreen",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShowFloatingDamageInfoDurationMs",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 1000,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "HudHealthUpdateCooldownMs",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 100,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 100);

            migrationBuilder.AlterColumn<int>(
                name: "HudAmmoUpdateCooldownMs",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 100,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 100);

            migrationBuilder.AlterColumn<int>(
                name: "BloodscreenCooldownMs",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 150,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 150);

            migrationBuilder.AlterColumn<int>(
                name: "AFKKickShowWarningLastSeconds",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 10,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 10);

            migrationBuilder.AlterColumn<int>(
                name: "AFKKickAfterSeconds",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 25,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 25);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCursorOnChatOpen",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HideDirtyChat",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HideChatInfo",
                table: "PlayerSettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "ChatWidth",
                table: "PlayerSettings",
                type: "real",
                nullable: false,
                defaultValue: 30f,
                oldClrType: typeof(float),
                oldNullable: true,
                oldDefaultValue: 30f);

            migrationBuilder.AlterColumn<float>(
                name: "ChatMaxHeight",
                table: "PlayerSettings",
                type: "real",
                nullable: false,
                defaultValue: 35f,
                oldClrType: typeof(float),
                oldNullable: true,
                oldDefaultValue: 35f);

            migrationBuilder.AlterColumn<int>(
                name: "ChatInfoMoveTimeMs",
                table: "PlayerSettings",
                type: "integer",
                nullable: false,
                defaultValue: 15000,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 15000);

            migrationBuilder.AlterColumn<float>(
                name: "ChatInfoFontSize",
                table: "PlayerSettings",
                type: "real",
                nullable: false,
                defaultValue: 1f,
                oldClrType: typeof(float),
                oldNullable: true,
                oldDefaultValue: 1f);

            migrationBuilder.AlterColumn<float>(
                name: "ChatFontSize",
                table: "PlayerSettings",
                type: "real",
                nullable: false,
                defaultValue: 1.4f,
                oldClrType: typeof(float),
                oldNullable: true,
                oldDefaultValue: 1.4f);

            migrationBuilder.AddColumn<float>(
                name: "FontWidth",
                table: "PlayerKillInfoSettings",
                type: "real",
                nullable: false,
                defaultValue: 1.4f);
        }
    }
}

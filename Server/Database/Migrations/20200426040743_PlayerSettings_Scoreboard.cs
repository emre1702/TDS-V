using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerSettings_Scoreboard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:scoreboard_player_sorting", "name,play_time,kills,assists,deaths,kills_deaths_ratio,kills_deaths_assists_ratio")
                .Annotation("Npgsql:Enum:time_span_units_of_time", "second,minute,hour_minute,hour,day,week");

            migrationBuilder.AddColumn<ScoreboardPlayerSorting>(
                name: "scoreboard_player_sorting",
                table: "player_settings",
                nullable: false,
                defaultValue: ScoreboardPlayerSorting.Name);

            migrationBuilder.AddColumn<bool>(
                name: "scoreboard_player_sorting_desc",
                table: "player_settings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpanUnitsOfTime>(
                name: "scoreboard_playtime_unit",
                table: "player_settings",
                nullable: false,
                defaultValue: TimeSpanUnitsOfTime.HourMinute);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "scoreboard_player_sorting",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "scoreboard_player_sorting_desc",
                table: "player_settings");

            migrationBuilder.DropColumn(
                name: "scoreboard_playtime_unit",
                table: "player_settings");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:scoreboard_player_sorting", "name,play_time,kills,assists,deaths,kills_deaths_ratio,kills_deaths_assists_ratio")
                .OldAnnotation("Npgsql:Enum:time_span_units_of_time", "second,minute,hour_minute,hour,day,week");
        }
    }
}

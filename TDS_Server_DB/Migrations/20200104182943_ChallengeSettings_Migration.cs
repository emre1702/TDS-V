using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;

namespace TDS_Server_DB.Migrations
{
    public partial class ChallengeSettings_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.Assists, EChallengeFrequency.Weekly },
                columns: new[] { "max_number", "min_number" },
                values: new object[] { 100, 50 });

            migrationBuilder.InsertData(
                table: "challenge_settings",
                columns: new[] { "type", "frequency", "max_number", "min_number" },
                values: new object[,]
                {
                    { EChallengeType.WriteHelpfulIssue, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.RoundPlayed, EChallengeFrequency.Weekly, 100, 50 },
                    { EChallengeType.ReviewMaps, EChallengeFrequency.Forever, 10, 10 },
                    { EChallengeType.ReadTheRules, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.ReadTheFAQ, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.PlayTime, EChallengeFrequency.Weekly, 1500, 300 },
                    { EChallengeType.Killstreak, EChallengeFrequency.Weekly, 7, 3 },
                    { EChallengeType.BeHelpfulEnough, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.JoinDiscordServer, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.Damage, EChallengeFrequency.Weekly, 100000, 20000 },
                    { EChallengeType.CreatorOfAcceptedMap, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.ChangeSettings, EChallengeFrequency.Forever, 1, 1 },
                    { EChallengeType.BuyMaps, EChallengeFrequency.Forever, 500, 500 },
                    { EChallengeType.BombPlant, EChallengeFrequency.Weekly, 10, 5 },
                    { EChallengeType.BombDefuse, EChallengeFrequency.Weekly, 10, 5 },
                    { EChallengeType.Kills, EChallengeFrequency.Weekly, 150, 75 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.Kills, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.Damage, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.PlayTime, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.RoundPlayed, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.BombDefuse, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.BombPlant, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.Killstreak, EChallengeFrequency.Weekly });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.BuyMaps, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.ReviewMaps, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.ReadTheRules, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.ReadTheFAQ, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.ChangeSettings, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.JoinDiscordServer, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.WriteHelpfulIssue, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.CreatorOfAcceptedMap, EChallengeFrequency.Forever });

            migrationBuilder.DeleteData(
                table: "challenge_settings",
                keyColumns: new[] { "type", "frequency" },
                keyValues: new object[] { EChallengeType.BeHelpfulEnough, EChallengeFrequency.Forever });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;

namespace TDS_Server_DB.Migrations
{
    public partial class Add_Challenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discord_identity",
                table: "player_settings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:e_challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .Annotation("Npgsql:Enum:e_challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .Annotation("Npgsql:PostgresExtension:tsm_system_rows", ",,");
                
            migrationBuilder.AddColumn<int>(
                name: "amount_weekly_challenges",
                table: "server_settings",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<decimal>(
                name: "discord_user_id",
                table: "player_settings",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "challenge_settings",
                columns: table => new
                {
                    type = table.Column<EChallengeType>(nullable: false),
                    frequency = table.Column<EChallengeFrequency>(nullable: false),
                    min_number = table.Column<int>(nullable: false, defaultValue: 1),
                    max_number = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_challenge_settings", x => new { x.type, x.frequency });
                });

            migrationBuilder.CreateTable(
                name: "player_challenges",
                columns: table => new
                {
                    player_id = table.Column<int>(nullable: false),
                    challenge = table.Column<EChallengeType>(nullable: false),
                    frequency = table.Column<EChallengeFrequency>(nullable: false),
                    amount = table.Column<int>(nullable: false, defaultValue: 1),
                    current_amount = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_challenges", x => new { x.player_id, x.challenge, x.frequency });
                    table.ForeignKey(
                        name: "FK_player_challenges_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "challenge_settings");

            migrationBuilder.DropTable(
                name: "player_challenges");

            migrationBuilder.DropColumn(
                name: "amount_weekly_challenges",
                table: "server_settings");

            migrationBuilder.DropColumn(
                name: "discord_user_id",
                table: "player_settings");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:e_challenge_frequency", "hourly,daily,weekly,monthly,yearly,forever")
                .OldAnnotation("Npgsql:Enum:e_challenge_type", "kills,assists,damage,play_time,round_played,bomb_defuse,bomb_plant,killstreak,buy_maps,review_maps,read_the_rules,read_the_faq,change_settings,join_discord_server,write_helpful_issue,creator_of_accepted_map,be_helpful_enough")
                .OldAnnotation("Npgsql:PostgresExtension:tsm_system_rows", ",,");

            migrationBuilder.AddColumn<string>(
                name: "discord_identity",
                table: "player_settings",
                type: "text",
                nullable: true);
        }
    }
}

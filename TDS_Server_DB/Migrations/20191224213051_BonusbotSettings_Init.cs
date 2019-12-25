using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Common.Enum;

namespace TDS_Server_DB.Migrations
{
    public partial class BonusbotSettings_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bonusbot_settings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guild_id = table.Column<decimal>(nullable: true),
                    admin_applications_channel_id = table.Column<decimal>(nullable: true),
                    server_infos_channel_id = table.Column<decimal>(nullable: true),
                    support_requests_channel_id = table.Column<decimal>(nullable: true),
                    actions_info_channel_id = table.Column<decimal>(nullable: true),
                    error_logs_channel_id = table.Column<decimal>(nullable: true),
                    send_private_message_on_ban = table.Column<bool>(nullable: false),
                    send_private_message_on_offline_message = table.Column<bool>(nullable: false),
                    refresh_server_stats_frequency_sec = table.Column<int>(nullable: false, defaultValue: 60)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonusbot_settings", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "bonusbot_settings",
                columns: new[] { "id", "actions_info_channel_id", "admin_applications_channel_id", "error_logs_channel_id", "guild_id", "send_private_message_on_ban", "send_private_message_on_offline_message", "server_infos_channel_id", "support_requests_channel_id" },
                values: new object[] { 1, 659088752890871818m, 659072893526736896m, 659073884796092426m, 320309924175282177m, true, true, 659073271911809037m, 659073029896142855m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonusbot_settings");
        }
    }
}

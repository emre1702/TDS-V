using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Add_ResetPassword_LogType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE log_type ADD VALUE 'reset_password' AFTER 'voice_mute'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Database.Migrations
{
    public partial class Add_DamageTestLobby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE lobby_type ADD VALUE 'damage_test_lobby' AFTER 'gang_action_lobby'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

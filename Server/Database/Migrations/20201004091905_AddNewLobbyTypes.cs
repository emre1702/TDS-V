using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class AddNewLobbyTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE lobby_type ADD VALUE 'gang_action_lobby' AFTER 'char_create_lobby'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

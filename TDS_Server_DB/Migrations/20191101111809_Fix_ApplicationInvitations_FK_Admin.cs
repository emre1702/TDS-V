using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS_Server_DB.Migrations
{
    public partial class Fix_ApplicationInvitations_FK_Admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_application_invitations_players_ApplicationID",
                table: "application_invitations");

            migrationBuilder.CreateIndex(
                name: "IX_application_invitations_AdminID",
                table: "application_invitations",
                column: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_application_invitations_players_AdminID",
                table: "application_invitations",
                column: "AdminID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_application_invitations_players_AdminID",
                table: "application_invitations");

            migrationBuilder.DropIndex(
                name: "IX_application_invitations_AdminID",
                table: "application_invitations");

            migrationBuilder.AddForeignKey(
                name: "FK_application_invitations_players_ApplicationID",
                table: "application_invitations",
                column: "ApplicationID",
                principalTable: "players",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);
        }
    }
}

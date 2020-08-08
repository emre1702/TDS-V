using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Char_Slots2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_Players_PlayerId",
                table: "PlayerCharDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_Players_PlayerId",
                table: "PlayerCharDatas",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_Players_PlayerId",
                table: "PlayerCharDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_Players_PlayerId",
                table: "PlayerCharDatas",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

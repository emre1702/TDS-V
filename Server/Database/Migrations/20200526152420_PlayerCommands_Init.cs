using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class PlayerCommands_Init : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerCommands");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "PlayerCommands",
               columns: table => new
               {
                   Id = table.Column<long>(nullable: false)
                       .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                   CommandId = table.Column<short>(nullable: false),
                   CommandText = table.Column<string>(maxLength: 100, nullable: true),
                   PlayerId = table.Column<int>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_PlayerCommands", x => x.Id);
                   table.ForeignKey(
                       name: "FK_PlayerCommands_Commands_CommandId",
                       column: x => x.CommandId,
                       principalTable: "Commands",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                   table.ForeignKey(
                       name: "FK_PlayerCommands_Players_PlayerId",
                       column: x => x.PlayerId,
                       principalTable: "Players",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCommands_CommandId",
                table: "PlayerCommands",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCommands_PlayerId",
                table: "PlayerCommands",
                column: "PlayerId");
        }

        #endregion Protected Methods
    }
}

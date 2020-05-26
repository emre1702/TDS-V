using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class ChatInfos_Init : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatInfos");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Language = table.Column<Language>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatInfos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ChatInfos",
                columns: new[] { "Id", "Language", "Message" },
                values: new object[,]
                {
                    { 1, Language.German, "Du kannst die Lobby mit \"/leave\" verlassen." },
                    { 2, Language.English, "You can leave the lobby with \"/leave.\"" },
                    { 3, Language.German, "VIPs sind keine Spender" },
                    { 4, Language.English, "VIPs are not donators" },
                    { 8, Language.English, "The project leader appoints administrators. The administrators appoint supporters." },
                    { 5, Language.German, "Es gibt 3 Admin-Ränge: Supporter, Administrator, Projektleiter" },
                    { 6, Language.English, "There are 3 admin ranks: Supporter, Administrator, Project Leader" },
                    { 7, Language.German, "Der Projektleiter ernennt Administratoren. Die Administratoren ernennen Supporter." }
                });
        }

        #endregion Protected Methods
    }
}

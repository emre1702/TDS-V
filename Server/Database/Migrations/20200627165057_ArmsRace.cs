using Microsoft.EntityFrameworkCore.Migrations;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class ArmsRace : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Maps",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Maps",
                keyColumn: "Id",
                keyValue: -5);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Maps",
                columns: new[] { "Id", "CreatorId", "Name" },
                values: new object[,]
                {
                    { -6, -1, "All Arms Races" },
                    { -5, -1, "All Gangwars" }
                });
        }

        #endregion Protected Methods
    }
}

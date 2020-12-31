using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class CharCreatorToBodyIndexesRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyDatas\" RENAME CONSTRAINT \"PK_PlayerCharDatas\" TO \"PK_PlayerBodyDatas\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyAppearanceDatas\" RENAME CONSTRAINT \"PK_PlayerCharAppearanceDatas\" TO \"PK_PlayerBodyAppearanceDatas\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyFeaturesDatas\" RENAME CONSTRAINT \"PK_PlayerCharFeaturesDatas\" TO \"PK_PlayerBodyFeaturesDatas\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyGeneralDatas\" RENAME CONSTRAINT \"PK_PlayerCharGeneralDatas\" TO \"PK_PlayerBodyGeneralDatas\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyHairAndColorsDatas\" RENAME CONSTRAINT \"PK_PlayerCharHairAndColorsDatas\" TO \"PK_PlayerBodyHairAndColorsDatas\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyHeritageDatas\" RENAME CONSTRAINT \"PK_PlayerCharHeritageDatas\" TO \"PK_PlayerBodyHeritageDatas\"");

            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyAppearanceDatas\" RENAME CONSTRAINT \"FK_PlayerCharAppearanceDatas_PlayerCharDatas_PlayerId\" TO \"FK_PlayerBodyAppearanceDatas_PlayerBodyDatas_PlayerId\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyFeaturesDatas\" RENAME CONSTRAINT \"FK_PlayerCharFeaturesDatas_PlayerCharDatas_PlayerId\" TO \"FK_PlayerBodyFeaturesDatas_PlayerBodyDatas_PlayerId\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyGeneralDatas\" RENAME CONSTRAINT \"FK_PlayerCharGeneralDatas_PlayerCharDatas_PlayerId\" TO \"FK_PlayerBodyGeneralDatas_PlayerBodyDatas_PlayerId\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyHairAndColorsDatas\" RENAME CONSTRAINT \"FK_PlayerCharHairAndColorsDatas_PlayerCharDatas_PlayerId\" TO \"FK_PlayerBodyHairAndColorsDatas_PlayerBodyDatas_PlayerId\"");
            migrationBuilder.Sql("ALTER TABLE \"PlayerBodyHeritageDatas\" RENAME CONSTRAINT \"FK_PlayerCharHeritageDatas_PlayerCharDatas_PlayerId\" TO \"FK_PlayerBodyHeritageDatas_PlayerBodyDatas_PlayerId\"");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
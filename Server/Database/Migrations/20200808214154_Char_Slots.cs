using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Migrations
{
    public partial class Char_Slots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharAppearanceDatas_AppearanceDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharFeaturesDatas_FeaturesDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharGeneralDatas_GeneralDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharHairAndColorsDatas_HairAndColorsD~",
                table: "PlayerCharDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharHeritageDatas_HeritageDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharHeritageDatas",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharHairAndColorsDatas",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharGeneralDatas",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharFeaturesDatas",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerCharDatas_AppearanceDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerCharDatas_FeaturesDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerCharDatas_GeneralDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerCharDatas_HairAndColorsDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerCharDatas_HeritageDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharAppearanceDatas",
                table: "PlayerCharAppearanceDatas");

            migrationBuilder.RenameColumn("Id", "PlayerCharHeritageDatas", "PlayerId");
            migrationBuilder.RenameColumn("Id", "PlayerCharHairAndColorsDatas", "PlayerId");
            migrationBuilder.RenameColumn("Id", "PlayerCharGeneralDatas", "PlayerId");
            migrationBuilder.RenameColumn("Id", "PlayerCharAppearanceDatas", "PlayerId");
            migrationBuilder.RenameColumn("Id", "PlayerCharFeaturesDatas", "PlayerId");

            migrationBuilder.DropColumn(
                name: "AppearanceDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "FeaturesDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "GeneralDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "HairAndColorsDataId",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "HeritageDataId",
                table: "PlayerCharDatas");

            migrationBuilder.AddColumn<byte>(
                name: "AmountCharSlots",
                table: "ServerSettings",
                nullable: false,
                defaultValue: (byte)3);

            migrationBuilder.AddColumn<byte>(
                name: "Slot",
                table: "PlayerCharHeritageDatas",
                nullable: false,
                defaultValue: (byte)0);


            migrationBuilder.AddColumn<byte>(
                name: "Slot",
                table: "PlayerCharHairAndColorsDatas",
                nullable: false,
                defaultValue: (byte)0);


            migrationBuilder.AddColumn<byte>(
                name: "Slot",
                table: "PlayerCharGeneralDatas",
                nullable: false,
                defaultValue: (byte)0);


            migrationBuilder.AddColumn<byte>(
                name: "Slot",
                table: "PlayerCharFeaturesDatas",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Slot",
                table: "PlayerCharDatas",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Slot",
                table: "PlayerCharAppearanceDatas",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharHeritageDatas",
                table: "PlayerCharHeritageDatas",
                columns: new[] { "PlayerId", "Slot" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharHairAndColorsDatas",
                table: "PlayerCharHairAndColorsDatas",
                columns: new[] { "PlayerId", "Slot" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharGeneralDatas",
                table: "PlayerCharGeneralDatas",
                columns: new[] { "PlayerId", "Slot" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharFeaturesDatas",
                table: "PlayerCharFeaturesDatas",
                columns: new[] { "PlayerId", "Slot" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharAppearanceDatas",
                table: "PlayerCharAppearanceDatas",
                columns: new[] { "PlayerId", "Slot" });

            migrationBuilder.InsertData(
               table: "Commands",
               columns: new[] { "Id", "Command", "LobbyOwnerCanUse", "NeededAdminLevel", "NeededDonation", "VipCanUse" },
               values: new object[,]
               {
                    { 27, "CreateHouse", false, (short)2, null, false }
               });

            migrationBuilder.InsertData(
                table: "CommandAlias",
                columns: new[] { "Alias", "Command" },
                values: new object[,]
                {
                    { "HouseCreate", (short)27 },
                    { "NewHouse", (short)27 },
                    { "HouseNew", (short)27 }
                });

            migrationBuilder.InsertData(
                table: "CommandInfos",
                columns: new[] { "Id", "Language", "Info" },
                values: new object[,]
                {
                    { (short)27, Language.German, "Erstellt ein Haus in der Gang-Lobby." },
                    { (short)27, Language.English, "Creates a house in the gang lobby." }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharAppearanceDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharAppearanceDatas",
                column: "PlayerId",
                principalTable: "PlayerCharDatas",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharFeaturesDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharFeaturesDatas",
                column: "PlayerId",
                principalTable: "PlayerCharDatas",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharGeneralDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharGeneralDatas",
                column: "PlayerId",
                principalTable: "PlayerCharDatas",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharHairAndColorsDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharHairAndColorsDatas",
                column: "PlayerId",
                principalTable: "PlayerCharDatas",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharHeritageDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharHeritageDatas",
                column: "PlayerId",
                principalTable: "PlayerCharDatas",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharAppearanceDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharAppearanceDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharFeaturesDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharGeneralDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharHairAndColorsDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharHeritageDatas_PlayerCharDatas_PlayerId",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharHeritageDatas",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharHairAndColorsDatas",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharGeneralDatas",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharFeaturesDatas",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerCharAppearanceDatas",
                table: "PlayerCharAppearanceDatas");

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "HouseCreate", (short)27 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "HouseNew", (short)27 });

            migrationBuilder.DeleteData(
                table: "CommandAlias",
                keyColumns: new[] { "Alias", "Command" },
                keyValues: new object[] { "NewHouse", (short)27 });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)27, Language.German });

            migrationBuilder.DeleteData(
                table: "CommandInfos",
                keyColumns: new[] { "Id", "Language" },
                keyValues: new object[] { (short)27, Language.English });

            migrationBuilder.DeleteData(
                table: "Commands",
                keyColumn: "Id",
                keyValue: (short)27);

            migrationBuilder.DropColumn(
                name: "AmountCharSlots",
                table: "ServerSettings");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PlayerCharHeritageDatas");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PlayerCharHairAndColorsDatas");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PlayerCharGeneralDatas");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PlayerCharFeaturesDatas");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PlayerCharDatas");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerCharAppearanceDatas");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PlayerCharAppearanceDatas");

            migrationBuilder.RenameColumn("PlayerId", "PlayerCharHeritageDatas", "Id");
            migrationBuilder.RenameColumn("PlayerId", "PlayerCharHairAndColorsDatas", "Id");
            migrationBuilder.RenameColumn("PlayerId", "PlayerCharGeneralDatas", "Id");
            migrationBuilder.RenameColumn("PlayerId", "PlayerCharFeaturesDatas", "Id");
            migrationBuilder.RenameColumn("PlayerId", "PlayerCharAppearanceDatas", "Id");


            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AppearanceDataId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FeaturesDataId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GeneralDataId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairAndColorsDataId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeritageDataId",
                table: "PlayerCharDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharHeritageDatas",
                table: "PlayerCharHeritageDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharHairAndColorsDatas",
                table: "PlayerCharHairAndColorsDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharGeneralDatas",
                table: "PlayerCharGeneralDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharFeaturesDatas",
                table: "PlayerCharFeaturesDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerCharAppearanceDatas",
                table: "PlayerCharAppearanceDatas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharDatas_AppearanceDataId",
                table: "PlayerCharDatas",
                column: "AppearanceDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharDatas_FeaturesDataId",
                table: "PlayerCharDatas",
                column: "FeaturesDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharDatas_GeneralDataId",
                table: "PlayerCharDatas",
                column: "GeneralDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharDatas_HairAndColorsDataId",
                table: "PlayerCharDatas",
                column: "HairAndColorsDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharDatas_HeritageDataId",
                table: "PlayerCharDatas",
                column: "HeritageDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharAppearanceDatas_AppearanceDataId",
                table: "PlayerCharDatas",
                column: "AppearanceDataId",
                principalTable: "PlayerCharAppearanceDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharFeaturesDatas_FeaturesDataId",
                table: "PlayerCharDatas",
                column: "FeaturesDataId",
                principalTable: "PlayerCharFeaturesDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharGeneralDatas_GeneralDataId",
                table: "PlayerCharDatas",
                column: "GeneralDataId",
                principalTable: "PlayerCharGeneralDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharHairAndColorsDatas_HairAndColorsD~",
                table: "PlayerCharDatas",
                column: "HairAndColorsDataId",
                principalTable: "PlayerCharHairAndColorsDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_PlayerCharHeritageDatas_HeritageDataId",
                table: "PlayerCharDatas",
                column: "HeritageDataId",
                principalTable: "PlayerCharHeritageDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharDatas_Players_PlayerId",
                table: "PlayerCharDatas",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

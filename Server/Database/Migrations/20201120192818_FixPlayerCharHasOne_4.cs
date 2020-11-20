using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Server.Database.Migrations
{
    public partial class FixPlayerCharHasOne_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SyncedData_SkinTonePercentage",
                table: "PlayerCharHeritageDatas",
                newName: "SkinTonePercentage");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ResemblancePercentage",
                table: "PlayerCharHeritageDatas",
                newName: "ResemblancePercentage");

            migrationBuilder.RenameColumn(
                name: "SyncedData_MotherIndex",
                table: "PlayerCharHeritageDatas",
                newName: "MotherIndex");

            migrationBuilder.RenameColumn(
                name: "SyncedData_FatherIndex",
                table: "PlayerCharHeritageDatas",
                newName: "FatherIndex");

            migrationBuilder.RenameColumn(
                name: "SyncedData_LipstickColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "LipstickColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_HairHighlightColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "HairHighlightColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_HairColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "HairColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Hair",
                table: "PlayerCharHairAndColorsDatas",
                newName: "Hair");

            migrationBuilder.RenameColumn(
                name: "SyncedData_FacialHairColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "FacialHairColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_EyebrowColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "EyebrowColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_EyeColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "EyeColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChestHairColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "ChestHairColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BlushColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "BlushColor");

            migrationBuilder.RenameColumn(
                name: "SyncedData_IsMale",
                table: "PlayerCharGeneralDatas",
                newName: "IsMale");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NoseWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "NoseWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NoseTip",
                table: "PlayerCharFeaturesDatas",
                newName: "NoseTip");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NoseLength",
                table: "PlayerCharFeaturesDatas",
                newName: "NoseLength");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NoseHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "NoseHeight");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NoseBridgeShift",
                table: "PlayerCharFeaturesDatas",
                newName: "NoseBridgeShift");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NoseBridge",
                table: "PlayerCharFeaturesDatas",
                newName: "NoseBridge");

            migrationBuilder.RenameColumn(
                name: "SyncedData_NeckWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "NeckWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Lips",
                table: "PlayerCharFeaturesDatas",
                newName: "Lips");

            migrationBuilder.RenameColumn(
                name: "SyncedData_JawWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "JawWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_JawHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "JawHeight");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Eyes",
                table: "PlayerCharFeaturesDatas",
                newName: "Eyes");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChinWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "ChinWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChinShape",
                table: "PlayerCharFeaturesDatas",
                newName: "ChinShape");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChinPosition",
                table: "PlayerCharFeaturesDatas",
                newName: "ChinPosition");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChinLength",
                table: "PlayerCharFeaturesDatas",
                newName: "ChinLength");

            migrationBuilder.RenameColumn(
                name: "SyncedData_CheeksWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "CheeksWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_CheekboneWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "CheekboneWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_CheekboneHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "CheekboneHeight");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BrowWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "BrowWidth");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BrowHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "BrowHeight");

            migrationBuilder.RenameColumn(
                name: "SyncedData_SunDamageOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SunDamageOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_SunDamage",
                table: "PlayerCharAppearanceDatas",
                newName: "SunDamage");

            migrationBuilder.RenameColumn(
                name: "SyncedData_MolesAndFrecklesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "MolesAndFrecklesOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_MolesAndFreckles",
                table: "PlayerCharAppearanceDatas",
                newName: "MolesAndFreckles");

            migrationBuilder.RenameColumn(
                name: "SyncedData_MakeupOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "MakeupOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Makeup",
                table: "PlayerCharAppearanceDatas",
                newName: "Makeup");

            migrationBuilder.RenameColumn(
                name: "SyncedData_LipstickOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "LipstickOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Lipstick",
                table: "PlayerCharAppearanceDatas",
                newName: "Lipstick");

            migrationBuilder.RenameColumn(
                name: "SyncedData_FacialHairOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "FacialHairOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_FacialHair",
                table: "PlayerCharAppearanceDatas",
                newName: "FacialHair");

            migrationBuilder.RenameColumn(
                name: "SyncedData_EyebrowsOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "EyebrowsOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Eyebrows",
                table: "PlayerCharAppearanceDatas",
                newName: "Eyebrows");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ComplexionOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "ComplexionOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Complexion",
                table: "PlayerCharAppearanceDatas",
                newName: "Complexion");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChestHairOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "ChestHairOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_ChestHair",
                table: "PlayerCharAppearanceDatas",
                newName: "ChestHair");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BodyBlemishesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "BodyBlemishesOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BodyBlemishes",
                table: "PlayerCharAppearanceDatas",
                newName: "BodyBlemishes");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BlushOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "BlushOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Blush",
                table: "PlayerCharAppearanceDatas",
                newName: "Blush");

            migrationBuilder.RenameColumn(
                name: "SyncedData_BlemishesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "BlemishesOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Blemishes",
                table: "PlayerCharAppearanceDatas",
                newName: "Blemishes");

            migrationBuilder.RenameColumn(
                name: "SyncedData_AgeingOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "AgeingOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_Ageing",
                table: "PlayerCharAppearanceDatas",
                newName: "Ageing");

            migrationBuilder.RenameColumn(
                name: "SyncedData_AddBodyBlemishesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "AddBodyBlemishesOpacity");

            migrationBuilder.RenameColumn(
                name: "SyncedData_AddBodyBlemishes",
                table: "PlayerCharAppearanceDatas",
                newName: "AddBodyBlemishes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkinTonePercentage",
                table: "PlayerCharHeritageDatas",
                newName: "SyncedData_SkinTonePercentage");

            migrationBuilder.RenameColumn(
                name: "ResemblancePercentage",
                table: "PlayerCharHeritageDatas",
                newName: "SyncedData_ResemblancePercentage");

            migrationBuilder.RenameColumn(
                name: "MotherIndex",
                table: "PlayerCharHeritageDatas",
                newName: "SyncedData_MotherIndex");

            migrationBuilder.RenameColumn(
                name: "FatherIndex",
                table: "PlayerCharHeritageDatas",
                newName: "SyncedData_FatherIndex");

            migrationBuilder.RenameColumn(
                name: "LipstickColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_LipstickColor");

            migrationBuilder.RenameColumn(
                name: "HairHighlightColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_HairHighlightColor");

            migrationBuilder.RenameColumn(
                name: "HairColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_HairColor");

            migrationBuilder.RenameColumn(
                name: "Hair",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_Hair");

            migrationBuilder.RenameColumn(
                name: "FacialHairColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_FacialHairColor");

            migrationBuilder.RenameColumn(
                name: "EyebrowColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_EyebrowColor");

            migrationBuilder.RenameColumn(
                name: "EyeColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_EyeColor");

            migrationBuilder.RenameColumn(
                name: "ChestHairColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_ChestHairColor");

            migrationBuilder.RenameColumn(
                name: "BlushColor",
                table: "PlayerCharHairAndColorsDatas",
                newName: "SyncedData_BlushColor");

            migrationBuilder.RenameColumn(
                name: "IsMale",
                table: "PlayerCharGeneralDatas",
                newName: "SyncedData_IsMale");

            migrationBuilder.RenameColumn(
                name: "NoseWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NoseWidth");

            migrationBuilder.RenameColumn(
                name: "NoseTip",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NoseTip");

            migrationBuilder.RenameColumn(
                name: "NoseLength",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NoseLength");

            migrationBuilder.RenameColumn(
                name: "NoseHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NoseHeight");

            migrationBuilder.RenameColumn(
                name: "NoseBridgeShift",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NoseBridgeShift");

            migrationBuilder.RenameColumn(
                name: "NoseBridge",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NoseBridge");

            migrationBuilder.RenameColumn(
                name: "NeckWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_NeckWidth");

            migrationBuilder.RenameColumn(
                name: "Lips",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_Lips");

            migrationBuilder.RenameColumn(
                name: "JawWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_JawWidth");

            migrationBuilder.RenameColumn(
                name: "JawHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_JawHeight");

            migrationBuilder.RenameColumn(
                name: "Eyes",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_Eyes");

            migrationBuilder.RenameColumn(
                name: "ChinWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_ChinWidth");

            migrationBuilder.RenameColumn(
                name: "ChinShape",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_ChinShape");

            migrationBuilder.RenameColumn(
                name: "ChinPosition",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_ChinPosition");

            migrationBuilder.RenameColumn(
                name: "ChinLength",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_ChinLength");

            migrationBuilder.RenameColumn(
                name: "CheeksWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_CheeksWidth");

            migrationBuilder.RenameColumn(
                name: "CheekboneWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_CheekboneWidth");

            migrationBuilder.RenameColumn(
                name: "CheekboneHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_CheekboneHeight");

            migrationBuilder.RenameColumn(
                name: "BrowWidth",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_BrowWidth");

            migrationBuilder.RenameColumn(
                name: "BrowHeight",
                table: "PlayerCharFeaturesDatas",
                newName: "SyncedData_BrowHeight");

            migrationBuilder.RenameColumn(
                name: "SunDamageOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_SunDamageOpacity");

            migrationBuilder.RenameColumn(
                name: "SunDamage",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_SunDamage");

            migrationBuilder.RenameColumn(
                name: "MolesAndFrecklesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_MolesAndFrecklesOpacity");

            migrationBuilder.RenameColumn(
                name: "MolesAndFreckles",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_MolesAndFreckles");

            migrationBuilder.RenameColumn(
                name: "MakeupOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_MakeupOpacity");

            migrationBuilder.RenameColumn(
                name: "Makeup",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Makeup");

            migrationBuilder.RenameColumn(
                name: "LipstickOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_LipstickOpacity");

            migrationBuilder.RenameColumn(
                name: "Lipstick",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Lipstick");

            migrationBuilder.RenameColumn(
                name: "FacialHairOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_FacialHairOpacity");

            migrationBuilder.RenameColumn(
                name: "FacialHair",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_FacialHair");

            migrationBuilder.RenameColumn(
                name: "EyebrowsOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_EyebrowsOpacity");

            migrationBuilder.RenameColumn(
                name: "Eyebrows",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Eyebrows");

            migrationBuilder.RenameColumn(
                name: "ComplexionOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_ComplexionOpacity");

            migrationBuilder.RenameColumn(
                name: "Complexion",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Complexion");

            migrationBuilder.RenameColumn(
                name: "ChestHairOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_ChestHairOpacity");

            migrationBuilder.RenameColumn(
                name: "ChestHair",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_ChestHair");

            migrationBuilder.RenameColumn(
                name: "BodyBlemishesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_BodyBlemishesOpacity");

            migrationBuilder.RenameColumn(
                name: "BodyBlemishes",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_BodyBlemishes");

            migrationBuilder.RenameColumn(
                name: "BlushOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_BlushOpacity");

            migrationBuilder.RenameColumn(
                name: "Blush",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Blush");

            migrationBuilder.RenameColumn(
                name: "BlemishesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_BlemishesOpacity");

            migrationBuilder.RenameColumn(
                name: "Blemishes",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Blemishes");

            migrationBuilder.RenameColumn(
                name: "AgeingOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_AgeingOpacity");

            migrationBuilder.RenameColumn(
                name: "Ageing",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_Ageing");

            migrationBuilder.RenameColumn(
                name: "AddBodyBlemishesOpacity",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_AddBodyBlemishesOpacity");

            migrationBuilder.RenameColumn(
                name: "AddBodyBlemishes",
                table: "PlayerCharAppearanceDatas",
                newName: "SyncedData_AddBodyBlemishes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Server.Database.Migrations
{
    public partial class CharCreatorClothes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_Accessor~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_BagPlaye~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_BodyArmo~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_Bracelet~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_DecalPla~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_EarAcces~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_GlassesP~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_HandsPla~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_HatPlaye~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_JacketPl~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_LegsPlay~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_MaskPlay~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_ShirtPla~",
                table: "PlayerClothesData");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_ShoesPla~",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_AccessoryPlayerId_AccessorySlot_Accessory~",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_BagPlayerId_BagSlot_BagKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_BodyArmorPlayerId_BodyArmorSlot_BodyArmor~",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_BraceletPlayerId_BraceletSlot_BraceletKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_DecalPlayerId_DecalSlot_DecalKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_EarAccessoryPlayerId_EarAccessorySlot_Ear~",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_GlassesPlayerId_GlassesSlot_GlassesKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_HandsPlayerId_HandsSlot_HandsKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_HatPlayerId_HatSlot_HatKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_JacketPlayerId_JacketSlot_JacketKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_LegsPlayerId_LegsSlot_LegsKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_MaskPlayerId_MaskSlot_MaskKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_ShirtPlayerId_ShirtSlot_ShirtKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesData_ShoesPlayerId_ShoesSlot_ShoesKey",
                table: "PlayerClothesData");

            migrationBuilder.DropIndex(
                name: "IX_PlayerClothesComponentOrPropData_PlayerId_Slot",
                table: "PlayerClothesComponentOrPropData");

            migrationBuilder.DropColumn(
                name: "AccessoryKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "AccessoryPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "AccessorySlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BagKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BagPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BagSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BodyArmorKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BodyArmorPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BodyArmorSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BraceletKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BraceletPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "BraceletSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "DecalKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "DecalPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "DecalSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "EarAccessoryKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "EarAccessoryPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "EarAccessorySlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "GlassesKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "GlassesPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "GlassesSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "HandsKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "HandsPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "HandsSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "HatKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "HatPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "HatSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "JacketKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "JacketPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "JacketSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "LegsKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "LegsPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "LegsSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "MaskKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "MaskPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "MaskSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "ShirtKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "ShirtPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "ShirtSlot",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "ShoesKey",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "ShoesPlayerId",
                table: "PlayerClothesData");

            migrationBuilder.DropColumn(
                name: "ShoesSlot",
                table: "PlayerClothesData");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData",
                columns: new[] { "PlayerId", "Slot" },
                principalTable: "PlayerClothesData",
                principalColumns: new[] { "PlayerId", "Slot" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData");

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "AccessoryKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccessoryPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "AccessorySlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "BagKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BagPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "BagSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "BodyArmorKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodyArmorPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "BodyArmorSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "BraceletKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BraceletPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "BraceletSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "DecalKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DecalPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "DecalSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "EarAccessoryKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EarAccessoryPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "EarAccessorySlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "GlassesKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GlassesPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "GlassesSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "HandsKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HandsPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "HandsSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "HatKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HatPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "HatSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "JacketKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JacketPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "JacketSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "LegsKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LegsPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "LegsSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "MaskKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaskPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MaskSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "ShirtKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShirtPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ShirtSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<ClothesDataKey>(
                name: "ShoesKey",
                table: "PlayerClothesData",
                type: "clothes_data_key",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoesPlayerId",
                table: "PlayerClothesData",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ShoesSlot",
                table: "PlayerClothesData",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_AccessoryPlayerId_AccessorySlot_Accessory~",
                table: "PlayerClothesData",
                columns: new[] { "AccessoryPlayerId", "AccessorySlot", "AccessoryKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_BagPlayerId_BagSlot_BagKey",
                table: "PlayerClothesData",
                columns: new[] { "BagPlayerId", "BagSlot", "BagKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_BodyArmorPlayerId_BodyArmorSlot_BodyArmor~",
                table: "PlayerClothesData",
                columns: new[] { "BodyArmorPlayerId", "BodyArmorSlot", "BodyArmorKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_BraceletPlayerId_BraceletSlot_BraceletKey",
                table: "PlayerClothesData",
                columns: new[] { "BraceletPlayerId", "BraceletSlot", "BraceletKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_DecalPlayerId_DecalSlot_DecalKey",
                table: "PlayerClothesData",
                columns: new[] { "DecalPlayerId", "DecalSlot", "DecalKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_EarAccessoryPlayerId_EarAccessorySlot_Ear~",
                table: "PlayerClothesData",
                columns: new[] { "EarAccessoryPlayerId", "EarAccessorySlot", "EarAccessoryKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_GlassesPlayerId_GlassesSlot_GlassesKey",
                table: "PlayerClothesData",
                columns: new[] { "GlassesPlayerId", "GlassesSlot", "GlassesKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_HandsPlayerId_HandsSlot_HandsKey",
                table: "PlayerClothesData",
                columns: new[] { "HandsPlayerId", "HandsSlot", "HandsKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_HatPlayerId_HatSlot_HatKey",
                table: "PlayerClothesData",
                columns: new[] { "HatPlayerId", "HatSlot", "HatKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_JacketPlayerId_JacketSlot_JacketKey",
                table: "PlayerClothesData",
                columns: new[] { "JacketPlayerId", "JacketSlot", "JacketKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_LegsPlayerId_LegsSlot_LegsKey",
                table: "PlayerClothesData",
                columns: new[] { "LegsPlayerId", "LegsSlot", "LegsKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_MaskPlayerId_MaskSlot_MaskKey",
                table: "PlayerClothesData",
                columns: new[] { "MaskPlayerId", "MaskSlot", "MaskKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_ShirtPlayerId_ShirtSlot_ShirtKey",
                table: "PlayerClothesData",
                columns: new[] { "ShirtPlayerId", "ShirtSlot", "ShirtKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesData_ShoesPlayerId_ShoesSlot_ShoesKey",
                table: "PlayerClothesData",
                columns: new[] { "ShoesPlayerId", "ShoesSlot", "ShoesKey" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesComponentOrPropData_PlayerId_Slot",
                table: "PlayerClothesComponentOrPropData",
                columns: new[] { "PlayerId", "Slot" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData",
                columns: new[] { "PlayerId", "Slot" },
                principalTable: "PlayerClothesData",
                principalColumns: new[] { "PlayerId", "Slot" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_Accessor~",
                table: "PlayerClothesData",
                columns: new[] { "AccessoryPlayerId", "AccessorySlot", "AccessoryKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_BagPlaye~",
                table: "PlayerClothesData",
                columns: new[] { "BagPlayerId", "BagSlot", "BagKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_BodyArmo~",
                table: "PlayerClothesData",
                columns: new[] { "BodyArmorPlayerId", "BodyArmorSlot", "BodyArmorKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_Bracelet~",
                table: "PlayerClothesData",
                columns: new[] { "BraceletPlayerId", "BraceletSlot", "BraceletKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_DecalPla~",
                table: "PlayerClothesData",
                columns: new[] { "DecalPlayerId", "DecalSlot", "DecalKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_EarAcces~",
                table: "PlayerClothesData",
                columns: new[] { "EarAccessoryPlayerId", "EarAccessorySlot", "EarAccessoryKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_GlassesP~",
                table: "PlayerClothesData",
                columns: new[] { "GlassesPlayerId", "GlassesSlot", "GlassesKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_HandsPla~",
                table: "PlayerClothesData",
                columns: new[] { "HandsPlayerId", "HandsSlot", "HandsKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_HatPlaye~",
                table: "PlayerClothesData",
                columns: new[] { "HatPlayerId", "HatSlot", "HatKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_JacketPl~",
                table: "PlayerClothesData",
                columns: new[] { "JacketPlayerId", "JacketSlot", "JacketKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_LegsPlay~",
                table: "PlayerClothesData",
                columns: new[] { "LegsPlayerId", "LegsSlot", "LegsKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_MaskPlay~",
                table: "PlayerClothesData",
                columns: new[] { "MaskPlayerId", "MaskSlot", "MaskKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_ShirtPla~",
                table: "PlayerClothesData",
                columns: new[] { "ShirtPlayerId", "ShirtSlot", "ShirtKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerClothesData_PlayerClothesComponentOrPropData_ShoesPla~",
                table: "PlayerClothesData",
                columns: new[] { "ShoesPlayerId", "ShoesSlot", "ShoesKey" },
                principalTable: "PlayerClothesComponentOrPropData",
                principalColumns: new[] { "PlayerId", "Slot", "Key" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
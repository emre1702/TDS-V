using Microsoft.EntityFrameworkCore.Migrations;
using TDS.Shared.Data.Enums.CharCreator;

namespace TDS.Server.Database.Migrations
{
    public partial class CharCreatorClothes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerClothes");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:clothes_data_key", "main,hats,glasses,masks,jackets,shirts,hands,accessories,bags,legs,shoes,body_armors,decals,ear_accessories,watches,bracelets,slot");

            migrationBuilder.CreateTable(
                name: "PlayerClothesDatas",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    SelectedSlot = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClothesDatas", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerClothesDatas_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerClothesData",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    Slot = table.Column<byte>(type: "smallint", nullable: false),
                    HatPlayerId = table.Column<int>(type: "integer", nullable: true),
                    HatSlot = table.Column<byte>(type: "smallint", nullable: true),
                    HatKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    GlassesPlayerId = table.Column<int>(type: "integer", nullable: true),
                    GlassesSlot = table.Column<byte>(type: "smallint", nullable: true),
                    GlassesKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    MaskPlayerId = table.Column<int>(type: "integer", nullable: true),
                    MaskSlot = table.Column<byte>(type: "smallint", nullable: true),
                    MaskKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    JacketPlayerId = table.Column<int>(type: "integer", nullable: true),
                    JacketSlot = table.Column<byte>(type: "smallint", nullable: true),
                    JacketKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    ShirtPlayerId = table.Column<int>(type: "integer", nullable: true),
                    ShirtSlot = table.Column<byte>(type: "smallint", nullable: true),
                    ShirtKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    HandsPlayerId = table.Column<int>(type: "integer", nullable: true),
                    HandsSlot = table.Column<byte>(type: "smallint", nullable: true),
                    HandsKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    AccessoryPlayerId = table.Column<int>(type: "integer", nullable: true),
                    AccessorySlot = table.Column<byte>(type: "smallint", nullable: true),
                    AccessoryKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    BagPlayerId = table.Column<int>(type: "integer", nullable: true),
                    BagSlot = table.Column<byte>(type: "smallint", nullable: true),
                    BagKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    LegsPlayerId = table.Column<int>(type: "integer", nullable: true),
                    LegsSlot = table.Column<byte>(type: "smallint", nullable: true),
                    LegsKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    ShoesPlayerId = table.Column<int>(type: "integer", nullable: true),
                    ShoesSlot = table.Column<byte>(type: "smallint", nullable: true),
                    ShoesKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    BodyArmorPlayerId = table.Column<int>(type: "integer", nullable: true),
                    BodyArmorSlot = table.Column<byte>(type: "smallint", nullable: true),
                    BodyArmorKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    DecalPlayerId = table.Column<int>(type: "integer", nullable: true),
                    DecalSlot = table.Column<byte>(type: "smallint", nullable: true),
                    DecalKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    EarAccessoryPlayerId = table.Column<int>(type: "integer", nullable: true),
                    EarAccessorySlot = table.Column<byte>(type: "smallint", nullable: true),
                    EarAccessoryKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true),
                    BraceletPlayerId = table.Column<int>(type: "integer", nullable: true),
                    BraceletSlot = table.Column<byte>(type: "smallint", nullable: true),
                    BraceletKey = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClothesData", x => new { x.PlayerId, x.Slot });
                    table.ForeignKey(
                        name: "FK_PlayerClothesData_PlayerClothesDatas_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "PlayerClothesDatas",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerClothesComponentOrPropData",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    Slot = table.Column<byte>(type: "smallint", nullable: false),
                    Key = table.Column<ClothesDataKey>(type: "clothes_data_key", nullable: false),
                    DrawableId = table.Column<int>(type: "integer", nullable: false),
                    TextureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClothesComponentOrPropData", x => new { x.PlayerId, x.Slot, x.Key });
                    table.ForeignKey(
                        name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                        columns: x => new { x.PlayerId, x.Slot },
                        principalTable: "PlayerClothesData",
                        principalColumns: new[] { "PlayerId", "Slot" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClothesComponentOrPropData_PlayerId_Slot",
                table: "PlayerClothesComponentOrPropData",
                columns: new[] { "PlayerId", "Slot" },
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerClothesComponentOrPropData_PlayerClothesData_PlayerId~",
                table: "PlayerClothesComponentOrPropData");

            migrationBuilder.DropTable(
                name: "PlayerClothesData");

            migrationBuilder.DropTable(
                name: "PlayerClothesComponentOrPropData");

            migrationBuilder.DropTable(
                name: "PlayerClothesDatas");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:clothes_data_key", "main,hats,glasses,masks,jackets,shirts,hands,accessories,bags,legs,shoes,body_armors,decals,ear_accessories,watches,bracelets,slot");

            migrationBuilder.CreateTable(
                name: "PlayerClothes",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClothes", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerClothes_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
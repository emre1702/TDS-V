using RAGE.Elements;
using System.Collections.Generic;
using System.Linq;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Data.Models.CharCreator;

namespace TDS.Client.Handler.Appearance.CharCreator.Clothes
{
    internal static class ClothesPedBaseHelper
    {
        private static readonly Dictionary<ClothesDataKey, int> _componentIdByDataKey = new Dictionary<ClothesDataKey, int>
        {
            // 0 => Has nothing, is empty
            [ClothesDataKey.Masks] = 1,
            // 2 => Hairs, handled in body
            [ClothesDataKey.Hands] = 3,
            [ClothesDataKey.Legs] = 4,
            [ClothesDataKey.Bags] = 5,
            [ClothesDataKey.Shoes] = 6,
            [ClothesDataKey.Accessories] = 7,
            [ClothesDataKey.Shirts] = 8,
            [ClothesDataKey.BodyArmors] = 9,
            [ClothesDataKey.Decals] = 10,
            [ClothesDataKey.Jackets] = 11,
        };

        private static readonly Dictionary<ClothesDataKey, int> _propIdByDataKey = new Dictionary<ClothesDataKey, int>
        {
            [ClothesDataKey.Hats] = 0,
            [ClothesDataKey.Glasses] = 1,
            [ClothesDataKey.EarAccessories] = 2,
            [ClothesDataKey.Watches] = 6,
            [ClothesDataKey.Bracelets] = 7
        };

        internal static void SetClothesData(this PedBase ped, ClothesConfigs configs)
        {
            var data = configs.DatasPerSlot.First(d => d.Slot == configs.SelectedSlot);

            ped.SetComponentVariation(1, data.Mask.DrawableId, data.Mask.TextureId, ped.GetPaletteVariation(1));
            ped.SetComponentVariation(3, data.Hands.DrawableId, data.Hands.TextureId, ped.GetPaletteVariation(3));
            ped.SetComponentVariation(4, data.Legs.DrawableId, data.Legs.TextureId, ped.GetPaletteVariation(4));
            ped.SetComponentVariation(5, data.Bag.DrawableId, data.Bag.TextureId, ped.GetPaletteVariation(5));
            ped.SetComponentVariation(6, data.Shoes.DrawableId, data.Shoes.TextureId, ped.GetPaletteVariation(6));
            ped.SetComponentVariation(7, data.Accessory.DrawableId, data.Accessory.TextureId, ped.GetPaletteVariation(7));
            ped.SetComponentVariation(8, data.Shirt.DrawableId, data.Shirt.TextureId, ped.GetPaletteVariation(8));
            ped.SetComponentVariation(9, data.BodyArmor.DrawableId, data.BodyArmor.TextureId, ped.GetPaletteVariation(9));
            ped.SetComponentVariation(10, data.Decal.DrawableId, data.Decal.TextureId, ped.GetPaletteVariation(10));
            ped.SetComponentVariation(11, data.Jacket.DrawableId, data.Jacket.TextureId, ped.GetPaletteVariation(11));

            ped.SetPropIndex(0, data.Hat.DrawableId, data.Hat.TextureId, true);
            ped.SetPropIndex(1, data.Glasses.DrawableId, data.Glasses.TextureId, true);
            ped.SetPropIndex(2, data.EarAccessory.DrawableId, data.EarAccessory.TextureId, true);
            ped.SetPropIndex(6, data.Watch.DrawableId, data.Watch.TextureId, true);
            ped.SetPropIndex(7, data.Bracelet.DrawableId, data.Bracelet.TextureId, true);
        }

        internal static bool TryGetComponentId(this ClothesDataKey key, out int componentId)
            => _componentIdByDataKey.TryGetValue(key, out componentId);

        internal static bool TryGetPropId(this ClothesDataKey key, out int propId)
            => _propIdByDataKey.TryGetValue(key, out propId);
    }
}
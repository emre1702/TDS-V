using RAGE.Elements;
using System.Collections.Generic;
using System.Linq;
using TDS.Shared.Data.Enums.CharCreator;
using TDS.Shared.Data.Models.CharCreator;
using TDS.Shared.Data.Models.CharCreator.Clothes;

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

            ped.SetComponent(1, data.Mask);
            ped.SetComponent(3, data.Hands);
            ped.SetComponent(4, data.Legs);
            ped.SetComponent(5, data.Bag);
            ped.SetComponent(6, data.Shoes);
            ped.SetComponent(7, data.Accessory);
            ped.SetComponent(8, data.Shirt);
            ped.SetComponent(9, data.BodyArmor);
            ped.SetComponent(10, data.Decal);
            ped.SetComponent(11, data.Jacket);

            ped.SetProp(0, data.Hat);
            ped.SetProp(1, data.Glasses);
            ped.SetProp(2, data.EarAccessory);
            ped.SetProp(6, data.Watch);
            ped.SetProp(7, data.Bracelet);
        }

        internal static bool TryGetComponentId(this ClothesDataKey key, out int componentId)
            => _componentIdByDataKey.TryGetValue(key, out componentId);

        internal static bool TryGetPropId(this ClothesDataKey key, out int propId)
            => _propIdByDataKey.TryGetValue(key, out propId);

        internal static void SetComponent(this PedBase ped, int componentId, int drawableId, int textureId)
        {
            var paletteId = ped.GetPaletteVariation(componentId);
            if (textureId != -1)
                ped.SetComponentVariation(componentId, drawableId, textureId, paletteId);
            else
                ped.SetComponentVariation(componentId, 0, -1, paletteId);
        }

        internal static void SetComponent(this PedBase ped, int componentId, ClothesComponentOrPropData data)
        {
            SetComponent(ped, componentId, data.DrawableId, data.TextureId);
        }

        internal static void SetProp(this PedBase ped, int propId, int drawableId, int textureId)
        {
            if (textureId != -1)
                ped.SetPropIndex(propId, drawableId, textureId, true);
            else
                ped.ClearProp(propId);
        }

        internal static void SetProp(this PedBase ped, int propId, ClothesComponentOrPropData data)
        {
            SetProp(ped, propId, data.DrawableId, data.TextureId);
        }
    }
}
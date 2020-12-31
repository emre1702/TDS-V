using RAGE.Elements;
using System.Collections.Generic;
using TDS.Shared.Data.Enums.CharCreator;
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

        internal static void SetClothesData(this PedBase ped, ClothesData data)
        {
            /*var generalData = data.GeneralDataSynced.First(e => e.Slot == data.Slot);
            var heritageData = data.HeritageDataSynced.First(e => e.Slot == data.Slot);
            var featuresData = data.FeaturesDataSynced.First(e => e.Slot == data.Slot);
            var appearanceData = data.AppearanceDataSynced.First(e => e.Slot == data.Slot);
            var hairAndColorsData = data.HairAndColorsDataSynced.First(e => e.Slot == data.Slot);*/
        }

        internal static bool TryGetComponentId(this ClothesDataKey key, out int componentId)
            => _componentIdByDataKey.TryGetValue(key, out componentId);

        internal static bool TryGetPropId(this ClothesDataKey key, out int propId)
            => _propIdByDataKey.TryGetValue(key, out propId);
    }
}
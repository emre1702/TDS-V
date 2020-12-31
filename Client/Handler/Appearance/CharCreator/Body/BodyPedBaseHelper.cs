using RAGE.Elements;
using System.Linq;
using TDS.Shared.Data.Models.CharCreator.Body;

namespace TDS.Client.Handler.Appearance.CharCreator.Body
{
    internal static class BodyPedBaseHelper
    {
        internal static void SetBodyData(this PedBase ped, BodyData data)
        {
            var generalData = data.GeneralDataSynced.First(e => e.Slot == data.Slot);
            var heritageData = data.HeritageDataSynced.First(e => e.Slot == data.Slot);
            var featuresData = data.FeaturesDataSynced.First(e => e.Slot == data.Slot);
            var appearanceData = data.AppearanceDataSynced.First(e => e.Slot == data.Slot);
            var hairAndColorsData = data.HairAndColorsDataSynced.First(e => e.Slot == data.Slot);

            UpdateHeritage(ped, heritageData);

            UpdateFaceFeature(ped, 0, featuresData.NoseWidth);
            UpdateFaceFeature(ped, 1, featuresData.NoseHeight);
            UpdateFaceFeature(ped, 2, featuresData.NoseLength);
            UpdateFaceFeature(ped, 3, featuresData.NoseBridge);
            UpdateFaceFeature(ped, 4, featuresData.NoseTip);
            UpdateFaceFeature(ped, 5, featuresData.NoseBridgeShift);
            UpdateFaceFeature(ped, 6, featuresData.BrowHeight);
            UpdateFaceFeature(ped, 7, featuresData.BrowWidth);
            UpdateFaceFeature(ped, 8, featuresData.CheekboneHeight);
            UpdateFaceFeature(ped, 9, featuresData.CheekboneWidth);
            UpdateFaceFeature(ped, 10, featuresData.CheeksWidth);
            UpdateFaceFeature(ped, 11, featuresData.Eyes);
            UpdateFaceFeature(ped, 12, featuresData.Lips);
            UpdateFaceFeature(ped, 13, featuresData.JawWidth);
            UpdateFaceFeature(ped, 14, featuresData.JawHeight);
            UpdateFaceFeature(ped, 15, featuresData.ChinLength);
            UpdateFaceFeature(ped, 16, featuresData.ChinPosition);
            UpdateFaceFeature(ped, 17, featuresData.ChinWidth);
            UpdateFaceFeature(ped, 18, featuresData.ChinShape);
            UpdateFaceFeature(ped, 19, featuresData.NeckWidth);

            UpdateAppearance(ped, 0, appearanceData.Blemishes, appearanceData.BlemishesOpacity);
            UpdateAppearance(ped, 1, appearanceData.FacialHair, appearanceData.FacialHairOpacity);
            UpdateAppearance(ped, 2, appearanceData.Eyebrows, appearanceData.EyebrowsOpacity);
            UpdateAppearance(ped, 3, appearanceData.Ageing, appearanceData.AgeingOpacity);
            UpdateAppearance(ped, 4, appearanceData.Makeup, appearanceData.MakeupOpacity);
            UpdateAppearance(ped, 5, appearanceData.Blush, appearanceData.BlushOpacity);
            UpdateAppearance(ped, 6, appearanceData.Complexion, appearanceData.ComplexionOpacity);
            UpdateAppearance(ped, 7, appearanceData.SunDamage, appearanceData.SunDamageOpacity);
            UpdateAppearance(ped, 8, appearanceData.Lipstick, appearanceData.LipstickOpacity);
            UpdateAppearance(ped, 9, appearanceData.MolesAndFreckles, appearanceData.MolesAndFrecklesOpacity);
            UpdateAppearance(ped, 10, appearanceData.ChestHair, appearanceData.ChestHairOpacity);
            UpdateAppearance(ped, 11, appearanceData.BodyBlemishes, appearanceData.BodyBlemishesOpacity);
            UpdateAppearance(ped, 12, appearanceData.AddBodyBlemishes, appearanceData.AddBodyBlemishesOpacity);

            UpdateHair(ped, hairAndColorsData.Hair);
            UpdateHairColor(ped, hairAndColorsData.HairColor, hairAndColorsData.HairHighlightColor);
            UpdateEyeColor(ped, hairAndColorsData.EyeColor);
            UpdateColor(ped, 1, 1, hairAndColorsData.FacialHairColor);
            UpdateColor(ped, 2, 1, hairAndColorsData.EyebrowColor);
            UpdateColor(ped, 5, 2, hairAndColorsData.BlushColor);
            UpdateColor(ped, 8, 2, hairAndColorsData.LipstickColor);
            UpdateColor(ped, 10, 1, hairAndColorsData.ChestHairColor);
        }

        internal static void UpdateAppearance(this PedBase ped, int overlayId, int index, float opacity)
        {
            index = index == 0 ? 255 : index - 1;
            ped.SetHeadOverlay(overlayId, index, opacity);
        }

        internal static void UpdateColor(this PedBase ped, int overlayId, int colorType, int colorId)
        {
            ped.SetHeadOverlayColor(overlayId, colorType, colorId, 0);
        }

        internal static void UpdateEyeColor(this PedBase ped, int index)
        {
            ped.SetEyeColor(index);
        }

        internal static void UpdateFaceFeature(this PedBase ped, int index, float scale)
        {
            ped.SetFaceFeature(index, scale);
        }

        internal static void UpdateHair(this PedBase ped, int id)
        {
            var paletteId = ped.GetPaletteVariation(2);
            ped.SetComponentVariation(2, id, 0, paletteId);
        }

        internal static void UpdateHairColor(this PedBase ped, int hairColor, int hairHighlightColor)
        {
            ped.SetHairColor(hairColor, hairHighlightColor);
        }

        internal static void UpdateHeritage(this PedBase ped, BodyHeritageData data)
        {
            ped.SetHeadBlendData(
                data.MotherIndex, data.FatherIndex, 0,
                data.MotherIndex, data.FatherIndex, 0,
                data.ResemblancePercentage, data.SkinTonePercentage, 0,
                false);
        }
    }
}
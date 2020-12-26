using System;
using System.Linq;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.CharCreator.Body;

namespace TDS.Client.Handler.Appearance.CharCreator.Body
{
    internal class BodyDataHandler
    {
        public BodyData Data { get; private set; }

        public BodyDataHandler(BrowserHandler browserHandler)
        {
            RAGE.Events.Add(FromBrowserEvent.LoadBodyData, (_) =>
            {
                browserHandler.Angular.ExecuteFast(FromBrowserEvent.LoadBodyData, Serializer.ToBrowser(Data));
            });
        }

        public void Start(BodyData data)
        {
            Data = data;
        }

        public void Stop()
        {
            Data = null;
        }

        public void DataChanged(CharCreatorDataKey key, ref ArraySegment<object> args)
        {
            switch (key)
            {
                case CharCreatorDataKey.All:
                    Data = Serializer.FromBrowser<BodyData>((string)args[0]);
                    break;

                case CharCreatorDataKey.Slot:
                    Data.Slot = Convert.ToByte(args[0]);
                    break;

                case CharCreatorDataKey.IsMale:
                    var generalData = Data.GeneralDataSynced.First(d => d.Slot == Data.Slot);
                    generalData.IsMale = Convert.ToBoolean(args[0]);
                    break;

                case CharCreatorDataKey.Heritage:
                    var newHeritageData = Serializer.FromBrowser<BodyHeritageData>((string)args[0]);
                    var oldHeritageData = Data.HeritageDataSynced.First(d => d.Slot == Data.Slot);
                    UseNewHeritageData(oldHeritageData, newHeritageData);
                    break;

                case CharCreatorDataKey.Feature:
                    var featureData = Data.FeaturesDataSynced.First(d => d.Slot == Data.Slot);
                    UseNewFeatureData(featureData, Convert.ToInt32(args[0]), Convert.ToSingle(args[1]));
                    break;

                case CharCreatorDataKey.Appearance:
                    var appearanceData = Data.AppearanceDataSynced.First(d => d.Slot == Data.Slot);
                    UseNewAppearanceData(appearanceData, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToSingle(args[2]));
                    break;

                case CharCreatorDataKey.Hair:
                    var hairAndColorsData1 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData1.Hair = Convert.ToInt32(args[0]);
                    break;

                case CharCreatorDataKey.HairColor:
                    var hairAndColorsData2 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData2.HairColor = Convert.ToInt32(args[0]);
                    hairAndColorsData2.HairHighlightColor = Convert.ToInt32(args[1]);
                    break;

                case CharCreatorDataKey.EyeColor:
                    var hairAndColorsData3 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData3.EyeColor = Convert.ToInt32(args[0]);
                    break;

                case CharCreatorDataKey.FacialHairColor:
                    var hairAndColorsData4 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData4.FacialHairColor = Convert.ToInt32(args[0]);
                    break;

                case CharCreatorDataKey.EyebrowColor:
                    var hairAndColorsData5 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData5.EyebrowColor = Convert.ToInt32(args[0]);
                    break;

                case CharCreatorDataKey.BlushColor:
                    var hairAndColorsData6 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData6.BlushColor = Convert.ToInt32(args[0]);
                    break;

                case CharCreatorDataKey.LipstickColor:
                    var hairAndColorsData7 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData7.LipstickColor = Convert.ToInt32(args[0]);
                    break;

                case CharCreatorDataKey.ChestHairColor:
                    var hairAndColorsData8 = Data.HairAndColorsDataSynced.First(d => d.Slot == Data.Slot);
                    hairAndColorsData8.ChestHairColor = Convert.ToInt32(args[0]);
                    break;
            }
        }

        private void UseNewHeritageData(BodyHeritageData oldData, BodyHeritageData newData)
        {
            oldData.FatherIndex = newData.FatherIndex;
            oldData.MotherIndex = newData.MotherIndex;
            oldData.ResemblancePercentage = newData.ResemblancePercentage;
            oldData.SkinTonePercentage = newData.SkinTonePercentage;
        }

        private void UseNewFeatureData(BodyFeaturesData data, int index, float value)
        {
            switch (index)
            {
                case 0: data.NoseWidth = value; break;
                case 1: data.NoseHeight = value; break;
                case 2: data.NoseLength = value; break;
                case 3: data.NoseBridge = value; break;
                case 4: data.NoseTip = value; break;
                case 5: data.NoseBridgeShift = value; break;
                case 6: data.BrowHeight = value; break;
                case 7: data.BrowWidth = value; break;
                case 8: data.CheekboneHeight = value; break;
                case 9: data.CheekboneWidth = value; break;
                case 10: data.CheeksWidth = value; break;
                case 11: data.Eyes = value; break;
                case 12: data.Lips = value; break;
                case 13: data.JawWidth = value; break;
                case 14: data.JawHeight = value; break;
                case 15: data.ChinLength = value; break;
                case 16: data.ChinPosition = value; break;
                case 17: data.ChinWidth = value; break;
                case 18: data.ChinShape = value; break;
                case 19: data.NeckWidth = value; break;
            }
        }

        private void UseNewAppearanceData(BodyAppearanceData data, int overlayId, int index, float opacity)
        {
            switch (overlayId)
            {
                case 0: data.Blemishes = index; data.BlemishesOpacity = opacity; break;
                case 1: data.FacialHair = index; data.FacialHairOpacity = opacity; break;
                case 2: data.Eyebrows = index; data.EyebrowsOpacity = opacity; break;
                case 3: data.Ageing = index; data.AgeingOpacity = opacity; break;
                case 4: data.Makeup = index; data.MakeupOpacity = opacity; break;
                case 5: data.Blush = index; data.BlushOpacity = opacity; break;
                case 6: data.SunDamage = index; data.SunDamageOpacity = opacity; break;
                case 7: data.Lipstick = index; data.LipstickOpacity = opacity; break;
                case 8: data.MolesAndFreckles = index; data.MolesAndFrecklesOpacity = opacity; break;
                case 9: data.ChestHair = index; data.ChestHairOpacity = opacity; break;
                case 10: data.BodyBlemishes = index; data.BodyBlemishesOpacity = opacity; break;
                case 11: data.AddBodyBlemishes = index; data.AddBodyBlemishesOpacity = opacity; break;
            }
        }
    }
}
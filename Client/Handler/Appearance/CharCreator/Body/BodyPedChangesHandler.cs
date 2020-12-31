using System;
using System.Linq;
using TDS.Client.Data.Enums.CharCreator;

namespace TDS.Client.Handler.Appearance.CharCreator.Body
{
    internal class BodyPedChangesHandler
    {
        private readonly CharCreatorPedHandler _pedHandler;
        private readonly BodyDataHandler _bodyDataHandler;

        public BodyPedChangesHandler(CharCreatorPedHandler pedHandler, BodyDataHandler bodyDataHandler)
            => (_pedHandler, _bodyDataHandler) = (pedHandler, bodyDataHandler);

        public void DataChanged(BodyDataKey key, ref ArraySegment<object> args)
        {
            switch (key)
            {
                case BodyDataKey.All:
                case BodyDataKey.Slot:
                case BodyDataKey.IsMale:
                    _pedHandler.CreatePed();
                    break;

                case BodyDataKey.Heritage:
                    var heritageData = _bodyDataHandler.Data.HeritageDataSynced.First(d => d.Slot == _bodyDataHandler.Data.Slot);
                    _pedHandler.Ped.UpdateHeritage(heritageData);
                    break;

                case BodyDataKey.Feature:
                    _pedHandler.Ped.UpdateFaceFeature(Convert.ToInt32(args[0]), Convert.ToSingle(args[1]));
                    break;

                case BodyDataKey.Appearance:
                    _pedHandler.Ped.UpdateAppearance(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToSingle(args[2]));
                    break;

                case BodyDataKey.Hair:
                    _pedHandler.Ped.UpdateHair(Convert.ToInt32(args[0]));
                    break;

                case BodyDataKey.HairColor:
                    _pedHandler.Ped.UpdateHairColor(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
                    break;

                case BodyDataKey.EyeColor:
                    _pedHandler.Ped.UpdateEyeColor(Convert.ToInt32(args[0]));
                    break;

                case BodyDataKey.FacialHairColor:
                    _pedHandler.Ped.UpdateColor(1, 1, Convert.ToInt32(args[0]));
                    break;

                case BodyDataKey.EyebrowColor:
                    _pedHandler.Ped.UpdateColor(2, 1, Convert.ToInt32(args[0]));
                    break;

                case BodyDataKey.BlushColor:
                    _pedHandler.Ped.UpdateColor(5, 2, Convert.ToInt32(args[0]));
                    break;

                case BodyDataKey.LipstickColor:
                    _pedHandler.Ped.UpdateColor(8, 2, Convert.ToInt32(args[0]));
                    break;

                case BodyDataKey.ChestHairColor:
                    _pedHandler.Ped.UpdateColor(10, 1, Convert.ToInt32(args[0]));
                    break;
            }
        }
    }
}
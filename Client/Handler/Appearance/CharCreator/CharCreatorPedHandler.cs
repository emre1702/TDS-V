using RAGE;
using System;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Appearance.CharCreator.Body;
using TDS.Client.Handler.Entities.GTA;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.CharCreator.Body;

namespace TDS.Client.Handler.Appearance
{
    internal class CharCreatorPedHandler
    {
        public ITDSPed Ped { get; private set; }
        private uint _dimension;

        private readonly LoggingHandler _logging;
        private readonly BodyDataHandler _bodyDataHandler;

        public CharCreatorPedHandler(LoggingHandler logging, BodyDataHandler bodyDataHandler)
        {
            _logging = logging;
            _bodyDataHandler = bodyDataHandler;
        }

        public void Start(uint dimension)
        {
            _dimension = dimension;
            CreatePed();
        }

        public void Stop()
        {
            if (!(Ped is null))
                Ped.Destroy();
        }

        private void CreatePed()
        {
            try
            {
                Ped?.Destroy();

                var data = _bodyDataHandler.Data;
                var generalData = data.GeneralDataSynced.First(e => e.Slot == data.Slot);
                var skin = generalData.IsMale ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
                var pos = new Vector3(-425.48f, 1123.55f, 325.85f);

                Ped = new TDSPed((uint)skin, pos, 345, _dimension);

                Ped.SetBodyData(data);

                //Todo Give him player outfits
            }
            catch (Exception ex)
            {
                _logging.LogError(ex);
            }
        }

        public void BodyDataChanged(CharCreatorDataKey key, ref ArraySegment<object> args)
        {
            if (Ped is null)
                return;

            switch (key)
            {
                case CharCreatorDataKey.All:
                case CharCreatorDataKey.Slot:
                case CharCreatorDataKey.IsMale:
                    CreatePed();
                    break;

                case CharCreatorDataKey.Heritage:
                    var heritageData = _bodyDataHandler.Data.HeritageDataSynced.First(d => d.Slot == _bodyDataHandler.Data.Slot);
                    Ped.UpdateHeritage(heritageData);
                    break;

                case CharCreatorDataKey.Feature:
                    Ped.UpdateFaceFeature(Convert.ToInt32(args[0]), Convert.ToSingle(args[1]));
                    break;

                case CharCreatorDataKey.Appearance:
                    Ped.UpdateAppearance(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToSingle(args[2]));
                    break;

                case CharCreatorDataKey.Hair:
                    Ped.UpdateHair(Convert.ToInt32(args[0]));
                    break;

                case CharCreatorDataKey.HairColor:
                    Ped.UpdateHairColor(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
                    break;

                case CharCreatorDataKey.EyeColor:
                    Ped.UpdateEyeColor(Convert.ToInt32(args[0]));
                    break;

                case CharCreatorDataKey.FacialHairColor:
                    Ped.UpdateColor(1, 1, Convert.ToInt32(args[0]));
                    break;

                case CharCreatorDataKey.EyebrowColor:
                    Ped.UpdateColor(2, 1, Convert.ToInt32(args[0]));
                    break;

                case CharCreatorDataKey.BlushColor:
                    Ped.UpdateColor(5, 2, Convert.ToInt32(args[0]));
                    break;

                case CharCreatorDataKey.LipstickColor:
                    Ped.UpdateColor(8, 2, Convert.ToInt32(args[0]));
                    break;

                case CharCreatorDataKey.ChestHairColor:
                    Ped.UpdateColor(10, 1, Convert.ToInt32(args[0]));
                    break;
            }
        }
    }
}
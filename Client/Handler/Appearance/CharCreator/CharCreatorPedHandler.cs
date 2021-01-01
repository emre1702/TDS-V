using RAGE;
using System;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Appearance.CharCreator.Body;
using TDS.Client.Handler.Appearance.CharCreator.Clothes;
using TDS.Client.Handler.Entities.GTA;
using TDS.Shared.Core;

namespace TDS.Client.Handler.Appearance
{
    internal class CharCreatorPedHandler
    {
        public ITDSPed Ped { get; private set; }
        private uint _dimension;

        private readonly LoggingHandler _logging;
        private readonly BodyDataHandler _bodyDataHandler;
        private readonly ClothesDataHandler _clothesDataHandler;

        public CharCreatorPedHandler(LoggingHandler logging, BodyDataHandler bodyDataHandler, ClothesDataHandler clothesDataHandler)
        {
            _logging = logging;
            _bodyDataHandler = bodyDataHandler;
            _clothesDataHandler = clothesDataHandler;
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
            Ped = null;
        }

        public void CreatePed()
        {
            try
            {
                Ped?.Destroy();

                var bodyData = _bodyDataHandler.Data;
                var generalData = bodyData.GeneralDataSynced.First(e => e.Slot == bodyData.Slot);
                var skin = generalData.IsMale ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01;
                var pos = new Vector3(-425.48f, 1123.55f, 325.85f);

                Ped = new TDSPed((uint)skin, pos, 345, _dimension);
                var clothesData = _clothesDataHandler.Data;

                new TDSTimer(() =>
                {
                    Ped?.SetBodyData(bodyData);
                    Ped?.SetClothesData(clothesData);
                }, 1000);
               
            }
            catch (Exception ex)
            {
                _logging.LogError(ex);
            }
        }
    }
}
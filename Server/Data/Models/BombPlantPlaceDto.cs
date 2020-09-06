using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Models
{
#nullable enable

    public class BombPlantPlaceDto
    {
        #region Public Fields

        public ITDSBlip? Blip;
        public ITDSObject? Object;
        public Vector3 Position;

        #endregion Public Fields

        #region Public Constructors

        public BombPlantPlaceDto(ITDSObject? obj, ITDSBlip? blip, Vector3 pos) => (Object, Blip, Position) = (obj, blip, pos);

        #endregion Public Constructors

        #region Public Methods

        public void Delete()
        {
            Object?.Delete();
            Blip?.Delete();
        }

        #endregion Public Methods
    }
}

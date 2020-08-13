using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Models
{
    public class BombPlantPlaceDto
    {
        #region Public Fields

        public ITDSBlip Blip;
        public ITDSObject Object;
        public Position Position;

        #endregion Public Fields

        #region Public Constructors

        public BombPlantPlaceDto(ITDSObject obj, ITDSBlip blip, Position pos) => (Object, Blip, Position) = (obj, blip, pos);

        #endregion Public Constructors

        #region Public Methods

        public void Delete()
        {
            Object.Delete();
            Blip.Delete();
        }

        #endregion Public Methods
    }
}

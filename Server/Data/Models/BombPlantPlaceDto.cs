using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Models
{
    public class BombPlantPlaceDto
    {
        #region Public Fields

        public IBlip Blip;
        public IMapObject Object;
        public Position3D Position;

        #endregion Public Fields

        #region Public Constructors

        public BombPlantPlaceDto(IMapObject obj, IBlip blip, Position3D pos) => (Object, Blip, Position) = (obj, blip, pos);

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

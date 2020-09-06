using TDS_Client.Data.Interfaces.RAGE.Game.MapObject;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.MapObject
{
    internal class MapObjectAPI : IMapObjectAPI
    {
        #region Public Constructors

        public MapObjectAPI()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IMapObject Create(uint hash, Position3D position, Position3D rotation, int alpha = 255, uint dimension = 0)
        {
            return new MapObject(hash, position.ToVector3(), rotation.ToVector3(), alpha, dimension);
        }

        public void PlaceObjectOnGroundProperly(int handle)
        {
            RAGE.Game.Object.PlaceObjectOnGroundProperly(handle);
        }

        #endregion Public Methods
    }
}
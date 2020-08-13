using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.MapObject
{
    public interface IMapObjectAPI
    {
        #region Public Methods

        IMapObject Create(uint hash, Position position, Position rotation, int alpha = 255, uint dimension = 0);

        void PlaceObjectOnGroundProperly(int handle);

        #endregion Public Methods
    }
}

using System.Drawing;
using TDS_Server.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Marker
{
    public interface IMarkerAPI
    {
        #region Public Methods

        IMarker Create(MarkerType type, Position3D position, Position3D direction, Position3D rotation, float scale, Color color, bool bobUpAndDown, ILobby lobby);

        #endregion Public Methods
    }
}

using System.Drawing;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Marker;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;
using MarkerType = TDS_Server.Data.Enums.MarkerType;

namespace TDS_Server.RAGEAPI.Marker
{
    class MarkerAPI : IMarkerAPI
    {
        public IMarker Create(MarkerType type, Position3D position, Position3D? direction, Position3D? rotation, float scale, Color color, bool bobUpAndDown, ILobby lobby)
        {
            var dir = direction is null ? new GTANetworkAPI.Vector3() : direction.ToVector3();
            var rot = rotation is null ? new GTANetworkAPI.Vector3() : rotation.ToVector3();
            var marker = GTANetworkAPI.NAPI.Marker.CreateMarker((GTANetworkAPI.MarkerType)type, position.ToVector3(), dir, rot, scale, 
                color.ToMod(), bobUpAndDown, lobby.Dimension);

            return new Marker(marker);
        }

    }
}

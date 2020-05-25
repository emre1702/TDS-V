using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.MapObject
{
    internal class MapObjectAPI : IMapObjectAPI
    {
        #region Public Methods

        public IMapObject Create(string hashName, Position3DDto position, Position3D? rotation, byte alpha, ILobby lobby)
        {
            var hash = GTANetworkAPI.NAPI.Util.GetHashKey(hashName);
            var pos = new GTANetworkAPI.Vector3(position.X, position.Y, position.Z);
            var rot = rotation is null ? new GTANetworkAPI.Vector3() : rotation.ToMod();
            var modObject = GTANetworkAPI.NAPI.Object.CreateObject(hash, pos, rot, alpha, lobby.Dimension);

            return new MapObject(modObject);
        }

        public IMapObject Create(int hash, Position3D position, Position3D? rotation, byte alpha, ILobby lobby)
        {
            var pos = new GTANetworkAPI.Vector3(position.X, position.Y, position.Z);
            var rot = rotation is null ? new GTANetworkAPI.Vector3() : rotation.ToMod();
            var modObject = GTANetworkAPI.NAPI.Object.CreateObject(hash, pos, rot, alpha, lobby.Dimension);

            return new MapObject(modObject);
        }

        #endregion Public Methods
    }
}

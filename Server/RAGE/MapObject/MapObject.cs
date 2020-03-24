using GTANetworkAPI;
using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.MapObject
{
    class MapObject : IMapObject
    {
        private readonly Object _instance;

        public MapObject(Object instance)
            => _instance = instance;

        public Position3D Position
        {
            get => new Position3D(_instance.Position.X, _instance.Position.Y, _instance.Position.Z);
            set => _instance.Position = new Vector3(value.X, value.Y, value.Z);
        }
        public Position3D Rotation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public uint Dimension { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        Position3D IEntity.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void AttachTo(ITDSPlayer player, PedBone sKEL_R_Finger01, Position3D? positionOffset, Position3D? rotationOffset, ILobby lobby)
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            _instance.Delete();
        }

        public void Detach()
        {
            throw new System.NotImplementedException();
        }

        public bool Equals([AllowNull] IMapObject other)
        {
            throw new System.NotImplementedException();
        }

        public void Freeze(bool toggle, ILobby lobby)
        {
            throw new System.NotImplementedException();
        }

        public void SetCollisionsless(bool toggle, ILobby lobby)
        {
            throw new System.NotImplementedException();
        }
    }
}

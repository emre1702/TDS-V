using System;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.MapObject
{
    #nullable enable
    public interface IMapObject : IEntity, IEquatable<IMapObject>
    {
        Position3D Rotation { get; set; }

        void Delete();
        void Freeze(bool toggle, ILobby lobby);
        void SetCollisionsless(bool toggle, ILobby lobby);
        void Detach();
        void AttachTo(ITDSPlayer player, PedBone sKEL_R_Finger01, Position3D? positionOffset, Position3D? rotationOffset, ILobby lobby);
    }
}

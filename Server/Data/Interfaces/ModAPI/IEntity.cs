using System;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI
{
    #nullable enable
    public interface IEntity : IEquatable<IEntity>
    {
        ushort Id { get; }
        Position3D Position { get; set; }
        Position3D Rotation { get; set; }
        uint Dimension { get; set; }

        void Freeze(bool toggle, ILobby lobby);
        void SetCollisionsless(bool toggle, ILobby lobby);
        void SetInvincible(bool toggle, ITDSPlayer forPlayer);
        void Detach(bool resetCollision = true);
        void AttachTo(ITDSPlayer player, PedBone bone, Position3D? positionOffset, Position3D? rotationOffset);
        void Delete();
    }
}

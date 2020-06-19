using System;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI
{
#nullable enable

    public interface IEntity : IEquatable<IEntity>
    {
        #region Public Properties

        uint Dimension { get; set; }
        bool Exists { get; }
        ushort Id { get; }
        bool IsNull { get; }
        Position3D Position { get; set; }
        ushort RemoteId { get; }
        Position3D Rotation { get; set; }

        #endregion Public Properties

        #region Public Methods

        void AttachTo(ITDSPlayer player, PedBone bone, Position3D? positionOffset, Position3D? rotationOffset);

        void Delete();

        void Detach();

        void Freeze(bool toggle, ILobby lobby);

        void SetCollisionsless(bool toggle, ILobby lobby);

        void SetInvincible(bool toggle, ITDSPlayer forPlayer);

        void SetInvincible(bool toggle, ILobby forLobby);

        #endregion Public Methods
    }
}

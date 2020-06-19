using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Pickup;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Pickup
{
    internal class Pickup : GTANetworkAPI.Pickup, IPickup
    {
        #region Public Constructors

        public Pickup(GTANetworkAPI.NetHandle netHandle) : base(netHandle)
        { }

        #endregion Public Constructors

        #region Public Properties

        public new Position3D Position
        {
            get => new Position3D(base.Position.X, base.Position.Y, base.Position.Z);
            set => base.Position = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        public ushort RemoteId => Handle.Value;

        public new Position3D Rotation
        {
            get => new Position3D(base.Rotation.X, base.Rotation.Y, base.Rotation.Z);
            set => base.Rotation = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        #endregion Public Properties

        #region Public Methods

        public void AttachTo(ITDSPlayer player, PedBone bone, Position3D? positionOffset, Position3D? rotationOffset)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;
            var positionOffsetVector = positionOffset?.ToMod() ?? new GTANetworkAPI.Vector3();
            var rotationOffsetVector = rotationOffset?.ToMod() ?? new GTANetworkAPI.Vector3();
            Init.WorkaroundsHandler.AttachEntityToEntity(this, modPlayer, bone, positionOffsetVector, rotationOffsetVector, player.Lobby);
        }

        public void Detach()
        {
            Init.WorkaroundsHandler.DetachEntity(this);
        }

        public bool Equals([AllowNull] IEntity other)
        {
            return Id == other?.Id;
        }

        public void Freeze(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.FreezeEntity(this, toggle, lobby);
        }

        public void SetCollisionsless(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityCollisionless(this, toggle, lobby);
        }

        public void SetInvincible(bool toggle, ITDSPlayer forPlayer)
        {
            if (!(forPlayer.ModPlayer is Player.Player modPlayer))
                return;
            Init.WorkaroundsHandler.SetEntityInvincible(modPlayer, this, toggle);
        }

        public void SetInvincible(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityInvincible(lobby, this, toggle);
        }

        #endregion Public Methods
    }
}

using GTANetworkAPI;
using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Blip
{
    internal class Blip : GTANetworkAPI.Blip, IBlip
    {
        #region Public Constructors

        public Blip(NetHandle handle) : base(handle)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new Position Position
        {
            get => new Position(base.Position.X, base.Position.Y, base.Position.Z);
            set => base.Position = new Vector3(value.X, value.Y, value.Z);
        }

        public ushort RemoteId => Handle.Value;

        public new Position Rotation
        {
            get => new Position(base.Rotation.X, base.Rotation.Y, base.Rotation.Z);
            set => base.Rotation = new Vector3(value.X, value.Y, value.Z);
        }

        #endregion Public Properties

        #region Public Methods

        public void AttachTo(ITDSPlayer player, PedBone bone, Position? positionOffset, Position? rotationOffset)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;

            var positionOffsetVector = positionOffset?.ToMod() ?? new Vector3();
            var rotationOffsetVector = rotationOffset?.ToMod() ?? new Vector3();
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
            if (!(forPlayer.ModPlayer is Player.Player player))
                return;
            Init.WorkaroundsHandler.SetEntityInvincible(player, this, toggle);
        }

        public void SetInvincible(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityInvincible(lobby, this, toggle);
        }

        #endregion Public Methods
    }
}

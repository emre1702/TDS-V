using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using static TDS_Server.Data.Interfaces.ModAPI.ColShape.ITDSColShape;

namespace TDS_Server.RAGEAPI.ColShape
{
    internal class ColShape : GTANetworkAPI.ColShape, ITDSColShape
    {
        #region Public Constructors

        public ColShape(GTANetworkAPI.NetHandle netHandle) : base(netHandle)
        {
            OnEntityEnterColShape += Instance_OnEntityEnterColShape;
            OnEntityExitColShape += Instance_OnEntityExitColShape;
        }

        #endregion Public Constructors

        #region Public Events

        public event ColshapeEnterExitDelegate? PlayerEntered;

        public event ColshapeEnterExitDelegate? PlayerExited;

        #endregion Public Events

        #region Public Properties

        public new Position Position
        {
            get => new Position(base.Position.X, base.Position.Y, base.Position.Z);
            set => base.Position = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        public ushort RemoteId => Handle.Value;

        public new Position Rotation
        {
            get => new Position(base.Rotation.X, base.Rotation.Y, base.Rotation.Z);
            set => base.Rotation = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        #endregion Public Properties

        #region Public Methods

        public void AttachTo(ITDSPlayer player, PedBone bone, Position? positionOffset, Position? rotationOffset)
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
            return this.Id == other?.Id;
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

        #region Private Methods

        private void Instance_OnEntityEnterColShape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player client)
        {
            if (this != colShape)
                return;
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn((IPlayer)client);
            if (tdsPlayer is null)
                return;

            PlayerEntered?.Invoke(tdsPlayer);
        }

        private void Instance_OnEntityExitColShape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player client)
        {
            if (this != colShape)
                return;
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn((IPlayer)client);
            if (tdsPlayer is null)
                return;

            PlayerExited?.Invoke(tdsPlayer);
        }

        #endregion Private Methods
    }
}

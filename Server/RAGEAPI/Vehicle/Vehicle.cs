using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models.GTA;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Vehicle
{
    internal class Vehicle : GTANetworkAPI.Vehicle, IVehicle
    {
        #region Public Constructors

        public Vehicle(GTANetworkAPI.NetHandle netHandle) : base(netHandle)
        { }

        #endregion Public Constructors

        #region Public Properties

        public new ITDSPlayer? Controller => Init.GetTDSPlayerIfLoggedIn(base.Controller as IPlayer);

        public new Color CustomPrimaryColor
        {
            get => base.CustomPrimaryColor.ToTDS();
            set => base.CustomPrimaryColor = value.ToMod();
        }

        public new Color CustomSecondaryColor
        {
            get => base.CustomSecondaryColor.ToTDS();
            set => base.CustomSecondaryColor = value.ToMod();
        }

        public new Color NeonColor
        {
            get => base.NeonColor.ToTDS();
            set => base.NeonColor = value.ToMod();
        }

        public new List<IEntity> Occupants => base.Occupants.OfType<IEntity>().ToList();

        public new Position3D Position
        {
            get => new Position3D(base.Position.X, base.Position.Y, base.Position.Z);
            set => base.Position = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        public new VehiclePaint PrimaryPaint
        {
            get => base.PrimaryPaint.ToTDS();
            set => base.PrimaryPaint = value.ToMod();
        }

        public new Position3D Rotation
        {
            get => new Position3D(base.Rotation.X, base.Rotation.Y, base.Rotation.Z);
            set => base.Rotation = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        public new VehiclePaint SecondaryPaint
        {
            get => base.SecondaryPaint.ToTDS();
            set => base.SecondaryPaint = value.ToMod();
        }

        public new IVehicle? Trailer => base.Trailer as IVehicle;

        public new IVehicle? TraileredBy => base.TraileredBy as IVehicle;

        public new Color TyreSmokeColor
        {
            get => base.TyreSmokeColor.ToTDS();
            set => base.TyreSmokeColor = value.ToMod();
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
            return base.Id == other?.Id;
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

        public void Spawn(Position3D position, float heading = 0)
            => base.Spawn(position.ToMod(), heading);

        #endregion Public Methods
    }
}

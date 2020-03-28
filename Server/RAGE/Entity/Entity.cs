using GTANetworkAPI;
using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.RAGE.Extensions;
using TDS_Server.RAGE.Startup;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.Entity
{
    class Entity : IEntity
    {
        private GTANetworkAPI.Entity _instance;

        protected Entity(GTANetworkAPI.Entity instance)
            => _instance = instance;

        public ushort Id => _instance.Id;
        public Position3D Position
        {
            get => new Position3D(_instance.Position.X, _instance.Position.Y, _instance.Position.Z);
            set => _instance.Position = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }
        public Position3D Rotation
        {
            get => new Position3D(_instance.Rotation.X, _instance.Rotation.Y, _instance.Rotation.Z);
            set => _instance.Rotation = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }
        public uint Dimension
        {
            get => _instance.Dimension;
            set => _instance.Dimension = value;
        }

        public void AttachTo(ITDSPlayer player, PedBone bone, Position3D? positionOffset, Position3D? rotationOffset)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;

            var positionOffsetVector = positionOffset?.ToVector3() ?? new Vector3();
            var rotationOffsetVector = rotationOffset?.ToVector3() ?? new Vector3();
            Init.WorkaroundsHandler.AttachEntityToEntity(_instance, modPlayer._instance, bone, positionOffsetVector, rotationOffsetVector, player.Lobby);
        }


        public void Detach(bool resetCollision = true)
        {
            Init.WorkaroundsHandler.DetachEntity(_instance, resetCollision);
        }

        public void Freeze(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.FreezeEntity(_instance, toggle, lobby);
        }

        public void SetCollisionsless(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityCollisionless(_instance, toggle, lobby);
        }

        public void SetInvincible(bool toggle, ITDSPlayer forPlayer)
        {
            if (!(forPlayer.ModPlayer is Player.Player player))
                return;
            Init.WorkaroundsHandler.SetEntityInvincible(player._instance, _instance, toggle);
        }

        public void SetInvincible(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityInvincible(lobby, _instance, toggle);
        }

        public void Delete()
        {
            _instance.Delete();
        }

        public bool Equals([AllowNull] IEntity other)
        {
            return _instance.Id == other?.Id;
        }
    }
}

using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        public override void SetEntityInvincible(ITDSVehicle vehicle, bool invincible)
        {
            vehicle.SetInvincible(invincible, this);
        }

        public override void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset)
        {
            _workaroundsHandler.AttachEntityToEntity(this, player, bone, positionOffset ?? new Vector3(), rotationOffset ?? new Vector3(), player.Lobby);
        }

        public override void Detach()
        {
            _workaroundsHandler.DetachEntity(this);
        }

        public override void Freeze(bool toggle)
        {
            _workaroundsHandler.FreezePlayer(this, toggle);
        }

        public override void SetCollisionsless(bool toggle)
        {
            _workaroundsHandler.SetEntityCollisionless(this, toggle, Lobby);
        }

        public override void SetInvincible(bool toggle)
        {
            _workaroundsHandler.SetPlayerInvincible(this, toggle);
        }
    }
}

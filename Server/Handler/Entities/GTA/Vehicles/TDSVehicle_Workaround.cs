using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Entities.GTA.Vehicles
{
    partial class TDSVehicle
    {
        public override void SetInvincible(bool toggle, ITDSPlayer forPlayer)
        {
            _workaroundsHandler.SetEntityInvincible(forPlayer, this, toggle);
        }

        public override void SetInvincible(bool toggle)
        {
            if (Lobby is null)
                return;
            _workaroundsHandler.SetEntityInvincible(Lobby, this, toggle);
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
            if (Lobby is null)
                return;
            _workaroundsHandler.FreezeEntity(this, toggle, Lobby);
        }

        public override void SetCollisionsless(bool toggle)
        {
            _workaroundsHandler.SetEntityCollisionless(this, toggle, Lobby);
        }
    }
}

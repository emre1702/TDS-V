using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Handler.Entities.GTA.Objects
{
    partial class TDSObject
    {
        public override void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset)
        {
            _workaroundsHandler.AttachEntityToEntity(this, player, bone, positionOffset ?? new Vector3(), rotationOffset ?? new Vector3(), player.Lobby);
        }

        public override void Detach()
        {
            _workaroundsHandler.DetachEntity(this);
        }

        public override void Freeze(bool toggle, IBaseLobby lobby)
        {
            _workaroundsHandler.FreezeEntity(this, toggle, lobby);
        }

        public override void SetCollisionsless(bool toggle, IBaseLobby lobby)
        {
            _workaroundsHandler.SetEntityCollisionless(this, toggle, lobby);
        }
    }
}

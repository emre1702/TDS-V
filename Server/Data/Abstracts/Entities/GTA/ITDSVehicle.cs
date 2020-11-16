using GTANetworkAPI;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSVehicle : Vehicle
    {
        public IBaseLobby? Lobby { get; set; }

        protected ITDSVehicle(NetHandle netHandle) : base(netHandle)
        {
        }

        public abstract void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset);

        public abstract void Detach();

        public abstract void Freeze(bool toggle);

        public abstract void SetCollisionsless(bool toggle);

        public abstract void SetInvincible(bool toggle, ITDSPlayer forPlayer);

        public abstract void SetInvincible(bool toggle);

        public abstract void SetGang(IGang gang);
    }
}

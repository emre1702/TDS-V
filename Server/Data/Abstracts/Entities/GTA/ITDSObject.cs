using GTANetworkAPI;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSObject : Object
    {
        public ushort RemoteId { get; set; }

        protected ITDSObject(NetHandle netHandle) : base(netHandle)
        {
            RemoteId = netHandle.Value;
        }

        public abstract void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset);

        public abstract void Detach();

        public abstract void Freeze(bool toggle, IBaseLobby lobby);

        public abstract void SetCollisionsless(bool toggle, IBaseLobby lobby);
    }
}

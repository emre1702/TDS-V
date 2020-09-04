using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
#nullable enable

    public abstract class ITDSObject : Object
    {
        public ITDSObject(NetHandle netHandle) : base(netHandle)
        {
        }

        public abstract void AttachTo(ITDSPlayer player, PedBone bone, Vector3? positionOffset, Vector3? rotationOffset);

        public abstract void Detach();

        public abstract void Freeze(bool toggle, ILobby lobby);

        public abstract void SetCollisionsless(bool toggle, ILobby lobby);
    }
}

using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.Entities
{
    public interface ITDSEntity
    {
        void Detach();
        void SetCollisionsless(bool toggle, ILobby forLobby);
        void AttachTo(ITDSPlayer player, PedBone bone, Position? offsetPos, DegreeRotation? offsetRot);
    }
}

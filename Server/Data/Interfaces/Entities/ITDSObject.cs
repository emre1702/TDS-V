using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;

namespace TDS_Server.Data.Interfaces.Entities
{
    public interface ITDSObject : ITDSEntity
    {
        Position Position { get; set; }
        DegreeRotation Rotation { get; set; }

        void Delete();
        void Freeze(bool toggle, ILobby forLobby);
    }
}

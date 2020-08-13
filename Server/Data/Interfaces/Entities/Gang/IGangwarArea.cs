using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces.Entities.Gang
{
    #nullable enable
    public interface IGangwarArea
    {
        GangwarAreas? Entity { get; }
        IArena? InLobby { get; set; }
    }
}

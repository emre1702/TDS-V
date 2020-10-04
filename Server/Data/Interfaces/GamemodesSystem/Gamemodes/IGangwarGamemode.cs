using TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IGangwarGamemode : IBaseGamemode
    {
        new IGangwarGamemodeMapHandler MapHandler { get; }
        new IGangwarGamemodeTeams Teams { get; }
    }
}

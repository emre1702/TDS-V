using TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler;
using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IGangwarGamemode : IBaseGamemode
    {
        new IGangwarGamemodeMapHandler MapHandler { get; }
        new IGangwarGamemodePlayers Players { get; }
        new IGangwarGamemodeSpecials Specials { get; }
        new IGangwarGamemodeTeams Teams { get; }
    }
}

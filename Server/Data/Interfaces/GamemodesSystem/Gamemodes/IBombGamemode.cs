using TDS_Server.Data.Interfaces.GamemodesSystem.Players;
using TDS_Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IBombGamemode : IBaseGamemode
    {
        new IBombGamemodePlayers Players { get; }
        new IBombGamemodeSpecials Specials { get; }
        new IBombGamemodeTeams Teams { get; }
    }
}

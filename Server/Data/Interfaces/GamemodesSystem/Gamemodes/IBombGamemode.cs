using TDS.Server.Data.Interfaces.GamemodesSystem.Players;
using TDS.Server.Data.Interfaces.GamemodesSystem.Specials;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Gamemodes
{
    public interface IBombGamemode : IBaseGamemode
    {
        new IBombGamemodePlayers Players { get; }
        new IBombGamemodeSpecials Specials { get; }
        new IBombGamemodeTeams Teams { get; }
    }
}

using TDS_Server.GamemodesSystem.Deathmatch;
using TDS_Server.GamemodesSystem.MapHandlers;
using TDS_Server.GamemodesSystem.Players;
using TDS_Server.GamemodesSystem.Rounds;
using TDS_Server.GamemodesSystem.Specials;
using TDS_Server.GamemodesSystem.Teams;
using TDS_Server.GamemodesSystem.Weapons;

namespace TDS_Server.GamemodesSystem.DependenciesModels
{
    public class BaseGamemodeDependencies
    {
        internal BaseGamemodeDeathmatch? Deathmatch { get; set; }
        internal BaseGamemodeMapHandler? MapHandler { get; set; }
        internal BaseGamemodePlayers? Players { get; set; }
        internal BaseGamemodeRounds? Rounds { get; set; }
        internal BaseGamemodeSpecials? Specials { get; set; }
        internal BaseGamemodeTeams? Teams { get; set; }
        internal BaseGamemodeWeapons? Weapons { get; set; }
    }
}

using TDS.Server.GamemodesSystem.Deathmatch;
using TDS.Server.GamemodesSystem.MapHandlers;
using TDS.Server.GamemodesSystem.Players;
using TDS.Server.GamemodesSystem.Rounds;
using TDS.Server.GamemodesSystem.Specials;
using TDS.Server.GamemodesSystem.Teams;
using TDS.Server.GamemodesSystem.Weapons;

namespace TDS.Server.GamemodesSystem.DependenciesModels
{
    public class BaseGamemodeDependencies
    {
        public BaseGamemodeDeathmatch? Deathmatch { get; set; }
        public BaseGamemodeMapHandler? MapHandler { get; set; }
        public BaseGamemodePlayers? Players { get; set; }
        public BaseGamemodeRounds? Rounds { get; set; }
        public BaseGamemodeSpecials? Specials { get; set; }
        public BaseGamemodeTeams? Teams { get; set; }
        public BaseGamemodeWeapons? Weapons { get; set; }
    }
}

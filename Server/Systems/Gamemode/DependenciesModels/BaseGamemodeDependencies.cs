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
        public BaseGamemodeDeathmatch? Deathmatch { get; set; }
        public BaseGamemodeMapHandler? MapHandler { get; set; }
        public BaseGamemodePlayers? Players { get; set; }
        public BaseGamemodeRounds? Rounds { get; set; }
        public BaseGamemodeSpecials? Specials { get; set; }
        public BaseGamemodeTeams? Teams { get; set; }
        public BaseGamemodeWeapons? Weapons { get; set; }
    }
}

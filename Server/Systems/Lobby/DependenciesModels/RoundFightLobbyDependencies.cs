using TDS.Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Rankings;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Statistics;

namespace TDS.Server.LobbySystem.DependenciesModels
{
    internal class RoundFightLobbyDependencies : FightLobbyDependencies
    {
        public new IRoundFightLobbyMapHandler? MapHandler { get; set; }
        public IRoundFightLobbyRanking? Ranking { get; set; }
        public IRoundFightLobbyRoundsHandler? Rounds { get; set; }
        public IRoundFightLobbyStatistics? Statistics { get; set; }
    }
}

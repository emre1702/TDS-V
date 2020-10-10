using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Rankings;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Statistics;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class RoundFightLobbyDependencies : FightLobbyDependencies
    {
        public new IRoundFightLobbyMapHandler? MapHandler { get; set; }
        public IRoundFightLobbyRanking? Ranking { get; set; }
        public IRoundFightLobbyRoundsHandler? Rounds { get; set; }
        public IRoundFightLobbyStatistics? Statistics { get; set; }
    }
}

using TDS_Server.Data.Interfaces.LobbySystem.GamemodesHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Rankings;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Statistics;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class RoundFightLobbyDependencies : FightLobbyDependencies
    {
        public IRoundFightLobbyGamemodesHandler? GamemodesHandler { get; set; }
        public IRoundFightLobbyRanking? Ranking { get; set; }
        public IRoundFightLobbyRoundsHandler? RoundsHandler { get; set; }
        public IRoundFightLobbyStatistics? Statistics { get; set; }
    }
}

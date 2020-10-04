using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.MapHandlers;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.RoundsHandlers;
using TDS_Server.LobbySystem.Sync;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class GangActionLobbyDependencies : RoundFightLobbyDependencies
    {
        public new GangActionLobbyBansHandler? Bans { get; set; }
        public new GangActionLobbyMapHandler? MapHandler { get; set; }
        public new GangActionLobbyPlayers? Players { get; set; }
        public new GangActionLobbyRoundsHandler? Rounds { get; set; }
        public new GangActionLobbySync? Sync { get; set; }
        public new IGangActionLobbyTeamsHandler? Teams { get; set; }
    }
}

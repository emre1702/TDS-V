using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.LobbySystem.BansHandlers;
using TDS.Server.LobbySystem.MapHandlers;
using TDS.Server.LobbySystem.Players;
using TDS.Server.LobbySystem.RoundsHandlers;
using TDS.Server.LobbySystem.Sync;

namespace TDS.Server.LobbySystem.DependenciesModels
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

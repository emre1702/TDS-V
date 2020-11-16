using TDS.Server.Data.Interfaces.LobbySystem.Actions;

namespace TDS.Server.LobbySystem.DependenciesModels
{
    internal class GangLobbyDependencies : FreeroamLobbyDependencies
    {
        public IGangLobbyActions? Actions { get; set; }
    }
}

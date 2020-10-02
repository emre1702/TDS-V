using TDS_Server.Data.Interfaces.LobbySystem.Actions;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class GangLobbyDependencies : FreeroamLobbyDependencies
    {
        public IGangLobbyActions? Actions { get; set; }
    }
}

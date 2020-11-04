using TDS_Server.Data.Interfaces.LobbySystem.Sync;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class MapCreatorLobbyDependencies : FreeroamLobbyDependencies
    {
        public new IMapCreatorLobbySync? Sync { get; set; }
    }
}

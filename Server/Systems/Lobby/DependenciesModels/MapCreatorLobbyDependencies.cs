using TDS.Server.Data.Interfaces.LobbySystem.Sync;

namespace TDS.Server.LobbySystem.DependenciesModels
{
    internal class MapCreatorLobbyDependencies : FreeroamLobbyDependencies
    {
        public new IMapCreatorLobbySync? Sync { get; set; }
    }
}

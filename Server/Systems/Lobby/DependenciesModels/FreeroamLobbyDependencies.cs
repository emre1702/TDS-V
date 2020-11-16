using TDS.Server.Data.Interfaces.LobbySystem.Freeroam;

namespace TDS.Server.LobbySystem.DependenciesModels
{
    internal class FreeroamLobbyDependencies : BaseLobbyDependencies
    {
        public IFreeroamLobbyFreeroam? Freeroam { get; set; }
    }
}

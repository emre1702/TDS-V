using TDS_Server.Data.Interfaces.LobbySystem.Freeroam;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class FreeroamLobbyDependencies : BaseLobbyDependencies
    {
        public IFreeroamLobbyFreeroam? Freeroam { get; set; }
    }
}

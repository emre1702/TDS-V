using TDS_Server.Data.Interfaces.LobbySystem.Spectator;
using TDS_Server.Data.Interfaces.LobbySystem.Weapons;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class FightLobbyDependencies : BaseLobbyDependencies
    {
        public IFightLobbySpectator? Spectator { get; set; }
        public IFightLobbyWeapons? Weapons { get; set; }
    }
}

using TDS.Server.Data.Interfaces.DamageSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Spectator;
using TDS.Server.Data.Interfaces.LobbySystem.Weapons;

namespace TDS.Server.LobbySystem.DependenciesModels
{
    internal class FightLobbyDependencies : BaseLobbyDependencies
    {
        public IDamageHandler? DamageHandler { get; set; }
        public IFightLobbySpectator? Spectator { get; set; }
        public IFightLobbyWeapons? Weapons { get; set; }
    }
}

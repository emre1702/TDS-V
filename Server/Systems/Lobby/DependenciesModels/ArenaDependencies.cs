using TDS.Server.Data.Interfaces.LobbySystem.MapVotings;

namespace TDS.Server.LobbySystem.DependenciesModels
{
    internal class ArenaDependencies : RoundFightLobbyDependencies
    {
        public IArenaMapVoting? MapVoting { get; set; }
    }
}

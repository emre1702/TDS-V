using TDS_Server.Data.Interfaces.LobbySystem.MapVotings;

namespace TDS_Server.LobbySystem.DependenciesModels
{
    internal class ArenaDependencies : RoundFightLobbyDependencies
    {
        public IArenaMapVoting? ArenaMapVoting { get; set; }
    }
}

using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IRoundFightLobbyTeamsHandler : IFightLobbyTeamsHandler
    {
        Task<ITeam?> GetTeamWithHighestHp();
    }
}

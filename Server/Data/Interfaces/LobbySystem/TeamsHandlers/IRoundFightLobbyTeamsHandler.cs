using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IRoundFightLobbyTeamsHandler : IFightLobbyTeamsHandler
    {
        Task<ITeam?> GetTeamWithHighestHp();
    }
}

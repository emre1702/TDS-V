using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
#nullable enable

    public interface IRoundFightLobbyTeamsHandler : IFightLobbyTeamsHandler
    {
        Task<ITeam?> GetTeamWithHighestHp();
    }
}

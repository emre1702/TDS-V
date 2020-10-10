using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
    public interface IArenaTeamsHandler : IRoundFightLobbyTeamsHandler
    {
        Task BalanceCurrentTeams();
    }
}

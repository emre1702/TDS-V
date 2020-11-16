using System.Threading.Tasks;

namespace TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers
{
    public interface IArenaTeamsHandler : IRoundFightLobbyTeamsHandler
    {
        Task BalanceCurrentTeams();
    }
}

using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;

namespace TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers
{
#nullable enable

    public interface IRoundFightLobbyRoundsHandler
    {
        IGamemode? CurrentGamemode { get; }
        IRoundStatesHandler RoundStates { get; }

        ValueTask<ITeam?> GetTimesUpWinnerTeam();
    }
}

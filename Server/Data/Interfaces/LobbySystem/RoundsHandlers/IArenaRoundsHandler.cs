using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers
{
#nullable enable

    public interface IArenaRoundsHandler : IRoundFightLobbyRoundsHandler
    {
        Task<bool> PlayerJoinedRound(ITDSPlayer player);
    }
}

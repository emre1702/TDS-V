using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers
{
#nullable enable

    public interface IArenaRoundsHandler : IRoundFightLobbyRoundsHandler
    {
        Task<bool> PlayerJoinedRound(ITDSPlayer player);
    }
}

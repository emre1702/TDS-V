using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IRoundFightLobbyDeathmatch : IFightLobbyDeathmatch
    {
        Task RemovePlayerFromAlive(ITDSPlayer player);
    }
}

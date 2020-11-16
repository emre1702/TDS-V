using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IRoundFightLobbyDeathmatch : IFightLobbyDeathmatch
    {
        Task RemovePlayerFromAlive(ITDSPlayer player);
    }
}

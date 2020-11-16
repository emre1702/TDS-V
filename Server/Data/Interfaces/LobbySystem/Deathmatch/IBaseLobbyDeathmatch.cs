using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IBaseLobbyDeathmatch
    {
        Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);
    }
}

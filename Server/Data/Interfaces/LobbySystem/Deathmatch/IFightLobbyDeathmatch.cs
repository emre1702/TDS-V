using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Deathmatch
{
    public interface IFightLobbyDeathmatch
    {
        int AmountLifes { get; set; }

        Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);
    }
}

using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Data.Interfaces.LobbySystem.Deathmatch
{
#nullable enable

    public interface IFightLobbyDeathmatch
    {
        int AmountLifes { get; set; }
        IDamagesys Damage { get; }

        Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);

        void DeathInfoSync(ITDSPlayer player, ITDSPlayer? killer, uint weapon);
    }
}

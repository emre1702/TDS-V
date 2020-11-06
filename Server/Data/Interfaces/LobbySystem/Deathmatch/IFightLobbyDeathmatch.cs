using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.DamageSystem;

namespace TDS_Server.Data.Interfaces.LobbySystem.Deathmatch
{
#nullable enable

    public interface IFightLobbyDeathmatch
    {
        int AmountLifes { get; set; }
        IDamageHandler Damage { get; }

        Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon);

        void Kill(ITDSPlayer player, string reason);
        void InitDamageHandler(IDamageHandler damageHandler);
    }
}

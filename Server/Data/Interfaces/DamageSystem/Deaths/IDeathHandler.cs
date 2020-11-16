using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.Data.Interfaces.DamageSystem.Deaths
{
    #nullable enable
    public interface IDeathHandler
    {
        void Init(IFightLobby lobby);
        void PlayerDeath(ITDSPlayer died, ITDSPlayer killReason, uint weapon, int diedPlayerLifes);
        void RewardLastHitter(ITDSPlayer died, out ITDSPlayer? killer);
    }
}
using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;

namespace TDS.Server.Data.Interfaces.DamageSystem.Damages
{
    #nullable enable
    public interface IHitterHandler
    {
        IEnumerable<ITDSPlayer> GetPlayersHitters(ITDSPlayer player, int atleastDamage = 0);
        ITDSPlayer? GetPlayersMostHitter(ITDSPlayer player);
        bool HasAnyHitters();
        void Init(IFightLobby fightLobby);
        void SetLastHitter(ITDSPlayer target, ITDSPlayer source, uint weapon, int damage);
    }
}
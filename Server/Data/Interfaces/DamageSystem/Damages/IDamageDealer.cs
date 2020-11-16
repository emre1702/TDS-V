using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Data.Enums;

namespace TDS.Server.DamageSystem.Damages
{
    #nullable enable
    public interface IDamageDealer
    {
        void DamagePlayer(ITDSPlayer target, WeaponHash weapon, PedBodyPart pedBodyPart, ITDSPlayer? source);
    }
}
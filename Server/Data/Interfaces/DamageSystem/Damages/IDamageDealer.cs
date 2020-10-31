using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Shared.Data.Enums;

namespace TDS_Server.DamageSystem.Damages
{
    #nullable enable
    public interface IDamageDealer
    {
        void DamagePlayer(ITDSPlayer target, WeaponHash weapon, PedBodyPart pedBodyPart, ITDSPlayer? source);
    }
}
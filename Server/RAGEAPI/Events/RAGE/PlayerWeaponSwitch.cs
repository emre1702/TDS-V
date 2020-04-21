using GTANetworkAPI;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void OnPlayerWeaponSwitch(GTANetworkAPI.Player player, GTANetworkAPI.WeaponHash oldWeaponHash, GTANetworkAPI.WeaponHash newWeaponHash)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.EventsHandler
                .OnPlayerWeaponSwitch(tdsPlayer, (TDS_Shared.Data.Enums.WeaponHash)oldWeaponHash, (TDS_Shared.Data.Enums.WeaponHash)newWeaponHash);
        }
    }
}

using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Events
{
    public class DeathmatchEvents : Script
    {
        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeaponHash, WeaponHash newWeaponHash)
        {
            EventsHandler.Instance.OnPlayerWeaponSwitch(player, oldWeaponHash, newWeaponHash);
        }
    }
}

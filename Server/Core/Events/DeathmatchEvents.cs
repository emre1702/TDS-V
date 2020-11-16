using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
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

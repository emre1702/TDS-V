using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Events.RAGE
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

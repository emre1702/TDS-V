using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
{
    public class DeathmatchEvents : Script
    {
        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeaponHash, WeaponHash newWeaponHash)
        {
            try
            {
                EventsHandler.Instance.OnPlayerWeaponSwitch(player, oldWeaponHash, newWeaponHash);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }
    }
}

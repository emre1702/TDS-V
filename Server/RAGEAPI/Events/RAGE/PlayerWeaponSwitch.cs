using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void OnPlayerWeaponSwitch(IPlayer player, WeaponHash oldWeaponHash, WeaponHash newWeaponHash)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.EventsHandler
                .OnPlayerWeaponSwitch(tdsPlayer, (TDS_Shared.Data.Enums.WeaponHash)oldWeaponHash, (TDS_Shared.Data.Enums.WeaponHash)newWeaponHash);
        }

        #endregion Public Methods
    }
}

using GTANetworkAPI;

namespace TDS.server.instance.lobby {

   partial class Arena {

        public override void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon, WeaponHash newweapon ) {
            base.OnPlayerWeaponSwitch ( player, oldweapon, newweapon );

            if ( bombAtPlayer == player ) {
                ToggleBombAtHand ( player, oldweapon, newweapon );
            }
        }
   }
}

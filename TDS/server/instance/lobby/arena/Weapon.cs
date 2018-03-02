using GTANetworkAPI;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

   partial class Arena {

        public override void OnPlayerWeaponSwitch ( Character character, WeaponHash oldweapon, WeaponHash newweapon ) {
            base.OnPlayerWeaponSwitch ( character, oldweapon, newweapon );

            if ( bombAtPlayer == character ) {
                ToggleBombAtHand ( character, oldweapon, newweapon );
            }
        }
   }
}

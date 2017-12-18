using GTANetworkAPI;
using System.Collections.Generic;

namespace TDS.server.instance.lobby.interfaces {

    interface IFight {

        void DeathInfoSync ( Client player, uint team, Client killer, uint weapon );
        void PlayerAmountInFightSync ( List<uint> amountinteam );
        void OnPlayerWeaponSwitch ( Client player, WeaponHash oldweapon, WeaponHash newweapon );

    }
}

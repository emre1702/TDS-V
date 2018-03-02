using GTANetworkAPI;
using System.Collections.Generic;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby.interfaces {

    interface IFight {

        void DeathInfoSync ( Client player, int team, Client killer, uint weapon );
        void PlayerAmountInFightSync ( List<uint> amountinteam );
        void OnPlayerWeaponSwitch ( Character character, WeaponHash oldweapon, WeaponHash newweapon );

    }
}

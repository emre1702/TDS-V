using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby.Interfaces
{

    interface IFight
    {

        //void DeathInfoSync(Client player, uint team, Client killer, uint weapon);
        //void PlayerAmountInFightSync(List<uint> amountinteam);
        void OnPlayerWeaponSwitch(Character character, WeaponHash oldweapon, WeaponHash newweapon);

    }
}

using GTANetworkAPI;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{

    partial class Arena
    {

        public override void OnPlayerWeaponSwitch(Character character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);

#warning todo Add after Arena implementation
            /*if (bombAtPlayer == character)
            {
                ToggleBombAtHand(character, oldweapon, newweapon);
            }*/
        }
    }
}

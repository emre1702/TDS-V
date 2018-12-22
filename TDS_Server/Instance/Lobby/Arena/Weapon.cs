using GTANetworkAPI;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{

    partial class Arena
    {

        public override void OnPlayerWeaponSwitch(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);

            if (bombAtPlayer == character)
            {
                ToggleBombAtHand(character, oldweapon, newweapon);
            }
        }
    }
}

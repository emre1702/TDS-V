using GTANetworkAPI;
using TDS.Default;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class FightLobby
    {
        public override uint OnPlayerDeath(Character character, Client killer, uint weapon)
        {
            if (character.Lifes > 0)
            {
                this.DeathInfoSync(character.Player, character.Team.Id, killer, weapon);
            }
            return base.OnPlayerDeath(character, killer, weapon);
        }

        public virtual void OnPlayerWeaponSwitch(Character character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvents.ClientPlayerWeaponChange, (int)newweapon);
        }
    }
}

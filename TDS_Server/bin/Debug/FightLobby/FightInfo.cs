using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Default;
using TDS.Manager.Utility;

namespace TDS.Instance.Lobby
{

    partial class FightLobby
    {

        private void DeathInfoSync(Client player, uint teamid, Client killer, uint weapon)
        {
            if (killer != null)
            {
                string weaponname = System.Enum.GetName(typeof(WeaponHash), weapon);
                this.FuncIterateAllPlayers((targetcharacter, teamID) =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(targetcharacter.Player, DCustomEvents.ClientPlayerDeath, player.Value, teamid,
                        Utils.GetReplaced(targetcharacter.Language.DEATH_KILLED_INFO, killer.Name, player.Name, weaponname)
                    );
                });
            }
            else
            {
                this.FuncIterateAllPlayers((targetcharacter, teamID) =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(targetcharacter.Player, DCustomEvents.ClientPlayerDeath, player.Value, teamid,
                        Utils.GetReplaced(targetcharacter.Language.DEATH_DIED_INFO, player.Name)
                    );
                });
            }

            
        }

        private void PlayerAmountInFightSync(List<uint> amountinteam)
        {
            this.SendAllPlayerEvent("onClientPlayerAmountInFightSync", null, JsonConvert.SerializeObject(amountinteam), false);
        }
    }

}

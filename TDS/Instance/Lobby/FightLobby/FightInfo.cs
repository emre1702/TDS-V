using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Default;
using TDS.Interface;
using TDS.Manager.Utility;

namespace TDS.Instance.Lobby
{

    partial class FightLobby
    {

        private void DeathInfoSync(Client player, uint teamID, Client killer, uint weapon)
        {
            Dictionary<ILanguage, string> killstr;
            if (killer != null)
            {
                string weaponname = System.Enum.GetName(typeof(WeaponHash), weapon);
                killstr = LangUtils.GetLangDictionary((lang) =>
                {
                    return Utils.GetReplaced(lang.DEATH_KILLED_INFO, killer.Name, player.Name, weaponname);
                });
            }
            else
            {
                killstr = LangUtils.GetLangDictionary((lang) =>
                {
                    return Utils.GetReplaced(lang.DEATH_DIED_INFO, player.Name);
                });
            }

            FuncIterateAllPlayers((targetcharacter, targetteam) =>
            {
                targetcharacter.Player.TriggerEvent(DCustomEvent.ClientPlayerDeath, player.Value, teamID, killstr[targetcharacter.Language]);
            });
        }

        public void PlayerAmountInFightSync(List<uint> amountinteam)
        {
            SendAllPlayerEvent(DCustomEvent.ClientPlayerAmountInFightSync, null, JsonConvert.SerializeObject(amountinteam), false);
        }
    }

}

using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Server.Default;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Server.Instance.Lobby
{

    partial class FightLobby
    {

        protected void DeathInfoSync(Client player, uint teamindex, Client killer, uint weapon)
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
                targetcharacter.Client.TriggerEvent(DToClientEvent.Death, player, teamindex, killstr[targetcharacter.Language]);
            });
        }

        public void PlayerAmountInFightSync(List<int> amountinteam)
        {
            SendAllPlayerEvent(DToClientEvent.AmountInFightSync, null, JsonConvert.SerializeObject(amountinteam), false);
        }
    }

}

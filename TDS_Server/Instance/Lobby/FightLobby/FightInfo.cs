using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Server.Default;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using System.Linq;
using TDS_Server.Instance.Utility;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{

    partial class FightLobby
    {

        protected void DeathInfoSync(TDSPlayer player, Client killer, uint weapon)
        {
            Dictionary<ILanguage, string> killstr;
            if (killer != null && player.Client != killer)
            {
                string weaponname = System.Enum.GetName(typeof(WeaponHash), weapon);
                killstr = LangUtils.GetLangDictionary((lang) =>
                {
                    return lang.DEATH_KILLED_INFO.Formatted(killer.Name, player.Client.Name, weaponname);
                });
            }
            else
            {
                killstr = LangUtils.GetLangDictionary((lang) =>
                {
                    return Utils.GetReplaced(lang.DEATH_DIED_INFO, player.Client.Name);
                });
            }

            FuncIterateAllPlayers((targetcharacter, targetteam) =>
            {
                targetcharacter.Client.TriggerEvent(DToClientEvent.Death, player, player.Team.Entity.Index, killstr[targetcharacter.Language]);
            });
        }
    }

}

using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        protected void DeathInfoSync(TDSPlayer player, TDSPlayer? killer, uint weapon)
        {
            Dictionary<ILanguage, string> killstr;
            if (killer != null && player != killer)
            {
                //Todo weaponname was empty, test it
                string weaponname = System.Enum.GetName(typeof(WeaponHash), weapon);
                killstr = LangUtils.GetLangDictionary((lang) =>
                {
                    return lang.DEATH_KILLED_INFO.Formatted(killer != null ? killer.Client.Name : "-", player.Client.Name, weaponname);
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
                targetcharacter.Client.TriggerEvent(DToClientEvent.Death, player.Client, player.Team?.Entity.Index ?? 0, killstr[targetcharacter.Language]);
            });
        }
    }
}
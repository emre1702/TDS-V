using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        protected void DeathInfoSync(ITDSPlayer player, ITDSPlayer? killer, uint weapon)
        {
            Dictionary<ILanguage, string> killstr;
            if (killer is { } && player != killer)
            {
                string? weaponName = System.Enum.GetName(typeof(WeaponHash), weapon);
                killstr = LangHelper.GetLangDictionary((lang) =>
                {
                    return string.Format(lang.DEATH_KILLED_INFO, killer?.DisplayName ?? "-", player.DisplayName, weaponName ?? "?");
                });
            }
            else
            {
                killstr = LangHelper.GetLangDictionary((lang) =>
                {
                    return string.Format(lang.DEATH_DIED_INFO, player.DisplayName);
                });
            }

            FuncIterateAllPlayers((targetPlayer, targetteam) =>
            {
                targetPlayer.TriggerEvent(ToClientEvent.Death, player.RemoteId, player.TeamIndex, killstr[targetPlayer.Language], player.Lifes > 1);
            });
        }
    }
}

using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using System;
using TDS_Server.Interface;
using TDS_Server.Instance.Utility;
using System.Linq;
using MoreLinq;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        protected Team[] Teams { get; set; }

        private static readonly Dictionary<ETeamOrder, Func<ILanguage, string>> teamOrderDict = new Dictionary<ETeamOrder, Func<ILanguage, string>>
        {
            [ETeamOrder.Attack] = lang => lang.ORDER_ATTACK,
            [ETeamOrder.StayBack] = lang => lang.ORDER_STAY_BACK,
            [ETeamOrder.GoToBomb] = lang => lang.ORDER_GO_TO_BOMB,
            [ETeamOrder.SpreadOut] = lang => lang.ORDER_SPREAD_OUT,
        };

        protected void SetPlayerTeam(TDSPlayer character, Team team, bool withOtherThings = true)
        {
            character.Client.SetSkin((PedHash)team.Entity.SkinHash);
            if (character.Team != team)
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerTeamChange, team.Entity.Name);
            character.SetTeam(team, withOtherThings);
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected Team GetTeamWithFewestPlayer()
        {
            return Teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in Teams)
            {
                entry.ClearPlayers();
                entry.AlivePlayers?.Clear();
                entry.SpectateablePlayers?.Clear();
            }

        }

        protected void MixTeams()
        {
            ClearTeamPlayersLists();
            foreach (TDSPlayer character in Players)
            {
                if (character.Team == null)
                    continue;
                if (!character.Team.IsSpectator)
                {
                    SetPlayerTeam(character, GetTeamWithFewestPlayer(), false);
                }
                character.Team.AddPlayer(character);
            }
        }

        protected void ClearTeamPlayersAmounts()
        {
            foreach (var team in Teams)
            {
                team.SyncedTeamData.AmountPlayers.AmountAlive = 0;
                team.SyncedTeamData.AmountPlayers.Amount = 0;
            }
        }

        public void SendTeamOrder(TDSPlayer character, ETeamOrder teamOrder)
        {
            if (!teamOrderDict.ContainsKey(teamOrder))
                return;

            Team team = character.Team;
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(teamOrderDict[teamOrder]);
        
            string str = $"[TEAM] {team.ChatColor}{character.Client.Name}: !{{150|0|0}}";
            team.FuncIterate((target, _) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, str + texts[target.Language]);
            });
        }
    }
}

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

        protected void SetPlayerTeam(TDSPlayer character, Team team)
        {
            if (character.Team != null && character.Team.Entity.Lobby == LobbyEntity.Id)
                character.Team.Players.Remove(character);
            character.Team.Players.Add(character);
            character.Client.SetSkin((PedHash)team.Entity.SkinHash);
            if (character.Team == null || character.Team != team)
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerTeamChange, team.Entity.Name);
            character.Team = team;
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected Team GetTeamWithFewestPlayer()
        {
            return Teams.MinBy(t => t.Players.Count).FirstOrDefault();
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in Teams)
            {
                entry.Players.Clear();
            }
        }

        protected void MixTeams()
        {
            ClearTeamPlayersLists();
            foreach (TDSPlayer character in Players)
            {
                if (character.Team == null)
                    continue;
                if (character.Team.Entity.Index == 0)
                    character.Team.Players.Add(character);  // because he is already in that team
                else
                    SetPlayerTeam(character, GetTeamWithFewestPlayer());
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
        
            string str = $"[TEAM] {team.ChatColor}{character.Client.Name} !{{150|0|0}}: ";
            team.FuncIterate((target, _) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, str + texts[target.Language]);
            });
        }
    }
}

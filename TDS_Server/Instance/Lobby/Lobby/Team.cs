using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using System;
using TDS_Server.Interface;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        protected Teams[] Teams { get; set; }
        protected readonly List<TDSPlayer>[] TeamPlayers;
        protected SyncedTeamDataDto[] SyncedTeamDatas { get; set; }

        private static readonly Dictionary<ETeamOrder, Func<ILanguage, string>> teamOrderDict = new Dictionary<ETeamOrder, Func<ILanguage, string>>
        {
            [ETeamOrder.Attack] = lang => lang.ORDER_ATTACK,
            [ETeamOrder.StayBack] = lang => lang.ORDER_STAY_BACK,
            [ETeamOrder.GoToBomb] = lang => lang.ORDER_GO_TO_BOMB,
            [ETeamOrder.SpreadOut] = lang => lang.ORDER_SPREAD_OUT,
        };

        protected void SetPlayerTeam(TDSPlayer character, Teams team)
        {
            if (character.Team != null && character.Team.Lobby == LobbyEntity.Id)
                TeamPlayers[character.Team.Index].Remove(character);
            TeamPlayers[team.Index].Add(character);
            character.Client.SetSkin((PedHash)team.SkinHash);
            if (character.Team == null || character.Team.Id != team.Id)
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.PlayerTeamChange, team.Name);
            character.Team = team;
        }

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected Teams GetTeamWithFewestPlayer()
        {
            int teamindexwithfewest = 1;
            int fewestamount = int.MaxValue;
            int length = TeamPlayers.Length;
            for (int i = 1; i < length; ++i) {
                List<TDSPlayer> entry = TeamPlayers[i];
                if (entry.Count < fewestamount
                    || entry.Count == fewestamount && Utils.Rnd.Next(length - 1) == 1)
                {
                    fewestamount = entry.Count;
                    teamindexwithfewest = i;
                }
            }
            return Teams[teamindexwithfewest];
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in TeamPlayers)
            {
                entry.Clear();
            }
        }

        protected void MixTeams()
        {
            ClearTeamPlayersLists();
            foreach (TDSPlayer character in Players)
            {
                if (character.Team == null)
                    continue;
                if (character.Team.Index == 0)
                    TeamPlayers[0].Add(character);  // because he is already in that team
                else
                    SetPlayerTeam(character, GetTeamWithFewestPlayer());
            }
        }

        protected void ClearTeamPlayersAmounts()
        {
            foreach (var team in SyncedTeamDatas)
            {
                team.AmountPlayers.AmountAlive = 0;
                team.AmountPlayers.Amount = 0;
            }
        }

        public void SendTeamOrder(TDSPlayer character, ETeamOrder teamOrder)
        {
            if (!teamOrderDict.ContainsKey(teamOrder))
                return;

            Teams team = character.Team;
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(teamOrderDict[teamOrder]);
        
            string str = $"[TEAM] {{{team.ColorR}|{team.ColorG}|{team.ColorB}}} {character.Client.Name}{{150|0|0}}: ";
            FuncIterateAllPlayers((target, _) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, str + texts[target.Language]);
            }, team.Index);
        }
    }
}

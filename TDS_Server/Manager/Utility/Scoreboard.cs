using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Server.Entity;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility
{

    class Scoreboard : Script
    {

        [RemoteEvent(DToServerEvent.RequestPlayersForScoreboard)]
        public static void SendDataToPlayer(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (player.CurrentLobby == null || player.CurrentLobby.Id == 0)
            {
                var entries = GetDataForMainmenu();
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncScoreboardData, JsonConvert.SerializeObject(entries));
            }
            else
            {
                var entries = GetDataForLobby(player.CurrentLobby.Id);
                if (entries == null)
                    return;
                var lobbydata = GetDataForMainmenu().Where(d => d.Id != player.CurrentLobby.Id);
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncScoreboardData, JsonConvert.SerializeObject(entries), JsonConvert.SerializeObject(lobbydata));
            }

        }

        private static List<SyncedScoreboardMainmenuLobbyDataDto> GetDataForMainmenu()
        {
            List<SyncedScoreboardMainmenuLobbyDataDto> list = new List<SyncedScoreboardMainmenuLobbyDataDto>();
            foreach (Lobby lobby in LobbyManager.Lobbies)
            {
                int playerscount = lobby.Players.Count;
                string playersstr = null;
                if (playerscount > 0)
                    playersstr = string.Join(", ", lobby.Players.Select(p => p.Client.Name).OrderBy(n => n));

                SyncedScoreboardMainmenuLobbyDataDto entry = new SyncedScoreboardMainmenuLobbyDataDto()
                {
                    Id = (int)lobby.Id,
                    IsOfficial = lobby.IsOfficial,
                    CreatorName = lobby.CreatorName,
                    LobbyName = lobby.Name,
                    PlayersCount = playerscount,
                    PlayersStr = playersstr
                };
                list.Add(entry);
            }
            return list;
        }

        private static List<SyncedScoreboardLobbyDataDto> GetDataForLobby(uint lobbyId)
        {
            Lobby lobby = LobbyManager.Lobbies.Where(l => l.Id == lobbyId).FirstOrDefault();
            if (lobby == null)
                return null;

            var list = new List<SyncedScoreboardLobbyDataDto>();
            foreach (var player in lobby.Players)
            {
                SyncedScoreboardLobbyDataDto entry = new SyncedScoreboardLobbyDataDto()
                {
                    Name = player.Client.Name,
                    //PlaytimeMinutes = player.Playtime,
                    Kills = player.CurrentLobbyStats.Kills,
                    Assists = player.CurrentLobbyStats.Assists,
                    Deaths = player.CurrentLobbyStats.Deaths,
                    TeamIndex = player.Team.Index
                };
                list.Add(entry);
            }
            return list;
        }
    }
}
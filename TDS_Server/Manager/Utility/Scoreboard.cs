using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility
{
    internal class Scoreboard : Script
    {
        [RemoteEvent(DToServerEvent.RequestPlayersForScoreboard)]
        public static void SendDataToPlayer(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (player.CurrentLobby == null || player.CurrentLobby.Id == 0)
            {
                var entries = GetDataForMainmenu();
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncScoreboardData, JsonConvert.SerializeObject(entries.Select(e => e.Json)));
            }
            else
            {
                var entries = GetDataForLobby(player.CurrentLobby.Id);
                if (entries == null)
                    return;
                var lobbydata = GetDataForMainmenu().Where(d => d.Id != player.CurrentLobby?.Id);
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncScoreboardData, JsonConvert.SerializeObject(entries.Select(e => e.Json)), JsonConvert.SerializeObject(lobbydata.Select(l => l.Json)));
            }
        }

        private static List<SyncedScoreboardMainmenuLobbyDataDto> GetDataForMainmenu()
        {
            List<SyncedScoreboardMainmenuLobbyDataDto> list = new List<SyncedScoreboardMainmenuLobbyDataDto>();
            foreach (Lobby lobby in LobbyManager.Lobbies)
            {
                int playerscount = lobby.Players.Count;
                string playersstr = string.Empty;
                if (playerscount > 0)
                    playersstr = string.Join(", ", lobby.Players.Select(p => p.Client.Name).OrderBy(n => n));

                SyncedScoreboardMainmenuLobbyDataDto entry = new SyncedScoreboardMainmenuLobbyDataDto
                (
                    Id: (int)lobby.Id,
                    IsOfficial: lobby.IsOfficial,
                    CreatorName: lobby.CreatorName,
                    LobbyName: lobby.Name,
                    PlayersCount: playerscount,
                    PlayersStr: playersstr
                );
                list.Add(entry);
            }
            return list;
        }

        private static List<SyncedScoreboardLobbyDataDto>? GetDataForLobby(uint lobbyId)
        {
            Lobby? lobby = LobbyManager.Lobbies.Where(l => l.Id == lobbyId).FirstOrDefault();
            if (lobby == null)
                return null;

            var list = new List<SyncedScoreboardLobbyDataDto>();
            foreach (var player in lobby.Players)
            {
                SyncedScoreboardLobbyDataDto entry = new SyncedScoreboardLobbyDataDto
                (
                    name: player.Client.Name,
                    playtimeMinutes: player.PlayMinutes,
                    kills: (player.CurrentLobbyStats?.Kills ?? 0) + (player.CurrentRoundStats?.Kills ?? 0),
                    assists: (player.CurrentLobbyStats?.Assists ?? 0) + (player.CurrentRoundStats?.Assists ?? 0),
                    deaths: player.CurrentLobbyStats?.Deaths ?? 0,
                    teamIndex: player.Team?.Entity.Index ?? 0
                );
                list.Add(entry);
            }
            return list;
        }
    }
}
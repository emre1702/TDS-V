using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.PlayerManager;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;

namespace TDS_Server.Core.Manager.Utility
{
    internal class Scoreboard : Script
    {
        [RemoteEvent(DToServerEvent.RequestPlayersForScoreboard)]
        public static void SendDataToPlayer(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (player.CurrentLobby is null || player.CurrentLobby.Type == ELobbyType.MainMenu)
            {
                var entries = GetDataForMainmenu();
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncScoreboardData, Serializer.ToClient(entries));
            }
            else
            {
                var entries = GetDataForLobby(player.CurrentLobby.Id);
                if (entries is null)
                    return;
                var lobbydata = GetDataForMainmenu().Where(d => d.Id != player.CurrentLobby?.Id);
                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncScoreboardData, Serializer.ToClient(entries), Serializer.ToClient(lobbydata));
            }
        }

        private static List<SyncedScoreboardMainmenuLobbyDataDto> GetDataForMainmenu()
        {
            List<SyncedScoreboardMainmenuLobbyDataDto> list = new List<SyncedScoreboardMainmenuLobbyDataDto>();
            foreach (Lobby lobby in LobbyManager.Lobbies.Where(l => l.Type != ELobbyType.MapCreateLobby))
            {
                int playerscount = lobby.Players.Count;
                string playersstr = string.Empty;
                if (playerscount > 0)
                    playersstr = string.Join(", ", lobby.Players.Select(p => p.DisplayName).OrderBy(n => n));

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

        private static List<SyncedScoreboardLobbyDataDto>? GetDataForLobby(int lobbyId)
        {
            Lobby? lobby = LobbyManager.Lobbies.Where(l => l.Id == lobbyId).FirstOrDefault();
            if (lobby is null)
                return null;

            var list = new List<SyncedScoreboardLobbyDataDto>();
            foreach (var player in lobby.Players)
            {
                SyncedScoreboardLobbyDataDto entry = new SyncedScoreboardLobbyDataDto
                (
                    name: player.DisplayName,
                    playtimeMinutes: player.PlayMinutes,
                    kills: (int)((player.CurrentLobbyStats?.Kills ?? 0) + (player.CurrentRoundStats?.Kills ?? 0)),
                    assists: (int)((player.CurrentLobbyStats?.Assists ?? 0) + (player.CurrentRoundStats?.Assists ?? 0)),
                    deaths: player.CurrentLobbyStats?.Deaths ?? 0,
                    teamIndex: player.Team?.Entity.Index ?? 0
                );
                list.Add(entry);
            }
            return list;
        }
    }
}
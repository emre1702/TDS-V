using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class ScoreboardHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;

        public ScoreboardHandler(LobbiesHandler lobbiesHandler)
        {
            _lobbiesHandler = lobbiesHandler;

            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.RequestPlayersForScoreboard, this, OnRequestPlayersForScoreboard);
        }

        public async void OnRequestPlayersForScoreboard(ITDSPlayer player)
        {
            if (player.Lobby is null || GetShowAllLobbies(player.Lobby.Type))
            {
                var entries = await GetDataForMainmenu();
                var entriesJson = Serializer.ToClient(entries);
                NAPI.Task.Run(() =>
                    player.TriggerEvent(ToClientEvent.SyncScoreboardData, entriesJson));
            }
            else
            {
                var entries = await GetDataForLobby(player.Lobby.Entity.Id);
                if (entries is null)
                    return;
                var lobbydata = (await GetDataForMainmenu()).Where(d => d.Id != player.Lobby?.Entity.Id);

                var entriesJson = Serializer.ToClient(entries);
                var lobbyDataJson = Serializer.ToClient(lobbydata);
                NAPI.Task.Run(() =>
                    player.TriggerEvent(ToClientEvent.SyncScoreboardData, entriesJson, lobbyDataJson));
            }
        }

        private async Task<List<SyncedScoreboardLobbyDataDto>?> GetDataForLobby(int lobbyId)
        {
            var lobby = _lobbiesHandler.Lobbies.Where(l => l.Entity.Id == lobbyId).FirstOrDefault();
            if (lobby is null)
                return null;

            var list = new List<SyncedScoreboardLobbyDataDto>();
            await lobby.Players.Do(player =>
            {
                var entry = new SyncedScoreboardLobbyDataDto
                (
                    name: player.DisplayName,
                    playtimeMinutes: player.PlayMinutes,
                    kills: ((player.LobbyStats?.Kills ?? 0) + (player.CurrentRoundStats?.Kills ?? 0)),
                    assists: ((player.LobbyStats?.Assists ?? 0) + (player.CurrentRoundStats?.Assists ?? 0)),
                    deaths: player.LobbyStats?.Deaths ?? 0,
                    teamIndex: player.Team?.Entity.Index ?? 0
                );
                list.Add(entry);
            });
            return list;
        }

        private async Task<List<SyncedScoreboardMainmenuLobbyDataDto>> GetDataForMainmenu()
        {
            List<SyncedScoreboardMainmenuLobbyDataDto> list = new List<SyncedScoreboardMainmenuLobbyDataDto>();
            foreach (var lobby in _lobbiesHandler.Lobbies.Where(l => !GetIgnoreLobbyInScoreboard(l)))
            {
                int playersCount = lobby.Players.Count;
                string playersStr = string.Empty;
                if (playersCount > 0)
                {
                    var playerNames = await lobby.Players.GetOrderedNames();
                    playersStr = string.Join(", ", playerNames);
                }

                SyncedScoreboardMainmenuLobbyDataDto entry = new SyncedScoreboardMainmenuLobbyDataDto
                (
                    Id: (int)lobby.Entity.Id,
                    IsOfficial: lobby.IsOfficial,
                    CreatorName: lobby.Entity.Owner?.Name ?? "??",
                    LobbyName: lobby.Entity.Name,
                    PlayersCount: playersCount,
                    PlayersStr: playersStr
                );
                list.Add(entry);
            }
            return list;
        }

        private bool GetIgnoreLobbyInScoreboard(IBaseLobby lobby)
            => (lobby.Type == LobbyType.MapCreateLobby && lobby.IsOfficial)     // Dummy map create lobby
            || lobby.Type == LobbyType.CharCreateLobby;

        private bool GetShowAllLobbies(LobbyType myLobbyType)
            => myLobbyType == LobbyType.MainMenu || myLobbyType == LobbyType.CharCreateLobby;
    }
}

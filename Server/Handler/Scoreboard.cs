﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler
{
    public class ScoreboardHandler
    {
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;

        public ScoreboardHandler(LobbiesHandler lobbiesHandler, ILoggingHandler loggingHandler)
        {
            _lobbiesHandler = lobbiesHandler;
            _loggingHandler = loggingHandler;

            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.RequestPlayersForScoreboard, this, OnRequestPlayersForScoreboard);
        }

        public async void OnRequestPlayersForScoreboard(ITDSPlayer player)
        {
            try
            {
                if (player.Lobby is null || GetShowAllLobbies(player.Lobby.Type))
                {
                    var entries = await GetDataForMainmenu().ConfigureAwait(false);
                    var entriesJson = Serializer.ToClient(entries);
                    NAPI.Task.RunSafe(() =>
                        player.TriggerEvent(ToClientEvent.SyncScoreboardData, entriesJson));
                }
                else
                {
                    var entries = await GetDataForLobby(player.Lobby.Entity.Id).ConfigureAwait(false);
                    if (entries is null)
                        return;
                    var lobbydata = (await GetDataForMainmenu().ConfigureAwait(false)).Where(d => d.Id != player.Lobby?.Entity.Id);

                    var entriesJson = Serializer.ToClient(entries);
                    var lobbyDataJson = Serializer.ToClient(lobbydata);
                    NAPI.Task.RunSafe(() =>
                        player.TriggerEvent(ToClientEvent.SyncScoreboardData, entriesJson, lobbyDataJson));
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private async Task<List<SyncedScoreboardLobbyDataDto>?> GetDataForLobby(int lobbyId)
        {
            var lobby = _lobbiesHandler.Lobbies.Where(l => l.Entity.Id == lobbyId).FirstOrDefault();
            if (lobby is null)
                return null;

            var list = new List<SyncedScoreboardLobbyDataDto>();
            await lobby.Players.DoForAll(player =>
            {
                var entry = new SyncedScoreboardLobbyDataDto
                (
                    name: player.DisplayName,
                    playtimeMinutes: player.PlayTime.Minutes,
                    kills: ((player.LobbyStats?.Kills ?? 0) + (player.CurrentRoundStats?.Kills ?? 0)),
                    assists: ((player.LobbyStats?.Assists ?? 0) + (player.CurrentRoundStats?.Assists ?? 0)),
                    deaths: player.LobbyStats?.Deaths ?? 0,
                    teamIndex: player.Team?.Entity.Index ?? 0
                );
                list.Add(entry);
            }).ConfigureAwait(false);
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
                    var playerNames = await lobby.Players.GetOrderedNames().ConfigureAwait(false);
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
            => lobby.IsOfficial &&
            (lobby.Type == LobbyType.MapCreateLobby
            || lobby.Type == LobbyType.CharCreateLobby && lobby.IsOfficial
            || lobby.Type == LobbyType.DamageTestLobby && lobby.IsOfficial);

        private bool GetShowAllLobbies(LobbyType myLobbyType)
            => myLobbyType == LobbyType.MainMenu || myLobbyType == LobbyType.CharCreateLobby;
    }
}

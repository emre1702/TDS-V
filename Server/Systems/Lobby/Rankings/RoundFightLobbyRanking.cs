using GTANetworkAPI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Rankings;
using TDS.Server.Data.Models;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Rankings
{
    public class RoundFightLobbyRanking : IRoundFightLobbyRanking
    {
        protected IRoundFightLobby Lobby { get; private set; }
        private readonly ISettingsHandler _settingsHandler;
        private readonly IRoundFightLobbyEventsHandler _events;

        public RoundFightLobbyRanking(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, ISettingsHandler settingsHandler)
        {
            Lobby = lobby;
            _settingsHandler = settingsHandler;
            _events = events;

            events.RoundEndRanking += Events_RoundEndRanking;
            events.RemoveAfter += RemoveEvents;
        }

        private void RemoveEvents(IBaseLobby lobby)
        {
            if (_events.RoundEndRanking is { })
                _events.RoundEndRanking -= Events_RoundEndRanking;
            _events.RemoveAfter -= RemoveEvents;
        }

        private async ValueTask Events_RoundEndRanking()
        {
            var rankingStats = GetOrderedRoundRanking();
            if (rankingStats is { })
                await ShowRoundRanking(rankingStats).ConfigureAwait(false);
        }

        private List<RoundPlayerRankingStat>? GetOrderedRoundRanking()
        {
            var list = Lobby.Players.GetPlayers()
                .Where(p => p.CurrentRoundStats is { } && p.Team is { } && !p.Team.IsSpectator)
                .Select(p => new RoundPlayerRankingStat(p))
                .ToList();

            float killsMult = _settingsHandler.ServerSettings.MultiplierRankingKills;
            float assistsMult = _settingsHandler.ServerSettings.MultiplierRankingAssists;
            float damageMult = _settingsHandler.ServerSettings.MultiplierRankingDamage;

            foreach (var ranking in list)
            {
                ranking.Points = (int)(ranking.Kills * killsMult + ranking.Assists * assistsMult + ranking.Damage * damageMult);
            }

            list.Sort((a, b) => a.Points.CompareTo(b.Points) * -1);

            int place = 0;
            foreach (var ranking in list)
            {
                ranking.Place = ++place;
            }

            return list;
        }

        private async Task ShowRoundRanking(List<RoundPlayerRankingStat> rankingStats)
        {
            if (rankingStats is null || rankingStats.Count == 0)
                return;

            var rankingPositions = GetPlayerRankingPositions(rankingStats);
            var (WinnerRemoteId, SecondRemoteId, ThirdRemoteId) = GetPlayerRankingRemoteIds(rankingStats);

            await Lobby.Players.DoInMain(player =>
            {
                if (rankingPositions.TryGetValue(player, out var posData))
                {
                    player.Spawn(posData.Pos, posData.Rot);
                    player.SetInvisible(false);
                }
                else
                {
                    player.Spawn(Constants.RoundRankingSpectatorPosition);
                    player.SetInvisible(true);
                }

                player.Freeze(true);
            }).ConfigureAwait(false);

            var json = Serializer.ToBrowser(rankingStats);
            Lobby.Sync.TriggerEvent(ToClientEvent.StartRankingShowAfterRound, json,
                WinnerRemoteId, SecondRemoteId, ThirdRemoteId);
        }

        private Dictionary<ITDSPlayer, (Vector3 Pos, float Rot)> GetPlayerRankingPositions(List<RoundPlayerRankingStat> rankingStats)
        {
            var data = new Dictionary<ITDSPlayer, (Vector3 Pos, float Rot)>();
            if (rankingStats is null)
                return data;

            for (int i = 0; i < Math.Min(rankingStats.Count, 3); ++i)
            {
                var rankingStat = rankingStats[i];
                if (!rankingStat.Player.Exists)
                    continue;
                var posData = Constants.RoundRankingPositions[i];
                data[rankingStat.Player] = posData;
            }

            return data;
        }

        private (int WinnerRemoteId, int SecondRemoteId, int ThirdRemoteId) GetPlayerRankingRemoteIds(List<RoundPlayerRankingStat> rankingStats)
        {
            if (rankingStats is null)
                return (0, 0, 0);

            var winnerRemoteId = rankingStats.ElementAtOrDefault(0)?.Player.RemoteId ?? 0;
            var secondRemoteId = rankingStats.ElementAtOrDefault(1)?.Player.RemoteId ?? 0;
            var thirdRemoteId = rankingStats.ElementAtOrDefault(2)?.Player.RemoteId ?? 0;

            return (winnerRemoteId, secondRemoteId, thirdRemoteId);
        }
    }
}

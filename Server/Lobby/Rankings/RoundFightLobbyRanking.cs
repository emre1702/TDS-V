using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Rankings;
using TDS_Server.Data.Models;
using TDS_Server.LobbySystem.Players;
using TDS_Server.LobbySystem.Sync;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Rankings
{
    public class RoundFightLobbyRanking : IRoundFightLobbyRanking
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly RoundFightLobbyPlayers _players;
        private readonly RoundFightLobbySync _sync;

        public RoundFightLobbyRanking(IRoundFightLobbyEventsHandler events, RoundFightLobbyPlayers players, RoundFightLobbySync sync, ISettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;
            _players = players;
            _sync = sync;
            events.RoundEndRanking += Events_RoundEndRanking;
        }

        private async ValueTask Events_RoundEndRanking()
        {
            var rankingStats = GetOrderedRoundRanking();
            if (rankingStats is { })
                await ShowRoundRanking(rankingStats);
        }

        private List<RoundPlayerRankingStat>? GetOrderedRoundRanking()
        {
            var list = _players.GetPlayers()
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
            var rankingRemoteIds = GetPlayerRankingRemoteIds(rankingStats);

            await _players.DoInMain(player =>
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
            });

            var json = Serializer.ToBrowser(rankingStats);
            _sync.TriggerEvent(ToClientEvent.StartRankingShowAfterRound, json,
                rankingRemoteIds.WinnerRemoteId, rankingRemoteIds.SecondRemoteId, rankingRemoteIds.ThirdRemoteId);
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

            var winnerRemoteId = rankingStats.ElementAtOrDefault(0).Player?.RemoteId ?? 0;
            var secondRemoteId = rankingStats.ElementAtOrDefault(1).Player?.RemoteId ?? 0;
            var thirdRemoteId = rankingStats.ElementAtOrDefault(2).Player?.RemoteId ?? 0;

            return (winnerRemoteId, secondRemoteId, thirdRemoteId);
        }
    }
}

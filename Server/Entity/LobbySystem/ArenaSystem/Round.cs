using AltV.Net.Async;
using AltV.Net.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.Entity.LobbySystem.ArenaSystem
{
    partial class Arena
    {
        public LobbyRoundSettings RoundSettings => Entity.LobbyRoundSettings;

        public IGamemode? CurrentGameMode;

        private TDSTimer? _nextRoundStatusTimer;

        public readonly Dictionary<RoundStatus, uint> DurationsDict = new Dictionary<RoundStatus, uint>
        {
            [RoundStatus.MapClear] = 1 * 1000,
            [RoundStatus.NewMapChoose] = 4 * 1000,
            [RoundStatus.Countdown] = 5 * 1000,
            [RoundStatus.Round] = 4 * 60 * 1000,
            [RoundStatus.RoundEnd] = 1 * 1000,
            [RoundStatus.RoundEndRanking] = 10 * 1000,
            [RoundStatus.None] = 0
        };

        private readonly Dictionary<RoundStatus, Action> _roundStatusMethod = new Dictionary<RoundStatus, Action>();

        private readonly Dictionary<RoundStatus, RoundStatus> _nextRoundStatsDict = new Dictionary<RoundStatus, RoundStatus>
        {
            [RoundStatus.MapClear] = RoundStatus.NewMapChoose,
            [RoundStatus.NewMapChoose] = RoundStatus.Countdown,
            [RoundStatus.Countdown] = RoundStatus.Round,
            [RoundStatus.Round] = RoundStatus.RoundEnd,
            [RoundStatus.RoundEnd] = RoundStatus.RoundEndRanking,
            [RoundStatus.RoundEndRanking] = RoundStatus.MapClear,
            [RoundStatus.None] = RoundStatus.NewMapChoose
        };

        private readonly Dictionary<MapType, Func<Arena, MapDto, IServiceProvider, IGamemode>> _gameModeByMapType
            = new Dictionary<MapType, Func<Arena, MapDto, IServiceProvider, IGamemode>>
            {
                [MapType.Normal] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<IDeathmatch>(serviceProvider, lobby, map),
                [MapType.Bomb] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<IBomb>(serviceProvider, lobby, map),
                [MapType.Sniper] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<ISniper>(serviceProvider, lobby, map),
                [MapType.Gangwar] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<IGangwar>(serviceProvider, lobby, map)
            };

        public RoundStatus CurrentRoundStatus = RoundStatus.None;
        public ITDSPlayer? CurrentRoundEndBecauseOfPlayer;
        public bool RemoveAfterOneRound { get; set; }
        public RoundEndReason CurrentRoundEndReason { get; private set; }
        public Dictionary<ILanguage, string>? RoundEndReasonText { get; private set; }

        private ITeam? _currentRoundEndWinnerTeam;

        private List<RoundPlayerRankingStat>? _ranking;

        public async void SetRoundStatus(RoundStatus status, RoundEndReason roundEndReason = RoundEndReason.Time)
        {
            CurrentRoundStatus = status;
            if (status == RoundStatus.RoundEnd)
                CurrentRoundEndReason = roundEndReason;
            RoundStatus nextStatus = _nextRoundStatsDict[status];
            _nextRoundStatusTimer?.Kill();
            if (!IsEmpty())
            {
                _nextRoundStatusTimer = new TDSTimer(() =>
                {
                    if (IsEmpty() && CurrentGameMode?.CanEndRound(RoundEndReason.Empty) != false)
                        SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Empty);
                    else
                        SetRoundStatus(nextStatus);
                }, DurationsDict[status]);
                try
                {
                    if (_roundStatusMethod.ContainsKey(status))
                        await AltAsync.Do(_roundStatusMethod[status]).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LoggingHandler.LogError($"Could not call method for round status {status.ToString()} for lobby {Name} with Id {Id}. Exception: " + ex.Message, ex.StackTrace ?? "?");
                    SendAllPlayerLangMessage((lang) => lang.LOBBY_ERROR_REMOVE);
                    if (!IsOfficial)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Remove();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
            else if (CurrentRoundStatus != RoundStatus.RoundEnd)
            {
                await AltAsync.Do(() =>
                {
                   _roundStatusMethod[RoundStatus.RoundEnd]();
                   _roundStatusMethod[RoundStatus.MapClear]();
                });
                
                CurrentRoundStatus = RoundStatus.None;
            }
        }

        private void StartMapClear()
        {
            DeleteMapBlips();
            ClearTeamPlayersAmounts();
            SendEvent(ToClientEvent.MapClear);

            CurrentGameMode?.StartMapClear();
        }

        private void StartNewMapChoose()
        {
            MapDto? nextMap = GetNextMap();
            if (nextMap == null)
                return;
            SavePlayerLobbyStats = !nextMap.Info.IsNewMap;
            if (nextMap.Info.IsNewMap)
                SendAllPlayerLangNotification(lang => lang.TESTING_MAP_NOTIFICATION, flashing: true);
            CurrentGameMode = _gameModeByMapType[nextMap.Info.Type](this, nextMap, _serviceProvider);
            CurrentGameMode?.StartMapChoose();
            CreateTeamSpawnBlips(nextMap);
            CreateMapLimitBlips(nextMap);
            if (RoundSettings.MixTeamsAfterRound)
                MixTeams();
            SendEvent(ToClientEvent.MapChange, nextMap.ClientSyncedDataJson);
            _currentMap = nextMap;
            _currentMapSpectatorPosition = _currentMap.LimitInfo.Center?.ToAltV().AddToZ(10);
            RoundEndReasonText = null;
        }

        private void StartRoundCountdown()
        {
            _allRoundWeapons = Entity.LobbyWeapons.Where(w => CurrentGameMode != null ? CurrentGameMode.IsWeaponAllowed(w.Hash) : true);
            SetAllPlayersInCountdown();
            CurrentGameMode?.StartRoundCountdown();
        }

        private void StartRound()
        {
            StartRoundForAllPlayer();
            CurrentGameMode?.StartRound();
        }

        private async void EndRound()
        {
            bool isEmpty = IsEmpty();
            if (!_dontRemove && (Entity.IsTemporary && isEmpty || RemoveAfterOneRound))
            {
                await Remove();
                return;
            }

            await AltAsync.Do(() =>
            {
                if (!isEmpty)
                {
                    _currentRoundEndWinnerTeam = GetRoundWinnerTeam();
                    RoundEndReasonText = GetRoundEndReasonText(_currentRoundEndWinnerTeam);

                    FuncIterateAllPlayers((player, team) =>
                    {
                        player.SendEvent(ToClientEvent.RoundEnd, team is null || team.IsSpectator, RoundEndReasonText != null ? RoundEndReasonText[player.Language] : string.Empty, _currentMap?.BrowserSyncedData.Id ?? 0);
                        if (player.Lifes > 0 && _currentRoundEndWinnerTeam != null && team != _currentRoundEndWinnerTeam && CurrentRoundEndReason != RoundEndReason.Death)
                            player?.Kill();
                        player.Lifes = 0;
                    });
                }

                foreach (var team in Teams)
                {
                    team.AlivePlayers?.Clear();
                }

                DmgSys.Clear();

                CurrentGameMode?.StopRound();

                if (!isEmpty)
                {
                    RewardAllPlayer();
                    _ranking = GetOrderedRoundRanking();
                    SaveAllPlayerRoundStats();
                }
                else
                {
                    _ranking = null;
                }
            });

            await ExecuteForDBAsync(async (dbContext) =>
            {
                await dbContext.SaveChangesAsync();
            });

            _serverStatsHandler.AddArenaRound(CurrentRoundEndReason, IsOfficial);
        }

        private void ShowRoundRanking()
        {
            if (_ranking is null || _ranking.Count == 0)
                return;

            try
            {
                ITDSPlayer winner = _ranking.First().Player;
                ITDSPlayer? second = _ranking.ElementAtOrDefault(1)?.Player;
                ITDSPlayer? third = _ranking.ElementAtOrDefault(2)?.Player;

                //Vector3 rot = new Vector3(0, 0, 345);
                winner.Spawn(new Position(-425.48f, 1123.55f, 325.85f), 345f);
                winner.Freeze(true);
                winner.Dimension = (int)Dimension;

                if (second is { })
                {
                    second.Spawn(new Position(-427.03f, 1123.21f, 325.85f), 345);
                    second.Freeze(true);
                    second.Dimension = (int)Dimension;
                }

                if (third is { })
                {
                    third.Spawn(new Position(-424.33f, 1122.5f, 325.85f), 345);
                    third.Freeze(true);
                    third.Dimension = (int)Dimension;
                }

                var othersPos = new Position(-425.48f, 1123.55f, 335.85f);
                foreach (var player in Players.Values)
                {
                    if (player != winner && player != second && player != third)
                        player.Position = othersPos;
                }

                string json = Serializer.ToBrowser(_ranking);
                if (third is { })
                    SendEvent(ToClientEvent.StartRankingShowAfterRound, json, winner, second!, third);
                else if (second is { })
                    SendEvent(ToClientEvent.StartRankingShowAfterRound, json, winner, second);
                else
                    SendEvent(ToClientEvent.StartRankingShowAfterRound, json, winner);
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError("Error occured: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
            }
        }

        private void RoundCheckForEnoughAlive()
        {
            int teamsInRound = GetTeamAmountStillInRound();
            if (teamsInRound <= 1 && CurrentGameMode?.CanEndRound(RoundEndReason.NewPlayer) != false)
            {
                SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.Death);
            }
        }

        private ITeam? GetRoundWinnerTeam()
        {
            if (CurrentGameMode?.WinnerTeam is { })
                return CurrentGameMode.WinnerTeam;
            return CurrentRoundEndReason switch
            {
                RoundEndReason.Death => GetTeamStillInRound(),
                RoundEndReason.Time => GetTeamWithHighestHP(),
                _ => null,
            };
        }

        private Dictionary<ILanguage, string>? GetRoundEndReasonText(ITeam? winnerTeam)
        {
            return CurrentRoundEndReason switch
            {
                RoundEndReason.Death => LangHelper.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? string.Format(lang.ROUND_END_DEATH_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_DEATH_ALL_INFO;
                    }),
                RoundEndReason.Time => LangHelper.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? string.Format(lang.ROUND_END_TIME_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_TIME_TIE_INFO;
                    }),
                RoundEndReason.BombExploded => LangHelper.GetLangDictionary(lang =>
                   {
                       return string.Format(lang.ROUND_END_BOMB_EXPLODED_INFO, winnerTeam?.Entity.Name ?? "-");
                   }),
                RoundEndReason.BombDefused => LangHelper.GetLangDictionary(lang =>
                   {
                       return string.Format(lang.ROUND_END_BOMB_DEFUSED_INFO, winnerTeam?.Entity.Name ?? "-");
                   }),
                RoundEndReason.Command => LangHelper.GetLangDictionary(lang =>
                   {
                       return string.Format(lang.ROUND_END_COMMAND_INFO, CurrentRoundEndBecauseOfPlayer?.DisplayName ?? "-");
                   }),
                RoundEndReason.NewPlayer => LangHelper.GetLangDictionary(lang =>
                   {
                       return lang.ROUND_END_NEW_PLAYER_INFO;
                   }),
                RoundEndReason.TargetEmpty => LangHelper.GetLangDictionary(lang =>
                   {
                       return lang.ROUND_END_TARGET_EMPTY_INFO;
                   }),
                RoundEndReason.Error => LangHelper.GetLangDictionary(lang =>
                   {
                       return lang.ERROR_INFO;
                   }),
                RoundEndReason.PlayerWon => LangHelper.GetLangDictionary(lang =>
                    {
                        return string.Format(lang.PLAYER_WON_INFO, CurrentRoundEndBecauseOfPlayer?.DisplayName ?? "-");
                    }),

                _ => null,
            };
        }
    }
}

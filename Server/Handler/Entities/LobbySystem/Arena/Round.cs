using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Entities.GameModes;
using TDS_Server.Handler.Entities.GameModes.Bomb;
using TDS_Server.Handler.Entities.GameModes.Normal;
using TDS_Server.Handler.Entities.GameModes.Sniper;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        public LobbyRoundSettings RoundSettings => Entity.LobbyRoundSettings;

        public GameMode? CurrentGameMode;

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
        private readonly Dictionary<MapType, Func<Arena, MapDto, IServiceProvider, GameMode>> _gameModeByMapType
            = new Dictionary<MapType, Func<Arena, MapDto, IServiceProvider, GameMode>>
            {
                [MapType.Normal] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<Normal>(serviceProvider, lobby, map),
                [MapType.Bomb] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<Bomb>(serviceProvider, lobby, map),
                [MapType.Sniper] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<Sniper>(serviceProvider, lobby, map),
                [MapType.Gangwar] = (lobby, map, serviceProvider) => ActivatorUtilities.CreateInstance<Gangwar>(serviceProvider, lobby, map)
            };

        public RoundStatus CurrentRoundStatus = RoundStatus.None;
        public ITDSPlayer? CurrentRoundEndBecauseOfPlayer;
        public bool RemoveAfterOneRound { get; set; }
        public RoundEndReason CurrentRoundEndReason { get; private set; }
        public Dictionary<ILanguage, string>? RoundEndReasonText { get; private set; }

        private ITeam? _currentRoundEndWinnerTeam;

        private List<RoundPlayerRankingStat>? _ranking;

        public void SetRoundStatus(RoundStatus status, RoundEndReason roundEndReason = RoundEndReason.Time)
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
                        ModAPI.Thread.RunInMainThread(_roundStatusMethod[status]);
                }
                catch (Exception ex)
                {
                    LoggingHandler.LogError($"Could not call method for round status {status.ToString()} for lobby {Name} with Id {Id}. Exception: " + ex.Message, ex.StackTrace ?? "?");
                    SendAllPlayerLangMessage((lang) => lang.LOBBY_ERROR_REMOVE);
                    if (!IsOfficial)
                        ModAPI.Thread.RunInMainThread(async () => await Remove());
                }
            }
            else if (CurrentRoundStatus != RoundStatus.RoundEnd)
            {
                ModAPI.Thread.RunInMainThread(_roundStatusMethod[RoundStatus.RoundEnd]);
                ModAPI.Thread.RunInMainThread(_roundStatusMethod[RoundStatus.MapClear]);
                CurrentRoundStatus = RoundStatus.None;
            }
        }

        private void StartMapClear()
        {
            DeleteMapBlips();
            ClearTeamPlayersAmounts();
            ModAPI.Sync.SendEvent(this, ToClientEvent.MapClear);

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
            ModAPI.Sync.SendEvent(this, ToClientEvent.MapChange, nextMap.ClientSyncedDataJson);
            _currentMap = nextMap;
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

            if (!isEmpty)
            {
                _currentRoundEndWinnerTeam = GetRoundWinnerTeam();
                RoundEndReasonText = GetRoundEndReasonText(_currentRoundEndWinnerTeam);

                FuncIterateAllPlayers((character, team) =>
                {
                    character.SendEvent(ToClientEvent.RoundEnd, team is null || team.IsSpectator, RoundEndReasonText != null ? RoundEndReasonText[character.Language] : string.Empty, _currentMap?.BrowserSyncedData.Id ?? 0);
                    if (character.Lifes > 0 && _currentRoundEndWinnerTeam != null && team != _currentRoundEndWinnerTeam && CurrentRoundEndReason != RoundEndReason.Death)
                        character.ModPlayer?.Kill();
                    character.Lifes = 0;
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
            await ExecuteForDBAsync(async (dbContext) =>
            {
                await dbContext.SaveChangesAsync();
            });

            _serverStatsHandler.AddArenaRound(CurrentRoundEndReason, IsOfficial);
        }

        private void ShowRoundRanking()
        {
            if (_ranking is null)
                return;

            try
            {
                ITDSPlayer winner = _ranking.First().Player;
                ITDSPlayer? second = _ranking.ElementAtOrDefault(1)?.Player;
                ITDSPlayer? third = _ranking.ElementAtOrDefault(2)?.Player;

                //Vector3 rot = new Vector3(0, 0, 345);
                winner.Spawn(new Position3D(-425.48, 1123.55, 325.85), 345);
                winner.ModPlayer!.Freeze(true);
                winner.ModPlayer.Dimension = Dimension;

                if (second is { })
                {
                    second.Spawn(new Position3D(-427.03, 1123.21, 325.85), 345);
                    second.ModPlayer!.Freeze(true);
                    second.ModPlayer.Dimension = Dimension;
                }

                if (third is { })
                {
                    third.Spawn(new Position3D(-424.33, 1122.5, 325.85), 345);
                    third.ModPlayer!.Freeze(true);
                    third.ModPlayer.Dimension = Dimension;
                }

                string json = Serializer.ToBrowser(_ranking);
                ModAPI.Sync.SendEvent(this, ToClientEvent.StartRankingShowAfterRound, json, winner.RemoteId, second?.RemoteId ?? 0, third?.RemoteId ?? 0);
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
            if (CurrentGameMode?.WinnerTeam != null)
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

                _ => null,
            };
        }
    }
}

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
using TDS_Server.Handler.Entities.GameModes.Gangwar;
using TDS_Server.Handler.Entities.GameModes.Normal;
using TDS_Server.Handler.Entities.GameModes.Sniper;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Instance;

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
        private readonly Dictionary<MapType, Func<Arena, MapDto, GameMode>> _gameModeByMapType
            = new Dictionary<MapType, Func<Arena, MapDto, GameMode>>
            {
                [MapType.Normal] = (lobby, map) => new Normal(lobby, map),
                [MapType.Bomb] = (lobby, map) => new Bomb(lobby, map),
                [MapType.Sniper] = (lobby, map) => new Sniper(lobby, map),
                [MapType.Gangwar] = (lobby, map) => new Gangwar(lobby, map)
            };

        public RoundStatus CurrentRoundStatus = RoundStatus.None;
        public TDSPlayer? CurrentRoundEndBecauseOfPlayer;
        public bool RemoveAfterOneRound { get; set; }
        public RoundEndReason CurrentRoundEndReason { get; private set; }
        public Dictionary<ILanguage, string>? RoundEndReasonText { get; private set; }

        private Team? _currentRoundEndWinnerTeam;

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
                    _loggingHandler.LogError($"Could not call method for round status {status.ToString()} for lobby {Name} with Id {Id}. Exception: " + ex.Message, ex.StackTrace ?? "?");
                    SendAllPlayerLangMessage((lang) => lang.LOBBY_ERROR_REMOVE);
                    if (!IsOfficial)
                        ModAPI.Thread.RunInMainThread(Remove);
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
            SendAllPlayerEvent(ToClientEvent.MapClear, null);

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
            CurrentGameMode = _gameModeByMapType[nextMap.Info.Type](this, nextMap);
            CurrentGameMode?.StartMapChoose();
            CreateTeamSpawnBlips(nextMap);
            CreateMapLimitBlips(nextMap);
            if (RoundSettings.MixTeamsAfterRound)
                MixTeams();
            SendAllPlayerEvent(ToClientEvent.MapChange, null, nextMap.ClientSyncedDataJson);
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
                Remove();
                return;
            }

            if (!isEmpty)
            {
                _currentRoundEndWinnerTeam = GetRoundWinnerTeam();
                RoundEndReasonText = GetRoundEndReasonText(_currentRoundEndWinnerTeam);

                FuncIterateAllPlayers((character, team) =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(character.Player, ToClientEvent.RoundEnd, RoundEndReasonText != null ? RoundEndReasonText[character.Language] : string.Empty, _currentMap?.BrowserSyncedData.Id ?? 0);
                    if (character.Lifes > 0 && _currentRoundEndWinnerTeam != null && team != _currentRoundEndWinnerTeam && CurrentRoundEndReason != ERoundEndReason.Death)
                        character.Player!.Kill();
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

            ServerTotalStatsManager.AddArenaRound(CurrentRoundEndReason, IsOfficial);
            ServerDailyStatsManager.AddArenaRound(CurrentRoundEndReason, IsOfficial);

        }

        private void ShowRoundRanking()
        {
            if (_ranking is null)
                return;

            try
            {
                Player winner = _ranking.First().Player.Player!;
                Player? second = _ranking.ElementAtOrDefault(1)?.Player.Player;
                Player? third = _ranking.ElementAtOrDefault(2)?.Player.Player;

                //Vector3 rot = new Vector3(0, 0, 345);

                NAPI.Player.SpawnPlayer(winner, new Vector3(-425.48, 1123.55, 325.85), 345);
                Workaround.FreezePlayer(winner, true);
                winner.Dimension = Dimension;

                if (second is { })
                {
                    NAPI.Player.SpawnPlayer(second, new Vector3(-427.03, 1123.21, 325.85), 345);
                    Workaround.FreezePlayer(second, true);
                    second.Dimension = Dimension;
                }

                if (third is { })
                {
                    NAPI.Player.SpawnPlayer(third, new Vector3(-424.33, 1122.5, 325.85), 345);
                    Workaround.FreezePlayer(third, true);
                    third.Dimension = Dimension;
                }

                string json = Serializer.ToBrowser(_ranking);
                SendAllPlayerEvent(ToClientEvent.StartRankingShowAfterRound, null, json, winner.Handle.Value, second?.Handle.Value ?? 0, third?.Handle.Value ?? 0);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log("Error occured: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
            }
        }

        private void RoundCheckForEnoughAlive()
        {
            int teamsInRound = GetTeamAmountStillInRound();
            int teamsWithPlayers = GetTeamAmount(true);
            // if there is only 1 team playing, it should continue to play
            // if there are 2+ teams playing and there is only 1 team alive, end the round
            if ((teamsWithPlayers <= 1 ? (teamsInRound < 1) : teamsInRound < 2) && CurrentGameMode?.CanEndRound(ERoundEndReason.NewPlayer) != false)
            {
                SetRoundStatus(RoundStatus.RoundEnd, ERoundEndReason.Death);
            }
        }

        private Team? GetRoundWinnerTeam()
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

        private Dictionary<ILanguage, string>? GetRoundEndReasonText(Team? winnerTeam)
        {
            return CurrentRoundEndReason switch
            {
                ERoundEndReason.Death => LangUtils.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? Utils.GetReplaced(lang.ROUND_END_DEATH_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_DEATH_ALL_INFO;
                    }),
                ERoundEndReason.Time => LangUtils.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? Utils.GetReplaced(lang.ROUND_END_TIME_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_TIME_TIE_INFO;
                    }),
                ERoundEndReason.BombExploded => LangUtils.GetLangDictionary(lang =>
                   {
                       return Utils.GetReplaced(lang.ROUND_END_BOMB_EXPLODED_INFO, winnerTeam?.Entity.Name ?? "-");
                   }),
                ERoundEndReason.BombDefused => LangUtils.GetLangDictionary(lang =>
                   {
                       return Utils.GetReplaced(lang.ROUND_END_BOMB_DEFUSED_INFO, winnerTeam?.Entity.Name ?? "-");
                   }),
                ERoundEndReason.Command => LangUtils.GetLangDictionary(lang =>
                   {
                       return Utils.GetReplaced(lang.ROUND_END_COMMAND_INFO, CurrentRoundEndBecauseOfPlayer?.DisplayName ?? "-");
                   }),
                ERoundEndReason.NewPlayer => LangUtils.GetLangDictionary(lang =>
                   {
                       return lang.ROUND_END_NEW_PLAYER_INFO;
                   }),
                ERoundEndReason.TargetEmpty => LangUtils.GetLangDictionary(lang =>
                   {
                       return lang.ROUND_END_TARGET_EMPTY_INFO;
                   }),
                ERoundEndReason.Error => LangUtils.GetLangDictionary(lang =>
                   {
                       return lang.ERROR_INFO;
                   }),

                _ => null,
            };
        }
    }
}

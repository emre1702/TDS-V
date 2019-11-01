using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Enum;
using TDS_Server.Instance.GameModes;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Stats;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server.Dto;
using System.Linq;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        public LobbyRoundSettings RoundSettings => LobbyEntity.LobbyRoundSettings;

        public GameMode? CurrentGameMode;

        private TDSTimer? _nextRoundStatusTimer;

        public readonly Dictionary<ERoundStatus, uint> DurationsDict = new Dictionary<ERoundStatus, uint>
        {
            [ERoundStatus.MapClear] = 1 * 1000,
            [ERoundStatus.NewMapChoose] = 4 * 1000,
            [ERoundStatus.Countdown] = 5 * 1000,
            [ERoundStatus.Round] = 4 * 60 * 1000,
            [ERoundStatus.RoundEnd] = 1 * 1000,
            [ERoundStatus.RoundEndRanking] = 10 * 1000,
            [ERoundStatus.None] = 0
        };

        private readonly Dictionary<ERoundStatus, Action> _roundStatusMethod = new Dictionary<ERoundStatus, Action>();

        private readonly Dictionary<ERoundStatus, ERoundStatus> _nextRoundStatsDict = new Dictionary<ERoundStatus, ERoundStatus>
        {
            [ERoundStatus.MapClear] = ERoundStatus.NewMapChoose,
            [ERoundStatus.NewMapChoose] = ERoundStatus.Countdown,
            [ERoundStatus.Countdown] = ERoundStatus.Round,
            [ERoundStatus.Round] = ERoundStatus.RoundEnd,
            [ERoundStatus.RoundEnd] = ERoundStatus.RoundEndRanking,
            [ERoundStatus.RoundEndRanking] = ERoundStatus.MapClear,
            [ERoundStatus.None] = ERoundStatus.NewMapChoose
        };
        private readonly Dictionary<EMapType, Func<Arena, MapDto, GameMode>> _gameModeByMapType
            = new Dictionary<EMapType, Func<Arena, MapDto, GameMode>>
        {
            [EMapType.Normal] = (lobby, map) => new Normal(lobby, map),
            [EMapType.Bomb] = (lobby, map) => new Bomb(lobby, map),
            [EMapType.Sniper] = (lobby, map) => new Sniper(lobby, map)
        };

        public ERoundStatus CurrentRoundStatus = ERoundStatus.None;
        public TDSPlayer? CurrentRoundEndBecauseOfPlayer;

        private Team? _currentRoundEndWinnerTeam;
        private ERoundEndReason _currentRoundEndReason;
        private List<RoundPlayerRankingStat>? _ranking; 

        public void SetRoundStatus(ERoundStatus status, ERoundEndReason roundEndReason = ERoundEndReason.Time)
        {
            CurrentRoundStatus = status;
            if (status == ERoundStatus.RoundEnd)
                _currentRoundEndReason = roundEndReason;
            ERoundStatus nextStatus = _nextRoundStatsDict[status];
            _nextRoundStatusTimer?.Kill();
            if (!IsEmpty())
            {
                _nextRoundStatusTimer = new TDSTimer(() =>
                {
                    if (IsEmpty())
                        SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Empty);
                    else
                        SetRoundStatus(nextStatus);
                }, DurationsDict[status]);
                try
                {
                    if (_roundStatusMethod.ContainsKey(status))
                        NAPI.Task.Run(_roundStatusMethod[status]);
                }
                catch (Exception ex)
                {
                    ErrorLogsManager.Log($"Could not call method for round status {status.ToString()} for lobby {Name} with Id {Id}. Exception: " + ex.Message, ex.StackTrace ?? "?");
                    SendAllPlayerLangMessage((lang) => lang.LOBBY_ERROR_REMOVE);
                    NAPI.Task.Run(Remove);
                }
            }
            else if (CurrentRoundStatus != ERoundStatus.RoundEnd)
            {
                NAPI.Task.Run(_roundStatusMethod[ERoundStatus.RoundEnd]);
                NAPI.Task.Run(_roundStatusMethod[ERoundStatus.MapClear]);
                CurrentRoundStatus = ERoundStatus.None;
            }
        }

        private void StartMapClear()
        {
            CurrentGameMode?.StartMapClear();
            ClearTeamPlayersAmounts();
            SendAllPlayerEvent(DToClientEvent.MapClear, null);
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
            SendAllPlayerEvent(DToClientEvent.MapChange, null, nextMap.Info.Name, nextMap.LimitInfo.EdgesJson, JsonConvert.SerializeObject(nextMap.LimitInfo.Center));
            _currentMap = nextMap;
        }

        private void StartRoundCountdown()
        {
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
            if (LobbyEntity.IsTemporary && IsEmpty())
            {
                Remove();
                return;
            }

            _currentRoundEndWinnerTeam = GetRoundWinnerTeam();
            Dictionary<ILanguage, string>? reasondict = GetRoundEndReasonText(_currentRoundEndWinnerTeam);

            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.RoundEnd, reasondict != null ? reasondict[character.Language] : string.Empty, _currentMap?.SyncedData.Id ?? 0);
                if (character.Lifes > 0 && _currentRoundEndWinnerTeam != null && team != _currentRoundEndWinnerTeam && _currentRoundEndReason != ERoundEndReason.Death)
                    character.Client.Kill();
                character.Lifes = 0;
            });

            foreach (var team in Teams)
            {
                team.AlivePlayers?.Clear();
            }

            DmgSys.Clear();

            DeleteMapBlips();
            CurrentGameMode?.StopRound();

            RewardAllPlayer();
            _ranking = GetOrderedRoundRanking();
            SaveAllPlayerRoundStats();
            await ExecuteForDBAsync(async (dbContext) => 
            {
                await dbContext.SaveChangesAsync();
            });
            
            ServerTotalStatsManager.AddArenaRound(_currentRoundEndReason, IsOfficial);
            ServerDailyStatsManager.AddArenaRound(_currentRoundEndReason, IsOfficial);

        }

        private void ShowRoundRanking()
        {
            if (_ranking is null)
                return;

            try
            {
                Client winner = _ranking.First().Player.Client;
                Client? second = _ranking.ElementAtOrDefault(1)?.Player.Client;
                Client? third = _ranking.ElementAtOrDefault(2)?.Player.Client;

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

                string json = JsonConvert.SerializeObject(_ranking);
                SendAllPlayerEvent(DToClientEvent.StartRankingShowAfterRound, null, json, winner.Handle.Value, second?.Handle.Value ?? 0, third?.Handle.Value ?? 0);
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
            if (teamsWithPlayers <= 1 ? (teamsInRound < 1) : teamsInRound < 2)
            {
                SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Death);
            }
        }

        private Team? GetRoundWinnerTeam()
        {
            if (CurrentGameMode?.WinnerTeam != null)
                return CurrentGameMode.WinnerTeam;
            switch (_currentRoundEndReason)
            {
                case ERoundEndReason.Death:
                    return GetTeamStillInRound();

                case ERoundEndReason.Time:
                    return GetTeamWithHighestHP();

                case ERoundEndReason.Command:
                case ERoundEndReason.NewPlayer:
                case ERoundEndReason.Empty:
                default:
                    return null;
            }
        }

        private Dictionary<ILanguage, string>? GetRoundEndReasonText(Team? winnerTeam)
        {
            switch (_currentRoundEndReason)
            {
                case ERoundEndReason.Death:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? Utils.GetReplaced(lang.ROUND_END_DEATH_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_DEATH_ALL_INFO;
                    });

                case ERoundEndReason.Time:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? Utils.GetReplaced(lang.ROUND_END_TIME_INFO, winnerTeam.Entity.Name) : lang.ROUND_END_TIME_TIE_INFO;
                    });
                case ERoundEndReason.BombExploded:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_BOMB_EXPLODED_INFO, winnerTeam?.Entity.Name ?? "-");
                    });
                case ERoundEndReason.BombDefused:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_BOMB_DEFUSED_INFO, winnerTeam?.Entity.Name ?? "-");
                    });
                case ERoundEndReason.Command:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_COMMAND_INFO, CurrentRoundEndBecauseOfPlayer?.DisplayName ?? "-");
                    });
                case ERoundEndReason.NewPlayer:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return lang.ROUND_END_NEW_PLAYER_INFO;
                    });
                case ERoundEndReason.Empty:
                default:
                    return null;
            }
        }
    }
}
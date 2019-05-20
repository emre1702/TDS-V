using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        private LobbyRoundSettings RoundSettings => LobbyEntity.LobbyRoundSettings;

        private TDSTimer? _nextRoundStatusTimer;

        public readonly Dictionary<ERoundStatus, uint> DurationsDict = new Dictionary<ERoundStatus, uint>
        {
            [ERoundStatus.MapClear] = 1 * 1000,
            [ERoundStatus.NewMapChoose] = 4 * 1000,
            [ERoundStatus.Countdown] = 5 * 1000,
            [ERoundStatus.Round] = 4 * 60 * 1000,
            [ERoundStatus.RoundEnd] = 8 * 1000,
        };

        private readonly Dictionary<ERoundStatus, Action> _roundStatusMethod = new Dictionary<ERoundStatus, Action>();

        private readonly Dictionary<ERoundStatus, ERoundStatus> _nextRoundStatsDict = new Dictionary<ERoundStatus, ERoundStatus>
        {
            [ERoundStatus.MapClear] = ERoundStatus.NewMapChoose,
            [ERoundStatus.NewMapChoose] = ERoundStatus.Countdown,
            [ERoundStatus.Countdown] = ERoundStatus.Round,
            [ERoundStatus.Round] = ERoundStatus.RoundEnd,
            [ERoundStatus.RoundEnd] = ERoundStatus.MapClear,
        };

        private ERoundStatus _currentRoundStatus = ERoundStatus.None;
        private ERoundEndReason _currentRoundEndReason;
        public TDSPlayer? CurrentRoundEndBecauseOfPlayer;
        private Team? _currentRoundEndWinnerTeam;

        public void SetRoundStatus(ERoundStatus status, ERoundEndReason roundEndReason = ERoundEndReason.Time)
        {
            _currentRoundStatus = status;
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
                    _roundStatusMethod[status]();
                }
                catch (Exception ex)
                {
                    ErrorLogsManager.Log($"Could not call method for round status {status.ToString()} for lobby {Name} with Id {Id}. Exception: " + ex.Message, ex.StackTrace);
                    SendAllPlayerLangMessage((lang) => lang.LOBBY_ERROR_REMOVE);
                    Remove();
                }
            }
            else if (_currentRoundStatus != ERoundStatus.RoundEnd)
                _roundStatusMethod[ERoundStatus.RoundEnd]();
        }

        private void StartMapClear()
        {
            if (_currentMap?.IsBomb ?? false)
                ClearBombRound();
            ClearTeamPlayersAmounts();
            SendAllPlayerEvent(DToClientEvent.MapClear, null);
        }

        private void StartNewMapChoose()
        {
            MapDto nextMap = GetNextMap();
            if (nextMap.IsBomb)
                StartBombMapChoose(nextMap);
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
        }

        private void StartRound()
        {
            StartRoundForAllPlayer();

            if (_currentMap?.IsBomb ?? false)
                StartRoundBomb();
        }

        private void EndRound()
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
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.RoundEnd, reasondict != null ? reasondict[character.Language] : string.Empty);
                if (character.Lifes > 0 && _currentRoundEndWinnerTeam != null && team != _currentRoundEndWinnerTeam && _currentRoundEndReason != ERoundEndReason.Death)
                    character.Client.Kill();
                character.Lifes = 0;
            });

            foreach (List<TDSPlayer> entry in AlivePlayers)
                entry.Clear();

            DmgSys.Clear();

            DeleteMapBlips();
            if (_currentMap?.IsBomb ?? false)
                StopBombRound();

            RewardAllPlayer();
            SaveAllPlayerLobbyStats();
        }

        private void RoundCheckForEnoughAlive()
        {
            int teamsinround = GetTeamAmountStillInRound();
            if (teamsinround < 2)
            {
                SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Death);
            }
        }

        private Team? GetRoundWinnerTeam()
        {
            switch (_currentRoundEndReason)
            {
                case ERoundEndReason.Death:
                    return GetTeamStillInRound();

                case ERoundEndReason.Time:
                    return GetTeamWithHighestHP();

                case ERoundEndReason.BombExploded:
                    return _terroristTeam;

                case ERoundEndReason.BombDefused:
                    return _counterTerroristTeam;

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
                        return Utils.GetReplaced(lang.ROUND_END_COMMAND_INFO, CurrentRoundEndBecauseOfPlayer?.Client.Name ?? "-");
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
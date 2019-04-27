using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using TDS_Common.Enum;
using TDS_Server.Instance.Utility;
using TDS_Common.Dto.Map;

namespace TDS_Server.Instance.Lobby
{

    partial class Arena
    {
        private TDSTimer? nextRoundStatusTimer;
        public readonly Dictionary<ERoundStatus, uint> DurationsDict = new Dictionary<ERoundStatus, uint>
        {
            [ERoundStatus.Mapchoose] = 4 * 1000,
            [ERoundStatus.Countdown] = 5 * 1000,
            [ERoundStatus.Round] = 4 * 60 * 1000,
            [ERoundStatus.RoundEnd] = 8 * 1000,
        };
        private readonly Dictionary<ERoundStatus, Action> roundStatusMethod = new Dictionary<ERoundStatus, Action>();
        private readonly Dictionary<ERoundStatus, ERoundStatus> nextRoundStatsDict = new Dictionary<ERoundStatus, ERoundStatus>
        {
            [ERoundStatus.Mapchoose] = ERoundStatus.Countdown,
            [ERoundStatus.Countdown] = ERoundStatus.Round,
            [ERoundStatus.Round] = ERoundStatus.RoundEnd,
            [ERoundStatus.RoundEnd] = ERoundStatus.Mapchoose
        };
        private ERoundStatus currentRoundStatus = ERoundStatus.None;
        private ERoundEndReason currentRoundEndReason;
        public TDSPlayer? CurrentRoundEndBecauseOfPlayer;
        private Team? currentRoundEndWinnerTeam;

        public void SetRoundStatus(ERoundStatus status, ERoundEndReason roundEndReason = ERoundEndReason.Time)
        {
            currentRoundStatus = status;
            if (status == ERoundStatus.RoundEnd)
                currentRoundEndReason = roundEndReason;
            ERoundStatus nextStatus = nextRoundStatsDict[status];
            nextRoundStatusTimer?.Kill();
            if (!IsEmpty())
            {
                nextRoundStatusTimer = new TDSTimer(() =>
                {
                    if (IsEmpty())
                        SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Empty);
                    else
                        SetRoundStatus(nextStatus);
                }, DurationsDict[status]);
                roundStatusMethod[status]();
            }
            else if (currentRoundStatus != ERoundStatus.RoundEnd)
                roundStatusMethod[ERoundStatus.RoundEnd]();
        }

        private void StartMapChoose()
        {
            ClearBombRound();
            ClearTeamPlayersAmounts();
            MapDto nextMap = GetNextMap();
            if (nextMap.IsBomb)
                StartBombMapChoose(nextMap);
            CreateTeamSpawnBlips(nextMap);
            CreateMapLimitBlips(nextMap);
            if (LobbyEntity.MixTeamsAfterRound ?? false)
                MixTeams();
            SendAllPlayerEvent(DToClientEvent.MapChange, null, nextMap.Info.Name, nextMap.LimitInfo.EdgesJson, JsonConvert.SerializeObject(nextMap.LimitInfo.Center));
            currentMap = nextMap;
        }

        private void StartRoundCountdown()
        {
            SetAllPlayersInCountdown();
        }

        private void StartRound()
        {
            StartRoundForAllPlayer();       

            if (currentMap?.IsBomb ?? false)
                StartRoundBomb();
        }

        private void EndRound()
        {
            if (LobbyEntity.IsTemporary && IsEmpty())
            {
                Remove();
                return;
            }

            currentRoundEndWinnerTeam = GetRoundWinnerTeam();
            Dictionary<ILanguage, string>? reasondict = GetRoundEndReasonText(currentRoundEndWinnerTeam);

            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.RoundEnd, reasondict != null ? reasondict[character.Language] : string.Empty);
                if (character.Lifes > 0 && currentRoundEndWinnerTeam != null && team != currentRoundEndWinnerTeam && currentRoundEndReason != ERoundEndReason.Death)
                    character.Client.Kill();
                character.Lifes = 0;
            });

            foreach (List<TDSPlayer> entry in AlivePlayers)
                entry.Clear();

            DmgSys.Clear();

            DeleteMapBlips();
            if (currentMap?.IsBomb ?? false)
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
            switch (currentRoundEndReason)
            {
                case ERoundEndReason.Death:
                    return GetTeamStillInRound();
                case ERoundEndReason.Time:
                    return GetTeamWithHighestHP();
                case ERoundEndReason.BombExploded:
                    return terroristTeam;
                case ERoundEndReason.BombDefused:
                    return counterTerroristTeam;
                case ERoundEndReason.Command:
                case ERoundEndReason.NewPlayer:
                case ERoundEndReason.Empty:
                default:
                    return null;
            }
        }

        private Dictionary<ILanguage, string>? GetRoundEndReasonText(Team? winnerTeam)
        {
            switch (currentRoundEndReason)
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
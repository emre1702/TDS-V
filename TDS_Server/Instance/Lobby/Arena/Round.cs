using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Default;
using TDS_Server.Dto;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interface;
using TDS_Server.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;

namespace TDS_Server.Instance.Lobby
{

    partial class Arena
    {
        private Timer nextRoundStatusTimer;
        private readonly Dictionary<ERoundStatus, uint> durationsDict = new Dictionary<ERoundStatus, uint>
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
        private TDSPlayer currentRoundEndBecauseOfPlayer;

        private void SetRoundStatus(ERoundStatus status, ERoundEndReason roundEndReason = ERoundEndReason.Time)
        {
            currentRoundStatus = status;
            if (status == ERoundStatus.RoundEnd)
                currentRoundEndReason = roundEndReason;
            ERoundStatus nextStatus = nextRoundStatsDict[status];
            nextRoundStatusTimer?.Kill();
            if (!IsEmpty())
            {
                nextRoundStatusTimer = new Timer(() =>
                {
                    if (IsEmpty())
                        SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Empty);
                    else
                        SetRoundStatus(nextStatus);
                }, durationsDict[nextStatus]);
                roundStatusMethod[status]();
            }
            else if (currentRoundStatus != ERoundStatus.RoundEnd)
                roundStatusMethod[ERoundStatus.RoundEnd]();
        }

        private void StartMapChoose()
        {
            ClearBombRound();
            MapDto nextMap = GetNextMap();
            if (nextMap.SyncedData.Type == EMapType.Bomb)
                StartBombMapChoose(nextMap);
            CreateTeamSpawnBlips(nextMap);
            CreateMapLimitBlips(nextMap);
            if (LobbyEntity.MixTeamsAfterRound.Value)
                MixTeams();
            SendAllPlayerEvent(DToClientEvent.MapChange, null, nextMap.SyncedData.Name, JsonConvert.SerializeObject(nextMap.MapLimits), nextMap.MapCenter);
            currentMap = nextMap;
        }

        private void StartRoundCountdown()
        {
            SetAllPlayersInCountdown();
        }

        private void StartRound()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                StartRoundForPlayer(player);
            });

            List<int> amountplayersinplayingteams = TeamPlayers.Skip(1).Select(list => list.Count).ToList(); 
            PlayerAmountInFightSync(amountplayersinplayingteams);            

            if (currentMap.SyncedData.Type == EMapType.Bomb)
                StartRoundBomb();
        }

        private void EndRound()
        {
            if (LobbyEntity.IsTemporary && IsEmpty())
            {
                Remove();
                return;
            }
            RewardAllPlayer();

            foreach (List<TDSPlayer> entry in AlivePlayers)
                entry.Clear();

            Teams winnerTeam = GetRoundWinnerTeam();
            Dictionary<ILanguage, string> reasondict = GetRoundEndReasonText(winnerTeam);

            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.RoundEnd, reasondict != null ? reasondict[character.Language] : string.Empty);
            });

            DmgSys.Clear();

            DeleteMapBlips();
            if (currentMap != null && currentMap.SyncedData.Type == EMapType.Bomb)
                StopBombRound();

        }

        private void RoundCheckForEnoughAlive()
        {
            int teamsinround = GetTeamAmountStillInRound();
            if (teamsinround < 2)
            {
                SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.Death);
            }
        }

        private Teams GetRoundWinnerTeam()
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

        private Dictionary<ILanguage, string> GetRoundEndReasonText(Teams winnerTeam)
        {
            switch (currentRoundEndReason)
            {
                case ERoundEndReason.Death:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return winnerTeam != null ? Utils.GetReplaced(lang.ROUND_END_DEATH_INFO, winnerTeam.Name) : lang.ROUND_END_DEATH_ALL_INFO;
                    });

                case ERoundEndReason.Time:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_TIME_INFO, winnerTeam.Name ?? "-");
                    });
                case ERoundEndReason.BombExploded:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_BOMB_EXPLODED_INFO, winnerTeam.Name ?? "-");
                    });
                case ERoundEndReason.BombDefused:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_BOMB_DEFUSED_INFO, winnerTeam.Name ?? "-");
                    });
                case ERoundEndReason.Command:
                    return LangUtils.GetLangDictionary(lang =>
                    {
                        return Utils.GetReplaced(lang.ROUND_END_COMMAND_INFO, currentRoundEndBecauseOfPlayer.Client.Name ?? "-");
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
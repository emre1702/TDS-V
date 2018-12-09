using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.Default;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Instance.Utility;

namespace TDS.Instance.Lobby
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

        private void SetRoundStatus(ERoundStatus status)
        {
            currentRoundStatus = status;
            roundStatusMethod[status]();
            ERoundStatus nextStatus = nextRoundStatsDict[status];
            nextRoundStatusTimer?.Kill();
            if (!IsEmpty())
            {
                nextRoundStatusTimer = new Timer(() =>
                {
                    if (IsEmpty())
                        SetRoundStatus(ERoundStatus.RoundEnd);
                    else
                        SetRoundStatus(nextStatus);
                }, durationsDict[nextStatus]);
            }
            else if (currentRoundStatus != ERoundStatus.RoundEnd)
                roundStatusMethod[ERoundStatus.RoundEnd]();
        }

        private void StartMapChoose()
        {
            var oldMap = currentMap;
            currentMap = GetNextMap();

            /*DmgSys.EmptyDamagesysData();

            if (oldMap != null && oldMap.SyncData.Type == MapType.BOMB)
                StopRoundBomb();
            if (currentMap.SyncData.Type == MapType.BOMB)
                BombMapChose();
            CreateTeamSpawnBlips();
            CreateMapLimitBlips();
            if (mixTeamsAfterRound)
                MixTeams();
            SendAllPlayerEvent("onClientMapChange", -1, currentMap.SyncData.Name, JsonConvert.SerializeObject(currentMap.MapLimits), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z);

            roundStartTimer = Timer.SetTimer(StartRoundCountdown, mapShowTime);*/
        }

        private void StartRoundCountdown()
        {
            SetAllPlayersInCountdown();
        }

        private void StartRound()
        {
            /*roundEndTimer = Timer.SetTimer(EndRoundTimesup, roundTime);
            AlivePlayers = new List<List<Character>>();
            List<uint> amountinteams = new List<uint>();
            for (int i = 0; i < TeamPlayers.Count; i++)
            {
                uint amountinteam = (uint)TeamPlayers[i].Count;
                if (i != 0)
                    amountinteams.Add(amountinteam);
                AlivePlayers.Add(new List<Character>());
                for (int j = 0; j < amountinteam; j++)
                {
                    StartRoundForPlayer(TeamPlayers[i][j], i);
                }
            }

            PlayerAmountInFightSync(amountinteams);

            if (currentMap.SyncData.Type == MapType.BOMB)
                StartRoundBomb();*/
        }

        private void EndRound()
        {
            if (LobbyEntity.IsTemporary && IsEmpty())
            {
                Remove();
                return;
            }
            RewardAllPlayer();

            foreach (List<Character> entry in AliveOrNotDisappearedPlayers)
            {
                entry.Clear();
            }

            FuncIterateAllPlayers((character, team) =>
            {
                NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvent.RoundEnd/*, reasonlangs[character.Language]*/ );
            });

            /*DeleteMapBlips();
            if (currentMap != null && currentMap.SyncData.Type == MapType.BOMB)
                StopRoundBombAtRoundEnd(); */

        }



        /* private void StartRound()
        {
            
        }


        private void EndRoundTimesup()
        {
            EndRound(GetRoundEndReasonLang(RoundEndReason.TIME, null));
        }

        public void EndRoundEarlier(RoundEndReason reason, object arg)
        {
            roundEndTimer?.Kill();
            countdownTimer?.Kill();
            EndRound(GetRoundEndReasonLang(reason, arg));
        }

        private Dictionary<Language, string> GetRoundEndReasonLang(RoundEndReason reasonenum, object arg)
        {
            Dictionary<Language, string> reasons;
            switch (reasonenum)
            {
                case RoundEndReason.DEATH:
                    if ((int)arg == 0)
                        reasons = ServerLanguage.GetLangDictionary("round_end_death_all");
                    else
                        reasons = ServerLanguage.GetLangDictionary("round_end_death", GetTeamName((int)arg));
                    break;
                case RoundEndReason.TIME:
                    reasons = ServerLanguage.GetLangDictionary("round_end_time");
                    break;
                case RoundEndReason.BOMB:
                    int teamID = (int)arg;
                    if (teamID == terroristTeamID)
                        reasons = ServerLanguage.GetLangDictionary("round_end_bomb_exploded", GetTeamName(teamID));
                    else
                        reasons = ServerLanguage.GetLangDictionary("round_end_bomb_defused", GetTeamName(teamID));
                    break;
                case RoundEndReason.COMMAND:
                    reasons = ServerLanguage.GetLangDictionary("round_end_command", (string)arg);
                    break;
                case RoundEndReason.NEWPLAYER:
                    reasons = ServerLanguage.GetLangDictionary("round_end_command", (string)arg);
                    break;
                default:
                    reasons = new Dictionary<Language, string>();   // Only to not get an error! Won't be used & can't be used!
                    break;
            }
            return reasons;
        }

        private void StartRoundForPlayer(Character character, int teamID)
        {
            NAPI.ClientEvent.TriggerClientEvent(character.Player, "onClientRoundStart", teamID == 0 ? 1 : 0);
            if (teamID != 0)
            {
                character.Lifes = Lifes;
                AlivePlayers[teamID].Add(character);
                character.Player.Freeze(false);
            }
        }

        private void RespawnPlayerInRound(Character character)
        {
            SetPlayerReadyForRound(character);
            character.Player.Freeze(false);
        }*/
    }
}
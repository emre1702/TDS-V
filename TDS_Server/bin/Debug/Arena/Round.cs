using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Default;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Instance.Utility;

namespace TDS.Instance.Lobby
{

    partial class Arena
    {

#warning Todo settable
        private Timer roundStartTimer,
                        countdownTimer,
                        roundEndTimer;
        private uint countdownTime = 5 * 1000;
        private uint roundTime = 4 * 60 * 1000;
        public uint RoundEndTime = 8 * 1000;
        private uint mapShowTime = 4 * 1000;
        private long startTick;
        private bool mixTeamsAfterRound = true;


        public void StartRoundGame()
        {
            StartMapChoose();
        }

        public void StartMapChoose()
        {
            try
            {
                status = ELobbyStatus.Mapchoose;
                if (IsOfficial)
                    RewardAllPlayer();
                DmgSys.EmptyDamagesysData();

                Map oldMap = currentMap;
                currentMap = GetNextMap();
                if (oldMap != null && oldMap.SyncData.Type == EMapType.Bomb)
                    StopRoundBomb();
                if (currentMap.SyncData.Type == EMapType.Bomb)
                    BombMapChose();
                CreateTeamSpawnBlips();
                CreateMapLimitBlips();
                if (mixTeamsAfterRound)
                    MixTeams();
                SendAllPlayerEvent(DCustomEvents.ClientMapChange, null, currentMap.SyncData.Name, JsonConvert.SerializeObject(currentMap.MapLimits), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z);

                roundStartTimer = Timer.SetTimer(StartRoundCountdown, mapShowTime);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }

        private void StartRoundCountdown()
        {
            status = ELobbyStatus.Countdown;
            spectatingMe.Clear();
            SetAllPlayersInCountdown();
            startTick = Environment.TickCount;

            countdownTimer = Timer.SetTimer(StartRound, countdownTime + 400);
        }

        private void StartRound()
        {
            status = ELobbyStatus.Round;
            startTick = Environment.TickCount;
            roundEndTimer = Timer.SetTimer(EndRoundTimesup, roundTime);
            AlivePlayers = new List<List<Character>>();
            List<uint> amountinteams = new List<uint>();
            for (int i = 0; i < teamPlayers.Count; i++)
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
                StartRoundBomb();
        }

        private void EndRound(Dictionary<ELanguage, string> reasonlangs)
        {
            status = ELobbyStatus.Roundend;
            NAPI.Util.ConsoleOutput(status.ToString());
            roundStartTimer?.Kill();
            DeleteMapBlips();
            if (currentMap != null && currentMap.SyncData.Type == MapType.BOMB)
                StopRoundBombAtRoundEnd();
            if (IsSomeoneInLobby())
            {
                roundStartTimer = Timer.SetTimer(StartMapChoose, RoundEndTime);
                FuncIterateAllPlayers((character, teamID) =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(character.Player, "onClientRoundEnd", reasonlangs[character.Language]);
                });
            }
            else if (DeleteWhenEmpty)
            {
                Remove();
            }
        }

        private void EndRoundTimesup()
        {
            EndRound(GetRoundEndReasonLang(ERoundEndReason.Time, null));
        }

#warning Why is arg an object?
        public void EndRoundEarlier(ERoundEndReason reason, object arg)
        {
            roundEndTimer?.Kill();
            countdownTimer?.Kill();
            EndRound(GetRoundEndReasonLang(reason, arg));
        }

        private Dictionary<ELanguage, string> GetRoundEndReasonLang(ERoundEndReason reasonenum, object arg)
        {
            Dictionary<ELanguage, string> reasons;
            switch (reasonenum)
            {
                case ERoundEndReason.Death:
                    if ((int)arg == 0)
                        reasons = ServerLanguage.GetLangDictionary("round_end_death_all");
                    else
                        reasons = ServerLanguage.GetLangDictionary("round_end_death", GetTeamName((int)arg));
                    break;
                case ERoundEndReason.Time:
                    reasons = ServerLanguage.GetLangDictionary("round_end_time");
                    break;
                case ERoundEndReason.Bomb:
                    int teamID = (int)arg;
                    if (teamID == terroristTeamID)
                        reasons = ServerLanguage.GetLangDictionary("round_end_bomb_exploded", GetTeamName(teamID));
                    else
                        reasons = ServerLanguage.GetLangDictionary("round_end_bomb_defused", GetTeamName(teamID));
                    break;
                case ERoundEndReason.Command:
                    reasons = ServerLanguage.GetLangDictionary("round_end_command", (string)arg);
                    break;
                case ERoundEndReason.NewPlayer:
                    reasons = ServerLanguage.GetLangDictionary("round_end_command", (string)arg);
                    break;
                default:
                    reasons = new Dictionary<ELanguage, string>();   // Only to not get an error! Won't be used & can't be used!
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
        }
    }
}
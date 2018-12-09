using GTANetworkAPI;
using System.Collections.Generic;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Lobby.Interfaces;

namespace TDS.Instance.Lobby
{

    partial class Arena : FightLobby, IRound
    {

        public Arena(Lobbies entity) : base(entity)
        {
            roundStatusMethod[ERoundStatus.Mapchoose] = StartMapChoose;
            roundStatusMethod[ERoundStatus.Countdown] = StartRoundCountdown;
            roundStatusMethod[ERoundStatus.Round] = StartRound;
            roundStatusMethod[ERoundStatus.RoundEnd] = EndRound;

            durationsDict[ERoundStatus.Round] = entity.DurationRound * 1000;
        }

        public override void Start()
        {
            base.Start();
            SetRoundStatus(ERoundStatus.Mapchoose);
        }

        protected override void Remove()
        {
            base.Remove();
            nextRoundStatusTimer?.Kill();
            nextRoundStatusTimer = null;

            /*if (currentMap != null && currentMap.SyncData.Type == MapType.BOMB)
                StopRoundBomb();*/
        }

        /*

        public void CheckForEnoughAlive()
        {
            int teamsinround = GetTeamAmountStillInRound();
            if (teamsinround < 2)
            {
                int winnerteam = GetTeamStillInRound();
                EndRoundEarlier(RoundEndReason.DEATH, winnerteam);
            }
        }*/
    }
}

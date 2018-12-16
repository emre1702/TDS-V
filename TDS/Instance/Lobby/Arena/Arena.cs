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

            durationsDict[ERoundStatus.Round] = entity.DurationRound.Value * 1000;

            spawnCounter = new int[entity.Teams.Count-1];
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

            if (currentMap != null && currentMap.SyncedData.Type == EMapType.Bomb)
                StopBombRound();
        }
    }
}

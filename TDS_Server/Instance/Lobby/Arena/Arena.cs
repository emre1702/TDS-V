using TDS_Server.Enum;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena : FightLobby
    {
        public Arena(Lobbies entity) : base(entity)
        {
            _roundStatusMethod[ERoundStatus.MapClear] = StartMapClear;
            _roundStatusMethod[ERoundStatus.NewMapChoose] = StartNewMapChoose;
            _roundStatusMethod[ERoundStatus.Countdown] = StartRoundCountdown;
            _roundStatusMethod[ERoundStatus.Round] = StartRound;
            _roundStatusMethod[ERoundStatus.RoundEnd] = EndRound;

            DurationsDict[ERoundStatus.Round] = (uint)entity.LobbyRoundSettings.RoundTime * 1000;

            terroristTeam = Teams[2];
            counterTerroristTeam = Teams[1];
        }

        public override void Start()
        {
            base.Start();
            SetRoundStatus(ERoundStatus.NewMapChoose);
        }

        protected override void Remove()
        {
            base.Remove();
            _nextRoundStatusTimer?.Kill();
            _nextRoundStatusTimer = null;

            if (_currentMap?.IsBomb ?? false)
                StopBombRound();
        }
    }
}
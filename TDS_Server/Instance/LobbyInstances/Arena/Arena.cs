using TDS_Server.Enums;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Arena : FightLobby
    {
        public GangwarArea? GangwarArea { get; set; }

        public Arena(Lobbies entity) : base(entity)
        {
            _roundStatusMethod[ERoundStatus.MapClear] = StartMapClear;
            _roundStatusMethod[ERoundStatus.NewMapChoose] = StartNewMapChoose;
            _roundStatusMethod[ERoundStatus.Countdown] = StartRoundCountdown;
            _roundStatusMethod[ERoundStatus.Round] = StartRound;
            _roundStatusMethod[ERoundStatus.RoundEnd] = EndRound;
            _roundStatusMethod[ERoundStatus.RoundEndRanking] = ShowRoundRanking;

            DurationsDict[ERoundStatus.Round] = (uint)entity.LobbyRoundSettings.RoundTime * 1000;
            DurationsDict[ERoundStatus.Countdown] = (uint)entity.LobbyRoundSettings.CountdownTime * 1000;

            if (!entity.LobbyRoundSettings.ShowRanking)
            {
                _nextRoundStatsDict[ERoundStatus.RoundEnd] = ERoundStatus.MapClear;
            }
        }

        public Arena(Lobbies entity, GangwarArea gangwarArea, bool removeAfterOneRound = true): this(entity)
        {
            IsGangActionLobby = true;
            RemoveAfterOneRound = removeAfterOneRound;

            GangwarArea = gangwarArea;
            gangwarArea.InLobby = this;
        }

        public override void Start()
        {
            base.Start();
        }

        protected override void Remove()
        {
            if (IsOfficial)
                return;
             base.Remove();
            DeleteMapBlips();
            _nextRoundStatusTimer?.Kill();
            _nextRoundStatusTimer = null;
            CurrentGameMode?.StopRound();
        }
    }
}

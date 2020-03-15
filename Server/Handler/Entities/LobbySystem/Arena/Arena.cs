using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Handler.Entities.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    public partial class Arena : FightLobby
    {
        public GangwarArea? GangwarArea { get; set; }

        private bool _dontRemove;

        public Arena(Lobbies entity, bool isGangActionLobby = false) : base(entity, isGangActionLobby)
        {
            _roundStatusMethod[RoundStatus.MapClear] = StartMapClear;
            _roundStatusMethod[RoundStatus.NewMapChoose] = StartNewMapChoose;
            _roundStatusMethod[RoundStatus.Countdown] = StartRoundCountdown;
            _roundStatusMethod[RoundStatus.Round] = StartRound;
            _roundStatusMethod[RoundStatus.RoundEnd] = EndRound;
            _roundStatusMethod[RoundStatus.RoundEndRanking] = ShowRoundRanking;

            DurationsDict[RoundStatus.Round] = (uint)entity.LobbyRoundSettings.RoundTime * 1000;
            DurationsDict[RoundStatus.Countdown] = (uint)entity.LobbyRoundSettings.CountdownTime * 1000;

            if (!entity.LobbyRoundSettings.ShowRanking)
            {
                _nextRoundStatsDict[RoundStatus.RoundEnd] = RoundStatus.MapClear;
            }
        }

        public Arena(Lobbies entity, GangwarArea gangwarArea, bool removeAfterOneRound = true) : this(entity, true)
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
            _nextRoundStatusTimer?.Kill();
            _nextRoundStatusTimer = null;

            _dontRemove = true;
            EndRound();
            StartMapClear();
            RoundEndReasonText = null;

            if (GangwarArea is { })
            {
                GangwarArea.InLobby = null;
                GangwarArea = null;
            }

            base.Remove();

        }
    }
}

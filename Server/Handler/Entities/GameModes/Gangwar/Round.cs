using TDS_Server.Data.Enums;
using TDS_Server.Handler.Entities.Utility;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Gangwar
    {
        #region Public Methods

        public override bool CanEndRound(RoundEndReason endReason)
        {
            return endReason switch
            {
                RoundEndReason.BombDefused => false,
                RoundEndReason.BombExploded => false,
                RoundEndReason.PlayerWon => false,
                RoundEndReason.Death => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                RoundEndReason.Empty => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                RoundEndReason.NewPlayer => !Lobby.IsGangActionLobby,

                RoundEndReason.TargetEmpty => true,
                RoundEndReason.Time => true,
                RoundEndReason.Command => true,

                _ => true
            };
        }

        public override void StartMapChoose()
        {
            base.StartMapChoose();

            CreateTargetBlip();
            CreateTargetObject();
            CreateTargetTextLabel();
            CreateTargetColShape();
        }

        public override void StartMapClear()
        {
            base.StartMapClear();

            ClearMapFromTarget();
        }

        public override void StartRound()
        {
            base.StartRound();

            // Do we need to force someone to stay at target? If yes, force him! Kill him if he
            // doesn't want to stay there!
            if (!Lobby.IsGangActionLobby && TargetObject is { })
            {
                var playerAtTarget = GetNextTargetMan();
                SetTargetMan(playerAtTarget);
                if (playerAtTarget is { })
                    playerAtTarget.ModPlayer!.Position = TargetObject.Position;
            }
        }

        public override void StartRoundCountdown()
        {
            base.StartRoundCountdown();
        }

        public override void StopRound()
        {
            base.StopRound();

            SetTargetMan(null);
        }

        #endregion Public Methods
    }
}

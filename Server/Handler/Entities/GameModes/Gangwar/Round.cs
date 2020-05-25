using TDS_Server.Data.Enums;
using TDS_Server.Handler.Entities.Utility;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar
    {
        public override void StartMapChoose()
        {
            base.StartMapChoose();

            CreateTargetBlip();
            CreateTargetObject();
            CreateTargetTextLabel();
            CreateTargetColShape();
        }

        public override void StartRoundCountdown()
        {
            base.StartRoundCountdown();

            if (Lobby.IsGangActionLobby)
            {
                LangHelper.SendAllNotification(lang => lang.GANGWAR_PREPARATION_INFO);
                _gangwarArea?.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Map.BrowserSyncedData.Name, OwnerTeam.Entity.Name));

                _gangwarArea?.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_PREPARATION_INFO, _gangwarArea.Map.BrowserSyncedData.Name, AttackerTeam.Entity.Name));

                _gangwarArea?.Attacker.FuncIterate(player =>
                {
                    _ = new Invitation(player.Language.GANGWAR_ATTACK_PREPARATION_INVITATION, player, null, Serializer, InvitationsHandler, onAccept: AcceptAttackPreparationInvitation)
                    {
                        RemoveOnLobbyLeave = true
                    };
                });
            }
        }

        public override void StartRound()
        {
            base.StartRound();

            if (Lobby.IsGangActionLobby)
            {
                Lobby.Entity.Name = $"[GW] {Lobby.GangwarArea!.Attacker!.Entity.Short} - {Lobby.GangwarArea.Owner!.Entity.Short}";

                LangHelper.SendAllNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea?.Map.BrowserSyncedData.Name ?? "?", OwnerTeam.Entity.Name));
                _gangwarArea?.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Map.BrowserSyncedData.Name, OwnerTeam.Entity.Name));

                _gangwarArea?.Attacker!.FuncIterate(player =>
                {
                    _ = new Invitation(player.Language.GANGWAR_ATTACK_INVITATION, player, null, Serializer, InvitationsHandler, onAccept: AcceptAttackInvitation)
                    {
                        RemoveOnLobbyLeave = true
                    };
                });

                _gangwarArea?.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, _gangwarArea.Map.BrowserSyncedData.Name, AttackerTeam.Entity.Name));
                _gangwarArea?.Owner!.FuncIterate(player =>
                {
                    _ = new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, Serializer, InvitationsHandler, onAccept: AcceptDefendInvitation)
                    {
                        RemoveOnLobbyLeave = true
                    };
                });
            }

            // Do we need to force someone to stay at target? If yes, force him! Kill him if he
            // don't want to stay there!
            else if (TargetObject is { })
            {
                var playerAtTarget = GetNextTargetMan();
                SetTargetMan(playerAtTarget);
                if (playerAtTarget is { })
                    playerAtTarget.ModPlayer!.Position = TargetObject.Position;
            }
        }

        public override void StopRound()
        {
            base.StopRound();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _gangwarArea?.SetAttackEnded(IsConquered);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (Lobby.IsGangActionLobby && Lobby.RoundEndReasonText is { })
            {
                Lobby.SendAllPlayerLangMessage(Lobby.RoundEndReasonText);
            }

            SetTargetMan(null);
        }

        public override void StartMapClear()
        {
            base.StartMapClear();

            ClearMapFromTarget();
        }

        public override bool CanEndRound(RoundEndReason endReason)
        {
            return endReason switch
            {
                RoundEndReason.BombDefused => false,
                RoundEndReason.BombExploded => false,
                RoundEndReason.Death => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                RoundEndReason.Empty => !Lobby.IsGangActionLobby,     // will handle this manually if gang action lobby
                RoundEndReason.NewPlayer => !Lobby.IsGangActionLobby,

                RoundEndReason.TargetEmpty => true,
                RoundEndReason.Time => true,
                RoundEndReason.Command => true,

                _ => true
            };
        }

        private bool IsConquered =>
            Lobby.CurrentRoundEndReason switch
            {
                RoundEndReason.Time => true,

                RoundEndReason.Command => false,
                RoundEndReason.TargetEmpty => false,
                RoundEndReason.Empty => false,

                RoundEndReason.Death when (Lobby.CurrentGameMode is Gangwar gangwar) => gangwar.AttackerTeam.AlivePlayers!.Count > 0,

                _ => false
            };
    }
}

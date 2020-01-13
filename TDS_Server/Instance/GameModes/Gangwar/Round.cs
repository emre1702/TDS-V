using TDS_Server.Enums;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        public override void StartMapChoose()
        {
            base.StartMapChoose();

            CreateTargetBlip();
            CreateTargetObject();
        }

        public override void StartRoundCountdown()
        {
            base.StartRoundCountdown();

            LangUtils.SendAllNotification(lang => lang.GANGWAR_PREPARATION_INFO);
            _gangwarArea.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));

            _gangwarArea.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, AttackerTeam.Entity.Name));

            _gangwarArea.Attacker.FuncIterate(player =>
            {
                _ = new Invitation(player.Language.GANGWAR_ATTACK_PREPARATION_INVITATION, player, null, onAccept: AcceptAttackPreparationInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });
        }

        public override void StartRound()
        {
            base.StartRound();

            Lobby.LobbyEntity.Name = $"[GW] {Lobby.GangwarArea!.Attacker!.Entity.Short} - {Lobby.GangwarArea.Owner!.Entity.Short}";

            LangUtils.SendAllNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.SetInAttack();

            _gangwarArea.Attacker!.FuncIterate(player =>
            {
                _ = new Invitation(player.Language.GANGWAR_ATTACK_INVITATION, player, null, onAccept: AcceptAttackInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });

            _gangwarArea.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, _gangwarArea.Entity.Map.Name, AttackerTeam.Entity.Name));
            _gangwarArea.Owner!.FuncIterate(player =>
            {
                _ = new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, onAccept: AcceptDefendInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });
        }

        public override void StopRound()
        {
            base.StopRound();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _gangwarArea.SetAttackEnded(IsConquered);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (Lobby.RoundEndReasonText is { })
            {
                Lobby.SendAllPlayerLangMessage(Lobby.RoundEndReasonText);
            }
        }

        public override void StartMapClear()
        {
            base.StartMapClear();

            ClearMapFromTarget();
        }

        public override bool CanEndRound(ERoundEndReason endReason)
        {
            return endReason switch
            {
                ERoundEndReason.BombDefused => false,
                ERoundEndReason.BombExploded => false,
                ERoundEndReason.Death => false,     // will handle this manually
                ERoundEndReason.Empty => false,     // will handle this manually
                ERoundEndReason.NewPlayer => false,

                ERoundEndReason.TargetEmpty => true,
                ERoundEndReason.Time => true,
                ERoundEndReason.Command => true,    //Todo: Am I sure with this?

                _ => true
            };
        }


        private bool IsConquered => 
            Lobby.CurrentRoundEndReason switch
            {
                ERoundEndReason.Time => true,

                ERoundEndReason.Command => false,
                ERoundEndReason.TargetEmpty => false,
                ERoundEndReason.Empty => false,

                ERoundEndReason.Death when (Lobby.CurrentGameMode is Gangwar gangwar) => gangwar.AttackerTeam.AlivePlayers!.Count > 0,

                _ => false 

            };
    }
}

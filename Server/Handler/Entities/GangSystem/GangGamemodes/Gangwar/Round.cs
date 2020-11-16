//using TDS.Server.Handler.Entities.Utility;

//namespace TDS.Server.Handler.Entities.GangSystem.GangGamemodes
//{
//    partial class GangGangwar
//    {
//        protected bool IsConquered =>
//            Lobby.CurrentRoundEndReason switch
//            {
//                RoundEndReason.Time => true,

//                RoundEndReason.Command => false,
//                RoundEndReason.TargetEmpty => false,
//                RoundEndReason.Empty => false,

//                RoundEndReason.Death when (Lobby.CurrentGameMode is Gangwar gangwar) => gangwar.AttackerTeam.AlivePlayers!.Count > 0,

//                _ => false
//            };

//        public override void StartRound()
//        {
//            base.StartRound();

//            _bonusBotConnectorClient.ChannelChat?.SendActionStartInfo(this);

//            Lobby.Entity.Name = $"[GW] {Lobby.GangwarArea!.Attacker!.Entity.Short} - {Lobby.GangwarArea.Owner!.Entity.Short}";

//            LangHelper.SendAllNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea?.Map.BrowserSyncedData.Name ?? "?", OwnerTeam.Entity.Name));
//            _gangwarArea?.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Map.BrowserSyncedData.Name, OwnerTeam.Entity.Name));

//            _gangwarArea?.Attacker!.FuncIterate(player =>
//            {
//                _ = new Invitation(player.Language.GANGWAR_ATTACK_INVITATION, player, null, InvitationsHandler, onAccept: AcceptAttackInvitation)
//                {
//                    RemoveOnLobbyLeave = true
//                };
//            });

//            _gangwarArea?.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, _gangwarArea.Map.BrowserSyncedData.Name, AttackerTeam.Entity.Name));
//            _gangwarArea?.Owner!.FuncIterate(player =>
//            {
//                _ = new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, InvitationsHandler, onAccept: AcceptDefendInvitation)
//                {
//                    RemoveOnLobbyLeave = true
//                };
//            });
//        }

//        public override void StartRoundCountdown()
//        {
//            base.StartRoundCountdown();

//            LangHelper.SendAllNotification(lang => lang.GANGWAR_PREPARATION_INFO);
//            _gangwarArea?.Attacker!.SendMessage(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Map.BrowserSyncedData.Name, OwnerTeam.Entity.Name));

//            _gangwarArea?.Owner!.SendMessage(lang => string.Format(lang.GANGWAR_OWNER_PREPARATION_INFO, _gangwarArea.Map.BrowserSyncedData.Name, AttackerTeam.Entity.Name));

//            _gangwarArea?.Attacker.FuncIterate(player =>
//            {
//                _ = new Invitation(player.Language.GANGWAR_ATTACK_PREPARATION_INVITATION, player, null, InvitationsHandler, onAccept: AcceptAttackPreparationInvitation)
//                {
//                    RemoveOnLobbyLeave = true
//                };
//            });
//        }

//        public override void StopRound()
//        {
//            base.StopRound();

//#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//            _gangwarArea?.SetAttackEnded(IsConquered);
//#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

//            if (Lobby.IsGangActionLobby && Lobby.RoundEndReasonText is { })
//            {
//                Lobby.SendMessage(Lobby.RoundEndReasonText);
//            }
//        }
//    }
//}

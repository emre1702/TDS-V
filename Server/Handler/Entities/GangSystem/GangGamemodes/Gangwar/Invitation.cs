//using GTANetworkAPI;
//using TDS_Server.Data.Abstracts.Entities.GTA;
//using TDS_Server.Data.Interfaces;
//using TDS_Server.Handler.Entities.Utility;

//namespace TDS_Server.Handler.Entities.GangSystem.GangGamemodes
//{
//    partial class GangGangwar
//    {
//        #region Private Methods

//        private async void AcceptAttackInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
//        {
//            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
//            {
//                NAPI.Task.RunSafe(() => invitation?.Resend());
//                return;
//            }

//            NAPI.Task.RunSafe(() =>
//            {
//                _gangwarArea?.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
//                _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
//            });
//        }

//        private async void AcceptAttackPreparationInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
//        {
//            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
//            {
//                NAPI.Task.RunSafe(() => invitation?.Resend());
//                return;
//            }

//            NAPI.Task.RunSafe(() => _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName)));
//        }

//        private async void AcceptDefendInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
//        {
//            if (!await Lobby.AddPlayer(player, (uint)OwnerTeam.Entity.Index))
//            {
//                NAPI.Task.RunSafe(() => invitation?.Resend());
//                return;
//            }

//            NAPI.Task.RunSafe(() =>
//            {
//                _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
//                _gangwarArea?.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
//            });
//        }

//        #endregion Private Methods
//    }
//}

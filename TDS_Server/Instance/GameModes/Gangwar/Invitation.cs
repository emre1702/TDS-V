using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        private async void AcceptAttackPreparationInvitation(TDSPlayer player, TDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                invitation?.Resend();
                return;
            }

            _gangwarArea.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }

        private async void AcceptAttackInvitation(TDSPlayer player, TDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                invitation?.Resend();
                return;
            }

            _gangwarArea.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
            _gangwarArea.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }


        private async void AcceptDefendInvitation(TDSPlayer player, TDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)OwnerTeam.Entity.Index))
            {
                invitation?.Resend();
                return;
            }

            _gangwarArea.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
            _gangwarArea.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }
    }
}

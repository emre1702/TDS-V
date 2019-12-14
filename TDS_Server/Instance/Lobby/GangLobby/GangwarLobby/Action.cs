using GTANetworkAPI;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{ 
    partial class GangwarLobby
    {
        public override bool StartPreparations()
        {
            if (!base.StartPreparations())
                return false;
            if (_gangwarArea.Owner is null)
                return false;

            _actionChangeTimer = new TDSTimer(StopPreparations, SettingsManager.ServerSettings.GangwarPreparationTimeMs, 1);

            SendAllPlayerLangNotification(lang => lang.GANGWAR_PREPARATION_INFO);
            SendLangMessageToAttacker(lang => string.Format(lang.GANGWAR_ATTACKER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.SetInPreparation();

            SendLangNotificationToOwner(lang => string.Format(lang.GANGWAR_OWNER_PREPARATION_INFO, _gangwarArea.Entity.Map.Name, AttackerTeam.Entity.Name));

            FuncIterateAttackerInGangLobby(player =>
            {
                new Invitation(player.Language.GANGWAR_ATTACK_PREPARATION_INVITATION, player, null, onAccept: AcceptAttackPreparationInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });

            return true;
        }

        public override void StopPreparations()
        {
            base.StopPreparations();
            StartAction();
        }

        public override bool StartAction()
        {
            base.StartAction();

            if (_gangwarArea.Owner is null)
                return false;

            _actionChangeTimer = new TDSTimer(StopAction, SettingsManager.ServerSettings.GangwarActionTimeMs, 1);

            SendAllPlayerLangNotification(lang => string.Format(lang.GANGWAR_STARTED_INFO, AttackerTeam.Entity.Name, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            SendLangMessageToAttacker(lang => string.Format(lang.GANGWAR_ATTACKER_STARTED_INFO, _gangwarArea.Entity.Map.Name, OwnerTeam.Entity.Name));
            _gangwarArea.SetInAttack();

            FuncIterateAttackerInGangLobby(player =>
            {
                new Invitation(player.Language.GANGWAR_ATTACK_INVITATION, player, null, onAccept: AcceptAttackInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });

            SendLangMessageToOwner(lang => string.Format(lang.GANGWAR_OWNER_STARTED_INFO, _gangwarArea.Entity.Map.Name, AttackerTeam.Entity.Name));
            FuncIterateOwnerInGangLobby(player =>
            {
                new Invitation(player.Language.GANGWAR_DEFEND_INVITATION, player, null, onAccept: AcceptDefendInvitation)
                {
                    RemoveOnLobbyLeave = true
                };
            });

            return true;
        }

        public override void StopAction()
        {
            base.StopAction();
            StartEnd();
        }

        private async void AcceptAttackPreparationInvitation(TDSPlayer player, TDSPlayer? sender, Invitation? invitation)
        {
            if (!await AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                invitation?.Resend();
                return;
            }

            SendLangNotificationToAttacker(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }

        private async void AcceptAttackInvitation(TDSPlayer player, TDSPlayer? sender, Invitation? invitation)
        {
            if (!await AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                invitation?.Resend();
                return;
            }

            SendLangNotificationToOwner(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
            SendLangNotificationToAttacker(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }


        private async void AcceptDefendInvitation(TDSPlayer player, TDSPlayer? sender, Invitation? invitation)
        {
            if (!await AddPlayer(player, (uint)OwnerTeam.Entity.Index))
            {
                invitation?.Resend();
                return;
            }

            SendLangNotificationToAttacker(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
            SendLangNotificationToOwner(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
        }
    }
}

using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS.Server.Handler;
using TDS.Server.Handler.Entities.Utility;

namespace TDS.Server.GangActionAreaSystem.Invitiations
{
    internal class BaseAreaInvitations
    {
        private readonly List<IInvitation> _invitations = new();

        private readonly LobbiesHandler _lobbiesHandler;
        private readonly InvitationsHandler _invitationsHandler;

#nullable disable
        private IBaseGangActionArea _area;
#nullable restore

        public BaseAreaInvitations(LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler)
            => (_lobbiesHandler, _invitationsHandler) = (lobbiesHandler, invitationsHandler);

        public void Init(IBaseGangActionArea area)
        {
            _area = area;
            var gangLobby = _lobbiesHandler.GangLobby;

            gangLobby.Events.PlayerJoinedAfter += OnPlayerJoinedGangLobby;
            area.Events.AddedToLobby += OnAddedToLobby;
            area.Events.RemovedFromLobby += OnRemovedFromLobby;
        }

        private ValueTask OnPlayerJoinedGangLobby((ITDSPlayer Player, int TeamIndex) data)
        {
            if (_area.InLobby is null)
                return default;

            if (data.Player.Gang == _area.Attacker)
            {
                var inPreparation = _area.InLobby.Rounds.RoundStates.CurrentState is ICountdownState;

                if (_area.InLobby.Rounds.RoundStates.CurrentState is not IInRoundState
                && !inPreparation)
                    return default;
                
                var invitation = new Invitation(
                    string.Format(inPreparation ? data.Player.Language.GANG_ACTION_ATTACK_PREPARATION_INVITATION : data.Player.Language.GANG_ACTION_ATTACK_INVITATION, _area),
                    data.Player, null, _invitationsHandler, 
                    inPreparation ? AcceptAttackPreparationInvitation : AcceptAttackInvitation, 
                    type: InvitationType.GangAction);
                lock (_invitations) { _invitations.Add(invitation); }
            }
            else if (data.Player.Gang == _area.Owner)
            {
                if (_area.InLobby.Rounds.RoundStates.CurrentState is not IInRoundState)
                    return default;
            }

            return default;
        }

        private void OnAddedToLobby(IGangActionLobby lobby)
        {
            lobby.Events.Countdown += OnCountdown;
            lobby.Events.InRound += OnInRound;
            lobby.Events.RoundEnd += OnRoundEnd;
        }

        private void OnRemovedFromLobby(IGangActionLobby lobby)
        {
            lobby.Events.Countdown -= OnCountdown;
            if (lobby.Events.InRound is { })
                lobby.Events.InRound -= OnInRound;
            if (lobby.Events.RoundEnd is { })
                lobby.Events.RoundEnd -= OnRoundEnd;
        }

        private void OnCountdown()
        {
            var invitations = _area.Attacker!.Invitations.Send(lang => string.Format(lang.GANG_ACTION_ATTACK_PREPARATION_INVITATION, _area), null, 
                AcceptAttackPreparationInvitation, type: InvitationType.GangAction);
            lock (_invitations) { _invitations.AddRange(invitations); }
        }

        private ValueTask OnInRound()
        {
            ClearInvitations();

            var invitations = _area.Attacker!.Invitations.Send(lang => string.Format(lang.GANG_ACTION_ATTACK_INVITATION, _area), null, AcceptAttackInvitation, type: InvitationType.GangAction);
            lock (_invitations) { _invitations.AddRange(invitations); }
            invitations = _area.Owner!.Invitations.Send(lang => string.Format(lang.GANG_ACTION_DEFEND_INVITATION, _area), null, AcceptDefendInvitation, type: InvitationType.GangAction);
            lock (_invitations) { _invitations.AddRange(invitations); }
            return default;
        }

        private ValueTask OnRoundEnd()
        {
            ClearInvitations();
            return default;
        }

        private async void PlayerAcceptedInvitation(ITDSPlayer player, IInvitation invitation, bool isAttacker, bool informOpponent)
        {
            try
            {
                if (_area.InLobby is null)
                    return;

                var teamIndex = isAttacker ? (int)GangActionLobbyTeamIndex.Attacker : (int)GangActionLobbyTeamIndex.Owner;
                if (!await _area.InLobby.Players.AddPlayer(player, teamIndex).ConfigureAwait(false))
                {
                    // Resend (maybe it will work later)
                    invitation.Show();
                    return;
                }

                var playerGang = isAttacker ? _area.Attacker : _area.Owner;
                playerGang!.Chat.SendNotification(lang => string.Format(lang.GANG_ACTION_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
                if (informOpponent)
                {
                    var oppGang = isAttacker ? _area.Owner : _area.Attacker;
                    oppGang!.Chat.SendNotification(lang => string.Format(lang.GANG_ACTION_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private void AcceptAttackPreparationInvitation(ITDSPlayer player, ITDSPlayer? _, IInvitation invitation)
        {
            PlayerAcceptedInvitation(player, invitation, true, false);
        }

        private void AcceptAttackInvitation(ITDSPlayer player, ITDSPlayer? _, IInvitation invitation)
        {
            PlayerAcceptedInvitation(player, invitation, true, true);
        }

        private void AcceptDefendInvitation(ITDSPlayer player, ITDSPlayer? _, IInvitation invitation)
        {
            PlayerAcceptedInvitation(player, invitation, false, false);
        }

        private void ClearInvitations()
        {
            lock (_invitations)
            {
                foreach (var invitation in _invitations)
                    invitation.Withdraw();
                _invitations.Clear();
            }
        }
    }
}

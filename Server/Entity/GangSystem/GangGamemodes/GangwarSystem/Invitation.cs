using AltV.Net.Async;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Handler;

namespace TDS_Server.Entity.GangSystem.GangGamemodes.GangwarSystem
{
    partial class GangGangwar
    {
        #region Private Methods

        private async void AcceptAttackInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                await AltAsync.Do(() => invitation?.Resend());
                return;
            }

            await AltAsync.Do(() =>
            {
                _gangwarArea?.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
                _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
            });
        }

        private async void AcceptAttackPreparationInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                await AltAsync.Do(() => invitation?.Resend());
                return;
            }

            await AltAsync.Do(() => _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName)));
        }

        private async void AcceptDefendInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)OwnerTeam.Entity.Index))
            {
                await AltAsync.Do(() => invitation?.Resend());
                return;
            }

            await AltAsync.Do(() =>
            {
                _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
                _gangwarArea?.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
            });
        }

        #endregion Private Methods
    }
}

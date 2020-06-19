﻿using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.Utility;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar
    {
        #region Private Methods

        private async void AcceptAttackInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                ModAPI.Thread.QueueIntoMainThread(() => invitation?.Resend());
                return;
            }

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                _gangwarArea?.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
                _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
            });
        }

        private async void AcceptAttackPreparationInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)AttackerTeam.Entity.Index))
            {
                ModAPI.Thread.QueueIntoMainThread(() => invitation?.Resend());
                return;
            }

            ModAPI.Thread.QueueIntoMainThread(() => _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName)));
        }

        private async void AcceptDefendInvitation(ITDSPlayer player, ITDSPlayer? sender, Invitation? invitation)
        {
            if (!await Lobby.AddPlayer(player, (uint)OwnerTeam.Entity.Index))
            {
                ModAPI.Thread.QueueIntoMainThread(() => invitation?.Resend());
                return;
            }

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                _gangwarArea?.Attacker!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_OPPONENT_PLAYER_JOINED_INFO, player.DisplayName));
                _gangwarArea?.Owner!.SendNotification(lang => string.Format(lang.GANGWAR_TEAM_YOURS_PLAYER_JOINED_INFO, player.DisplayName));
            });
        }

        #endregion Private Methods
    }
}

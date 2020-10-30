using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using DB = TDS_Server.Database.Entity.Command;

namespace TDS_Server.Handler.Commands.System
{
    public class CommandsUseRightsChecker
    {
        public CommandUsageRight GetRights(ITDSPlayer player, DB.Commands entity)
        {
            if (!NeedsRights(entity))
                return CommandUsageRight.User;

            if (CheckLobbyOwnerRights(player, entity))
                return CommandUsageRight.LobbyOwner;

            if (CheckAdminRights(player, entity))
                return CommandUsageRight.Admin;

            if (CheckDonatorRights(player, entity))
                return CommandUsageRight.Donator;

            if (CheckVipRights(player, entity))
                return CommandUsageRight.VIP;

            return CommandUsageRight.NotAllowed;
        }

        private bool NeedsRights(DB.Commands entity)
            => entity.LobbyOwnerCanUse || entity.NeededAdminLevel.HasValue || entity.NeededDonation.HasValue || entity.VipCanUse;

        private bool CheckLobbyOwnerRights(ITDSPlayer player, DB.Commands entity)
            => entity.LobbyOwnerCanUse && player.IsLobbyOwner;

        private bool CheckAdminRights(ITDSPlayer player, DB.Commands entity)
            => entity.NeededAdminLevel.HasValue && player.Admin.Level.Level >= entity.NeededAdminLevel.Value;

        private bool CheckDonatorRights(ITDSPlayer player, DB.Commands entity)
            => entity.NeededDonation.HasValue && (player.Entity?.Donation ?? 0) >= entity.NeededDonation.Value;

        private bool CheckVipRights(ITDSPlayer player, DB.Commands entity)
            => entity.VipCanUse && player.Entity?.IsVip == true;
    }
}

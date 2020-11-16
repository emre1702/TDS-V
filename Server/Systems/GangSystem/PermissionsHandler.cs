using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.GangsSystem
{
    public class PermissionsHandler : IGangPermissionsHandler
    {
        private readonly IGang _gang;

        public PermissionsHandler(IGang gang)
        {
            _gang = gang;
        }

        public bool IsAllowedTo(ITDSPlayer player, GangCommand type)
        {
            if (player.IsGangOwner)
                return true;

            var rank = player.GangRank?.Rank ?? 0;

            return type switch
            {
                GangCommand.Invite => rank >= _gang.Entity.RankPermissions.InviteMembers,
                GangCommand.Kick => rank >= _gang.Entity.RankPermissions.KickMembers,
                GangCommand.RankDown => rank >= _gang.Entity.RankPermissions.SetRanks,
                GangCommand.RankUp => rank >= _gang.Entity.RankPermissions.SetRanks,
                GangCommand.ModifyRanks => rank >= _gang.Entity.RankPermissions.ManageRanks,
                GangCommand.ModifyPermissions => rank >= _gang.Entity.RankPermissions.ManagePermissions,
                _ => true
            };
        }

    }
}

using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Handler.Extensions;

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
            if (_gang.Entity is null || _gang.Deleted || _gang.Entity.Id < 0)
                return false;

            var rank = player.GangRank?.Rank ?? 0;

            return type switch
            {
                GangCommand.Invite => rank >= _gang.Entity.RankPermissions.InviteMembers,
                GangCommand.Kick => rank >= _gang.Entity.RankPermissions.KickMembers,
                GangCommand.RankDown => rank >= _gang.Entity.RankPermissions.SetRanks,
                GangCommand.RankUp => rank >= _gang.Entity.RankPermissions.SetRanks,
                GangCommand.ModifyRanks => rank >= _gang.Entity.RankPermissions.ManageRanks,
                GangCommand.ModifyPermissions => rank >= _gang.Entity.RankPermissions.ManagePermissions,
                GangCommand.StartGangAction => rank >= _gang.Entity.RankPermissions.StartGangAction,
                _ => true
            };
        }

        public bool CheckIsAllowedTo(ITDSPlayer player, GangCommand type)
        {
            if (!IsAllowedTo(player, type))
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.NOT_ALLOWED));
                return false;
            }
            return true;
        }
    }
}

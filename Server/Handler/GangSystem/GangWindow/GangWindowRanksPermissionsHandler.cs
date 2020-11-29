using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models.GangWindow;
using TDS.Shared.Core;
using TDS.Shared.Data.Models;

namespace TDS.Server.Handler.GangSystem.GangWindow
{
    public class GangWindowRanksPermissionsHandler
    {
        public async Task<object?> Modify(ITDSPlayer player, string json)
        {
            var from = Serializer.FromBrowser<SyncedGangPermissions>(json);

            var to = player.Gang.Entity.RankPermissions;
            to.InviteMembers = from.InviteMembers;
            to.KickMembers = from.KickMembers;
            to.ManagePermissions = from.ManagePermissions;
            to.ManageRanks = from.ManageRanks;
            to.SetRanks = from.SetRanks;
            to.StartGangAction = from.StartGangAction;

            await player.Gang.Database.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            return "";
        }

        public string? GetPermissions(ITDSPlayer player, GangWindowRanksLevelsHandler ranksLevels)
        {
            var data = new GangPermissionsWindowData
            {
                Permissions = player.Gang.Entity.RankPermissions,
                Ranks = ranksLevels.GetRanks(player)
            };
            return Serializer.ToBrowser(data);
        }
    }
}

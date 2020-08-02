using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;

namespace TDS_Server.Handler.GangSystem.GangWindow
{
    public class GangWindowRanksPermissionsHandler
    {
        private readonly Serializer _serializer;

        public GangWindowRanksPermissionsHandler(Serializer serializer)
        {
            _serializer = serializer;
        }

        public async Task<object?> Modify(ITDSPlayer player, string json)
        {
            var from = _serializer.FromBrowser<SyncedGangPermissions>(json);

            var to = player.Gang.Entity.RankPermissions;
            to.InviteMembers = from.InviteMembers;
            to.KickMembers = from.KickMembers;
            to.ManagePermissions = from.ManagePermissions;
            to.ManageRanks = from.ManageRanks;
            to.SetRanks = from.SetRanks;
            to.StartGangwar = from.StartGangwar;

            await player.Gang.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });

            return "";
        }

        public string? GetPermissions(ITDSPlayer player)
        {
            return _serializer.ToBrowser(player.Gang.Entity.RankPermissions);
        }
    }
}

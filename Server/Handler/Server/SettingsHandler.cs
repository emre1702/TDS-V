using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Server;
using TDS_Shared.Data.Models;
using Command = TDS_Server.Database.Entity.Command.Commands;

namespace TDS_Server.Handler.Server
{
    public class SettingsHandler : ISettingsHandler
    {

        private readonly Command _loadMapOfOthersRightInfos;

        public SettingsHandler(TDSDbContext dbContext)
        {
            ServerSettings = dbContext.ServerSettings.Single();

            SyncedSettings = new SyncedServerSettingsDto()
            {
                DistanceToSpotToPlant = ServerSettings.DistanceToSpotToPlant,
                DistanceToSpotToDefuse = ServerSettings.DistanceToSpotToDefuse,
                RoundEndTime = 8 * 1000,
                MapChooseTime = 4 * 1000,
                TeamOrderCooldownMs = ServerSettings.TeamOrderCooldownMs,
                NametagMaxDistance = ServerSettings.NametagMaxDistance,
                ShowNametagOnlyOnAiming = ServerSettings.ShowNametagOnlyOnAiming
            };

            _loadMapOfOthersRightInfos = dbContext.Commands.First(c => c.Command == "LoadMapOfOthers");
        }

        public ServerSettings ServerSettings { get; }
        public SyncedServerSettingsDto SyncedSettings { get; }

        public bool CanLoadMapsFromOthers(ITDSPlayer player)
        {
            return _loadMapOfOthersRightInfos.NeededAdminLevel.HasValue && _loadMapOfOthersRightInfos.NeededAdminLevel <= player.Admin.Level.Level
                || _loadMapOfOthersRightInfos.VipCanUse && player.IsVip == true;
        }

    }
}

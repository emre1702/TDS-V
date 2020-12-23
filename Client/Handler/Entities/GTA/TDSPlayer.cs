using System;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Handler.Sync;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSPlayer : ITDSPlayer
    {
        public override string DisplayName => GetDisplayName();

        private readonly DataSyncHandler _dataSyncHandler;

        public TDSPlayer(ushort id, ushort remoteId, DataSyncHandler dataSyncHandler) : base(id, remoteId)
        {
            _dataSyncHandler = dataSyncHandler;
        }

        private string GetDisplayName()
        {
            string name = Name;
            int adminLevel = Convert.ToInt32(_dataSyncHandler.GetData(this, PlayerDataKey.AdminLevel));
            if (adminLevel > SharedConstants.ServerTeamSuffixMinAdminLevel)
                name = SharedConstants.ServerTeamSuffix + name;
            return name;
        }
    }
}

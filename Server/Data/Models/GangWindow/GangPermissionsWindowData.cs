using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Shared.Data.Models;

namespace TDS_Server.Data.Models.GangWindow
{
    public class GangPermissionsWindowData
    {
        [JsonProperty("0")]
        public SyncedGangPermissions Permissions { get; set; }

        [JsonProperty("1")]
        public List<GangRanks> Ranks { get; set; }
    }
}

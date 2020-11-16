using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Server.Database.Entity.GangEntities;
using TDS.Shared.Data.Models;

namespace TDS.Server.Data.Models.GangWindow
{
    public class GangPermissionsWindowData
    {
        [JsonProperty("0")]
        public SyncedGangPermissions Permissions { get; set; }

        [JsonProperty("1")]
        public List<GangRanks> Ranks { get; set; }
    }
}

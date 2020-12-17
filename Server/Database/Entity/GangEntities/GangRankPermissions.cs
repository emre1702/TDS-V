using Newtonsoft.Json;
using TDS.Shared.Data.Models;

namespace TDS.Server.Database.Entity.GangEntities
{
    public class GangRankPermissions : SyncedGangPermissions
    {
        [JsonIgnore]
        public int GangId { get; set; }

        [JsonIgnore]
        public virtual Gangs Gang { get; set; }
    }
}

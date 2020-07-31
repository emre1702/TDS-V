using Newtonsoft.Json;
using TDS_Shared.Data.Models;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangRankPermissions : SyncedGangPermissions
    {
        #region Public Properties

        [JsonIgnore]
        public virtual Gangs Gang { get; set; }

        [JsonIgnore]
        public int GangId { get; set; }

       

        #endregion Public Properties
    }
}

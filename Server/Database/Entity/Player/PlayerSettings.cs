using Newtonsoft.Json;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.Settings;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerSettings : SyncedPlayerSettings
    {
        #region Public Properties

        [JsonIgnore]
        public virtual Players Player { get; set; }

        #endregion Public Properties
    }
}

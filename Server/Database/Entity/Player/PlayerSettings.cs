using Newtonsoft.Json;
using TDS_Shared.Data.Models;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerSettings : SyncedPlayerSettingsDto
    {
        #region Public Properties

        [JsonIgnore]
        public virtual Players Player { get; set; }

        #endregion Public Properties
    }
}

using Newtonsoft.Json;
using TDS_Shared.Data.Models;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerSettings : SyncedPlayerSettingsDto
    {
        [JsonIgnore]
        public virtual Players Player { get; set; }
    }
}

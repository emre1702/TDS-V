using Newtonsoft.Json;
using TDS_Common.Dto;

namespace TDS_Server_DB.Entity.Player
{
    public class PlayerSettings : SyncedPlayerSettingsDto
    {
        [JsonIgnore]
        public virtual Players Player { get; set; }
    }
}

using MessagePack;
using TDS_Common.Dto;

namespace TDS_Server_DB.Entity.Player
{
    [MessagePackObject]
    public class PlayerSettings : SyncedPlayerSettingsDto
    {
        [IgnoreMember]
        public virtual Players Player { get; set; }
    }
}

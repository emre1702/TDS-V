using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedServerSettingsDto
    {
        [Key(0)]
        public float DistanceToSpotToPlant;
        [Key(1)]
        public float DistanceToSpotToDefuse;
        [Key(2)]
        public int RoundEndTime;
        [Key(3)]
        public int MapChooseTime;
        [Key(4)]
        public int TeamOrderCooldownMs;
        [Key(5)]
        public float NametagMaxDistance;
        [Key(6)]
        public bool ShowNametagOnlyOnAiming;
        [Key(7)]
        public int AFKKickAfterSec;
    }
}
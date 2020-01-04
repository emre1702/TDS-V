using MessagePack;
using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedPlayerSettingsDto
    {
        [Key(0)]
        public int PlayerId { get; set; }

        #region General 
        [Key(1)]
        public ELanguage Language { get; set; }
        [Key(2)]
        public bool AllowDataTransfer { get; set; }
        [Key(3)]
        public bool ShowConfettiAtRanking { get; set; }
        [Key(4)]
        public string Timezone { get; set; }
        [Key(13)]
        public string DateTimeFormat { get; set; }
        [Key(5)]
        public ulong DiscordUserId { get; set; }
        #endregion

        #region Fight
        [Key(6)]
        public bool Hitsound { get; set; }
        [Key(7)]
        public bool Bloodscreen { get; set; }
        [Key(8)]
        public bool FloatingDamageInfo { get; set; }
        #endregion

        #region Voice
        [Key(9)]
        public bool Voice3D { get; set; }
        [Key(10)]
        public bool VoiceAutoVolume { get; set; }
        [Key(11)]
        public float VoiceVolume { get; set; }
        #endregion

        #region Graphical 
        [Key(12)]
        public string MapBorderColor { get; set; }
        #endregion
    }
}

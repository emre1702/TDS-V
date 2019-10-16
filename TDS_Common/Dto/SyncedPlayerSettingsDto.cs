using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    public class SyncedPlayerSettingsDto
    {
        public int PlayerId { get; set; }
        public ELanguage Language { get; set; }
        public bool Hitsound { get; set; }
        public bool Bloodscreen { get; set; }
        public bool FloatingDamageInfo { get; set; }
        public bool AllowDataTransfer { get; set; }
        public bool Voice3D { get; set; }
        public bool VoiceAutoVolume { get; set; }
        public float VoiceVolume { get; set; }
        public string MapBorderColor { get; set; }
    }
}

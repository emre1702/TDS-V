using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    public class SyncedPlayerSettingsDto
    {
        public int PlayerId { get; set; }

        #region General 
        public ELanguage Language { get; set; }
        public bool AllowDataTransfer { get; set; }
        public bool ShowConfettiAtRanking { get; set; }
        #endregion

        #region Fight
        public bool Hitsound { get; set; }
        public bool Bloodscreen { get; set; }
        public bool FloatingDamageInfo { get; set; }
        #endregion

        #region Voice
        public bool Voice3D { get; set; }
        public bool VoiceAutoVolume { get; set; }
        public float VoiceVolume { get; set; }
        #endregion

        #region Graphical 
        public string MapBorderColor { get; set; }
        #endregion
    }
}

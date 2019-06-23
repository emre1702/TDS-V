using TDS_Common.Enum;

namespace TDS_Server_DB.Entity.Player
{
    public partial class PlayerSettings
    {
        public int PlayerId { get; set; }
        public ELanguage Language { get; set; }
        public bool Hitsound { get; set; }
        public bool Bloodscreen { get; set; }
        public bool FloatingDamageInfo { get; set; }
        public bool AllowDataTransfer { get; set; }

        public virtual Players Player { get; set; }
    }
}

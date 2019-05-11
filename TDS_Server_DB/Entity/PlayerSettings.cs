using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class PlayerSettings
    {
        public int PlayerId { get; set; }
        public short Language { get; set; }
        public bool Hitsound { get; set; }
        public bool Bloodscreen { get; set; }
        public bool FloatingDamageInfo { get; set; }
        public bool AllowDataTransfer { get; set; }

        public virtual Languages LanguageNavigation { get; set; }
        public virtual Players Player { get; set; }
    }
}

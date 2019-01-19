using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class PlayerSettings
    {
        public uint Id { get; set; }
        public bool HitsoundOn { get; set; }
        public byte Language { get; set; }
        public bool AllowDataTransfer { get; set; }

        public virtual Players IdNavigation { get; set; }
        public virtual Languages LanguageNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TDS.Entities
{
    public partial class Playersettings
    {
        public uint Id { get; set; }
        public bool? HitsoundOn { get; set; }
        public byte Language { get; set; }

        public Players IdNavigation { get; set; }
        public Languages LanguageNavigation { get; set; }
    }
}

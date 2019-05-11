using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class AdminLevelNames
    {
        public short Level { get; set; }
        public short Language { get; set; }
        public string Name { get; set; }

        public virtual Languages LanguageNavigation { get; set; }
        public virtual AdminLevels LevelNavigation { get; set; }
    }
}

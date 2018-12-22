using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Adminlevelnames
    {
        public byte Level { get; set; }
        public byte Language { get; set; }
        public string Name { get; set; }

        public virtual Languages LanguageNavigation { get; set; }
        public virtual Adminlevels LevelNavigation { get; set; }
    }
}

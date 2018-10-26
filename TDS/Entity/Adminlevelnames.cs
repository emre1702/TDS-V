using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Adminlevelnames
    {
        public byte Level { get; set; }
        public byte Language { get; set; }
        public string Name { get; set; }

        public Languages LanguageNavigation { get; set; }
        public Adminlevels LevelNavigation { get; set; }
    }
}

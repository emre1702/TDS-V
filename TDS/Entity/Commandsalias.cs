﻿using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class CommandsAlias
    {
        public string Alias { get; set; }
        public ushort? Command { get; set; }

        public virtual Commands CommandNavigation { get; set; }
    }
}

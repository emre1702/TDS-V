﻿using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class CommandsAlias
    {
        public string Alias { get; set; }
        public byte Command { get; set; }

        public virtual Commands CommandNavigation { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Settings
    {
        public uint Id { get; set; }
        public string GamemodeName { get; set; }
        public string MapsPath { get; set; }
        public string NewMapsPath { get; set; }
    }
}

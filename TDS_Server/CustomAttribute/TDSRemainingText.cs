﻿using System;

namespace TDS_Server.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class TDSRemainingText : Attribute
    {
        public int MinLength { get; set; } = 1;
        public int MaxLength { get; set; } = int.MaxValue;
    }
}
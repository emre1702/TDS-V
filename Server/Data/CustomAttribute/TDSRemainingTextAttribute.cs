using System;

namespace TDS.Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class TDSRemainingTextAttribute : Attribute
    {
        public int MaxLength { get; set; } = int.MaxValue;
        public int MinLength { get; set; } = 1;
    }
}

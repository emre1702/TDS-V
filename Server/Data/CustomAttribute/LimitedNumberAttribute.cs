using System;

namespace TDS.Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class LimitedNumberAttribute : Attribute
    {
        public uint MinValue { get; set; } = 0;
        public uint MaxValue { get; set; } = uint.MaxValue;
    }
}

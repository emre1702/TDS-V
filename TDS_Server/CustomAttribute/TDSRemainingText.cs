using System;

namespace TDS_Server.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class TDSRemainingText : Attribute
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public TDSRemainingText(int minLength = 1, int maxLength = int.MaxValue)
        {

        }
    }
}
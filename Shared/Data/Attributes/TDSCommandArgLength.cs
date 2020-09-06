using System;

namespace TDS_Shared.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TDSCommandArgLength : Attribute
    {
        public TDSCommandArgLength(int argLength)
            => ArgLength = argLength;

        public int ArgLength { get; }
    }
}

using System;

namespace TDS_Server.Data.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class TDSRemainingText : Attribute
    {
        #region Public Properties

        public int MaxLength { get; set; } = int.MaxValue;
        public int MinLength { get; set; } = 1;

        #endregion Public Properties
    }
}

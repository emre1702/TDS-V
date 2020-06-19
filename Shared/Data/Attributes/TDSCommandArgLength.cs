using System;

namespace TDS_Shared.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TDSCommandArgLength : Attribute
    {
        #region Public Constructors

        public TDSCommandArgLength(int argLength)
        {
            ArgLength = argLength;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ArgLength { get; }

        #endregion Public Properties
    }
}
